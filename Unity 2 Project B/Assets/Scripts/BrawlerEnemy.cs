using UnityEngine;
using System.Collections;

public class BrawlerEnemy : MonoBehaviour

{
    [Header("Player and Movement")]
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Attack")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackWindUp = 1f;
    [SerializeField] float attackCooldown = 1.5f;

    [Header("Attack Warning")]
    [SerializeField] Color attackColor = Color.red;

    [Header("Animation")]
    Animator animator;

    bool isAttacking = false;

    Renderer[] enemyRenderers;
    Material[][] enemyMaterials;
    Color[][] originalColors;

    // Finds the Animator and saves the original colors / materials
    void Awake()
    {
        // Get the Animator on this enemy
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning(
                "BrawlerEnemy could not find an Animator");
        }

        // Find the enemy's visible renderers
        // including those on child objects
        enemyRenderers = GetComponentsInChildren<Renderer>();

        enemyMaterials = new Material[enemyRenderers.Length][];
        originalColors = new Color[enemyRenderers.Length][];

        for (int i = 0; i < enemyRenderers.Length; i++)
        {
            enemyMaterials[i] = enemyRenderers[i].materials;

            originalColors[i] =
                new Color[enemyMaterials[i].Length];

            for (int j = 0; j < enemyMaterials[i].Length; j++)
            {
                originalColors[i][j] =
                    enemyMaterials[i][j].color;
            }
        }
    }

    // find the player
    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer =
                GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogWarning(
                    "BrawlerEnemy could not find the Player");
            }
        }
    }

    // this handles looking at the player, chasing them, and attacking
    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // Turn toward the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Do not move while performing an attack
        if (isAttacking)
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }

            return;
        }

        // Chase player
        if (distance > attackRange)
        {
            // Play the walking animation
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // Stop walking when close enough to attack
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }

            StartCoroutine(Attack());
        }
    }

    // turn red for warning, attack animation, do damage if possible
    IEnumerator Attack()
    {
        isAttacking = true;

        // Play the attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Warn the player by flashing red
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(attackWindUp);

        // Return the enemy to its original colors / materials
        RestoreEnemyColor();
        

        // Only do damage if the player is within a certain range
        if (player != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                player.position
            );

            if (distance <= attackRange)
            {
                PlayerHealth playerHealth =
                    player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }

        // cooldown for attacking
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


    // this is responsible for changing the colors / materials
    void SetEnemyColor(Color newColor)
    {
        foreach (Material[] materials in enemyMaterials)
        {
            foreach (Material material in materials)
            {
                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor("_BaseColor", newColor);
                }
                else if (material.HasProperty("_Color"))
                {
                    material.SetColor("_Color", newColor);
                }
            }
        }
    }

    // This is responsible for restoring color / material
    void RestoreEnemyColor()
    {
        for (int i = 0; i < enemyMaterials.Length; i++)
        {
            for (int j = 0; j < enemyMaterials[i].Length; j++)
            {
                Material material = enemyMaterials[i][j];

                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor(
                        "_BaseColor", originalColors[i][j]);
                }
                else if (material.HasProperty("_Color"))
                {
                    material.SetColor(
                        "_Color", originalColors[i][j]);
                }
            }
        }
    }
}


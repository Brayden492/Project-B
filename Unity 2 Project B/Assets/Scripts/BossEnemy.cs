using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [Header("Animation")]
    Animator animator;

    [Header("Target")]
    [SerializeField] Transform player;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Melee Attack")]
    [SerializeField] float meleeRange = 2f;
    [SerializeField] int meleeDamage = 20;
    [SerializeField] float meleeWindUp = 1f;
    [SerializeField] float meleeCooldown = 1.5f;

    [Header("Ranged Attack")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float rangedRange = 15f;
    [SerializeField] float rangedWindUp = 1f;
    [SerializeField] float rangedCooldown = 2f;

    [Header("Attack Warning")]
    [SerializeField] Color attackColor = Color.red;

    bool isAttacking;

    Renderer[] enemyRenderers;
    Material[][] enemyMaterials;
    Color[][] originalColors;


    // finding bosses animation and colors
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

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
                Material mat = enemyMaterials[i][j];

                if (mat.HasProperty("_BaseColor"))
                    originalColors[i][j] = mat.GetColor("_BaseColor");
                else if (mat.HasProperty("_Color"))
                    originalColors[i][j] = mat.GetColor("_Color");
            }
        }
    }

    // finds player
    void Start()
    {
        FindPlayer();
    }


    // this is finding the player by looking at tags
    void FindPlayer()
    {
        if (player != null)
            return;

        GameObject foundPlayer =
            GameObject.FindGameObjectWithTag("Player");

        if (foundPlayer != null)
            player = foundPlayer.transform;
        else
            Debug.LogWarning("BossHybrid could not find the Player!");
    }

    // handling a lot of boss behaviours
    void Update()
    {
        // find player reference
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // check distance
        float distance =
            Vector3.Distance(transform.position, player.position);

        // make sure boss is looking at player
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

        // Keep chasing the player even while attacking at range. Updated to include animations
        if (distance > meleeRange)
        {
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
            // stop walking if close enough for melee attack
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
        }

        // Don't start another attack while one is already happening.
        if (isAttacking)
            return;

        // Use melee when close, otherwise shoot projectiles.
        if (distance <= meleeRange)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (distance <= rangedRange)
        {
            if (firePoint != null && projectilePrefab != null)
                StartCoroutine(RangedAttack());
        }
    }


    //big idea is glow red for warning, attack, do damage if player is hit
    IEnumerator MeleeAttack()
    {
        isAttacking = true;

        // attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // flash red before attacking
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(meleeWindUp);

        //back to normal color / material
        RestoreEnemyColor();


        // damage player check
        if (player != null)
        {
            float distance =
                Vector3.Distance(transform.position, player.position);

            if (distance <= meleeRange)
            {
                PlayerHealth health =
                    player.GetComponent<PlayerHealth>();

                if (health != null)
                    health.TakeDamage(meleeDamage);
            }
        }

        // cooldown
        yield return new WaitForSeconds(meleeCooldown);
        isAttacking = false;
    }


    // similar to the idea of melee, just shoots a projectile instead
    IEnumerator RangedAttack()
    {
        isAttacking = true;
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(rangedWindUp);

        RestoreEnemyColor();


        // checks if player is withing range or not before attacking
        if (player != null && firePoint != null)
        {
            float distance =
                Vector3.Distance(transform.position, player.position);

            if (distance > meleeRange && distance <= rangedRange)
            {
                Vector3 direction =
                    player.position - firePoint.position;
                // get the projectile prefab 
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(direction)
                );

                // Projectile sometimes hits the enemy shooting it
                // Prevents the projectile from being able to collide with enemy
                Collider projectileCollider =
                    projectile.GetComponent<Collider>();

                Collider[] bossColliders =
                    GetComponentsInChildren<Collider>();

                if (projectileCollider != null)
                {
                    foreach (Collider bossCollider in bossColliders)
                    {
                        Physics.IgnoreCollision(
                            projectileCollider,
                            bossCollider
                        );
                    }
                }

                // fire projectile
                Projectile projectileScript =
                    projectile.GetComponent<Projectile>();

                if (projectileScript != null)
                {
                    projectileScript.Launch(direction);
                }
                else
                {
                    Debug.LogWarning(
                        "Boss projectile prefab is missing Projectile!"
                    );

                    Destroy(projectile);
                }
            }
        }

        yield return new WaitForSeconds(rangedCooldown);
        isAttacking = false;
    }

    // changing the color to red to warn for attack
    void SetEnemyColor(Color newColor)
    {
        foreach (Material[] materials in enemyMaterials)
        {
            foreach (Material mat in materials)
            {
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", newColor);
                else if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", newColor);
            }
        }
    }

    // restores the original color / material
    void RestoreEnemyColor()
    {
        for (int i = 0; i < enemyMaterials.Length; i++)
        {
            for (int j = 0; j < enemyMaterials[i].Length; j++)
            {
                Material mat = enemyMaterials[i][j];

                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", originalColors[i][j]);
                else if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", originalColors[i][j]);
            }
        }
    }
}

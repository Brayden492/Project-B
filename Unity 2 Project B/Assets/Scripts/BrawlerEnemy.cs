using UnityEngine;
using System.Collections;

public class BrawlerEnemy : MonoBehaviour

{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 3f;

    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackWindUp = 1f;
    [SerializeField] float attackCooldown = 1.5f;

    [Header("Attack Warning")]
    [SerializeField] Color attackColor = Color.red;

    bool isAttacking = false;

    Renderer[] enemyRenderers;
    Material[][] enemyMaterials;
    Color[][] originalColors;

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
                    "BrawlerAI could not find the Player!");
            }
        }
    }

    void Awake()
    {
        // Find the enemy's visible renderers,
        // including those on child objects.
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

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // Warn the player by flashing red.
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(attackWindUp);

        // Return the enemy to its original colors.
        RestoreEnemyColor();

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

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

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


using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;

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

    void Awake()
    {
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

    void Start()
    {
        FindPlayer();
    }

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

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distance =
            Vector3.Distance(transform.position, player.position);

        // Keep chasing the player even while attacking at range.
        if (distance > meleeRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
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

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(meleeWindUp);

        RestoreEnemyColor();

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

        yield return new WaitForSeconds(meleeCooldown);
        isAttacking = false;
    }

    IEnumerator RangedAttack()
    {
        isAttacking = true;
        SetEnemyColor(attackColor);

        yield return new WaitForSeconds(rangedWindUp);

        RestoreEnemyColor();

        if (player != null && firePoint != null)
        {
            float distance =
                Vector3.Distance(transform.position, player.position);

            if (distance > meleeRange && distance <= rangedRange)
            {
                Vector3 direction =
                    player.position - firePoint.position;

                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(direction)
                );

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

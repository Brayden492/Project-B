using UnityEngine;
using System.Collections;

public class EnemyRanged : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;
    [SerializeField] Transform firePoint;

    [Header("Attack")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackRange = 15f;
    [SerializeField] float attackWindUp = 1f;
    [SerializeField] float attackCooldown = 2f;

    [Header("Warning")]
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
                Debug.LogWarning("EnemyRanged could not find the Player!");
            }
        }
    }

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
                {
                    originalColors[i][j] =
                        mat.GetColor("_BaseColor");
                }
                else if (mat.HasProperty("_Color"))
                {
                    originalColors[i][j] =
                        mat.GetColor("_Color");
                }
            }
        }
    }

    void Update()
    {
        if (player == null || projectilePrefab == null
            || firePoint == null)
        {
            return;
        }

        float distance = Vector3.Distance(
            transform.position, player.position);

        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        SetEnemyColor(attackColor);

        // Give the player time to react.
        yield return new WaitForSeconds(attackWindUp);

        RestoreEnemyColor();

        if (player != null)
        {
            float distance = Vector3.Distance(
                transform.position, player.position);

            if (distance <= attackRange)
            {
                // Aim toward the player's current position.
                Vector3 direction =
                    player.position - firePoint.position;

                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(direction));

                Collider projectileCollider =
                    projectile.GetComponent<Collider>();

                Collider[] archerColliders =
                    GetComponentsInChildren<Collider>();

                if (projectileCollider != null)
                {
                    foreach (Collider archerCollider in archerColliders)
                    {
                        Physics.IgnoreCollision(
                            projectileCollider,
                            archerCollider
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
                    Destroy(projectile);
                    Debug.LogWarning(
                        "Projectile prefab is missing EnemyProjectile!");
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
            foreach (Material mat in materials)
            {
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", newColor);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", newColor);
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
                Material mat = enemyMaterials[i][j];

                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor(
                        "_BaseColor", originalColors[i][j]);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor(
                        "_Color", originalColors[i][j]);
                }
            }
        }
    }
}

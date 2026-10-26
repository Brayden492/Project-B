using UnityEngine;
using System.Collections;

public class EnemyRanged : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;
    [SerializeField] Transform firePoint;
    [SerializeField] float rotationSpeed = 10f;

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


    // Finds and assigns the player
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
                Debug.LogWarning("EnemyRanged could not find the Player");
            }
        }
    }

    // saves enemies original color
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

    // Check players distance, make enemy face the player, and attack when the player is within a certain range
    void Update()
    {
        // If something is missing in references, this stops the code
        if (player == null || projectilePrefab == null
            || firePoint == null)
        {
            return;
        }

        // Start attack when conditions met
        float distance = Vector3.Distance(
            transform.position, player.position);

        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }

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
    }

    // This handles the enemies attack
    IEnumerator Attack()
    {
        isAttacking = true;

        // Changing the enemy to red as a warning for attack
        SetEnemyColor(attackColor);

        // Give the player time to react
        yield return new WaitForSeconds(attackWindUp);
        
        // Change color back to normal
        RestoreEnemyColor();

        // Check if player is within range
        if (player != null)
        {
            float distance = Vector3.Distance(
                transform.position, player.position);

            if (distance <= attackRange)
            {
                // Aim toward the player's current position
                Vector3 direction =
                    player.position - firePoint.position;
                
                // Create projectile
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(direction));

                // Prevent enemy from shooting itself
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

                // Shoot the projectile
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
                        "Projectile prefab is missing EnemyProjectile");
                }
            }
        }

        // Cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    //Responisble for changing color
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

    //Responsible for restoring original color
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

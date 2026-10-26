using UnityEngine;

// Wanted the health bar to keep facing the player, so this script does that
public class HealthBarLookAtPlayer : MonoBehaviour
{
    // Needs to reference the player, I thought the camera would work best
    Camera playerCamera;

    void LateUpdate()
    {
        // Find camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
                return;
        }

        // Face health bar towards the camera
        transform.rotation = Quaternion.LookRotation(
            transform.position - playerCamera.transform.position
        );
    }
}

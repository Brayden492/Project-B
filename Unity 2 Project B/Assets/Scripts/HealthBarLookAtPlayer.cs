using UnityEngine;

public class HealthBarLookAtPlayer : MonoBehaviour
{
    Camera playerCamera;

    void LateUpdate()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
                return;
        }

        transform.rotation = Quaternion.LookRotation(
            transform.position - playerCamera.transform.position
        );
    }
}

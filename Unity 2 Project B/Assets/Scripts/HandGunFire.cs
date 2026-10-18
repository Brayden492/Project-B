
using System.Collections;
using UnityEngine;

public class HandGunFire : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] GameObject handgun;
    [SerializeField] AudioSource gunFire;

    [Header("UI")]
    [SerializeField] GameObject extraCross;

    [Header("Shooting")]
    [SerializeField] bool canFire = true;
    [SerializeField] GameObject objectDetection;

    Animator gunAnimator;

    void Start()
    {
        // Find the spawned Player.
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("HandGunFire could not find the Player!");
            return;
        }

        // Find the gun Animator under the Player's camera.
        if (handgun == null)
        {
            Camera playerCamera = player.GetComponentInChildren<Camera>();

            if (playerCamera != null)
            {
                Animator animator =
                    playerCamera.GetComponentInChildren<Animator>();

                if (animator != null)
                    handgun = animator.gameObject;
            }
        }

        if (handgun != null)
            gunAnimator = handgun.GetComponent<Animator>();

        // Find the gun's AudioSource under the Player.
        if (gunFire == null)
        {
            Transform gunFireObject = player.transform.Find("GunFireAudio");

            if (gunFireObject != null)
                gunFire = gunFireObject.GetComponent<AudioSource>();
        }

        // Find the crosshair on the scene Canvas.
        if (extraCross == null)
            extraCross = GameObject.Find("extraCross");

        if (gunAnimator == null)
            Debug.LogError("HandGunFire could not find the gun Animator!");

        if (gunFire == null)
            Debug.LogError("HandGunFire could not find the gun audio!");

        if (extraCross == null)
            Debug.LogWarning("HandGunFire could not find the extra crosshair!");
    }

    void OnEnable()
    {
        canFire = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !GameComplete.gameComplete)
        {
            if (canFire)
            {
                canFire = false;

                objectDetection = PlayerCasting.lookedAtObject;

                StartCoroutine(FiringGun());
            }
        }
    }

    IEnumerator FiringGun()
    {
        if (gunFire != null)
            gunFire.Play();

        if (extraCross != null)
            extraCross.SetActive(true);

        if (gunAnimator != null)
            gunAnimator.Play("GunShoot");

        if (objectDetection != null)
        {
            Enemy enemy =
                objectDetection.GetComponentInParent<Enemy>();

            if (enemy != null)
                enemy.TakeDamage(10);
        }

        yield return new WaitForSeconds(0.5f);

        if (gunAnimator != null)
            gunAnimator.Play("New State");

        if (extraCross != null)
            extraCross.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        canFire = true;
    }
}


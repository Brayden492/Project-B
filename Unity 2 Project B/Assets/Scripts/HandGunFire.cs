
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

    [SerializeField] int damagePerShot = 10;
    [SerializeField] float shotCooldown = 0.6f;

    Animator gunAnimator;


    // Finds the player that spawned in and builds its connections
    IEnumerator Start()
    {
        GameObject player = null;

        // This makes sure the player has actually spawned in first before trying to make connections
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        // Find the gun animator under the players camera
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

        // Find the guns audioSource under the player
        if (gunFire == null)
        {
            Transform gunFireObject = player.transform.Find("GunFireAudio");

            if (gunFireObject != null)
                gunFire = gunFireObject.GetComponent<AudioSource>();
        }

        // Find the crosshair on the scene canvas
        if (extraCross == null)
            extraCross = GameObject.Find("extraCross");

        // Lets me know if something didnt get connected
        if (gunAnimator == null)
            Debug.LogError("HandGunFire could not find the gun Animator!");

        if (gunFire == null)
            Debug.LogError("HandGunFire could not find the gun audio!");

        if (extraCross == null)
            Debug.LogWarning("HandGunFire could not find the extra crosshair!");
    }

    // Gives a way to let the gun fire again
    void OnEnable()
    {
        canFire = true;
    }

    // Increases damage 
    public void UpgradeDamage()
    {
        damagePerShot += 10;
        //Debug.Log("Damage upgraded to " + damagePerShot);
    }

    // Faster shooting
    public void UpgradeFireRate()
    {
        shotCooldown = Mathf.Max(0.3f, shotCooldown - 0.1f);
        //Debug.Log("Shot cooldown is now " + shotCooldown);
    }

    // Shoots when it gets input from user
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

    // Handles lots of gun related interactions
    IEnumerator FiringGun()
    {
        // Gun sound
        if (gunFire != null)
            gunFire.Play();

        // Crosshair visual to let the player know they shot
        if (extraCross != null)
            extraCross.SetActive(true);

        // Play the gun animation
        if (gunAnimator != null)
            gunAnimator.Play("GunShoot");


        // This handles doing damage
        if (objectDetection != null)
        {
            Enemy enemy =
                objectDetection.GetComponentInParent<Enemy>();

            if (enemy != null)
                enemy.TakeDamage(damagePerShot);

            else
            {
                BossHealth boss =
                    objectDetection.GetComponentInParent<BossHealth>();

                if (boss != null)
                    boss.TakeDamage(damagePerShot);
            }
        }

        // Gun cooldown
        yield return new WaitForSeconds(0.5f);

        if (gunAnimator != null)
            gunAnimator.Play("New State");

        if (extraCross != null)
            extraCross.SetActive(false);

        yield return new WaitForSeconds(Mathf.Max(0f, shotCooldown - 0.5f));

        canFire = true;
    }
}


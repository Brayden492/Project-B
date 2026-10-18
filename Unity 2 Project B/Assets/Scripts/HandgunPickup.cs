using UnityEngine;

public class HandgunPickup : MonoBehaviour
{
    [SerializeField] float theDistance;
    [SerializeField] GameObject actionText;
    [SerializeField] GameObject theGun;
    [SerializeField] GameObject gunInHand;
    [SerializeField] GameObject gunMechanics;

    // Jimmy Vegas supplied all of this code for picking up the gun
    void Update()
    {
        theDistance = PlayerCasting.distanceFromTarget;
    }

    void OnMouseOver()
    {
        if (theDistance <= 2)
        {
            actionText.SetActive(true);
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (theDistance <= 2)
            {
                this.GetComponent<BoxCollider>().enabled = false;
                actionText.SetActive(false);
                gunInHand.SetActive(true);
                theGun.SetActive(false);
                gunMechanics.GetComponent<HandGunFire>().enabled = true;
            }
        }
    }

    void OnMouseExit()
    {
        actionText.SetActive(false); 
    }
}

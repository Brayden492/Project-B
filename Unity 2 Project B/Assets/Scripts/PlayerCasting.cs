using UnityEngine;

public class PlayerCasting : MonoBehaviour
{

    public static float distanceFromTarget;
    public static GameObject lookedAtObject;
    [SerializeField] float toTarget;

    // Code provided by Jimmy Vegas with a few changes from me

    void Update()
    {

        lookedAtObject = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            distanceFromTarget = hit.distance;
            toTarget = hit.distance;
            // added a way to detect the actual object that is being looked at
            lookedAtObject = hit.collider.gameObject;

            //Debug.Log(lookedAtObject.name);
        }
    }
}

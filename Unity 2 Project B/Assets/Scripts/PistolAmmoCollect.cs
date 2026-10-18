using UnityEngine;

public class PistolAmmoCollect : MonoBehaviour
{
    [SerializeField] AudioSource ammoCollect;

    void Start()
    {
        
    }

 
    void Update()
    {
        
    }  
    // Code provided by Jimmy Vegas

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        ammoCollect.Play();
        GlobalAmmo.handgunAmmoCount += 10;
        Destroy(gameObject);
    }
}

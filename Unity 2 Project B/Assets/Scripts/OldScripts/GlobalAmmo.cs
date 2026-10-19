using UnityEngine;

public class GlobalAmmo : MonoBehaviour
{
    public static int handgunAmmoCount = 10;
    [SerializeField] GameObject ammoDisplay;
    // Code provided by Jimmy Vegas
    void Start()
    {
        handgunAmmoCount = 10;
    }
    void Update()
    {
        ammoDisplay.GetComponent<TMPro.TMP_Text>().text = "" + handgunAmmoCount;
    }
}

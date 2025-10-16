using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotGun;
    [SerializeField] private GameObject machineGun;

    public int weaponInt = 1;
    // Update is called once per frame
    void Update()
    {
        switch (weaponInt)
        {
            case 1:
                pistol.SetActive(true); 
                shotGun.SetActive(false);
                machineGun.SetActive(false);
                break;
            case 2:
                shotGun.SetActive(true);
                pistol.SetActive(false);
                machineGun.SetActive(false);
                break;
            case 3:
                pistol.SetActive(false);
                shotGun.SetActive(false);
                machineGun.SetActive(true);
                break;
        }
    }
}

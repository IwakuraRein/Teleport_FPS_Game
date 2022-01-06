using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Text rifleAmmoText, handGunAmmoText;
    public GameObject rifleUI,handGunUI;
    public string currentWeapon;
    public Slider lifeSlider;
    public Player player;
    public Scripts.Weapon.AssualtRifle rifile;
    public Scripts.Weapon.AssualtRifle handgun;
    public WeaponManager weaponManager;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        string name = weaponManager.carriedWeapon.name;
        
        if (name== "arms_assault_rifle_01")
        {
            rifleUI.SetActive(true);
            handGunUI.SetActive(false);
        }
        else if(name == "arms_handgun_01")
        {
            rifleUI.SetActive(false);
            handGunUI.SetActive(true);
        }

        rifleAmmoText.text = rifile.currentAmmo.ToString() + "/" + rifile.currentMaxAmmoCarried.ToString();
        handGunAmmoText.text = handgun.currentAmmo.ToString() + "/" + handgun.currentMaxAmmoCarried.ToString();
        lifeSlider.value = player.Heath*1.0f/100;
    }
}

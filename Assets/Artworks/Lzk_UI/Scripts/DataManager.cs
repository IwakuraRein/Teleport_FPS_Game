using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text score1Text, score2Text;
    public string score1, score2;
    public Text rifleAmmoText, handGunAmmoText;
    public string rifleAmmo, handGunAmmo;
    public GameObject rifle,handGun;
    public string currentWeapon;
    public Slider lifeSlider;
    public float life;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWeapon=="Rifle")
        {
            rifle.SetActive(true);
            handGun.SetActive(false);
        }
        else if(currentWeapon=="HandGun")
        {
            rifle.SetActive(false);
            handGun.SetActive(true);
        }
        score1Text.text = score1;
        score2Text.text = score2;
        rifleAmmoText.text = rifleAmmo;
        handGunAmmoText.text = handGunAmmo;
        lifeSlider.value = life;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CogAmmoController : MonoBehaviour
{
    // Start is called before the first frame update
    public static readonly int MaxAmmo = 5;
    TMP_Text cogAmmoCount;
    int currentAmmo = 0;

    int INIT_AMMO = 0;

    public int Ammo
    {
        get { return this.currentAmmo; }
        set { this.currentAmmo = value; }
    }

    void Start()
    {
        cogAmmoCount = GameObject.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        cogAmmoCount.text = INIT_AMMO.ToString();
        currentAmmo = INIT_AMMO;
        RubyController ruby = GameObject.Find("ruby").GetComponent<RubyController>();
        if(ruby != null)
        {
            foreach(string name in ruby.destroyed)
            {
                if(this.gameObject.name == name)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTextCount(int count) {
        cogAmmoCount.text = count.ToString();
    }

    public bool UpdateAmmo(int count) {
        currentAmmo += count;
        if(currentAmmo < 0) { 
            currentAmmo = 0;
            SetTextCount(currentAmmo);
            return false;
        }
        else if(currentAmmo > MaxAmmo) {
            currentAmmo = MaxAmmo;
            SetTextCount(currentAmmo);
            return false;
        }
        else {
            SetTextCount(currentAmmo);
            return true;            
        }
    }
}

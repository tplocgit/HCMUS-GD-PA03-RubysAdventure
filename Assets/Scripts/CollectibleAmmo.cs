using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAmmo : MonoBehaviour
{
    public AudioClip collectedClip;
    // Start is called before the first frame update
    void Start()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        CogAmmoController controller = other.GetComponent<CogAmmoController>();
        if (controller != null)
        {
            if(controller.UpdateAmmo(1))
            {
                RubyController rubyCtrl = other.GetComponent<RubyController>();
                if(rubyCtrl != null) {
                    rubyCtrl.PlaySound(collectedClip);
                    rubyCtrl.destroyed.Add(this.gameObject.name);
                }
                Destroy(gameObject);
            }
        }
    }
}

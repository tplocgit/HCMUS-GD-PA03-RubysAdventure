using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(controller.Health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                controller.PlaySound(collectedClip);
                controller.destroyed.Add(this.gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    Animator animator;

    bool isBroken = true;
    
    public ParticleSystem smokeEffect;


    public AudioClip fixedClip;

    GameObject npc;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        npc = GameObject.Find("JambiIdle");
    }

    void Update()
    {
        if(!isBroken) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        if(!isBroken) return;

        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fixed() {
        isBroken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Indie");
        smokeEffect.Stop();
        NPC c = npc.GetComponent<NPC>();
        c.RubyPlaySound(fixedClip);
        if(c != null)
            c.updateEnemy(-1);
    }
}

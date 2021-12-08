using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController  : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int Health { get { return currentHealth; }}
    int currentHealth;
    
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;

    float cdThrowCog;

    float timeSinceLastLaunch = 0f;

    AudioSource audioSrc;

    public AudioClip throwCogClip;
    public AudioClip damagedClip;
    public AudioClip footStepClip;

    CDController cdCtrl;

    bool isMoving = false;

    CogAmmoController ammoCtrl;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;

        audioSrc = GetComponent<AudioSource>();

        cdCtrl = GetComponent<CDController>();

        ammoCtrl = GetComponent<CogAmmoController>();

        cdThrowCog = CDController.cogCdTime;

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        timeSinceLastLaunch += Time.deltaTime;
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            ThrowCog();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }

        if(isMoving && !audioSrc.isPlaying)
            PlaySound(footStepClip);
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
    
    void FixedUpdate()
    {
        Vector2 lastPosition = rigidbody2d.position;
        Vector2 movePosition = rigidbody2d.position;
        movePosition.x = movePosition.x + speed * horizontal * Time.deltaTime;
        movePosition.y = movePosition.y + speed * vertical * Time.deltaTime;

        isMoving = lastPosition != movePosition;

        rigidbody2d.MovePosition(movePosition);

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            PlaySound(damagedClip);

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
        UIHpBar.Instance.SetValue(currentHealth / (float)maxHealth);

        if(currentHealth <= 0) {
            SceneController sc = GetComponent<SceneController>();
            if(sc != null)
                sc.GameOverScene();
        }
    }

    void ThrowCog()
    {
        if(timeSinceLastLaunch < cdThrowCog) return;

        timeSinceLastLaunch = 0;

        if(!ammoCtrl.UpdateAmmo(-1)) return;

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwCogClip);

        cdCtrl.ThrowCogCdEffect();
    }
}

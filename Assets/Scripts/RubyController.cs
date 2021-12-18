using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] private PlayerInputActions inputActions;
    Vector2 input = Vector2.zero;


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

        InitInputAction();

    }

    // Update is called once per frame
    void Update()
    {
        // horizontal = Input.GetAxis("Horizontal");
        // vertical = Input.GetAxis("Vertical");

        timeSinceLastLaunch += Time.deltaTime;
        
        Vector2 move = new Vector2(input.x, input.y);
        
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

        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        //     if (hit.collider != null)
        //     {
        //         NPC character = hit.collider.GetComponent<NPC>();
        //         if (character != null)
        //         {
        //             character.DisplayDialog();
        //         }  
        //     }
        // }

        // if(Input.GetKeyDown(KeyCode.C) || throwCogButton.Pressed)
        // {
        //     ThrowCog();
        // }

        if(isMoving && !audioSrc.isPlaying)
            PlaySound(footStepClip);

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Click");
        }
    }
    
    void InitInputAction()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Movement.performed += OnMovement;
        inputActions.Player.Movement.canceled += OnMovement;
        inputActions.Player.ThrowCog.performed += OnThrowCog;
        inputActions.Player.InteractNPC.performed += OnInteractNPC;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
    
    void FixedUpdate()
    {
        Vector2 lastPosition = rigidbody2d.position;
        Vector2 movePosition = rigidbody2d.position;
        movePosition +=  speed * input * Time.deltaTime;

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

    public void ThrowCog()
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

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            input = context.ReadValue<Vector2>();
        }
        if(context.canceled)
        {
            input = Vector2.zero;
        }
    }

    public void OnThrowCog(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ThrowCog();
        }
    }

    public bool isLookingNPC()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.3f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    return true;
                }  
                return false;
            }
            return false;
    }
    public void OnInteractNPC(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.3f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }
    }
}

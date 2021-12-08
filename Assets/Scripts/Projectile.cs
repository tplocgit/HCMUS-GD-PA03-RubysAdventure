using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    AudioSource audioSrc;
    public List<AudioClip> hitRobotClips;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySound(AudioClip clip) {
        audioSrc.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController eCtrl = other.collider.GetComponent<EnemyController>();
        if(eCtrl) {
            eCtrl.Fixed();
            PlaySound(hitRobotClips[Random.Range(0, hitRobotClips.Count)]);
        }
        Destroy(gameObject);
    }
}

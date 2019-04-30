using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;
    static int STATE_MOVE = 1;
    static int STATE_SHOOT = 2;
    static int STATE_MELEE = 3;
    static int STATE_DEATH = 4;

    public float moveSpeed = 20.0f;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform firingPoint;
    private Transform playerPosition;

    public float shotTimer = 2.0f;

    private bool left = true;
    public bool active = false;
    //private bool punch = false;
    private bool dead = false;

    public GameObject projectile;

    public AudioClip shootSound;
    public AudioClip deathSound;
    private AudioSource source;

    Animator animator;

    private int life = 3;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerPosition = GameObject.FindWithTag("Player").transform;
        Physics2D.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>());
	}
	
	// Update is called once per frame
	void Update () {
        if (!dead)
        {
            if (active)
            {
                if (shotTimer < 0.0f)
                {
                    animator.SetInteger(STATE_NAME, STATE_SHOOT);
                }
                else
                {
                    shotTimer -= Time.deltaTime;
                    if (left)
                    {
                        animator.SetInteger(STATE_NAME, STATE_MOVE);
                        if (gameObject.transform.position.x > leftPoint.transform.position.x)
                        {
                            gameObject.transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
                        }
                        else
                        {
                            left = false;
                        }
                    }
                    else
                    {
                        animator.SetInteger(STATE_NAME, STATE_MOVE);
                        if (gameObject.transform.position.x < rightPoint.transform.position.x)
                        {
                            gameObject.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                        }
                        else
                        {
                            left = true;
                        }
                    }
                }
            }
            else
            {
                animator.SetInteger(STATE_NAME, STATE_IDLE);
                float distance = Mathf.Abs(playerPosition.position.x - transform.position.x);
                if (distance < 500.0f)
                {
                    active = true;
                }
            }
        }
        else
        {
            animator.SetInteger(STATE_NAME, STATE_DEATH);
        }
	}

    void SizeUp()
    {
        transform.localScale = new Vector3(1.05f, 1.0f, 1.0f);
    }

    void SizeDown()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    void Shoot()
    {
        source.PlayOneShot(shootSound, 0.5f);
        GameObject beam = Instantiate(projectile, firingPoint.transform.position, firingPoint.transform.rotation);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), beam.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), beam.GetComponent<BoxCollider2D>());
    }

    void ResetTimer()
    {
        shotTimer = 2.0f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetType() == typeof(BoxCollider2D) && col.gameObject.layer == PLAYER_PROJECTILE_LAYER)
        {
            source.PlayOneShot(deathSound, 0.5f);
            Damage();
        }
    }

    void Damage()
    {
        if (life != 0)
        {
            life -= 1;
        }
        else
        {
            dead = true;
        }
    }

    void End()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;
    static int STATE_WALKING = 1;
    static int STATE_SHOOTING = 2;
    static int STATE_DEAD = 3;

    public float moveSpeed = 1.0f;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform firingPoint;
    private bool left = true;
    private bool dead = false;
    private bool targetAcquired = false;

    private Transform playerPosition;
    public GameObject projectile;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip sightingSound;
    private AudioSource source;

    Animator animator;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            animator.SetInteger(STATE_NAME, STATE_WALKING);
            float distance = Mathf.Abs(playerPosition.position.x - transform.position.x);
            if (left)
            {
                float angle = Vector2.Angle(-transform.right, playerPosition.position - transform.position);
                if (!targetAcquired)
                {
                    if (distance < 150.0f && angle < 5)
                    {
                        targetAcquired = true;
                        source.PlayOneShot(sightingSound, 0.7f);
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                    else
                    {
                        if (gameObject.transform.position.x > leftPoint.transform.position.x)
                        {
                            gameObject.transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
                        }
                        else
                        {
                            left = false;
                            gameObject.transform.localScale = new Vector3(-1, 1, 1);
                        }
                    }
                }
                else
                {
                    if(distance > 150.0f || angle > 5)
                    {
                        targetAcquired = false;
                    }
                    else
                    {
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                }
                
            }
            else
            {
                float angle = Vector2.Angle(transform.right, playerPosition.position - transform.position);
                if (!targetAcquired)
                {
                    if (distance < 150.0f && angle < 5)
                    {
                        targetAcquired = true;
                        source.PlayOneShot(sightingSound, 0.7f);
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                    else
                    {
                        if (gameObject.transform.position.x < rightPoint.transform.position.x)
                        {
                            gameObject.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                        }
                        else
                        {
                            left = true;
                            gameObject.transform.localScale = new Vector3(1, 1, 1);
                        }
                    }
                }
                else
                {
                    if (distance > 150.0f || angle > 5)
                    {
                        targetAcquired = false;
                    }
                    else
                    {
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                }
            }
        }
        else
        {
            animator.SetInteger(STATE_NAME, STATE_DEAD);
        }
    }

    void Decapitate()
    {
        moveSpeed = 0;
        dead = true;
        source.PlayOneShot(deathSound, 0.7f);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }

    void End()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        source.PlayOneShot(shootSound, 0.5f);
        GameObject beam = Instantiate(projectile, firingPoint.transform.position, firingPoint.transform.rotation);
        beam.GetComponent<BeamMovement>().spawnedRight = !left;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), beam.GetComponent<BoxCollider2D>());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == PLAYER_PROJECTILE_LAYER)
        {
            Decapitate();
        }
    }
}

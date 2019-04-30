using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;
    static int STATE_LEFT = 1;
    static int STATE_RIGHT = 2;
    static int STATE_SHOOTING = 3;
    static int STATE_DEAD = 4;

    public float moveSpeed = 1.0f;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform firingPoint;
    private bool left = true;
    private bool shooting = false;
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
    void Start () {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead)
        {
            if (!shooting)
            {
                animator.SetInteger(STATE_NAME, STATE_IDLE);
                float distance = Mathf.Abs(playerPosition.position.x - transform.position.x);
                if (left)
                {
                    animator.SetInteger(STATE_NAME, STATE_LEFT);
                    float angle = Vector2.Angle(transform.right, playerPosition.position - transform.position);
                    if (!targetAcquired)
                    {
                        if (distance < 150.0f && angle < 5)
                        {
                            targetAcquired = true;
                            source.PlayOneShot(sightingSound, 0.7f);
                            animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                            shooting = true;
                        }
                        else
                        {
                            if (gameObject.transform.position.x > leftPoint.transform.position.x)
                            {
                                Vector3 move = new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
                                gameObject.transform.Translate(move, Space.World);
                            }
                            else
                            {
                                left = false;
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
                else
                {
                    animator.SetInteger(STATE_NAME, STATE_RIGHT);
                    float angle = Vector2.Angle(transform.right, playerPosition.position - transform.position);
                    if (!targetAcquired)
                    {
                        if (distance < 150.0f && angle < 5)
                        {
                            targetAcquired = true;
                            source.PlayOneShot(sightingSound, 0.7f);
                            animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                            shooting = true;
                        }
                        else
                        {
                            if (gameObject.transform.position.x < rightPoint.transform.position.x)
                            {
                                Vector3 move = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                                gameObject.transform.Translate(move, Space.World);
                            }
                            else
                            {
                                left = true;
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
        }
        else
        {
            animator.SetInteger(STATE_NAME, STATE_DEAD);
        }
    }

    void End()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        source.PlayOneShot(shootSound, 0.5f);
        GameObject beam = Instantiate(projectile, firingPoint.transform.position, transform.rotation);
        beam.GetComponent<BeamMovement>().spawnedRight = true;
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), beam.GetComponent<BoxCollider2D>());
    }

    void DoneShooting()
    {
        shooting = false;
    }

    void MoveLeft()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    void MoveRight()
    {
        transform.Rotate(Vector3.forward * -Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == PLAYER_PROJECTILE_LAYER)
        {
            dead = true;
            shooting = false;
            animator.SetInteger(STATE_NAME, STATE_DEAD);
            source.PlayOneShot(deathSound, 0.7f);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CircleCollider2D>());
        }
    }
}

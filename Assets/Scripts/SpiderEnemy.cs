using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;
    static int STATE_SHOOTING = 1;
    static int STATE_DEAD = 2;

    public Transform firingPoint1;
    public Transform firingPoint2;
    public Transform headPoint;
    private bool left = false;
    private bool dead = false;
    private bool targetAcquired = false;

    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip sightingSound;
    private AudioSource source;

    private Transform playerPosition;
    public GameObject projectile;

    public GameObject head;

    Animator animator;

    // Use this for initialization
    void Start () {
        left = GetComponent<SpriteRenderer>().flipX;
        if (left)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            animator.SetInteger(STATE_NAME, STATE_IDLE);
            float distance = Mathf.Abs(playerPosition.position.x - transform.position.x);
            if (left)
            {
                float angle = Vector2.Angle(-transform.right, playerPosition.position - transform.position);
                if (!targetAcquired)
                {
                    if (distance < 200.0f && angle < 10.0f)
                    {
                        targetAcquired = true;
                        source.PlayOneShot(sightingSound, 0.7f);
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                }
                else
                {
                    if(distance > 200.0f || angle > 10.0f)
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
                    if (distance < 200.0f && angle < 10.0f)
                    {
                        targetAcquired = true;
                        source.PlayOneShot(sightingSound, 0.7f);
                        animator.SetInteger(STATE_NAME, STATE_SHOOTING);
                    }
                }
                else
                {
                    if(distance > 200.0f || angle > 10.0f)
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
        dead = true;
        source.PlayOneShot(deathSound, 0.7f);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }

    void LostHead()
    {
        Instantiate(head, headPoint.transform.position, headPoint.transform.rotation).GetComponent<SpiderHead>().spawnedRight = !left;
    }

    void End()
    {
        Destroy(gameObject);
    }

    void UpperShot()
    {
        source.PlayOneShot(shootSound, 0.7f);
        GameObject beam = Instantiate(projectile, firingPoint1.transform.position, firingPoint1.transform.rotation);
        beam.GetComponent<BeamMovement>().spawnedRight = !left;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), beam.GetComponent<BoxCollider2D>());
    }

    void LowerShot()
    {
        source.PlayOneShot(shootSound, 0.7f);
        GameObject beam = Instantiate(projectile, firingPoint2.transform.position, firingPoint2.transform.rotation);
        beam.GetComponent<BeamMovement>().spawnedRight = !left;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), beam.GetComponent<BoxCollider2D>());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == PLAYER_PROJECTILE_LAYER)
        {
            Decapitate();
        }
    }
}

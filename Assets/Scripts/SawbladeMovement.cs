using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeMovement : MonoBehaviour {

    static float SLOWDOWN_TIME = 0.2f;

    enum PowerRating
    {
        Bounce = 0,
        Stick = 1,
        Pierce = 2
    }

    [Header("Properties")]
    public float launchSpeed = 150.0f;
    public float returnDistance = 80.0f;
    public bool shredderBuff = false;

    [Header("Terrain Collision Properties")]
    public float tier1TerrainHealth = 150f;
    public float tier2TerrainHealth = 300f;
    public float tier3TerrainHealth = 450f;

    [HideInInspector]
    public bool justFired;

    Vector2 currentSpeed;
    float currentDistance;
    float currentBuffDuration;
    float originPosition;
    
    bool hasBounced;
    PlayerControls owner;
    SpriteRenderer sprite;
    Animator animator;
    AudioSource audioSource;

    // Use this for initialization
    public void Initialize(PlayerControls _owner)
    {
        owner = _owner;
        currentSpeed = Vector2.right * launchSpeed;
        if (!owner.facingRight)
        {
            currentSpeed = -currentSpeed;
        }
        originPosition = _owner.transform.position.x;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        justFired = true;
        hasBounced = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Set Sawblade Color to Red if it has a luster buff, otherwise, use the default.
        if (shredderBuff)
        {
            sprite.color = Color.red;
        }
        else
        {
            sprite.color = Color.white;
        }
        
        // Set Velocity
        GetComponent<Rigidbody2D>().velocity = currentSpeed;

        // Check current distance traveled.
        float newPosition = transform.position.x;
        currentDistance = newPosition - originPosition;
        
        // If this object hasn't bounced...
        if (!hasBounced)
        {
            // Substract speed if maximum distance has been reached.
            if (currentDistance > returnDistance)
            {
                currentSpeed.x -= (launchSpeed * Time.deltaTime) / SLOWDOWN_TIME;
            }
            else if (currentDistance < -returnDistance)
            {
                currentSpeed.x += (launchSpeed * Time.deltaTime) / SLOWDOWN_TIME;
            }

            // Reset the distance and speed when currentSpeed exceeds the desired speed.
            if (currentSpeed.x > launchSpeed)
            {
                currentSpeed.x = launchSpeed;
                originPosition = transform.position.x;
            }
            else if (currentSpeed.x < -launchSpeed)
            {
                currentSpeed.x = -launchSpeed;
                originPosition = transform.position.x;
            }
        }

        // If this object is at certain distance away from the player...
        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);
        if (viewport.x < -0.05f || viewport.x > 1.05f ||
            viewport.y < -0.05f || viewport.y > 1.05f)
        {
            owner.GiveSawblade();
            Destroy(gameObject);
        }
    }

    public void Bounce(Vector2 position, float speedIncrease)
    {
        Vector2 speedToVector2 = Vector2.right * speedIncrease;
        Bounce(position, speedToVector2);
    }
    public void Bounce(Vector2 position, Vector2 speedIncrease)
    {
        hasBounced = true;
        Vector2 newSpeed = currentSpeed + speedIncrease;
        if (transform.position.x < position.x)
        {
            currentSpeed = -newSpeed;
        }
        else
        {
            currentSpeed = newSpeed;
        }
    }

    public float Hit(float objectHealth, Vector2 objectPosition, bool isBreakable)
    {
        int behaviour = 0;
        behaviour += isBreakable ? 1 : 0;
        behaviour += currentSpeed.magnitude >= objectHealth ? 1 : 0;

        int sawbladeWidth = 0;
        sawbladeWidth += objectPosition.x > transform.position.x ? 13 : -13;

        //currentSpeed = shredderBuff ? currentSpeed : currentSpeed / 2;

        switch ((PowerRating)behaviour)
        {
            case PowerRating.Bounce:
                Debug.Log("Bounce");
                Bounce(objectPosition, 0);
                break;
            case PowerRating.Stick:
                Debug.Log("Stick");
                audioSource.Stop();
                currentSpeed.x = 0;
                currentSpeed.y = 0;
                transform.position += Vector3.right * sawbladeWidth;
                break;
            case PowerRating.Pierce:
                Debug.Log("Pierce");

                break;
        }

        float resultingObjectHealth = objectHealth -= currentSpeed.magnitude;
        return resultingObjectHealth;
    }

    float GetTierHealth(string tierName)
    {
        if(tierName == "tier1")
        {
            return tier1TerrainHealth;
        }
        else if (tierName == "tier2")
        {
            return tier2TerrainHealth;
        }
        else if (tierName == "tier3")
        {
            return tier3TerrainHealth;
        }
        else
        {
            return 0;
        }
    }

    bool GetBreakability(string breakability)
    {
        if(breakability == "Breakable")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        float objectHealth = 0;
        Vector2 objectPosition = c.transform.position;
        bool isBreakable = true;

        if (c.tag == "Melee")
        {

        }
        else if (c.tag.Split('_').Length > 1)
        {
            string[] values = c.tag.Split('_');
            objectHealth = GetTierHealth(values[0]);
            isBreakable = GetBreakability(values[1]);

            Hit(objectHealth, objectPosition, isBreakable);
        }
        else if (c.tag == "Enemy")
        {
            SawObject objectHit = c.GetComponent<SawObject>();
            isBreakable =
                objectHit != null ?
                    objectHit.isBreakable
                :
                    false
            ;
            objectHealth =
                objectHit != null ?
                    objectHit.health
                :
                    tier2TerrainHealth
            ;

            objectHit.health -= Hit(objectHealth, objectPosition, isBreakable);
        }
    }
}

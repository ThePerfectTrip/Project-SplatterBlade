using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    public float verticalKnockback;
    public float horizontalKnockback;

    public float narrowSawVerticalKnockback;
    public float narrowSawHorizontalKnockback;
    public float wideSawVerticalKnockback;
    public float wideSawHorizontalKnockback;

    Vector3 knockback;
    PlayerControls owner;
    float duration;
    bool isInitialized = false;

    public void Initialize(PlayerControls _owner, float _duration)
    {
        owner = _owner;
        duration = _duration;
        if(!owner.facingRight)
        {
            horizontalKnockback = -horizontalKnockback;
            //transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
        knockback = new Vector2(horizontalKnockback, verticalKnockback);
        isInitialized = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isInitialized)
        {
            duration -= Time.deltaTime;
            if (!(duration > 0))
            {
                Destroy(this.gameObject);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wide")
        {
            //collision.GetComponent<SawbladeMovement>().Bounce(transform.position, )
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHead : MonoBehaviour {

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;

    Animator animator;

    public bool spawnedRight;

    // Use this for initialization
    void Start () {
        Vector2 headForce;
        float torque;
        if (!spawnedRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            headForce = new Vector2(800.0f, 7000.0f);
            torque = -2000.0f;
        }
        else
        {
            headForce = new Vector2(-800.0f, 7000.0f);
            torque = 2000.0f;
        }
        GetComponent<Rigidbody2D>().AddForce(headForce);
        GetComponent<Rigidbody2D>().AddTorque(torque);
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        animator.SetInteger(STATE_NAME, STATE_IDLE);
	}

    void End()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;

    static string STATE_NAME = "state";
    static int STATE_IDLE = 0;
    static int STATE_BREAKING = 1;

    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void End()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        SawbladeMovement sawblade = col.GetComponent<SawbladeMovement>();
        if (sawblade != null)
        {
            animator.SetInteger(STATE_NAME, STATE_BREAKING);
        }
    }
}

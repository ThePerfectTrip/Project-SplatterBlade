using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    public bool isUnlocked;

	// Use this for initialization
	void Awake () {
        isUnlocked = false;
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.layer == LayerMask.NameToLayer("Player") || c.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            isUnlocked = true;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}

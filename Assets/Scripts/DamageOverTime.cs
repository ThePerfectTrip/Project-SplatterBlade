using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour {

    public float damagePerSecond = 10f;
    PlayerControls player;

	// Update is called once per frame
	void Update () {
		if(player != null)
        {
            player.luster -= damagePerSecond * Time.deltaTime;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControls hasHealth = collision.GetComponent<PlayerControls>();

        if (hasHealth != null)
        {
            player = hasHealth;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerControls hasHealth = collision.GetComponent<PlayerControls>();

        if (hasHealth != null)
        {
            player = null;
        }
    }
}

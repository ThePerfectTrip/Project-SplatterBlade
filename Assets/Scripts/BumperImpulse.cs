using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperImpulse : MonoBehaviour {

    public float bumperImpulse = 20000f;

	void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bumperImpulse);
        }
    }
}

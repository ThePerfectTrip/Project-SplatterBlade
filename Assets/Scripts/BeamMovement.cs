using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMovement : MonoBehaviour {

    public float speed = 150.0f;
    public float lifeTime = 1.0f;

    public bool spawnedRight;

    // Use this for initialization
    void Start () {
        if (!spawnedRight)
        {
            speed = -speed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
	
	// Update is called once per frame
	void Update () {

        // Set Velocity
        Vector3 move = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        transform.Translate(move, Space.Self);

        //How long has this been alive
        lifeTime -= Time.deltaTime;

        // Destory if lifetime amount has ended
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}

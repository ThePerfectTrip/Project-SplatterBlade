using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBoss : MonoBehaviour {

    static int PLAYER_PROJECTILE_LAYER = 9;
    static int TERRAIN_LAYER = 12;

    public float speed = 50.0f;

    private Transform playerPosition;

    // Use this for initialization
    void Start () {
        playerPosition = GameObject.FindWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 route = (playerPosition.position - gameObject.transform.position).normalized;
        if(route != Vector3.zero)
        {
            float angle = Mathf.Atan2(route.y, route.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        transform.position += route * speed * Time.deltaTime;
    }

    void End()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == PLAYER_PROJECTILE_LAYER || col.gameObject.layer == TERRAIN_LAYER)
        {
            End();
        }
    }
}

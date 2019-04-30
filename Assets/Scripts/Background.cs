using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public Transform playerPos;
    private Vector3 prevPos;

    public float horizontalMult = 20.0f;
    public float verticalMult = 20.0f;

	// Use this for initialization
	void Start () {
        prevPos = playerPos.position;
	}

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Vector3 speed = playerPos.position - prevPos;
        Vector3 movement = new Vector3(speed.x * horizontalMult, speed.y * verticalMult, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
        prevPos = playerPos.position;
    }
}

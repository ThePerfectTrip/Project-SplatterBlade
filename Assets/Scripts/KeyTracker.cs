using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour {

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public KeyScript[] locks;
    public float doorLength;
    public bool isOpen = true;
    public Direction openDirection;

    bool isUnlocked;
    BoxCollider2D doorCollider;
    float distanceTraveled;

	// Use this for initialization
	void Start () {
        isUnlocked = (locks.Length <= 0);
        doorCollider = GetComponent<BoxCollider2D>();

	}
	
	// Update is called once per frame
	void Update () {
		if(!isUnlocked)
        {
            foreach(KeyScript lck in locks)
            {
                if((isUnlocked = lck.isUnlocked) == false)
                {
                    break;
                }
            }
        }
        else if (distanceTraveled < doorLength)
        {
            float distanceToMove = doorLength * Time.deltaTime;
            if(distanceTraveled + distanceToMove > doorLength)
            {
                distanceToMove = doorLength - distanceTraveled;
            }

            switch(openDirection)
            {
                case Direction.Up:
                    transform.Translate(Vector3.up * distanceToMove);
                    break;
                case Direction.Down:
                    transform.Translate(Vector3.down * distanceToMove);
                    break;
                case Direction.Left:
                    transform.Translate(Vector3.left * distanceToMove);
                    break;
                case Direction.Right:
                    transform.Translate(Vector3.right * distanceToMove);
                    break;
            }

            distanceTraveled += distanceToMove;
        }

        doorCollider.enabled = !(isUnlocked && isOpen);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Idle = 0,
    Running = 1,
    Crouching = 2,
    Jumping = 3,
    Kicking = 4
}

public class PlayerCore : MonoBehaviour {
    
	PlayerInput inputManager;
	
	// Update is called once per frame
	void Update () {
        inputManager.Refresh();

        if(inputManager.horizontalAxis < 0 || inputManager.horizontalAxis > 0)
        {

        }
        if(inputManager.jump)
        {

        }
        if(inputManager.sprint)
        {

        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    string horizontalAxisName = "Horizontal";
    KeyCode jumpKey = KeyCode.Space;
    KeyCode sprintKey = KeyCode.LeftShift;

    public float horizontalAxis;
    public bool jump;
    public bool sprint;

    public void Refresh()
    {
        horizontalAxis = Input.GetAxis(horizontalAxisName);
        jump = Input.GetKeyDown(jumpKey);
        sprint = Input.GetKey(sprintKey);
	}
}

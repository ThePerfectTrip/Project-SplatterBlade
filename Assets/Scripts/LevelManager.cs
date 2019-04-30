using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Camera levelPerspective;
    public Canvas levelCanvas;
    public Color backgroundColor;
    public Image backgroundImage;
    public GameObject playerAvatar;

    PerpectiveHandler[] perspectiveHandlers;
    Vector3 cameraPosition;

	// Finds and creates all components needed for the level and initializes them if needed.
	void Awake () {
        // Initialize UI
        playerAvatar = GameObject.FindGameObjectWithTag("Player");
        Instantiate(levelCanvas);

        // Initialize Perspective
        /*GameObject[] perspectiveHandlerParents = GameObject.FindGameObjectsWithTag("PerspectiveHandler");
        perspectiveHandlers = new PerpectiveHandler[perspectiveHandlerParents.Length];
        if (perspectiveHandlers.Length > 0)
        {
            for (int handlerNum = 0; handlerNum < perspectiveHandlers.Length; handlerNum++)
            {
                if (perspectiveHandlers[handlerNum].isSpawnHandler)
                {
                    cameraPosition = perspectiveHandlers[handlerNum].transform.position;
                }
                perspectiveHandlers[handlerNum] = perspectiveHandlerParents[handlerNum].GetComponent<PerpectiveHandler>();
            }

        }*/
        playerAvatar.GetComponent<PlayerControls>().playerPerspective = Instantiate(levelPerspective);
        levelPerspective.transform.position = playerAvatar.transform.position;

        // Initialization
        levelPerspective.backgroundColor = backgroundColor;
    }
	
	// Level Behavior that happens during the level's lifetime.
	void Update () {

	}
}

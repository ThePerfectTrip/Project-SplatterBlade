using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

    public bool loadsLevel = true;
    public string targetScene;
    public RectTransform transition;
    bool sceneLoadConfirmed = false;
    bool isGameQuit = false;
    float unitsToMove;
    float unitsMoved = 0f;
	
    void Start()
    {
        unitsToMove = transition.sizeDelta.x / 1.1f;
        sceneLoadConfirmed = !loadsLevel;
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            sceneLoadConfirmed = true;
        }

        if(sceneLoadConfirmed)
        {
            if (unitsToMove > unitsMoved)
            {
                transition.Translate(Vector2.left * (unitsToMove * Time.deltaTime));
                unitsMoved += unitsToMove * Time.deltaTime;
            }
            else if (loadsLevel && !isGameQuit)
            {
                SceneManager.LoadScene(targetScene);
            }


            if(unitsToMove > unitsMoved && isGameQuit)
            {
                Application.Quit();
            }
        }
	}

    public void ManualActivation()
    {
        sceneLoadConfirmed = true;
    }

    public void ManualExitActivation()
    {
        isGameQuit = true;
    }
}

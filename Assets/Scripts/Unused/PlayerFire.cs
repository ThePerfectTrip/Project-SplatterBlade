using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour {

    public GameObject narrowShotObject;
    public GameObject wideShot;

    bool wideShotFired;
    bool narrowShotFired;

    bool hasSawBlade;

    // Use this for initialization
    void Start ()
    {
        wideShotFired = false;
        narrowShotFired = false;

        hasSawBlade = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(hasSawBlade)
        {
            if(wideShotFired)
            {

            }
            else if(narrowShotFired)
            {

            }
        }
	}
    
    void LaunchHorizontalSawBlade()
    {

    }

    public void ShootHorizontalSawBlade()
    {
        wideShotFired = hasSawBlade ? true : false;
    }

    public void ShootVerticalSawBlade()
    {
        narrowShotFired = hasSawBlade ? true : false;
    }
}

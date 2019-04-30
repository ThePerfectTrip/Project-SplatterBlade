using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpectiveHandler : MonoBehaviour {

    public enum ThresholdMagnitude
    {
        None,
        Positive,
        Negative
    }

    public bool isSpawnHandler;
    public ThresholdMagnitude horizontalThreshold;
    public ThresholdMagnitude verticalThreshold;

    [HideInInspector]
    public Camera perspective;


    // Use this for initialization
    void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Camera previewPerspective = GetComponent<Camera>();
        if(sprite != null)
        {
            sprite.enabled = false;
        }
        if(previewPerspective != null && previewPerspective != perspective)
        {
            Debug.LogWarning("PerspectiveHandler still contains a preview camera!");
            previewPerspective.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutscenePlayback : MonoBehaviour {

    public RawImage image;
    public VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
        StartCoroutine(PlayVideo());
	}

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while(!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }
        image.texture = videoPlayer.texture;
    }
}

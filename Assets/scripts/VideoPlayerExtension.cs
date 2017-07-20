using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerExtension : MonoBehaviour {
    public GameObject canvas;
    private VideoPlayer player;
    private bool play = false;
    private void Awake()
    {
        canvas.SetActive(false);
        player = this.GetComponent<VideoPlayer>();
        player.Play();
        player.loopPointReached += Stop;
    }

    void Stop(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        canvas.SetActive(true);
    }
}

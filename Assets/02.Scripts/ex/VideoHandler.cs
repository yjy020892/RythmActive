using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    public RawImage screen = null;
    public VideoPlayer topVideoPlayer;
    public VideoPlayer middleVideoPlayer;
    public VideoPlayer leftVideoPlayer;
    public VideoPlayer rightVideoPlayer;

    [HideInInspector] public VideoControl topVideoControl;
    [HideInInspector] public VideoControl middleVideoControl;
    [HideInInspector] public VideoControl leftVideoControl;
    [HideInInspector] public VideoControl rightVideoControl;

    public GameObject[] ledObj;

    GameObject stageObj;

    // Start is called before the first frame update
    void Start()
    {
        //VideoPlayerInit()
    }

    private void Update()
    {
        //if(topVideoPlayer.isPlaying)
        //{
        //    Debug.Log("frameCount : " + topVideoPlayer.clip.frameCount);
        //    Debug.Log("frameRate : " + topVideoPlayer.clip.length);
        //    Debug.Log("frame : " + topVideoPlayer.frame);
        //}
    }

    //public void VideoPlayerInit()
    //{
    //    if (screen != null && videoPlayer != null)
    //    {
    //        StartCoroutine(PrepareVideo());
    //    }
    //}

    //protected IEnumerator PrepareVideo()
    //{
    //    videoPlayer.Prepare();

    //    while(!topVideoPlayer.isPrepared)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    //screen.texture = videoPlayer.texture;
    //    Debug.Log("Prepared");

    //    //PlayVideo();
    //}

    public void SetStage(GameObject stage)
    {
        stageObj = stage;

        stageObj.SetActive(true);

        stageObj.transform.Find("Top").TryGetComponent(out topVideoPlayer);
        stageObj.transform.Find("Middle").TryGetComponent(out middleVideoPlayer);
        stageObj.transform.Find("Left").TryGetComponent(out leftVideoPlayer);
        stageObj.transform.Find("Right").TryGetComponent(out rightVideoPlayer);
        
        stageObj.transform.Find("Top").TryGetComponent(out topVideoControl);
        stageObj.transform.Find("Middle").TryGetComponent(out middleVideoControl);
        stageObj.transform.Find("Left").TryGetComponent(out leftVideoControl);
        stageObj.transform.Find("Right").TryGetComponent(out rightVideoControl);

        ledObj = GameObject.FindGameObjectsWithTag("LED");
    }

    public void PlayVideo(VideoPlayer video)
    {
        //if(videoPlayer != null && videoPlayer.isPrepared)
        //{
        video.Play();
            //videoPlayer.enabled = true;
            Debug.Log("PlayVideo");
        //}
    }

    public void StopVideo(VideoPlayer video)
    {
        //if(videoPlayer != null && videoPlayer.isPrepared)
        //{
        video.Stop();
            Debug.Log("StopVideo");
            //videoPlayer.enabled = false;
        //}
    }
}

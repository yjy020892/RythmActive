using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoControl : MonoBehaviour
{
    BackGroundVideo backGroundVideo;
    public VideoHandler videoHandler;

    public float topVideoTimer, middleVideoTimer, leftVideoTimer, rightVideoTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        backGroundVideo = BackGroundVideo.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(backGroundVideo.b_Play)
        {
            if (gameObject.name.Equals("Top"))
            {
                if (videoHandler.topVideoPlayer.isPlaying)
                {
                    topVideoTimer += Time.deltaTime;
                }
            }
            else if(gameObject.name.Equals("Middle"))
            {
                if (videoHandler.middleVideoPlayer.isPlaying)
                {
                    middleVideoTimer += Time.deltaTime;
                }
            }
            else if (gameObject.name.Equals("Left"))
            {
                if (videoHandler.leftVideoPlayer.isPlaying)
                {
                    leftVideoTimer += Time.deltaTime;
                }
            }
            else if (gameObject.name.Equals("Right"))
            {
                if (videoHandler.rightVideoPlayer.isPlaying)
                {
                    rightVideoTimer += Time.deltaTime;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Game;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] private Transform cameraMain;
    Transform center;

    [SerializeField] AudioSource music;
    [SerializeField] Animator anim;

    private int bpmCnt = 0;
    public float beatNumber = 1.0f;
    [SerializeField] private float bpm, songOffset;

    private float musicTime, songSPB, checkMusicTime = 0.0f;
    private bool b_BPM, b_Dance = false;

    [SerializeField] string songName;

    private void Start()
    {
        songSPB = (60.0f / bpm) * beatNumber;

        //Debug.Log(songSPB);
        center = GameObject.FindGameObjectWithTag("Center").transform;
    }

    private void FixedUpdate()
    {
        if (music.isPlaying)
        {
            Vector3 vec = cameraMain.position;

            vec.x = Mathf.Lerp(cameraMain.position.x, center.position.x, Time.deltaTime);
            //vec.x = center.position.x;

            cameraMain.position = vec;

            musicTime = music.time * 1000;

            if (!b_BPM)
            {
                if (musicTime >= songOffset)
                {
                    checkMusicTime = music.time;
                    b_BPM = true;
                }
            }
            else
            {
                if (music.time >= checkMusicTime)
                {
                    //Debug.Log(string.Format("time - {0} :: +SPB - {1} :: true - {2}", music.time, music.time + songSPB, checkMusicTime + songSPB));

                    bpmCnt++;

                    //SoundManager.instance.PlayBPM();
                    ScreenCapture.CaptureScreenshot(string.Format("{0}{1}{2}{3}.png", Application.dataPath, "/StreamingAssets/", "ScreenShot/", bpmCnt));

                    checkMusicTime += songSPB;
                }
            }
        }
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.S))
    //    {
    //        if (anim != null && !b_Dance)
    //        {
    //            b_Dance = true;

    //            anim.SetBool(songName, true);
    //        }
    //    }
    //}
}

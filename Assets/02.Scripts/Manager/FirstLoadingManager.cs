using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Enums_Common;

public class FirstLoadingManager : MonoBehaviour
{
    [SerializeField] private Camera kioskCam;

    [SerializeField] GameObject[] blackObjs;

    [SerializeField] RawImage kioskScreen = null;
    [SerializeField] private RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;

    [SerializeField] private Transform circle;

    private float timer = 0;

    bool b_Change = false;

    // Start is called before the first frame update
    void Start()
    {
        VideoPlayerInit();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);
        }

        circle.Rotate(Vector3.forward * -60.0f * Time.deltaTime);

        if(timer <= 4.0f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if(!b_Change)
            {
                b_Change = true;

                SceneController.instance.GotoScene(SceneType.Intro);
            }
        }
    }

    void VideoPlayerInit()
    {
        if (kioskScreen != null && screenScreen != null && kioskVideoPlayer != null && screenVideoPlayer != null)
        {
            StartCoroutine(PrepareKioskVideoPlayer());
            StartCoroutine(PrepareScreenVideoPlayer());
        }
    }

    protected IEnumerator PrepareKioskVideoPlayer()
    {
        kioskVideoPlayer.Prepare();

        while (!kioskVideoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        kioskScreen.texture = kioskVideoPlayer.texture;
        //Debug.Log("kioskVideoPlayer Prepared");

        PlayKioskVideoPlayer();
    }

    void PlayKioskVideoPlayer()
    {
        if (kioskVideoPlayer != null && kioskVideoPlayer.isPrepared)
        {
            blackObjs[0].SetActive(false);

            kioskVideoPlayer.Play();
            //Debug.Log("PlayKioskVideoPlayer");
        }
    }

    void StopKioskVideoPlayer()
    {
        if (kioskVideoPlayer != null && kioskVideoPlayer.isPrepared)
        {
            kioskVideoPlayer.Stop();
        }
    }

    // ----

    protected IEnumerator PrepareScreenVideoPlayer()
    {
        screenVideoPlayer.Prepare();

        while (!screenVideoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        screenScreen.texture = screenVideoPlayer.texture;
        //Debug.Log("screenVideoPlayer Prepared");

        PlayScreenVideoPlayer();
    }

    void PlayScreenVideoPlayer()
    {
        if (screenVideoPlayer != null && screenVideoPlayer.isPrepared)
        {
            blackObjs[1].SetActive(false);

            screenVideoPlayer.Play();
            //Debug.Log("PlayScreenVideoPlayer");
        }
    }

    void StopScreenVideoPlayer()
    {
        if (screenVideoPlayer != null && screenVideoPlayer.isPrepared)
        {
            screenVideoPlayer.Stop();
        }
    }
}

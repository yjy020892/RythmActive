using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    WaitForSeconds tutoRestTime = new WaitForSeconds(5.5f);

    #region Video
    [SerializeField] GameObject[] blackObjs;

    [SerializeField] RawImage kioskScreen = null;
    [SerializeField] RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;
    #endregion

    private static TutorialManager _instance;

    public static TutorialManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TutorialManager>();
            }

            return _instance;
        }
    }

    [SerializeField] GameObject[] tutorialUIs;
    [SerializeField] private Camera kioskCam;

    int tutorialNumber = 0;
    bool b_Wait = false;

    // Start is called before the first frame update
    void Start()
    {
        VideoPlayerInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);
        }

        if(!b_Wait)
        {
            if(!tutorialNumber.Equals(tutorialUIs.Length - 1))
            {
                StartCoroutine(TutorialControl());
            }
            
        }
    }

    public void ClickButton()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        string name = btn.name;

        switch(name)
        {
            case "Skip":
                StartCoroutine(GotoScene());
                break;
        }
    }

    IEnumerator TutorialControl()
    {
        b_Wait = true;

        yield return tutoRestTime;

        tutorialUIs[tutorialNumber].SetActive(false);
        tutorialUIs[tutorialNumber + 1].SetActive(true);
        tutorialNumber++;

        b_Wait = false;
    }

    public IEnumerator GotoScene(float time = 0.0f)
    {
        yield return new WaitForSeconds(time);

        BaseManager.instance.b_Tutorial = true;
        SceneController.instance.GotoScene(Enums_Common.SceneType.Game_Single);
    }

    #region Video
    void VideoPlayerInit()
    {
        if (kioskScreen != null && kioskVideoPlayer != null && screenScreen != null && screenVideoPlayer != null)
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

    // ---

    protected IEnumerator PrepareScreenVideoPlayer()
    {
        screenVideoPlayer.Prepare();

        while (!screenVideoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        screenScreen.texture = screenVideoPlayer.texture;
        //Debug.Log("kioskVideoPlayer Prepared");

        PlayScreenVideoPlayer();
    }

    void PlayScreenVideoPlayer()
    {
        if (screenVideoPlayer != null && screenVideoPlayer.isPrepared)
        {
            blackObjs[1].SetActive(false);

            screenVideoPlayer.Play();
            //Debug.Log("PlayKioskVideoPlayer");
        }
    }

    void StopScreenVideoPlayer()
    {
        if (screenVideoPlayer != null && screenVideoPlayer.isPrepared)
        {
            screenVideoPlayer.Stop();
        }
    }
    #endregion
}

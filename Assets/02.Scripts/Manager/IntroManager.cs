using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums_Common;

public class IntroManager : MonoBehaviour
{
    [SerializeField] SocketClient socketClient;

    private static IntroManager _instance = null;

    public static IntroManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<IntroManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private Camera kioskCam;

    [SerializeField] GameObject logo;
    [SerializeField] GameObject[] blackObjs;

    [SerializeField] RawImage kioskScreen = null;
    [SerializeField] private RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;

    float timer = 0.0f;
    [HideInInspector] public bool b_Start, b_Connect = false;
    bool b_Logo = false;

    //private void Awake()
    //{
    //    //btn_1p.onClick.AddListener(GotoScene);
    //    //btn_2p.onClick.AddListener(GotoScene);
    //}

    private void Start()
    {
        DataManager.instance.resultState = Enums_Game.ResultState.None;
        BaseManager.instance.gameCnt = 0;

        VideoPlayerInit();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);

            if (!b_Start && b_Logo)
            {
                if(b_Connect)
                {
                    b_Start = true;

                    socketClient.SendMessage();
                    //BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
                    //SceneController.instance.GotoScene(SceneType.SongSelect);
                }
            }
        }

        if(!b_Logo)
        {
            timer += Time.deltaTime;

            if(timer >= 1.5f)
            {
                b_Logo = true;
                //logo.SetActive(true);
            }
        }

    }

    public void GotoScene()
    {
        BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
        SceneController.instance.GotoScene(SceneType.SongSelect);

        //GameObject btn = EventSystem.current.currentSelectedGameObject;

        //switch (btn.name)
        //{
        //    case "1P":
        //        BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
        //        SceneController.instance.GotoScene(SceneType.SongSelect);
        //        //SceneController.instance.GotoScene(SceneType.Game_Single);
        //        break;

        //    case "2P":
        //        //SceneController.instance.GotoScene(SceneType.Lobby);
        //        break;
        //}
    }

    //public void GotoScene()
    //{
    //    GameObject btn = EventSystem.current.currentSelectedGameObject;

    //    switch(btn.name)
    //    {
    //        case "1P":
    //            BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
    //            SceneController.instance.GotoScene(SceneType.SongSelect);
    //            //SceneController.instance.GotoScene(SceneType.Game_Single);
    //            break;

    //        case "2P":
    //            //SceneController.instance.GotoScene(SceneType.Lobby);
    //            break;
    //    }
    //}

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

    public void StopKioskVideoPlayer()
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

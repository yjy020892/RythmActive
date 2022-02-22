using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enums_Game;
using EasyBanner;

public class SongSelectManager : MonoBehaviour
{
    SongDifficult songDifficult = SongDifficult.Easy;

    #region Video
    [SerializeField] GameObject[] blackObjs;

    [SerializeField] RawImage kioskScreen = null;
    [SerializeField] private RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;
    #endregion

    [SerializeField] private Camera kioskCam;
    [SerializeField] private GameObject[] startUI;
    private bool b_First;

    #region KioskUI
    private List<Sprite> leftCDSprites = new List<Sprite>();
    private List<Sprite> rightCDSprites = new List<Sprite>();

    [SerializeField] private BannerView _view;
    [SerializeField] private BannerItem _item;
    private BannerManager<Sprite> _mgr;
    [SerializeField] private List<Sprite> Sprites;
    private int _itemCount = 3;

    WaitForSeconds arrowTime = new WaitForSeconds(0.4f);

    [SerializeField] private Text timerText, songNameText, songArtistText, BPMText, songLevelText,
        /*songDateText, */songContentText/*, songSourceText*/, leftSongNameText, leftSongArtistText;

    [SerializeField] private Transform cameraUI;
    [SerializeField] private RectTransform bannerViewTrans, centerPanelSetTrans, leftPanelSetTrans, rightPanelSetTrans;

    public AudioSource audioSource;
    private AudioClip audioClip;

    public Image centerCDImage, leftCDImage, rightCDImage, rightAlbumImage;
    [SerializeField] private Sprite[] easySprite, hardSprite;
    [SerializeField] private Image[] difficultImg;
    public Font redFont;

    [HideInInspector] public int playCount, difficultCount, songListNumber = 0;

    private string songNameStr, songArtistStr, songBPM;
    private float timer;
    private float warningTimer;
    private bool b_SelectTimer, b_SceneChange, b_Warning, b_Arrow = false;
    #endregion

    #region ScreenUI
    private int goalNumber;
    private string[] goalStr = {"New", "Silver", "Gold", "Diamond", "Ruby"};

    [SerializeField] private GameObject playEffectObj;

    [SerializeField] private Sprite[] badgeSprites;

    [SerializeField] private Animator badgeAnim;
    [SerializeField] private SpriteRenderer badgeImg;
    [SerializeField] private Text songNameScreenText, songArtistScreenText, songBPMScreenText, songLevelScreenText, goalText, likeText, contentText;
    #endregion

    static private SongSelectManager _instance;

    public static SongSelectManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SongSelectManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (BaseManager.instance.gameCnt.Equals(0))
        {
            startUI[0].SetActive(true);
        }
        else if (BaseManager.instance.gameCnt >= 1)
        {
            b_First = false;
            startUI[1].SetActive(true);
        }

        for (int i = 0; i < DataManager.instance.songList.Count; i++)
        {
            string str = DataManager.instance.songList[i];

            leftCDSprites.Add(Resources.Load<Sprite>(string.Format("Song/{0}/{1}_Left", str, str)));
            rightCDSprites.Add(Resources.Load<Sprite>(string.Format("Song/{0}/{1}_Right", str, str)));
        }
    }

    void Start()
    {
        VideoPlayerInit();

        Init();

        if (!b_First)
        {
            SetScreenUI();
        }
        
        //SkeletonController.SkeletonOn += OrbbecOn;

        //if (Screen.width == 1920 && Screen.height == 1200)
        //{
        //    Debug.Log("1920x1200");
        //    cameraUI.localPosition = new Vector3(8.41f, 5.3f);
        //}
        //else if (Screen.width == 1920 && Screen.height == 1080)
        //{
        //    Debug.Log("1920x1080");
        //    cameraUI.localPosition = new Vector3(8.41f, 4.6f);
        //}
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);
        }

        if (b_SceneChange)
        {
            return;
        }

        if(b_SelectTimer)
        {
            timer -= Time.deltaTime;

            if(timer > 100)
            {
                timerText.text = timer.ToString("N0");
            }
            else
            {
                timerText.text = string.Format("{0}{1}", 0, timer.ToString("N0"));
            }
            

            if(!b_Warning)
            {
                if(timer <= warningTimer)
                {
                    b_Warning = true;
                    timerText.color = Color.red;
                }
            }

            if (timer <= 0)
            {
                b_SelectTimer = false;

                timer = 0.0f;
                PlaySong();
            }
        }
    }

    public void ClickArrow()
    {
        if(!b_SceneChange && !b_Arrow)
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;

            StartCoroutine(ClickArrowCo(btn));
        }
    }

    private IEnumerator ClickArrowCo(GameObject obj)
    {
        int i;

        b_Arrow = true;

        switch (obj.name)
        {
            case "LeftArrow":
                if (songListNumber.Equals(0))
                {
                    songListNumber = DataManager.instance.songList.Count - 1;
                }
                else
                {
                    songListNumber -= 1;
                }

                _mgr.OnNextPageClick();
                
                SoundManager.instance.PlayArrow();
                audioSource.Stop();
                SetSong();

                if (songListNumber.Equals(DataManager.instance.songList.Count - 1))
                {
                    i = 0;
                }
                else
                {
                    i = songListNumber + 1;
                }

                leftCDImage.sprite = leftCDSprites[i];

                centerPanelSetTrans.SetParent(bannerViewTrans.GetChild(2));
                centerPanelSetTrans.anchoredPosition = Vector3.zero;
                centerPanelSetTrans.localScale = Vector3.one;

                rightPanelSetTrans.SetParent(bannerViewTrans.GetChild(0));
                rightPanelSetTrans.anchoredPosition = Vector3.zero;
                rightPanelSetTrans.localScale = Vector3.one;

                leftPanelSetTrans.SetParent(bannerViewTrans.GetChild(1));
                leftPanelSetTrans.anchoredPosition = Vector3.zero;
                leftPanelSetTrans.localScale = Vector3.one;
                break;

            case "RightArrow":
                if (songListNumber.Equals(DataManager.instance.songList.Count - 1))
                {
                    songListNumber = 0;
                }
                else
                {
                    songListNumber += 1;
                }

                _mgr.OnPrePageClick();

                SoundManager.instance.PlayArrow();
                audioSource.Stop();
                SetSong();

                if (songListNumber.Equals(0))
                {
                    i = DataManager.instance.songList.Count - 1;
                }
                else
                {
                    i = songListNumber - 1;
                }

                rightCDImage.sprite = leftCDSprites[i];

                centerPanelSetTrans.SetParent(bannerViewTrans.GetChild(2));
                centerPanelSetTrans.anchoredPosition = Vector3.zero;
                centerPanelSetTrans.localScale = Vector3.one;

                rightPanelSetTrans.SetParent(bannerViewTrans.GetChild(1));
                rightPanelSetTrans.anchoredPosition = Vector3.zero;
                rightPanelSetTrans.localScale = Vector3.one;

                leftPanelSetTrans.SetParent(bannerViewTrans.GetChild(0));
                leftPanelSetTrans.anchoredPosition = Vector3.zero;
                leftPanelSetTrans.localScale = Vector3.one;
                break;
        }

        yield return arrowTime;

        b_Arrow = false;
    }

    public void GotoScene()
    {
        if (b_SceneChange)
        {
            return;
        }

        GameObject btn = EventSystem.current.currentSelectedGameObject;

        switch (btn.name)
        {
            case "Easy":
                if(difficultImg[0].sprite.Equals(easySprite[0]))
                {
                    SoundManager.instance.PlayDifficult();

                    difficultImg[0].sprite = easySprite[1];
                    difficultImg[1].sprite = hardSprite[0];

                    songDifficult = SongDifficult.Easy;

                    DataManager.instance.songData._Difficult = songDifficult;
                }
                break;

            case "Hard":
                if (difficultImg[1].sprite.Equals(hardSprite[0]))
                {
                    SoundManager.instance.PlayDifficult();

                    difficultImg[1].sprite = hardSprite[1];
                    difficultImg[0].sprite = easySprite[0];

                    songDifficult = SongDifficult.Hard;

                    DataManager.instance.songData._Difficult = songDifficult;
                }
                break;

            case "Start":
                PlaySong();
                break;
        }
    }

    private void OrbbecOn()
    {
        Init();
    }

    private void Init()
    {
        timer = DataManager.instance.gameData._SelectTime;
        warningTimer = DataManager.instance.gameData._WarningTime;
        b_SelectTimer = true;
        songDifficult = SongDifficult.Easy;
        DataManager.instance.songData._Difficult = songDifficult;

        _mgr = new BannerManager<Sprite>(_view, _item);
        _mgr.Init(_itemCount, Sprites);

        centerPanelSetTrans.SetParent(bannerViewTrans.GetChild(2));
        centerPanelSetTrans.anchoredPosition = Vector3.zero;
        centerPanelSetTrans.localScale = Vector3.one;
        centerPanelSetTrans.gameObject.SetActive(true);

        rightPanelSetTrans.SetParent(bannerViewTrans.GetChild(0));
        rightPanelSetTrans.anchoredPosition = Vector3.zero;
        rightPanelSetTrans.localScale = Vector3.one;
        rightPanelSetTrans.gameObject.SetActive(true);

        leftPanelSetTrans.SetParent(bannerViewTrans.GetChild(1));
        leftPanelSetTrans.anchoredPosition = Vector3.zero;
        leftPanelSetTrans.localScale = Vector3.one;
        leftPanelSetTrans.gameObject.SetActive(true);

        SetSong();
    }

    private void SetScreenUI()
    {
        goalNumber = DataManager.instance.songData._Rank;
        songDifficult = DataManager.instance.songData._Difficult;

        songNameScreenText.text = DataManager.instance.songData._SongName;
        songArtistScreenText.text = DataManager.instance.songData._Artist;
        songBPMScreenText.text = string.Format("{0} {1}", "BPM :", DataManager.instance.songData._BPM);

        if (songDifficult.Equals(SongDifficult.Easy))
        {
            songLevelScreenText.text = string.Format("LEVEL : {0}", "Easy");
        }
        else if (songDifficult.Equals(SongDifficult.Hard))
        {
            songLevelScreenText.text = string.Format("LEVEL : {0}", "Hard");
        }

        badgeImg.sprite = badgeSprites[goalNumber];

        if (goalStr[goalNumber].Equals("Ruby"))
        {
            badgeAnim.SetBool("Ruby", true);
        }
        else
        {
            badgeAnim.SetBool("Other", true);
        }

        likeText.text = string.Format("♥{0}", DataManager.instance.songData._Score);
        goalText.text = string.Format("{0}", goalStr[goalNumber]);
        contentText.text = string.Format("{0}개의 좋아요를 받아 '{1}'등급을 달성하셨습니다", DataManager.instance.songData._Score, goalStr[goalNumber]);
    }

    public void SetSong()
    {
        songNameStr = DataManager.instance.songList[songListNumber];
        songArtistStr = DataManager.instance.songArtist[songListNumber];
        songBPM = DataManager.instance.songBPM[songListNumber];
        string songLevel = DataManager.instance.songLevel[songListNumber];
        //string songDate = DataManager.instance.songDate[songListNumber];
        string songContent = DataManager.instance.songContent[songListNumber];
        //string songSource = DataManager.instance.songSource[songListNumber];

        centerCDImage.sprite = leftCDSprites[songListNumber];
        rightAlbumImage.sprite = rightCDSprites[songListNumber];
        songNameText.text = songNameStr;
        leftSongNameText.text = songNameStr;
        songArtistText.text = songArtistStr;
        leftSongArtistText.text = songArtistStr;
        BPMText.text = string.Format("BPM {0}", songBPM);
        songLevelText.text = string.Format("{0}", songLevel);
        //songDateText.text = string.Format("SINCE: {0}", songDate);
        songContentText.text = songContent.Replace("\\n", "\n");
        
        //songSourceText.text = songSource;
        audioClip = Resources.Load<AudioClip>(string.Format("Song/{0}/{1}_Highlight", songNameStr, songNameStr));
        audioSource.clip = audioClip;

        audioSource.Play();
    }

    public void PlaySong()
    {
        b_SceneChange = true;
        SoundManager.instance.PlayMusicSelect();
        playEffectObj.SetActive(true);

        DataManager.instance.songData._AudioFileName = songNameStr;
        DataManager.instance.songData._SongName = songNameStr;
        DataManager.instance.songData._Artist = songArtistStr;
        int.TryParse(songBPM, out DataManager.instance.songData._BPM);
        BaseManager.instance.gameCnt += 1;

        SceneController.instance.GotoScene(Enums_Common.SceneType.Game_Single);
        //SceneController.instance.GotoScene(Enums_Common.SceneType.Lobby);
    }

    void ChangeButtonText(Text txt, string colorCode, string text = null)
    {
        if (colorCode != null || text != null)
        {
            ColorUtility.TryParseHtmlString(colorCode, out Color newColor);
            txt.color = newColor;

            if (text != null)
            {
                txt.text = text;
            }
        }
    }

    //private void OnDestroy()
    //{
        //Destroy(GameObject.Find("OrbbecController"));
        //SkeletonController.SkeletonOn -= OrbbecOn;
    //}
    
    public void PointerDown()
    {
        if(b_SceneChange)
        {
            return;
        }

        Image img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        img.name = songDifficult.ToString();
        //Text txt = btn.transform.GetChild(0).GetComponent<Text>();

        switch (img.name)
        {
            case "Difficult":
                //if (txt.text.Equals("EASY"))
                //{
                //    ChangeButtonText(txt, "#E06535");
                //}
                //else if (txt.text.Equals("NORMAL"))
                //{
                //    ChangeButtonText(txt, "#8C90F9");
                //}
                //else if (txt.text.Equals("HARD"))
                //{
                //    ChangeButtonText(txt, "#DC4D66");
                //}
                break;

            case "Easy":
                //img.sprite = difficultPushSprite[0];
                break;

            case "Normal":
                //img.sprite = difficultPushSprite[1];
                break;

            case "Hard":
                //img.sprite = difficultPushSprite[2];
                break;
        }
    }

    public void PointerUp()
    {
        Image img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        //Text txt = btn.transform.GetChild(0).GetComponent<Text>();

        switch (img.name)
        {
            case "Difficult":
                //if (txt.text.Equals("EASY"))
                //{
                //    DataManager.instance.songData._Difficult = SongDifficult.Normal;
                //    ChangeButtonText(txt, "#8CB3F9", "NORMAL");
                //}
                //else if (txt.text.Equals("NORMAL"))
                //{
                //    DataManager.instance.songData._Difficult = SongDifficult.Hard;
                //    ChangeButtonText(txt, "#FF6062", "HARD");
                //}
                //else if (txt.text.Equals("HARD"))
                //{
                //    DataManager.instance.songData._Difficult = SongDifficult.Easy;
                //    ChangeButtonText(txt, "#F7944E", "EASY");
                //}
                break;

            case "Easy":
                SoundManager.instance.PlayDifficult();
                //img.sprite = difficultSprite[1];
                songDifficult = SongDifficult.Normal;
                break;

            case "Normal":
                SoundManager.instance.PlayDifficult();
                //img.sprite = difficultSprite[2];
                songDifficult = SongDifficult.Hard;
                break;

            case "Hard":
                SoundManager.instance.PlayDifficult();
                //img.sprite = difficultSprite[0];
                songDifficult = SongDifficult.Easy;
                break;
        }

        DataManager.instance.songData._Difficult = songDifficult;
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

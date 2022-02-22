using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Enums_Game;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Camera kioskCam;
    [SerializeField] GameObject[] blackObjs;

    [SerializeField] private Animator badgeKioskAnim, badgeScreenAnim;

    [SerializeField] RawImage kioskScreen = null;
    [SerializeField] private RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;

    private string[] goalStr = { "New", "Silver", "Gold", "Diamond", "Ruby" };

    [SerializeField] private Sprite[] badgeSprites;

    [SerializeField] private SpriteRenderer badgeKioskImg;
    [SerializeField] private SpriteRenderer badgeScreenImg;
    [SerializeField] private Text songNameKioskText, songArtistKioskText, songBPMKioskText, songLevelKioskText, goalKioskText, likeKioskText, kioskEndTimeText, kioskContentText;
    [SerializeField] private Text songNameScreenText, songArtistScreenText, songBPMScreenText, songLevelScreenText, goalScreenText, likeScreenText, screenEndTimeText, screenContentText;

    private float endTimer = 0.0f;

    bool b_Change = false;

    // Start is called before the first frame update
    void Start()
    {
        VideoPlayerInit();

        Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);
        }

        if (endTimer > 0.0f)
        {
            endTimer -= Time.deltaTime;

            kioskEndTimeText.text = endTimer.ToString("N0");
            screenEndTimeText.text = endTimer.ToString("N0");
        }
        else if (endTimer <= 0.0f)
        {
            if (!b_Change)
            {
                b_Change = true;

                SceneController.instance.GotoScene(Enums_Common.SceneType.Intro);
            }
        }
    }

    private void Init()
    {
        string songName = DataManager.instance.songData._SongName;
        string songArtist = DataManager.instance.songData._Artist;
        string bpm = DataManager.instance.songData._BPM.ToString();
        int score = DataManager.instance.songData._Score;
        int goalNumber = DataManager.instance.songData._Rank;
        Sprite cd = Resources.Load<Sprite>(string.Format("Song/{0}/{1}_Left", songName, songName));

        songNameKioskText.text = songName;
        songNameScreenText.text = songName;
        songArtistKioskText.text = songArtist;
        songArtistScreenText.text = songArtist;
        songBPMKioskText.text = string.Format("BPM : {0}", bpm);
        songBPMScreenText.text = string.Format("BPM : {0}", bpm);
        goalKioskText.text = string.Format("{0}", goalStr[goalNumber]);
        goalScreenText.text = string.Format("{0}", goalStr[goalNumber]);
        likeKioskText.text = string.Format("♥{0}", score);
        likeScreenText.text = string.Format("♥{0}", score);

        if(goalStr[goalNumber].Equals("Diamond"))
        {
            badgeKioskAnim.SetBool("Gold", true);
            badgeScreenAnim.SetBool("Gold", true);
        }
        else if(goalStr[goalNumber].Equals("New"))
        {
            badgeKioskAnim.SetBool("Start", true);
            badgeScreenAnim.SetBool("Start", true);
        }
        else
        {
            badgeKioskAnim.SetBool(goalStr[goalNumber], true);
            badgeScreenAnim.SetBool(goalStr[goalNumber], true);
        }

        //likeScreenText.text = string.Format("♥{0}개를 받았지만\n{1}등급으로 올라가지 못했어요", score, goalStr[DataManager.instance.gameData._Goal]);

        if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Easy))
        {
            songLevelKioskText.text = string.Format("LEVEL : {0}", "Easy");
            songLevelScreenText.text = string.Format("LEVEL : {0}", "Easy");
        }
        else if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Hard))
        {
            songLevelKioskText.text = string.Format("LEVEL : {0}", "Hard");
            songLevelScreenText.text = string.Format("LEVEL : {0}", "Hard");
        }

        if(DataManager.instance.resultState.Equals(ResultState.LifeEnd))
        {
            kioskContentText.text = "게임이 종료되었습니다";
            screenContentText.text = "게임이 종료되었습니다";
        }
        else if(DataManager.instance.resultState.Equals(ResultState.ScoreEnd))
        {
            kioskContentText.text = string.Format("'{0}'등급으로 올라가지 못했어요 다음에 다시 도전해주세요!", goalStr[DataManager.instance.gameData._Goal]);
            screenContentText.text = string.Format("'{0}'등급으로 올라가지 못했어요 다음에 다시 도전해주세요!", goalStr[DataManager.instance.gameData._Goal]);
        }

        badgeKioskImg.sprite = badgeSprites[goalNumber];
        badgeScreenImg.sprite = badgeSprites[goalNumber];

        endTimer = DataManager.instance.gameData._EndTime;
    }

    #region Video
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
    #endregion
}

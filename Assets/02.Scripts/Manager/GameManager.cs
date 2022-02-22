using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DATA;
using Enums_Game;

[System.Serializable]
public class previewObjArray
{
    public Sprite[] previews;
}

public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.None;

    public delegate void SongStartEvent();
    public event SongStartEvent SongStart;

    private static GameManager _instance;
    //VideoHandler videoHandler;

    [SerializeField] RectTransform segmentRT;
    [SerializeField] ParticleSystem previewPs;
    private ParticleSystem.MainModule psMain;
    [SerializeField] GameObject sensorPanel, sensorReady, sensorFinish, tutorial, blackWall,
        blackImg, readyUI, completeUI, blackCanvas;
    [SerializeField] GameObject[] firstGameObjs, maps, characters, charactersMid;
    GameObject character, map, characterMid;

    AudioClip clip;
    [SerializeField] AudioSource music;
    [SerializeField] Sync sync;
    [SerializeField] Sheet sheet;
    Note leftNote;
    Note rightNote;

    float deltaTime = 0.0f;

    private string audioFileName;
    [HideInInspector] public int comboCnt, noteNumber, perfectCnt, goodCnt, missCnt = 0;
    private int previewNum, songBPM, songOffset, gameRank = 0;
    private int bpmCnt = 1;

    private const float slidingTime = 5.0f;
    private float desiredNumber, initialNumber, currentNumber, musicTime = 0.0f;

    [HideInInspector] public int sensorCnt, chatCnt = 0;
    [HideInInspector] public bool b_MusicStart, b_Press, b_Goal = false;

    #region stop, preview, bpm
    public Transform previewSpawn;
    [SerializeField] previewObjArray[] previewSprites;
    [SerializeField] Image previewImg;
    private RectTransform previewTrans;
    [SerializeField] GameObject[] stopObjs;
    [SerializeField] Transform previewPosi;
    [SerializeField] Image[] stopImgs;
    [SerializeField] RectTransform[] stopOriPosi;
    [SerializeField] RectTransform[] stopImgPosi;

    private float songSPB4, songSPB8, songSPB16, songSPB32, checkMusicTime4, checkMusicTime8, checkMusicTime16;
    [HideInInspector] public int noteCnt = 0;
    #endregion

    [HideInInspector] public float gameCircleVal = 1.0f;

    public Vector3 camPosi;
    [SerializeField] private Transform cameraMain, heartSpawn, heartKioskSpawn;
    [SerializeField] public Transform bigEffectSpawn;
    Transform center;

    private bool b_BPM = false;

    #region UI
    [SerializeField] private Camera kioskCam;
    [SerializeField] private Animator heartAnim;
    [SerializeField] private GameObject blackObj, blackFinishObj, badgeChangeEffect;
    [SerializeField] private GameObject[] goalObjs;
    [SerializeField] private GameObject[] rankEffectObjs;

    [SerializeField] private Transform[] rankEffects;

    [SerializeField] private Sprite[] difficultSprites;
    [SerializeField] private Sprite[] badgeSprites;

    [SerializeField] private SpriteRenderer badgeImg;
    [SerializeField] private Animator badgeAnim;
    [SerializeField] private Image difficultImg, kioskCDImg, gaugeInImg, gaugeOutImg;
    [SerializeField] private RawImage kioskScreen = null;
    [SerializeField] private RawImage screenScreen = null;

    [SerializeField] private VideoPlayer kioskVideoPlayer = null;
    [SerializeField] private VideoPlayer screenVideoPlayer = null;

    [SerializeField] private ScrollRect chatScroll;
    [SerializeField] private Slider songSlider;
    [SerializeField] private Slider badgeSlider;
    [SerializeField] private Slider[] likeSliders;

    [SerializeField] private Text songNameTxt, songArtistTxt, bpmTxt, likeTxt, screenLikeTxt, screenGoalTxt;
    [SerializeField] private Text[] likeGoalTexts;

    private string[] goalStr = { "New", "Silver", "Gold", "Diamond", "Ruby" };
    private int silverLikeValue, goldLikeValue, diaLikeValue, rubyLikeValue = 0;
    private bool b_New = true;
    private bool b_Silver, b_Gold, b_Diamond, b_Ruby = false;
    #endregion

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        VideoPlayerInit();
        StartCoroutine(Init());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = BaseManager.instance.GetPooledObject_TouchEffect(kioskCam);
            obj.SetActive(true);
        }
        //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(!maps[0].activeInHierarchy)
            {
                for(int i = 0; i < maps.Length; i++)
                {
                    if(maps[i].activeInHierarchy)
                    {
                        maps[i].SetActive(false);

                        break;
                    }
                }

                maps[0].SetActive(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!maps[1].activeInHierarchy)
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    if (maps[i].activeInHierarchy)
                    {
                        maps[i].SetActive(false);
                        
                        break;
                    }
                }

                maps[1].SetActive(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!maps[2].activeInHierarchy)
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    if (maps[i].activeInHierarchy)
                    {
                        maps[i].SetActive(false);

                        break;
                    }
                }

                maps[2].SetActive(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!maps[3].activeInHierarchy)
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    if (maps[i].activeInHierarchy)
                    {
                        maps[i].SetActive(false);
                        
                        break;
                    }
                }

                maps[3].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!maps[4].activeInHierarchy)
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    if (maps[i].activeInHierarchy)
                    {
                        maps[i].SetActive(false);

                        break;
                    }
                }

                maps[4].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            gameState = GameState.End;
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            GameObject obj = PooledManager.instance.GetPooledObject_ChatBox();

            // null 비교시 속도 우수
            if (!object.ReferenceEquals(obj, null))
            {
                chatCnt += 1;
                obj.SetActive(true);
            }
            //chatScroll.verticalNormalizedPosition = 1;

        }

        GameStateFunc();

        //if(b_BlackWall)
        //{
        //    blackWall.color = new Color(0, 0, 0, blackWall.color.a + 0.2f * Time.deltaTime);

        //    if(blackWall.color.a >= 0.65f)
        //    {
        //        b_BlackWall = false;
        //    }
        //}
    }

    //private void FixedUpdate()
    //{
    //    if(gameState.Equals(GameState.Play))
    //    {
    //        musicTime = music.time * 1000;

    //        if (!b_BPM)
    //        {
    //            if (musicTime >= songOffset)
    //            {
    //                checkMusicTime = music.time;
    //                b_BPM = true;
    //            }
    //        }
    //        else
    //        {
    //            previewImg.transform.Translate(Vector2.up * Time.deltaTime);

    //            if (music.time >= checkMusicTime + songSPB)
    //            {
    //                bpmCnt++;

    //                //ScreenCapture.CaptureScreenshot(string.Format("{0}{1}{2}{3}.png", Application.dataPath, "/StreamingAssets/", "ScreenShot/", bpmCnt));
    //                checkMusicTime += songSPB;
    //            }
    //        }
    //    }
    //}

    IEnumerator Init()
    {
        songBPM = DataManager.instance.songData._BPM;
        songOffset = DataManager.instance.songData._Offset;

        yield return new WaitForSeconds(0.1f);

        audioFileName = DataManager.instance.songData._AudioFileName;
        camPosi = cameraMain.position;
        psMain = previewPs.main;

        // nuitrack
        //EOSkeletonController.SkeletonOn += SkeletonController_SkeletonOn;

        clip = Resources.Load<AudioClip>(string.Format("Song/{0}/{1}", audioFileName, audioFileName));

        maps[Random.Range(0, maps.Length)].SetActive(true);

        for(int i = 0; i < DataManager.instance.songList.Count; i++)
        {
            if(characters[i].name.Equals(audioFileName))
            {
                previewNum = i;

                character = Instantiate(characters[i]);
                character.SetActive(false);

                characterMid = Instantiate(charactersMid[i]);
                characterMid.SetActive(false);

                break;
            }
        }

        for (int i = 0; i < DataManager.instance.songList.Count; i++)
        {
            if(maps[i].name.Equals(audioFileName))
            {
                map = Instantiate(maps[i], GameObject.Find(audioFileName).transform);
                map.SetActive(false);

                break;
            }
        }
        
        music.clip = clip;

        SongStart();

        noteNumber = sheet.leftHandList.Count + sheet.rightHandList.Count;
        //Debug.Log("noteNumber : " + noteNumber);

        gameRank = 0;
        
        songSPB4 = 60.0f / songBPM;
        songSPB8 = (60.0f / songBPM) * 2.0f;
        songSPB16 = (60.0f / songBPM) * 4.0f;
        songSPB32 = (60.0f / songBPM) * 8.0f;

        DataManager.instance.songData._SPB4 = songSPB4;
        DataManager.instance.songData._SPB8 = songSPB8;
        DataManager.instance.songData._SPB16 = songSPB16;
        DataManager.instance.songData._SPB32 = songSPB32;
        
        previewTrans = previewImg.rectTransform;

        SetKioskUI();
        StartCoroutine(SetSensor());
    }

    public void AddToNumber(float value)
    {
        if (Random.Range(0, 5).Equals(0))
        {
            GameObject obj = PooledManager.instance.GetPooledObject_ChatBox();

            // null 비교시 속도 우수
            if (!object.ReferenceEquals(obj, null))
            {
                chatCnt += 1;
                obj.SetActive(true);
            }
            else
            {
                PooledManager.instance.poolObjs_ChatBox[PooledManager.instance.poolObjs_ChatBox.Count - 1].transform.SetAsLastSibling();
            }

            //chatScroll.verticalNormalizedPosition = 1;
        }

        initialNumber = currentNumber;
        desiredNumber += value;
    }

    Vector3 badgeEffectOffset = new Vector3(0.5f, 0.5f, 1);
    void GameStateFunc()
    {
        switch (gameState)
        {
            case GameState.None:
                break;

            case GameState.Sensor:
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (sensorCnt != -1)
                    {
                        sensorCnt = 3;
                    }
                }

                if (sensorCnt.Equals(3))
                {
                    sensorCnt = -1;
                    StartCoroutine(StartSong());
                }
                break;

            case GameState.Wait:
                break;

            case GameState.Tutorial:

                break;

            case GameState.UI:
                //previewPosi = previewImg.rectTransform.position;
                // 가비지 컬렉션 강제
                System.GC.Collect();

                blackWall.SetActive(true);
                badgeAnim.SetBool("Start", true);
                //readyUI.SetActive(true);
                gameState = GameState.Wait;
                break;

            case GameState.Play:

                //if (!b_Press)
                //{
                Vector3 vec = cameraMain.position;

                vec.x = Mathf.Lerp(cameraMain.position.x, center.position.x, Time.deltaTime);
                //vec.x = center.position.x;

                cameraMain.position = vec;
                //}

                musicTime = music.time * 1000;

                songSlider.value = music.time;

                GenerateBPM();
                ActiveNote();

                if (badgeAnim.GetCurrentAnimatorStateInfo(0).IsName("change_badge_all") && badgeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                {
                    badgeChangeEffect.SetActive(false);
                    badgeAnim.ResetTrigger("Change");
                }

                //Debug.Log(string.Format("Time : {0}", music.time));
                //Debug.Log(string.Format("TimeSample : {0}", music.timeSamples));
                if (currentNumber != desiredNumber)
                {
                    if (initialNumber < desiredNumber)
                    {
                        currentNumber += (slidingTime * Time.deltaTime) * (desiredNumber - initialNumber);

                        if (currentNumber >= desiredNumber)
                        {
                            currentNumber = desiredNumber;
                        }
                    }
                    else
                    {
                        currentNumber -= (slidingTime * Time.deltaTime) * (initialNumber - desiredNumber);

                        if (currentNumber <= desiredNumber)
                        {
                            currentNumber = desiredNumber;
                        }
                    }

                    if (b_New)
                    {
                        likeSliders[0].value = currentNumber;
                        badgeSlider.value = currentNumber;
                        
                        if (currentNumber >= silverLikeValue)
                        {
                            badgeChangeEffect.SetActive(true);
                            badgeAnim.SetTrigger("Change");
                            badgeAnim.SetBool("Silver", true);

                            goalObjs[0].SetActive(true);
                            goalObjs[1].SetActive(false);
                            goalObjs[2].SetActive(false);
                            goalObjs[3].SetActive(true);

                            rankEffectObjs[0].SetActive(false);
                            rankEffectObjs[1].SetActive(true);

                            likeGoalTexts[1].text = string.Format("다음 등급까지 {0} 좋아요!", goldLikeValue);
                            screenGoalTxt.color = ParseStringColor("#A4A4A4");
                            screenLikeTxt.color = ParseStringColor("#A4A4A4");
                            gaugeInImg.color = ParseStringColor("#A4A4A4");
                            gaugeOutImg.color = ParseStringColor("#808080");
                            screenGoalTxt.text = goalStr[1];
                            badgeImg.sprite = badgeSprites[0];
                            badgeSlider.value = 0;
                            badgeSlider.maxValue = goldLikeValue - silverLikeValue;

                            psMain.startColor = ParseStringColor("#C3C3C3"); // preview 이펙트 색 변경

                            gameRank = 1;
                            b_New = false;
                            b_Silver = true;
                        }
                    }
                    else if (b_Silver)
                    {
                        float f = currentNumber - silverLikeValue;

                        likeSliders[1].value = f;
                        badgeSlider.value = f;

                        if (currentNumber >= goldLikeValue)
                        {
                            goalObjs[2].SetActive(true);
                            goalObjs[3].SetActive(false);
                            goalObjs[4].SetActive(false);
                            goalObjs[5].SetActive(true);

                            rankEffectObjs[1].SetActive(false);
                            rankEffectObjs[2].SetActive(true);

                            badgeChangeEffect.SetActive(true);
                            badgeAnim.SetTrigger("Change");
                            badgeAnim.SetBool("Silver", false);
                            badgeAnim.SetBool("Gold", true);
                            
                            likeGoalTexts[2].text = string.Format("다음 등급까지 {0} 좋아요!", diaLikeValue);
                            screenGoalTxt.color = ParseStringColor("#FFC44E");
                            screenLikeTxt.color = ParseStringColor("#FFC44E");
                            gaugeInImg.color = ParseStringColor("#FFC44E");
                            gaugeOutImg.color = ParseStringColor("#D48423");
                            screenGoalTxt.text = goalStr[2];
                            badgeImg.sprite = badgeSprites[1];
                            badgeSlider.value = 0;
                            badgeSlider.maxValue = diaLikeValue - goldLikeValue;

                            psMain.startColor = ParseStringColor("#FFB300");

                            gameRank = 2;
                            b_Silver = false;
                            b_Gold = true;
                        }
                    }
                    else if (b_Gold)
                    {
                        float f = currentNumber - goldLikeValue;

                        likeSliders[2].value = f;
                        badgeSlider.value = f;

                        if (currentNumber >= diaLikeValue)
                        {
                            goalObjs[4].SetActive(true);
                            goalObjs[5].SetActive(false);
                            goalObjs[6].SetActive(false);
                            goalObjs[7].SetActive(true);

                            rankEffectObjs[2].SetActive(false);
                            rankEffectObjs[3].SetActive(true);

                            badgeChangeEffect.SetActive(true);
                            badgeAnim.SetTrigger("Change");
                            
                            likeGoalTexts[3].text = string.Format("다음 등급까지 {0} 좋아요!", rubyLikeValue);
                            screenGoalTxt.color = ParseStringColor("#0FF3D7");
                            screenLikeTxt.color = ParseStringColor("#0FF3D7");
                            gaugeInImg.color = ParseStringColor("#0FF3D7");
                            gaugeOutImg.color = ParseStringColor("#2C1684");
                            screenGoalTxt.text = goalStr[3];
                            badgeImg.sprite = badgeSprites[2];
                            badgeSlider.value = 0;
                            badgeSlider.maxValue = rubyLikeValue - diaLikeValue;

                            psMain.startColor = ParseStringColor("#00FF79");

                            gameRank = 3;
                            b_Gold = false;
                            b_Diamond = true;
                        }
                    }
                    else if (b_Diamond)
                    {
                        float f = currentNumber - diaLikeValue;

                        likeSliders[3].value = f;
                        badgeSlider.value = f;

                        if (currentNumber >= rubyLikeValue)
                        {
                            goalObjs[6].SetActive(true);
                            goalObjs[7].SetActive(false);
                            goalObjs[8].SetActive(false);
                            goalObjs[9].SetActive(true);

                            rankEffectObjs[3].SetActive(false);
                            rankEffectObjs[4].SetActive(true);

                            badgeChangeEffect.SetActive(true);
                            badgeAnim.SetTrigger("Change");
                            badgeAnim.SetBool("Gold", false);
                            badgeAnim.SetBool("Ruby", true);

                            badgeImg.sprite = badgeSprites[3];
                            screenGoalTxt.color = ParseStringColor("#F30F25");
                            screenLikeTxt.color = ParseStringColor("#F30F25");
                            gaugeInImg.color = ParseStringColor("#F30F25");
                            gaugeOutImg.color = ParseStringColor("#B20D31");
                            screenGoalTxt.text = goalStr[4];

                            psMain.startColor = ParseStringColor("#FF0045");

                            gameRank = 4;
                            b_Diamond = false;
                            b_Ruby = true;
                        }
                    }

                    string str = currentNumber.ToString("N0");

                    screenLikeTxt.text = string.Format("♥{0}", str);
                    likeTxt.text = str;
                }

                if (!b_MusicStart && music.time > 0)
                {
                    b_MusicStart = true;
                }

                if (b_MusicStart && music.time.Equals(0))
                {
                    gameState = GameState.End;
                }
                break;

            case GameState.End:
                StartCoroutine(EndSong());
                gameState = GameState.Wait;
                break;
        }
    }

    public void SetRankEffect()
    {
        if(b_Ruby)
        {
            return;
        }

        Vector3 v = Vector3.zero;

        v.x += (gaugeInImg.fillAmount * 0.5f);
        v.y += (gaugeInImg.fillAmount * 0.5f);

        if (b_New)
        {
            rankEffects[0].localScale = v + badgeEffectOffset;
        }
        else if(b_Silver)
        {
            rankEffects[1].localScale = v + badgeEffectOffset;
        }
        else if (b_Gold)
        {
            rankEffects[2].localScale = v + badgeEffectOffset;
        }
        else if (b_Diamond)
        {
            rankEffects[3].localScale = v + badgeEffectOffset;
        }
    }

    void GenerateBPM()
    {
        if (!b_BPM)
        {
            if (musicTime >= songOffset)
            {
                heartAnim.SetBool("Play", true);
                checkMusicTime4 = music.time;
                checkMusicTime8 = music.time;
                checkMusicTime16 = music.time;

                b_BPM = true;

                // stop note
                //for (int i = 0; i < stopObjs.Length; i++)
                //{
                //    stopObjs[i].SetActive(true);
                //}
            }
        }
        else
        {
            if (music.time >= checkMusicTime4 + songSPB4)
            {
                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(previewSpawn, "PreviewEffect");
                obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                obj.SetActive(true);

                //SoundManager.instance.PlayBPM();
                checkMusicTime4 += songSPB4;
            }

            if (previewSprites[previewNum].previews.Length > bpmCnt)
            {
                previewTrans.Translate(Vector2.left * 0.2f * Time.deltaTime);
                
                if (music.time >= checkMusicTime8 + songSPB8)
                {
                    //GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(previewSpawn, "PreviewEffect");
                    //obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    //obj.SetActive(true);

                    previewImg.sprite = previewSprites[previewNum].previews[bpmCnt];

                    // stop note
                    //for (int i = 0; i < stopImgs.Length; i++)
                    //{
                    //    stopImgs[i].sprite = previewSprites8[bpmCnt];
                    //    stopImgPosi[i].position = stopOriPosi[i].position;
                    //}

                    //previewImg.sprite = previewSprites8[bpmCnt];
                    bpmCnt++;

                    previewTrans.position = previewPosi.position;
                    //ScreenCapture.CaptureScreenshot(string.Format("{0}{1}{2}{3}.png", Application.dataPath, "/StreamingAssets/", "ScreenShot/", bpmCnt));
                    checkMusicTime8 += songSPB8;
                }
            }
            else
            {
                if(previewImg.enabled)
                {
                    previewImg.enabled = false;

                    for (int i = 0; i < stopObjs.Length; i++)
                    {
                        stopObjs[i].SetActive(false);
                    }
                }    
            }
        }
    }

    void ActiveNote()
    {
        //Debug.Log("time : " + music.time);
        if (DataManager.instance.songData._Difficult == SongDifficult.Easy)
        {
            if (!sheet.leftHandList.Count.Equals(0))
            {
                if (leftNote != null)
                {
                    //if (musicTime >= (leftNote.noteTime + 500) - 1200)
                    if (musicTime >= (leftNote.noteTime + 200) - 2200)
                    {
                        noteCnt++;
                        sheet.leftHandList[0].SetActive(true);
                        sheet.leftHandList.Remove(sheet.leftHandList[0]);
                        leftNote = null;

                        if(noteCnt.Equals(10))
                        {
                            noteCnt = 0;
                        }
                    }

                    //if (music.time * 1000 >= leftNote.noteTime - 800)
                    //{
                    //    //Debug.Log(string.Format("[{0}] , [{1}]", music.time * 1000, sheet.leftHandList[0].GetComponent<Note>().noteTime - 1000));
                    //    sheet.leftHandList[0].SetActive(true);
                    //    sheet.leftHandList.Remove(sheet.leftHandList[0]);
                    //    //Debug.Log("Remove");
                    //    //Debug.Log("Count : " + sheet.leftHandList.Count);
                    //    leftNote = null;
                    //}
                }
                else
                {
                    leftNote = sheet.leftHandList[0].GetComponent<Note>();
                }
            }

            if (!sheet.rightHandList.Count.Equals(0))
            {
                if (rightNote != null)
                {
                    if (musicTime >= (rightNote.noteTime + 200) - 2200)
                    {
                        noteCnt++;
                        //Debug.Log(string.Format("[{0}] , [{1}]", music.time * 1000, sheet.leftHandList[0].GetComponent<Note>().noteTime - 1000));
                        sheet.rightHandList[0].SetActive(true);
                        sheet.rightHandList.Remove(sheet.rightHandList[0]);
                        //Debug.Log("Remove");
                        //Debug.Log("Count : " + sheet.leftHandList.Count);
                        rightNote = null;

                        if (noteCnt.Equals(10))
                        {
                            noteCnt = 0;
                        }
                    }
                }
                else
                {
                    rightNote = sheet.rightHandList[0].GetComponent<Note>();
                }
            }
        }
        else if(DataManager.instance.songData._Difficult == SongDifficult.Hard)
        {
            if (!sheet.leftHandList.Count.Equals(0))
            {
                if (leftNote != null)
                {
                    if (musicTime >= (leftNote.noteTime + 500) - 1200)
                    {
                        noteCnt++;
                        sheet.leftHandList[0].SetActive(true);
                        sheet.leftHandList.Remove(sheet.leftHandList[0]);
                        leftNote = null;

                        if (noteCnt.Equals(10))
                        {
                            noteCnt = 0;
                        }
                    }

                    //if (music.time * 1000 >= leftNote.noteTime - 800)
                    //{
                    //    //Debug.Log(string.Format("[{0}] , [{1}]", music.time * 1000, sheet.leftHandList[0].GetComponent<Note>().noteTime - 1000));
                    //    sheet.leftHandList[0].SetActive(true);
                    //    sheet.leftHandList.Remove(sheet.leftHandList[0]);
                    //    //Debug.Log("Remove");
                    //    //Debug.Log("Count : " + sheet.leftHandList.Count);
                    //    leftNote = null;
                    //}
                }
                else
                {
                    leftNote = sheet.leftHandList[0].GetComponent<Note>();
                }
            }

            if (!sheet.rightHandList.Count.Equals(0))
            {
                if (rightNote != null)
                {
                    if (musicTime >= (rightNote.noteTime + 500) - 1200)
                    {
                        noteCnt++;
                        //Debug.Log(string.Format("[{0}] , [{1}]", music.time * 1000, sheet.leftHandList[0].GetComponent<Note>().noteTime - 1000));
                        sheet.rightHandList[0].SetActive(true);
                        sheet.rightHandList.Remove(sheet.rightHandList[0]);
                        //Debug.Log("Remove");
                        //Debug.Log("Count : " + sheet.leftHandList.Count);
                        rightNote = null;

                        if (noteCnt.Equals(10))
                        {
                            noteCnt = 0;
                        }
                    }
                }
                else
                {
                    rightNote = sheet.rightHandList[0].GetComponent<Note>();
                }
            }
        }
    }

    // nuitrack
    //private void SkeletonController_SkeletonOn()
    //{
    //    StartCoroutine(SetSensor());
    //    //StartCoroutine(StartSong());
    //}

    //void SetBackGround()
    //{
        //backGroundNumber = Random.Range(0, backGroundSprite.Length);

        //backGroundImage.sprite = backGroundSprite[backGroundNumber];
        //Debug.Log(string.Format("Song/{0}/{1}_Bg", audioFileName, audioFileName));
        ////backGround.clip = Resources.Load<VideoClip>(string.Format("Song/{0}/{1}_Bg", audioFileName, audioFileName));
        //videoHandler.VideoPlayerInit();
    //}

    IEnumerator SetSensor()
    {
        yield return new WaitForSeconds(0.5f);

        blackCanvas.SetActive(false);
        for(int i = 0; i < firstGameObjs.Length; i++)
        {
            firstGameObjs[i].SetActive(true);
        }

        yield return new WaitForSeconds(1.0f);

        gameState = GameState.Sensor;
    }

    Vector2 previewSegSize = new Vector2(763.4f, 429.4f);
    Vector3 previewSegPosi = new Vector3(344.0f, -324.8f, 0.0f);
    IEnumerator StartSong()
    {
        //clip = Resources.Load<AudioClip>(string.Format("Song/{0}/{0}", songName, songName));
        //music.clip = clip;
        //SongStart();

        //Debug.Log("frequency : " + music.clip.frequency);
        //Debug.Log("length : " + music.clip.length);

        sensorReady.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.PlaySensor();

        segmentRT.anchoredPosition = previewSegPosi;
        segmentRT.sizeDelta = previewSegSize;
        //segmentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 656.6f);
        //segmentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 369.4f);

        sensorFinish.SetActive(true);
        character.SetActive(true);
        characterMid.SetActive(true);
        map.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        center = GameObject.FindGameObjectWithTag("Center").transform;
        blackImg.SetActive(false);
        screenScreen.enabled = false;
        StopScreenVideoPlayer();
        sensorPanel.SetActive(false);
        blackWall.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gameState = GameState.UI;

        //gameState = GameState.Tutorial;
        //music.Play();
        //videoHandler.PlayVideo();
    }

    IEnumerator EndSong()
    {
        //blackWall.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        //PlayerPrefs.SetString("Score", currentNumber.ToString("N0"));

        DataManager.instance.songData._Score = (int)currentNumber;

        //if (maxCombo.Equals(noteNumber))
        //{
        //    DataManager.instance.songData._FullCombo = true;
        //}
        //else
        //{
        //    DataManager.instance.songData._FullCombo = false;
        //}

        DataManager.instance.songData._Rank = gameRank;

        //DataManager.instance.songData._Good = goodCnt;
        //DataManager.instance.songData._Perfect = perfectCnt;
        //DataManager.instance.songData._Miss = missCnt;
        //DataManager.instance.songData._Combo = maxCombo;

        blackFinishObj.SetActive(true);
        completeUI.SetActive(true);
        SoundManager.instance.PlayGameFinish();
        //startBarAnim.SetTrigger("End");
    }

    public void SetScore(Note note, Transform notePosi)
    {
        float good = 0;
        float perfect = 0;

        if (note.b_Good)
        {
            good = 1;

            //if(!b_Fever)
            //{
            //    SetGage(-0.02f);
            //}
            //else if(b_Fever)
            //{
            //    good *= 1.5f;
            //}

            AddToNumber(good);
            comboCnt += 1;
            goodCnt += 1;
            GameObject objt = PooledManager.instance.GetPooledObject_GameText(notePosi, "GOOD");
            objt.SetActive(true);
        }
        else if (note.b_Perfect)
        {
            perfect = 1;

            //if(!b_Fever)
            //{
            //    SetGage(-0.04f);
            //}
            //else if (b_Fever)
            //{
            //    perfect *= 1.5f;
            //}

            AddToNumber(perfect);
            comboCnt += 1;
            perfectCnt += 1;
            GameObject objt = PooledManager.instance.GetPooledObject_GameText(notePosi, "PERFECT");
            objt.SetActive(true);
        }
    }

    //public void SetGage(float value)
    //{
    //    if (!b_Fever)
    //    {
    //        gameCircleVal += value;

    //        if (gameCircleVal >= 1.0f)
    //        {
    //            gameCircleVal = 1.0f;
    //        }
    //        else if (gameCircleVal <= 0 || (100 - (gameCircleVal * 100f)).ToString("N0").Equals("100"))
    //        {
    //            gameCircleVal = 0;
    //            gageCircleMat.SetFloat("_ClipUvUp", gameCircleVal);
    //            feverEffect.SetActive(true);
    //            b_Fever = true;
    //        }

    //        //gageText.text = string.Format("{0}%", (100 - (gameCircleVal * 100f)).ToString("N0"));
    //        gageCircleMat.SetFloat("_ClipUvUp", gameCircleVal);
    //    }
    //    else
    //    {
    //        gameCircleVal += (0.1f * Time.deltaTime);
    //        gageCircleMat.SetFloat("_ClipUvUp", gameCircleVal);
    //        //gageText.text = string.Format("{0}%", (100 - (gameCircleVal * 100f)).ToString("N0"));

    //        if (gameCircleVal >= 1.0f)
    //        {
    //            b_Fever = false;
    //            feverEffect.SetActive(false);
    //            //gageText.text = "0%";
    //            gameCircleVal = 1.0f;
    //        }
    //    }
    //}

    /// <summary>
    /// #값 색을 Color로 변환
    /// </summary>
    /// <param name="str">string Color값</param>
    public Color ParseStringColor(string str)
    {
        ColorUtility.TryParseHtmlString(str, out Color color);

        return color;
    }

    public void MoveCamera(bool b, Transform posi = null)
    {
        if(b)
        {
            if(cameraMain.position.z <= -32.5f)
            {
                cameraMain.position = Vector3.MoveTowards(cameraMain.position, new Vector3(posi.position.x, posi.position.y, posi.position.z), Time.deltaTime * 0.5f);
            }
            //cameraMain.position = Vector3.MoveTowards(cameraMain.position, new Vector3(cameraMain.position.x, cameraMain.position.y, 1.0f), 0.3f);
        }
        else
        {
            //cameraMain.position = Vector3.MoveTowards(cameraMain.position, camPosi, Time.deltaTime);
            cameraMain.position = camPosi;
        }
    }

    #region Kiosk UI
    void SetKioskUI()
    {
        //silverLikeValue = Mathf.RoundToInt(maxLikeValue * 0.25f);
        //goldLikeValue = Mathf.RoundToInt(maxLikeValue * 0.5f);
        //diaLikeValue = Mathf.RoundToInt(maxLikeValue * 0.75f);

        if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Easy))
        {
            difficultImg.sprite = difficultSprites[0];
        }
        else if(DataManager.instance.songData._Difficult.Equals(SongDifficult.Hard))
        {
            difficultImg.sprite = difficultSprites[1];
        }

        silverLikeValue = DataManager.instance.gameData._Silver;
        likeSliders[0].maxValue = silverLikeValue;

        goldLikeValue = DataManager.instance.gameData._Gold;
        likeSliders[1].maxValue = goldLikeValue - silverLikeValue;

        diaLikeValue = DataManager.instance.gameData._Diamond;
        likeSliders[2].maxValue = diaLikeValue - goldLikeValue;

        rubyLikeValue = DataManager.instance.gameData._Ruby;
        likeSliders[3].maxValue = rubyLikeValue - diaLikeValue;

        songSlider.maxValue = music.clip.length;
        badgeSlider.maxValue = silverLikeValue;

        likeGoalTexts[0].text = string.Format("다음 등급까지 {0} 좋아요!", silverLikeValue);
        screenGoalTxt.text = goalStr[0];
        //screenContentText.text = string.Format("'{0}'등급에 도달하면 무료로 1곡을 플레이 할 수 있어요", goalStr[DataManager.instance.gameData._Goal]);

        kioskCDImg.sprite = Resources.Load<Sprite>(string.Format("Song/{0}/{1}_Left", audioFileName, audioFileName));
        songNameTxt.text = DataManager.instance.songData._SongName;
        songArtistTxt.text = DataManager.instance.songData._Artist;
        bpmTxt.text = string.Format("{0} {1}","BPM", DataManager.instance.songData._BPM);
    }

    public void SpawnHeart()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject obj = PooledManager.instance.GetPooledObject_HeartEffect(heartSpawn, "HeartEffectScreen");

            if (!object.ReferenceEquals(obj, null))
            {
                obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                obj.SetActive(true);
            }

            obj = PooledManager.instance.GetPooledObject_HeartEffect(heartKioskSpawn, "HeartEffectKiosk");

            if (!object.ReferenceEquals(obj, null))
            {
                obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                obj.SetActive(true);
            }
        }
    }

    public void LikeButton()
    {
        if(gameState.Equals(GameState.Play))
        {
            SpawnHeart();

            SetRankEffect();
            AddToNumber(1);
        }
    }
    #endregion

    #region Background Video
    void VideoPlayerInit()
    {
        if (kioskScreen != null&& kioskVideoPlayer != null)
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
            blackObj.SetActive(false);

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
            blackObj.SetActive(false);

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

    //private void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;

    //    GUIStyle style = new GUIStyle();

    //    Rect rect = new Rect(0, 0, w, h * 2 / 100);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 2 / 100;
    //    style.normal.textColor = Color.white;
    //    float msec = deltaTime * 1000.0f;
    //    float fps = 1.0f / deltaTime;
    //    string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    //    GUI.Label(rect, text, style);
    //}

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();

        // nuitrack
        //Destroy(GameObject.Find("OrbbecController"));
        //EOSkeletonController.SkeletonOn -= SkeletonController_SkeletonOn;
    }
}

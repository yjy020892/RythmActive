using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class BackGroundVideo : MonoBehaviour
{
    VideoHandler videoHandler;

    public List<string> topVideoList, middleVideoList, leftVideoList, rightVideoList = new List<string>();
    public List<float> topVideoTimeList, middleVideoTimeList, leftVideoTimeList, rightVideoTimeList = new List<float>();

    public AudioSource song;

    public Text timeTxt;
    public Toggle firstMap, secondMap;
    GameObject stage;
    public GameObject[] stages;
    GameObject[] stageLights;
    public Animator uiPanelAnim;

    private string line;
    string audioFileName = string.Empty;

    public Dropdown startDropDown;

    string[] dropDownSongs = {
        "BBoom BBoom",
        "Gee",
        "Kissing You",
        "Mr.Chu",
        "Next Level",
        "SHAKE IT",
        "±îÅ»·¹³ª",
        "¸¶¹ý¼Ò³à",
        "¿À´ÃºÎÅÍ ¿ì¸®´Â"
        };

    private int topNum, middleNum, leftNum, rightNum = 0;
    private int lightNum = 0;

    private float playTime = 0.0f;

    public bool b_Play = false;

    private static BackGroundVideo _instance;

    public static BackGroundVideo instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BackGroundVideo>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("Manager", typeof(BackGroundVideo));
                    _instance = obj.GetComponent<BackGroundVideo>();
                }
            }

            return _instance;
        }
    }

    public void StartDropDown(Dropdown select)
    {
        string songName = select.options[select.value].text;
        //Debug.Log(str);

        for (int i = 0; i < dropDownSongs.Length; i++)
        {
            if (songName.Equals(dropDownSongs[i]))
            {
                audioFileName = songName;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BetterStreamingAssets.Initialize();

        videoHandler = GetComponent<VideoHandler>();

        startDropDown.onValueChanged.AddListener((delegate {
            StartDropDown(startDropDown);
        }));

        audioFileName = "BBoom BBoom";
        stage = stages[0];

        //LoadVideo();
    }

    //void LoadVideo()
    //{
    //    if(BetterStreamingAssets.DirectoryExists("Video/TopVideo"))
    //    {
    //        string[] path = GetStreamingAssetFileList();

    //        //videoHandler.topVideoPlayer.url = path[0];
    //        videoHandler.topVideoPlayer.url = Application.streamingAssetsPath +"/" + path[0];
    //    }

    //    //topVideoClips = Resources.LoadAll<VideoClip>("Video/TopVideo");
    //}

    public void LoadVideoData()
    {
        ResetList();
        b_Play = false;

        string pathBasic = Application.dataPath + "/StreamingAssets/";
        string path = string.Empty;
        string[] textSplit;
        line = string.Empty;

        path = "Video/TopVideo/";
        using (StreamReader file = new StreamReader(pathBasic + path + "TopList.ini"))
        {
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(";") || string.IsNullOrEmpty(line))
                {
                    continue;
                }

                textSplit = line.Split(',');
                
                topVideoList.Add(string.Format("{0}{1}{2}", pathBasic, path, textSplit[0]));
                topVideoTimeList.Add(float.Parse(textSplit[1]));
            }

            file.Close();
            line = string.Empty;
        }

        path = "Video/MiddleVideo/";
        using (StreamReader file = new StreamReader(pathBasic + path + "MiddleList.ini"))
        {
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(";") || string.IsNullOrEmpty(line))
                {
                    continue;
                }

                textSplit = line.Split(',');

                middleVideoList.Add(string.Format("{0}{1}{2}", pathBasic, path, textSplit[0]));
                middleVideoTimeList.Add(float.Parse(textSplit[1]));
            }

            file.Close();
            line = string.Empty;
        }

        path = "Video/LeftVideo/";
        using (StreamReader file = new StreamReader(pathBasic + path + "LeftList.ini"))
        {
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(";") || string.IsNullOrEmpty(line))
                {
                    continue;
                }

                textSplit = line.Split(',');

                leftVideoList.Add(string.Format("{0}{1}{2}", pathBasic, path, textSplit[0]));
                leftVideoTimeList.Add(float.Parse(textSplit[1]));
            }

            file.Close();
            line = string.Empty;
        }

        path = "Video/RightVideo/";
        using (StreamReader file = new StreamReader(pathBasic + path + "RightList.ini"))
        {
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(";") || string.IsNullOrEmpty(line))
                {
                    continue;
                }

                textSplit = line.Split(',');

                rightVideoList.Add(string.Format("{0}{1}{2}", pathBasic, path, textSplit[0]));
                rightVideoTimeList.Add(float.Parse(textSplit[1]));
            }

            file.Close();
            line = string.Empty;
        }
    }

    List<Dictionary<string, object>> lightData;

    void GetLightData()
    {
        string path = "Video/LightData.csv";

        lightData = CSVReader.Read(path);

        stageLights = GameObject.FindGameObjectsWithTag("LED");

        //ColorUtility.TryParseHtmlString(lightData[0]["Color"].ToString(), out color);

        //for(int i = 0; i < stageLights.Length; i++)
        //{
        //    MeshRenderer mr = stageLights[i].GetComponent<MeshRenderer>();
        //    Material mat = mr.material;
        //    mat.SetColor("_EmissiveColor", color);
        //    mat.SetInt("_UseEmissiveIntensity", 0);
        //}

        //for (int i = 0; i < lightData.Count; i++)
        //{
        //    Debug.Log("Number : " + lightData[i]["Number"] +
        //        " Color : " + lightData[i]["Color"] +
        //        " Time : " + lightData[i]["Time"] +
        //        " Light : " + lightData[i]["Light"]);
        //}
    }

    void LightControl()
    {
        if (lightData.Count > lightNum)
        {
            if (float.Parse(lightData[lightNum]["Time"].ToString()) <= playTime)
            {
                Color color;

                //if (!string.IsNullOrEmpty(lightData[lightNum]["Color"].ToString()))
                //{

                ColorUtility.TryParseHtmlString(lightData[lightNum]["Color"].ToString(), out color);

                

                for (int i = 0; i < stageLights.Length; i++)
                {
                    MeshRenderer mr = stageLights[i].GetComponent<MeshRenderer>();
                    Material mat = mr.material;

                    if(!string.IsNullOrEmpty(lightData[lightNum]["Color"].ToString()))
                    {
                        //Debug.Log(color);

                        //mat.SetColor("_EmissionColor", color * 100);
                        mat.SetColor("_EmissiveColor", color * 100);
                        //mat.SetColor("_EmissiveColorLDR", color * 100);
                    }
                    else
                    {
                        mat.SetColor("_EmissiveColor", Color.black * 100);
                    }

                    switch (lightData[lightNum]["Light"].ToString())
                    {
                        case "on":
                            mat.SetInt("_UseEmissiveIntensity", 1);
                            //mat.SetFloat("_EmissiveExposureWeight", 0.0f);
                            break;

                        case "off":
                            mat.SetInt("_UseEmissiveIntensity", 0);
                            //mat.SetFloat("_EmissiveExposureWeight", 1.0f);
                            break;
                    }

                    mat.EnableKeyword("_EMISSION");
                    mat.EnableKeyword("_EMISSIVE_COLOR_MAP");
                }
                //}

                lightNum++;
            }
        }
        
    }

    void ResetList()
    {
        topVideoList.Clear();
        middleVideoList.Clear();
        leftVideoList.Clear();
        rightVideoList.Clear();

        topVideoTimeList.Clear();
        middleVideoTimeList.Clear();
        leftVideoTimeList.Clear();
        rightVideoTimeList.Clear();

        if(videoHandler.topVideoPlayer)
        {
            videoHandler.topVideoPlayer.Stop();
        }
        if (videoHandler.middleVideoPlayer)
        {
            videoHandler.middleVideoPlayer.Stop();
        }
        if (videoHandler.leftVideoPlayer)
        {
            videoHandler.leftVideoPlayer.Stop();
        }
        if (videoHandler.rightVideoPlayer)
        {
            videoHandler.rightVideoPlayer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(song.isPlaying)
        {
            timeTxt.text = string.Format("{0}{1}","SongTime : ", song.time.ToString("N1"));
        }

        if (b_Play)
        {
            playTime += Time.deltaTime;

            LightControl();

            if (videoHandler.topVideoPlayer.isPlaying)
            {
                if(videoHandler.topVideoControl.topVideoTimer >= topVideoTimeList[topNum])
                {
                    videoHandler.topVideoControl.topVideoTimer = 0.0f;

                    if (topVideoList.Count - 1 > topNum)
                    {
                        topNum++;
                    }
                    else if((topVideoList.Count - 1).Equals(topNum))
                    {
                        topNum = 0;
                    }

                    videoHandler.topVideoPlayer.url = topVideoList[topNum];
                    videoHandler.topVideoPlayer.Play();
                }
            }

            if (videoHandler.middleVideoPlayer.isPlaying)
            {
                if (videoHandler.middleVideoControl.middleVideoTimer >= middleVideoTimeList[middleNum])
                {
                    videoHandler.middleVideoControl.middleVideoTimer = 0.0f;

                    if (middleVideoList.Count - 1 > middleNum)
                    {
                        middleNum++;
                    }
                    else if ((middleVideoList.Count - 1).Equals(middleNum))
                    {
                        middleNum = 0;
                    }

                    videoHandler.middleVideoPlayer.url = middleVideoList[middleNum];
                    videoHandler.middleVideoPlayer.Play();
                }
            }

            if (videoHandler.leftVideoPlayer.isPlaying)
            {
                if (videoHandler.leftVideoControl.leftVideoTimer >= leftVideoTimeList[leftNum])
                {
                    videoHandler.leftVideoControl.leftVideoTimer = 0.0f;

                    if (leftVideoList.Count - 1 > leftNum)
                    {
                        leftNum++;
                    }
                    else if ((leftVideoList.Count - 1).Equals(leftNum))
                    {
                        leftNum = 0;
                    }

                    videoHandler.leftVideoPlayer.url = leftVideoList[leftNum];
                    videoHandler.leftVideoPlayer.Play();
                }
            }

            if (videoHandler.rightVideoPlayer.isPlaying)
            {
                if (videoHandler.rightVideoControl.rightVideoTimer >= rightVideoTimeList[rightNum])
                {
                    videoHandler.rightVideoControl.rightVideoTimer = 0.0f;

                    if (rightVideoList.Count - 1 > rightNum)
                    {
                        rightNum++;
                    }
                    else if ((rightVideoList.Count - 1).Equals(rightNum))
                    {
                        rightNum = 0;
                    }

                    videoHandler.rightVideoPlayer.url = rightVideoList[rightNum];
                    videoHandler.rightVideoPlayer.Play();
                }
            }
        }
        //ControlVideo();
    }

    void ControlVideo()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    videoHandler.topVideoPlayer.clip = topVideoClips[UnityEngine.Random.Range(0, topVideoClips.Length)];
        //}

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!videoHandler.topVideoPlayer.isPlaying)
            {
                videoHandler.PlayVideo(videoHandler.topVideoPlayer);
            }
            else
            {
                videoHandler.StopVideo(videoHandler.topVideoPlayer);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!videoHandler.leftVideoPlayer.isPlaying && !videoHandler.rightVideoPlayer.isPlaying)
            {
                videoHandler.PlayVideo(videoHandler.leftVideoPlayer);
                videoHandler.PlayVideo(videoHandler.rightVideoPlayer);
            }
            else
            {
                videoHandler.StopVideo(videoHandler.leftVideoPlayer);
                videoHandler.StopVideo(videoHandler.rightVideoPlayer);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!videoHandler.leftVideoPlayer.isPlaying)
            {
                videoHandler.PlayVideo(videoHandler.leftVideoPlayer);
            }
            else
            {
                videoHandler.StopVideo(videoHandler.leftVideoPlayer);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!videoHandler.rightVideoPlayer.isPlaying)
            {
                videoHandler.PlayVideo(videoHandler.rightVideoPlayer);
            }
            else
            {
                videoHandler.StopVideo(videoHandler.rightVideoPlayer);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!videoHandler.middleVideoPlayer.isPlaying)
            {
                videoHandler.PlayVideo(videoHandler.middleVideoPlayer);
            }
            else
            {
                videoHandler.StopVideo(videoHandler.middleVideoPlayer);
            }
        }
    }

    public void ToggleEvent()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;

        switch(btn.name)
        {
            case "Stage1Toggle":
                stage = stages[0];
                break;

            case "Stage2Toggle":
                stage = stages[1];
                break;
        }
    }

    public void ButtonEvent()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;

        switch (btn.name)
        {
            case "Load":
                LoadVideoData();
                
                if (firstMap.isOn)
                {
                    videoHandler.SetStage(stage);

                    videoHandler.topVideoPlayer.url = topVideoList[0];
                    videoHandler.middleVideoPlayer.url = middleVideoList[0];
                    videoHandler.leftVideoPlayer.url = leftVideoList[0];
                    videoHandler.rightVideoPlayer.url = rightVideoList[0];
                }

                GetLightData();
                break;

            case "VideoStart":
                if(!b_Play)
                {
                    videoHandler.topVideoPlayer.Play();
                    videoHandler.middleVideoPlayer.Play();
                    videoHandler.leftVideoPlayer.Play();
                    videoHandler.rightVideoPlayer.Play();

                    b_Play = true;
                }
                break;

            case "SongStart":
                song.clip = Resources.Load<AudioClip>(string.Format("Song/{0}/{1}", audioFileName, audioFileName));

                song.Play();
                break;

            case "SongPause":
                song.Pause();
                break;

            case "UIArrow":
                if(uiPanelAnim.GetCurrentAnimatorStateInfo(0).IsName("UIPanelIn"))
                {
                    uiPanelAnim.SetBool("IN", false);
                }
                else if(uiPanelAnim.GetCurrentAnimatorStateInfo(0).IsName("UIPanelOut"))
                {
                    uiPanelAnim.SetBool("IN", true);
                }
                
                break;
        }
    }

    //string[] GetStreamingAssetFileList(string filepath = null)
    //{
    //    string[] paths = BetterStreamingAssets.GetFiles("Video/TopVideo", "*.mp4", SearchOption.AllDirectories);

    //    for(int i = 0; i < paths.Length; i++)
    //    {
    //        Debug.Log(paths[i]);
    //    }

    //    return paths;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class CreateControl : MonoBehaviour
{
    [SerializeField] private Transform cameraMain;
    Transform center;

    public List<string> noteList = new List<string>();
    [HideInInspector] public List<Vector2> fingerPositionLeft;
    [HideInInspector] public List<Vector2> fingerPositionRight;

    public Animator anim;
    AnimatorStateInfo animationState;
    public AudioSource audioSource;
    public AudioClip song;
    public AudioClip[] dropDownSongs;

    public Transform characterTR;
    public Transform leftHand;
    public Transform rightHand;
    public Transform canvasPos;

    [Space()]
    public Text timeText;
    public Text realTimeText;
    public Text animFrameText;
    public Text positionXText;
    public Text positionYText;
    public Text positionZText;
    public Text RotationYText;

    public InputField speedInputField;
    public InputField timeSamplesInputField;
    public InputField animFrameInputField;
    public InputField positionXInputField;
    public InputField positionYInputField;
    public InputField positionZInputField;
    public InputField rotationYInputField;

    public Dropdown songDropDown;

    [Space()]
    public GameObject linePinkPrefab;
    public GameObject lineBluePrefab;
    public GameObject currentLineLeft;
    public GameObject currentLineRight;
    public LineRenderer lineRendererLeft;
    public LineRenderer lineRendererRight;

    [Space()]
    public Vector3 characterRot;

    Vector2 leftHandPosi;
    Vector2 rightHandPosi;

    string songName;
    string lineStartTime;
    string leftHandLineStr;
    string rightHandLineStr;
    string leftPressStr;
    string rightPressStr;

    private int timeSamples;
    private int timeSamplesValue;
    private int timeSamplesSave;
    private float speed = 1.0f;
    private float animFrame;
    private double animFrameValue;
    private double animFrameSave;
    private float posiX;
    private float posiXValue;
    private float posiY;
    private float posiYValue;
    private float posiZ;
    private float posiZValue;
    private float rotY;
    private float rotYValue;

    private float time;
    float leftPressTime;
    float rightPressTime;

    private bool b_Pause = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (song != null)
        {
            audioSource.clip = song;
        }

        speedInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(speedInputField.text);
        }));

        timeSamplesInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(timeSamplesInputField.text);
        }));

        animFrameInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(animFrameInputField.text);
        }));
        
        positionXInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(positionXInputField.text);
        }));

        positionYInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(positionYInputField.text);
        }));

        positionZInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(positionZInputField.text);
        }));

        rotationYInputField.onEndEdit.AddListener((delegate {
            InputFieldEndEdit(rotationYInputField.text);
        }));

        songDropDown.onValueChanged.AddListener((delegate {
            SongDropDown(songDropDown);
        }));

        center = GameObject.FindGameObjectWithTag("Center").transform;
    }

    // Update is called once per frame
    void Update()
    {
        SetNote();
    }

    void SetNote()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //anim.Play("K-POP Dance 1", 0, 0.09840505f);
            // 포지션, 각도 값 같이 저장해서 넣어야될듯
            if (b_Pause)
            {
                anim.speed = 1.0f;
                anim.SetFloat("Reverse", speed);
                PlaySong();
                b_Pause = false;
            }
            else
            {
                anim.speed = 0.0f;
                PauseSong();
                b_Pause = true;
            }
        }

        if (audioSource.isPlaying)
        {
            Vector3 vec = cameraMain.position;

            vec.x = Mathf.Lerp(cameraMain.position.x, center.position.x, Time.deltaTime);
            //vec.x = center.position.x;

            cameraMain.position = vec;

            time = audioSource.time;
            timeText.text = string.Format("{0}:{1}", "시간(초,)", time.ToString("N1"));
            timeSamplesValue = audioSource.timeSamples;
            realTimeText.text = string.Format("{0}:{1}", "TimeSamples", timeSamplesValue.ToString());
            //Debug.Log(string.Format("{0}:{1}", "TimeSamples", audioSource.timeSamples.ToString()));
            time *= 1000;

            animationState = anim.GetCurrentAnimatorStateInfo(0);
            //AnimatorClipInfo[] myAnimatorClip = anim.GetCurrentAnimatorClipInfo(0);
            //float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
            //Debug.Log(myTime);
            //Debug.Log(animationState.normalizedTime % 1);
            double animTime = animationState.normalizedTime % 1;
            animFrameValue = Math.Truncate(animTime * 10000) / 10000;

            //Debug.Log(string.Format("{0}:{1}", "animTime", animTime));
            animFrameText.text = string.Format("{0}:{1}", "AnimFrame", animFrameValue);

            posiXValue = characterTR.position.x;
            posiYValue = characterTR.position.y;
            posiZValue = characterTR.position.z;
            positionXText.text = string.Format("{0}:{1}", "PositionX", posiXValue);
            positionYText.text = string.Format("{0}:{1}", "PositionY", posiYValue);
            positionZText.text = string.Format("{0}:{1}", "PositionZ", posiZValue);

            rotYValue = characterTR.rotation.eulerAngles.y;
            RotationYText.text = string.Format("{0}:{1}", "RotationY", rotYValue);
        }

        #region Single Note
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    if (leftHand != null && rightHand != null)
        //    {
        //        string timeStr = time.ToString("N0");
        //        timeStr = timeStr.Replace(",", "");
        //        //timeStr.Replace(",","");
        //        leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
        //        rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
        //        string leftStr = string.Empty;
        //        string rightStr = string.Empty;

        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // ---------------------------------------------------------------------------- 왼손 ----------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------

        //        if (leftHandPosi.x <= 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //            leftHandPosi.x = -(960 - leftHandPosi.x);
        //        }
        //        else if (leftHandPosi.x > 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //            leftHandPosi.x -= 960;
        //        }

        //        leftStr += leftHandPosi.x.ToString("0");
        //        leftStr += "#/";

        //        if (leftHandPosi.y <= 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //            leftHandPosi.y = -(540 - leftHandPosi.y);
        //        }
        //        else if (leftHandPosi.y > 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //            leftHandPosi.y -= 540;
        //        }

        //        leftStr += leftHandPosi.y.ToString("0");
        //        leftStr += "#/";

        //        leftStr += timeStr;
        //        leftStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}", "#/1#/0#/0#/0#/x#/", timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

        //        noteList.Add(leftStr);


        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------- 오른손 ---------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------

        //        if (rightHandPosi.x <= 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //            rightHandPosi.x = -(960 - rightHandPosi.x);
        //        }
        //        else if (rightHandPosi.x > 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //            rightHandPosi.x -= 960;
        //        }

        //        rightStr += rightHandPosi.x.ToString("0");
        //        rightStr += "#/";

        //        if (rightHandPosi.y <= 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //            rightHandPosi.y = -(540 - rightHandPosi.y);
        //        }
        //        else if (rightHandPosi.y > 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //            rightHandPosi.y -= 540;
        //        }

        //        rightStr += rightHandPosi.y.ToString("0");
        //        rightStr += "#/";

        //        rightStr += timeStr;
        //        rightStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}", "#/0#/1#/0#/0#/x#/", timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);


        //        noteList.Add(rightStr);

        //        ExcelDirectSave(noteList);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                //timeStr.Replace(",","");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                string leftStr = string.Empty;

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------
                // ---------------------------------------------------------------------------- 왼손 ----------------------------------------------------------------------------
                // --------------------------------------------------------------------------------------------------------------------------------------------------------------

                if (leftHandPosi.x <= 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                    leftHandPosi.x = -(960 - leftHandPosi.x);
                }
                else if (leftHandPosi.x > 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                    leftHandPosi.x -= 960;
                }

                leftStr += leftHandPosi.x.ToString("0");
                leftStr += "#/";

                if (leftHandPosi.y <= 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                    leftHandPosi.y = -(540 - leftHandPosi.y);
                }
                else if (leftHandPosi.y > 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                    leftHandPosi.y -= 540;
                }

                leftStr += leftHandPosi.y.ToString("0");
                leftStr += "#/";

                leftStr += timeStr;
                leftStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}", "#/1#/0#/0#/0#/x#/", timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

                noteList.Add(leftStr);

                ExcelDirectSave(noteList);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                //timeStr.Replace(",","");
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                string rightStr = string.Empty;

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------
                // --------------------------------------------------------------------------- 오른손 ---------------------------------------------------------------------------
                // --------------------------------------------------------------------------------------------------------------------------------------------------------------

                if (rightHandPosi.x <= 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                    rightHandPosi.x = -(960 - rightHandPosi.x);
                }
                else if (rightHandPosi.x > 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                    rightHandPosi.x -= 960;
                }

                rightStr += rightHandPosi.x.ToString("0");
                rightStr += "#/";

                if (rightHandPosi.y <= 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                    rightHandPosi.y = -(540 - rightHandPosi.y);
                }
                else if (rightHandPosi.y > 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                    rightHandPosi.y -= 540;
                }

                rightStr += rightHandPosi.y.ToString("0");
                rightStr += "#/";

                rightStr += timeStr;
                rightStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}", "#/0#/1#/0#/0#/x#/", timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);


                noteList.Add(rightStr);

                ExcelDirectSave(noteList);
            }
        }
        #endregion

        #region Long Note
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    CreateLine("Both");
        //}
        //if (Input.GetKey(KeyCode.F2))
        //{
        //    leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);

        //    if (leftHandPosi.x <= 960.0f)
        //    {
        //        //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //        leftHandPosi.x = -(960 - leftHandPosi.x);
        //    }
        //    else if (leftHandPosi.x > 960.0f)
        //    {
        //        //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //        leftHandPosi.x -= 960;
        //    }

        //    if (leftHandPosi.y <= 540.0f)
        //    {
        //        //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //        leftHandPosi.y = -(540 - leftHandPosi.y);
        //    }
        //    else if (leftHandPosi.y > 540.0f)
        //    {
        //        //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //        leftHandPosi.y -= 540;
        //    }

        //    Vector2 tempFingerPos = leftHandPosi;
        //    //Debug.Log(string.Format("tempFingerPos : {0}, fingerPositionLeft[fingerPositionLeft.Count - 1] : {1}", tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]));
        //    //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
        //    if (Vector2.Distance(tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]) > .1f)
        //    {
        //        UpdateLine(tempFingerPos, "left");
        //    }

        //    // -----------------------------------------------------------------------------------------------------------------

        //    rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);

        //    if (rightHandPosi.x <= 960.0f)
        //    {
        //        //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //        rightHandPosi.x = -(960 - rightHandPosi.x);
        //    }
        //    else if (rightHandPosi.x > 960.0f)
        //    {
        //        //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //        rightHandPosi.x -= 960;
        //    }

        //    if (rightHandPosi.y <= 540.0f)
        //    {
        //        //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //        rightHandPosi.y = -(540 - rightHandPosi.y);
        //    }
        //    else if (rightHandPosi.y > 540.0f)
        //    {
        //        //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //        rightHandPosi.y -= 540;
        //    }

        //    Vector2 tempFingerPos2 = rightHandPosi;
        //    //Debug.Log(string.Format("tempFingerPos : {0}, fingerPosition[fingerPosition.Count - 1] : {1}", tempFingerPos, fingerPosition[fingerPosition.Count - 1]));
        //    //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
        //    if (Vector2.Distance(tempFingerPos2, fingerPositionRight[fingerPositionRight.Count - 1]) > .1f)
        //    {
        //        UpdateLine(tempFingerPos2, "right");
        //    }
        //}
        //if (Input.GetKeyUp(KeyCode.F2))
        //{
        //    lineRendererLeft.Simplify(1.5f);

        //    for(int i = 0; i < lineRendererLeft.positionCount; i++)
        //    {
        //        if(i.Equals(0))
        //        {
        //            leftHandLineStr += string.Format("{0}_{1}", lineRendererLeft.GetPosition(i).x.ToString("N0"), lineRendererLeft.GetPosition(i).y.ToString("N0"));
        //        }
        //        else
        //        {
        //            leftHandLineStr += string.Format("@{0}_{1}", lineRendererLeft.GetPosition(i).x.ToString("N0"), lineRendererLeft.GetPosition(i).y.ToString("N0"));
        //        }
        //    }

        //    string timeStr = time.ToString("N0");
        //    timeStr = timeStr.Replace(",", "");

        //    leftHandLineStr += "#/";
        //    leftHandLineStr += string.Format("{0}#/{1}", lineStartTime, timeStr); ;
        //    leftHandLineStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}", "#/1#/0#/1#/0#/x#/", timeSamplesSave, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

        //    noteList.Add(leftHandLineStr);

        //    // -----------------------------------------------------------------------------------------------------------------

        //    lineRendererRight.Simplify(1.5f);

        //    for (int i = 0; i < lineRendererRight.positionCount; i++)
        //    {
        //        if (i.Equals(0))
        //        {
        //            rightHandLineStr += string.Format("{0}_{1}", lineRendererRight.GetPosition(i).x.ToString("N0"), lineRendererRight.GetPosition(i).y.ToString("N0"));
        //        }
        //        else
        //        {
        //            rightHandLineStr += string.Format("@{0}_{1}", lineRendererRight.GetPosition(i).x.ToString("N0"), lineRendererRight.GetPosition(i).y.ToString("N0"));
        //        }
        //    }

        //    string timeStr2 = time.ToString("N0");
        //    timeStr2 = timeStr2.Replace(",", "");

        //    rightHandLineStr += "#/";
        //    rightHandLineStr += string.Format("{0}#/{1}{2}{3}#/{4}#/{5}#/{6}#/{7}#/{8}", lineStartTime, timeStr2, "#/0#/1#/1#/0#/x#/", timeSamplesSave, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

        //    noteList.Add(rightHandLineStr);

        //    ExcelDirectSave(noteList);
        //}
        #endregion

        #region Press Note
        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    if (leftHand != null && rightHand != null)
        //    {
        //        string timeStr = time.ToString("N0");
        //        timeStr = timeStr.Replace(",", "");
        //        //timeStr.Replace(",","");
        //        leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
        //        rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
        //        float pressTime = 1.0f;
        //        string leftStr = string.Empty;
        //        string rightStr = string.Empty;

        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // ---------------------------------------------------------------------------- 왼손 ----------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------

        //        if (leftHandPosi.x <= 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //            leftHandPosi.x = -(960 - leftHandPosi.x);
        //        }
        //        else if (leftHandPosi.x > 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //            leftHandPosi.x -= 960;
        //        }

        //        leftStr += leftHandPosi.x.ToString("0");
        //        leftStr += "#/";

        //        if (leftHandPosi.y <= 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //            leftHandPosi.y = -(540 - leftHandPosi.y);
        //        }
        //        else if (leftHandPosi.y > 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //            leftHandPosi.y -= 540;
        //        }

        //        leftStr += leftHandPosi.y.ToString("0");
        //        leftStr += "#/";

        //        leftStr += timeStr;
        //        leftStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/1#/0#/0#/1#/", pressTime.ToString("N1"), timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

        //        noteList.Add(leftStr);


        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------- 오른손 ---------------------------------------------------------------------------
        //        // --------------------------------------------------------------------------------------------------------------------------------------------------------------

        //        if (rightHandPosi.x <= 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //            rightHandPosi.x = -(960 - rightHandPosi.x);
        //        }
        //        else if (rightHandPosi.x > 960.0f)
        //        {
        //            //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //            rightHandPosi.x -= 960;
        //        }

        //        rightStr += rightHandPosi.x.ToString("0");
        //        rightStr += "#/";

        //        if (rightHandPosi.y <= 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //            rightHandPosi.y = -(540 - rightHandPosi.y);
        //        }
        //        else if (rightHandPosi.y > 540.0f)
        //        {
        //            //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //            rightHandPosi.y -= 540;
        //        }

        //        rightStr += rightHandPosi.y.ToString("0");
        //        rightStr += "#/";

        //        rightStr += timeStr;
        //        rightStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/0#/1#/0#/1#/", pressTime.ToString("N1"), timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

        //        noteList.Add(rightStr);

        //        ExcelDirectSave(noteList);


        //        //string timeStr = time.ToString("N0");
        //        //timeStr = timeStr.Replace(",", "");
        //        //leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
        //        //leftPressTime = 0;
        //        //leftPressStr = string.Empty;
        //        //timeSamplesSave = timeSamplesValue;
        //        //animFrameSave = animFrameValue;

        //        //if (leftHandPosi.x <= 960.0f)
        //        //{
        //        //    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //        //    leftHandPosi.x = -(960 - leftHandPosi.x);
        //        //}
        //        //else if (leftHandPosi.x > 960.0f)
        //        //{
        //        //    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //        //    leftHandPosi.x -= 960;
        //        //}

        //        //leftPressStr += leftHandPosi.x.ToString("0");
        //        //leftPressStr += "#/";

        //        //if (leftHandPosi.y <= 540.0f)
        //        //{
        //        //    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //        //    leftHandPosi.y = -(540 - leftHandPosi.y);
        //        //}
        //        //else if (leftHandPosi.y > 540.0f)
        //        //{
        //        //    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //        //    leftHandPosi.y -= 540;
        //        //}

        //        //leftPressStr += leftHandPosi.y.ToString("0");
        //        //leftPressStr += "#/";

        //        //leftPressStr += timeStr;

        //        //// ------------------------------------------------------------------------------------------------------------------------------

        //        //string timeStr2 = time.ToString("N0");
        //        //timeStr2 = timeStr2.Replace(",", "");
        //        //rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
        //        //rightPressTime = 0;
        //        //rightPressStr = string.Empty;

        //        //if (rightHandPosi.x <= 960.0f)
        //        //{
        //        //    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
        //        //    rightHandPosi.x = -(960 - rightHandPosi.x);
        //        //}
        //        //else if (rightHandPosi.x > 960.0f)
        //        //{
        //        //    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
        //        //    rightHandPosi.x -= 960;
        //        //}

        //        //rightPressStr += rightHandPosi.x.ToString("0");
        //        //rightPressStr += "#/";

        //        //if (rightHandPosi.y <= 540.0f)
        //        //{
        //        //    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
        //        //    rightHandPosi.y = -(540 - rightHandPosi.y);
        //        //}
        //        //else if (rightHandPosi.y > 540.0f)
        //        //{
        //        //    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
        //        //    rightHandPosi.y -= 540;
        //        //}

        //        //rightPressStr += rightHandPosi.y.ToString("0");
        //        //rightPressStr += "#/";

        //        //rightPressStr += timeStr2;
        //    }
        //}
        //if (Input.GetKey(KeyCode.F3))
        //{
        //    leftPressTime += Time.deltaTime;
        //    rightPressTime += Time.deltaTime;
        //}
        //if (Input.GetKeyUp(KeyCode.F3))
        //{
        //    leftPressStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/1#/0#/0#/1#/", leftPressTime.ToString("N1"), timeSamplesSave, animFrameSave, posiXValue, posiYValue, posiZValue, rotYValue);

        //    noteList.Add(leftPressStr);

        //    // ------------------------------------------------------------------------------------------------------------------------------

        //    rightPressStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/0#/1#/0#/1#/", rightPressTime.ToString("N1"), timeSamplesSave, animFrameSave, posiXValue, posiYValue, posiZValue, rotYValue);

        //    noteList.Add(rightPressStr);

        //    ExcelDirectSave(noteList);
        //}

        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                //timeStr.Replace(",","");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                float pressTime = 1.0f;
                string leftStr = string.Empty;

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------
                // ---------------------------------------------------------------------------- 왼손 ----------------------------------------------------------------------------
                // --------------------------------------------------------------------------------------------------------------------------------------------------------------

                if (leftHandPosi.x <= 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                    leftHandPosi.x = -(960 - leftHandPosi.x);
                }
                else if (leftHandPosi.x > 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                    leftHandPosi.x -= 960;
                }

                leftStr += leftHandPosi.x.ToString("0");
                leftStr += "#/";

                if (leftHandPosi.y <= 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                    leftHandPosi.y = -(540 - leftHandPosi.y);
                }
                else if (leftHandPosi.y > 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                    leftHandPosi.y -= 540;
                }

                leftStr += leftHandPosi.y.ToString("0");
                leftStr += "#/";

                leftStr += timeStr;
                leftStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/1#/0#/0#/1#/", pressTime.ToString("N1"), timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

                noteList.Add(leftStr);

                ExcelDirectSave(noteList);
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                //timeStr.Replace(",","");
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                float pressTime = 1.0f;
                string rightStr = string.Empty;

                // --------------------------------------------------------------------------------------------------------------------------------------------------------------
                // --------------------------------------------------------------------------- 오른손 ---------------------------------------------------------------------------
                // --------------------------------------------------------------------------------------------------------------------------------------------------------------

                if (rightHandPosi.x <= 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                    rightHandPosi.x = -(960 - rightHandPosi.x);
                }
                else if (rightHandPosi.x > 960.0f)
                {
                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                    rightHandPosi.x -= 960;
                }

                rightStr += rightHandPosi.x.ToString("0");
                rightStr += "#/";

                if (rightHandPosi.y <= 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                    rightHandPosi.y = -(540 - rightHandPosi.y);
                }
                else if (rightHandPosi.y > 540.0f)
                {
                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                    rightHandPosi.y -= 540;
                }

                rightStr += rightHandPosi.y.ToString("0");
                rightStr += "#/";

                rightStr += timeStr;
                rightStr += string.Format("{0}{1}#/{2}#/{3}#/{4}#/{5}#/{6}#/{7}", "#/0#/1#/0#/1#/", pressTime.ToString("N1"), timeSamplesValue, animFrameValue, posiXValue, posiYValue, posiZValue, rotYValue);

                noteList.Add(rightStr);

                ExcelDirectSave(noteList);
            }
        }
        #endregion
    }

    #region LongNote Control
    void CreateLine(string hand)
    {
        leftHandLineStr = string.Empty;
        rightHandLineStr = string.Empty;
        lineStartTime = string.Empty;

        if (currentLineLeft != null)
        {
            Destroy(currentLineLeft);
        }
        if (currentLineRight != null)
        {
            Destroy(currentLineRight);
        }

        if (hand.Equals("Both"))
        {
            timeSamplesSave = timeSamplesValue;
            animFrameSave = animFrameValue;
            currentLineLeft = Instantiate(linePinkPrefab, canvasPos.position, Quaternion.identity);
            currentLineLeft.transform.parent = canvasPos;
            currentLineLeft.transform.localScale = Vector3.one;
            lineRendererLeft = currentLineLeft.GetComponent<LineRenderer>();
            //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
            fingerPositionLeft.Clear();

            leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);

            if (leftHandPosi.x <= 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                leftHandPosi.x = -(960 - leftHandPosi.x);
            }
            else if (leftHandPosi.x > 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                leftHandPosi.x = leftHandPosi.x - 960;
            }

            if (leftHandPosi.y <= 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                leftHandPosi.y = -(540 - leftHandPosi.y);
            }
            else if (leftHandPosi.y > 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                leftHandPosi.y = leftHandPosi.y - 540;
            }

            float time = audioSource.time * 1000;
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");
            lineStartTime = timeStr;

            fingerPositionLeft.Add(leftHandPosi);
            fingerPositionLeft.Add(leftHandPosi);
            lineRendererLeft.SetPosition(0, fingerPositionLeft[0]);
            lineRendererLeft.SetPosition(1, fingerPositionLeft[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            //leftHandLineStr += string.Format("{0}_{1}", leftHandPosi.x.ToString("N0"), leftHandPosi.y.ToString("N0"));

            // -----------------------------------------------------------------------------------------------------------------

            currentLineRight = Instantiate(lineBluePrefab, canvasPos.position, Quaternion.identity);
            currentLineRight.transform.parent = canvasPos;
            currentLineRight.transform.localScale = Vector3.one;
            lineRendererRight = currentLineRight.GetComponent<LineRenderer>();
            //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
            fingerPositionRight.Clear();

            rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);

            if (rightHandPosi.x <= 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                rightHandPosi.x = -(960 - rightHandPosi.x);
            }
            else if (rightHandPosi.x > 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                rightHandPosi.x = rightHandPosi.x - 960;
            }

            if (rightHandPosi.y <= 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                rightHandPosi.y = -(540 - rightHandPosi.y);
            }
            else if (rightHandPosi.y > 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                rightHandPosi.y = rightHandPosi.y - 540;
            }

            float time2 = audioSource.time * 1000;
            string timeStr2 = time2.ToString("N0");
            timeStr2 = timeStr2.Replace(",", "");
            lineStartTime = timeStr2;

            fingerPositionRight.Add(rightHandPosi);
            fingerPositionRight.Add(rightHandPosi);
            lineRendererRight.SetPosition(0, fingerPositionRight[0]);
            lineRendererRight.SetPosition(1, fingerPositionRight[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            //rightHandLineStr += string.Format("{0}_{1}", rightHandPosi.x.ToString("N0"), rightHandPosi.y.ToString("N0"));
        }
    }

    void UpdateLine(Vector2 newFingerPos, string hand)
    {
        if (hand.Equals("left"))
        {
            fingerPositionLeft.Add(newFingerPos);

            lineRendererLeft.positionCount++;
            lineRendererLeft.SetPosition(lineRendererLeft.positionCount - 1, newFingerPos);

            //leftHandLineStr += "@" + string.Format("{0}_{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
        else if (hand.Equals("right"))
        {
            fingerPositionRight.Add(newFingerPos);

            lineRendererRight.positionCount++;
            lineRendererRight.SetPosition(lineRendererRight.positionCount - 1, newFingerPos);

            //rightHandLineStr += "@" + string.Format("{0}_{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
        else if (hand.Equals("Bote"))
        {
            lineRendererLeft.positionCount++;
            lineRendererLeft.SetPosition(lineRendererLeft.positionCount - 1, newFingerPos);

            //leftHandLineStr += "@" + string.Format("{0}_{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));

            // -----------------------------------------------------------------------------------------------------------------

            lineRendererRight.positionCount++;
            lineRendererRight.SetPosition(lineRendererRight.positionCount - 1, newFingerPos);

            //rightHandLineStr += "@" + string.Format("{0}_{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
    }
    #endregion

    public void PlaySong()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PauseSong()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void StopSong()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            timeText.text = "0";
        }
    }

    #region Button
    public void ButtonEvent()
    {
        string str = EventSystem.current.currentSelectedGameObject.name;

        switch(str)
        {
            case "StartButton":
                if(string.IsNullOrEmpty(songName))
                {
                    songName = "BBoom BBoom";
                }

                Vector3 r;

                switch (songName)
                {
                    case "까탈레나":
                        r = characterTR.eulerAngles;
                        r.y = 0;
                        characterTR.eulerAngles = r;
                        break;

                    case "마법소녀":
                        r = characterTR.eulerAngles;
                        r.y = 0;
                        characterTR.eulerAngles = r;
                        break;
                }

                anim.SetBool(songName, true);
                b_Pause = false;
                break;

            case "ResetButton":
                if (String.IsNullOrEmpty(timeSamplesInputField.text) || String.IsNullOrEmpty(animFrameInputField.text) || String.IsNullOrEmpty(positionXInputField.text)
            || String.IsNullOrEmpty(positionYInputField.text) || String.IsNullOrEmpty(positionZInputField.text) || String.IsNullOrEmpty(rotationYInputField.text))
                {
                    return;
                }

                characterTR.SetPositionAndRotation(new Vector3(posiX, posiY, posiZ), Quaternion.Euler(characterTR.rotation.eulerAngles.x, rotY, characterTR.rotation.eulerAngles.z));

                audioSource.timeSamples = timeSamples;
                anim.Play("K-POP Dance 1", 0, animFrame);
                
                //Debug.Log(string.Format("X: {0}, Y: {1}, Z: {2}", posiX, posiY, posiZ));
                //characterTR.localPosition = new Vector3(posiX, posiY, posiZ);
                //characterTR.rotation = Quaternion.Euler(characterTR.rotation.eulerAngles.x, rotY, characterTR.rotation.eulerAngles.z);
                break;

            case "x-1":
                speed = -1.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;

            case "x-2":
                speed = -2.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;

            case "x-3":
                speed = -3.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;

            case "x1":
                speed = 1.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;

            case "x2":
                speed = 2.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;

            case "x3":
                speed = 3.0f;

                anim.SetFloat("Reverse", speed);
                audioSource.pitch = speed;
                break;
        }
    }
    #endregion

    #region DropDown
    public void SongDropDown(Dropdown select)
    {
        string str = select.options[select.value].text;
        //Debug.Log(str);
        
        for(int i = 0; i < dropDownSongs.Length; i++)
        {
            if(str.Equals(dropDownSongs[i].name))
            {
                audioSource.clip = dropDownSongs[i];
                songName = str;
            }
        }
    }
    #endregion

    #region InputField
    public void InputFieldEndEdit(string text)
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;

        if (!string.IsNullOrEmpty(text))
        {
            switch (obj.name)
            {
                case "TimeSamplesInputField":

                    timeSamples = int.Parse(text);

                    break;

                case "AnimFrameInputField":
                    animFrame = float.Parse(text);
                    break;

                case "PositionXInputField":
                    posiX = float.Parse(text);
                    break;

                case "PositionYInputField":
                    posiY = float.Parse(text);
                    break;

                case "PositionZInputField":
                    posiZ = float.Parse(text);
                    break;

                case "RotationYInputField":
                    rotY = float.Parse(text);
                    break;

                case "SpeedInputField":
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        float f = float.Parse(text);

                        if (f >= -3.0f && f <= 3.0f)
                        {
                            speed = f;

                            anim.SetFloat("Reverse", speed);
                            audioSource.pitch = speed;
                        }
                        else
                        {
                            speedInputField.text = string.Empty;
                        }
                        //Debug.Log(int.Parse(text));
                    }
                    break;
            }
        }
    }
    #endregion

    #region File
    public void TxtSave()
    {
        //Debug.Log(string.Format("Path : {0}", Application.streamingAssetsPath));
        if (noteList.Count != 0)
        {
            for (int i = 0; i < noteList.Count; i++)
            {
                string filePath = Path.Combine(string.Format("{0}{1}", Application.streamingAssetsPath, songName), ".txt");
                string message = noteList[i];

                WriteTxt(filePath, message);
            }
        }
    }

    void WriteTxt(string filePath, string message)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        //FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        //StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode,);

        StreamWriter writer = new StreamWriter(filePath, true);

        writer.WriteLine(message);

        writer.Close();
    }

    string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = string.Empty; ;

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
        }
        else
        {
            value = "파일이 없습니다.";
        }

        return value;
    }

    private List<string[]> rowData = new List<string[]>();

    // 즉시저장
    public void ExcelDirectSave(List<string> noteList)
    {
        string[] rowDataTemp;

        // 파일 존재 유무 확인
        if (!File.Exists(getPath()))
        {
            rowDataTemp = new string[14];

            rowDataTemp[0] = "NoteX";
            rowDataTemp[1] = "NoteY";
            rowDataTemp[2] = "NoteTime";
            rowDataTemp[3] = "LeftHand";
            rowDataTemp[4] = "RightHand";
            rowDataTemp[5] = "LongNote";
            rowDataTemp[6] = "PressNote";
            rowDataTemp[7] = "PressTime";
            rowDataTemp[8] = "TimeSamples";
            rowDataTemp[9] = "AnimatorFrame";
            rowDataTemp[10] = "CharacterX";
            rowDataTemp[11] = "CharacterY";
            rowDataTemp[12] = "CharacterZ";
            rowDataTemp[13] = "RotationY";

            rowData.Add(rowDataTemp);
        }

        char split = '#';

        for (int i = 0; i < noteList.Count; i++)
        {
            rowDataTemp = new string[14];

            rowDataTemp = noteList[i].Split(split);
            //Debug.Log(noteList[i].Split(split)[0]);

            rowData.Add(rowDataTemp);
        }

        noteList.Clear(); // 노트 저장값 초기화

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);

        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        //Debug.Log(sb);

        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);

        outStream.WriteLine(sb);

        outStream.Close();
    }

    // 모았다가 저장
    public void ExcelSave()
    {
        string[] rowDataTemp = new string[14];

        rowDataTemp[0] = "NoteX";
        rowDataTemp[1] = "NoteY";
        rowDataTemp[2] = "NoteTime";
        rowDataTemp[3] = "LeftHand";
        rowDataTemp[4] = "RightHand";
        rowDataTemp[5] = "LongNote";
        rowDataTemp[6] = "PressNote";
        rowDataTemp[7] = "PressTime";
        rowDataTemp[8] = "TimeSamples";
        rowDataTemp[9] = "AnimatorFrame";
        rowDataTemp[10] = "CharacterX";
        rowDataTemp[11] = "CharacterY";
        rowDataTemp[12] = "CharacterZ";
        rowDataTemp[13] = "RotationY";

        rowData.Add(rowDataTemp);

        char split = '#';

        for (int i = 0; i < noteList.Count; i++)
        {
            rowDataTemp = new string[14];

            rowDataTemp = noteList[i].Split(split);
            

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);

        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);

        outStream.WriteLine(sb);

        outStream.Close();
    }

    private string getPath()
    {
#if UNITY_EDITOR

        //Debug.Log(Application.dataPath + "/CSV/" + "Saved_note_data.csv");
        //return Application.dataPath + "/CSV/" + "Saved_note_data.csv";
        //return Path.Combine(string.Format("{0}", Application.streamingAssetsPath), "Notedata.csv");
        return Path.Combine(string.Format("{0}", Application.streamingAssetsPath), string.Format("{0}{1}", songName, ".csv"));

#elif UNITY_ANDROID

        return Application.persistentDataPath+"Saved_data.csv";

#elif UNITY_IPHONE

        return Application.persistentDataPath+"/"+"Saved_data.csv";

#else

        //return Application.dataPath +"/"+"Saved_data.csv";
        //return Path.Combine(string.Format("{0}", Application.streamingAssetsPath), "Notedata.csv");
        return Path.Combine(string.Format("{0}", Application.streamingAssetsPath), string.Format("{0}{1}", songName, ".csv"));

#endif
    }
    #endregion
}

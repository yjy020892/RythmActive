using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

/// <summary>
/// cal BPM = Sub BPM + MusicTime
/// cal BPM부터 시작
/// 댄스 누르고 스타트송
/// </summary>

public class NoteCreateManager : MonoBehaviour
{
    Vector2 mousePosition;
    public Animator anim;
    public Camera cam;
    public Transform leftHand;
    public Transform rightHand;
    public AudioSource music;
    [Tooltip("재생될곡")]
    public AudioClip song;

    [Space()][Space()]

    public Text timeText;
    public Text realTimeText;
    public Text beatText;
    public Text beatTimeText;

    [Space()]

    public GameObject linePinkPrefab;
    public GameObject lineBluePrefab;
    public GameObject currentLineLeft;
    public GameObject currentLineRight;
    public LineRenderer lineRendererLeft;
    public LineRenderer lineRendererRight;
    //public EdgeCollider2D edgeCollider;
    [HideInInspector] public List<Vector2> fingerPositionLeft;
    [HideInInspector] public List<Vector2> fingerPositionRight;
    public Transform canvasPos;

    [Space()][Space()][Space()][Space()][Space()]
    List<string> noteList = new List<string>();
    public List<string> lineList = new List<string>();

    [Space()]
    float time;
    public int BPM;
    public int subBPM;
    //[Tooltip("곡시작")]
    //public int Offset;
    public int musicTime = 0;
    [HideInInspector] public int changeMusicTime = 0;

    int offsetBeat = 0;
    float currentBPM = 0;
    public float calBPM = 0;
    Vector2 leftHandPosi;
    Vector2 rightHandPosi;

    string lineStartTime;
    string leftHandLineStr;
    string rightHandLineStr;
    string leftPressStr;
    string rightPressStr;

    Vector3 leftHandLinePosi;
    Vector3 rightHandLinePosi;
    float leftPressTime;
    float rightPressTime;
    float realTimeVal = 0;
    public string timeView = string.Empty;

    [HideInInspector] public bool b_Click, b_Dance, b_Self, b_LeftHand, b_RightHand, b_XMirror, b_YMirror = false;
    bool b_Pause = true;

    void Start()
    {
        //cam = GameObject.Find("Camera_Background").GetComponent<Camera>();
        //music = GetComponent<AudioSource>();

        if(song != null)
        {
            music.clip = song;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetNote();

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if(b_Click)
        //        {
        //            if (!(b_LeftHand && b_RightHand) || !(!b_LeftHand && !b_RightHand))
        //            {
        //                mousePosition = Input.mousePosition;
        //                string handStr = string.Empty;
        //                string mirrorStr = string.Empty;
        //                //mousePosition = cam.ScreenToWorldPoint(mousePosition);
        //                if (mousePosition.x <= 960.0f)
        //                {
        //                    //Debug.Log(string.Format("x : {0}", -(960 - mousePosition.x)));
        //                    handStr += (-(960 - mousePosition.x)).ToString("0");

        //                    if (b_XMirror)
        //                    {
        //                        mirrorStr += (960 - mousePosition.x).ToString("0");
        //                    }
        //                    else if (b_YMirror && !b_XMirror)
        //                    {
        //                        mirrorStr += (-(960 - mousePosition.x)).ToString("0");
        //                    }
        //                }
        //                else if (mousePosition.x > 960.0f)
        //                {
        //                    //Debug.Log(string.Format("x : {0}", mousePosition.x - 960));
        //                    handStr += (mousePosition.x - 960).ToString("0");

        //                    if (b_XMirror)
        //                    {
        //                        mirrorStr += (-(mousePosition.x - 960)).ToString("0");
        //                    }
        //                    if (b_YMirror && !b_XMirror)
        //                    {
        //                        mirrorStr += (-(960 - mousePosition.x)).ToString("0");
        //                    }
        //                }

        //                handStr += ",";
        //                if (b_XMirror || b_YMirror)
        //                {
        //                    mirrorStr += ",";
        //                }

        //                if (mousePosition.y <= 540.0f)
        //                {
        //                    //Debug.Log(string.Format("y: {0}", -(600 - mousePosition.y)));
        //                    handStr += (-(540 - mousePosition.y)).ToString("0");

        //                    if (b_YMirror)
        //                    {
        //                        mirrorStr += (540 - mousePosition.y).ToString("0");
        //                    }
        //                    else if (b_XMirror && !b_YMirror)
        //                    {
        //                        mirrorStr += (-(540 - mousePosition.y)).ToString("0");
        //                    }
        //                }
        //                else if (mousePosition.y > 540.0f)
        //                {
        //                    //Debug.Log(string.Format("y: {0}", mousePosition.y - 600));
        //                    handStr += (mousePosition.y - 540).ToString("0");

        //                    if (b_YMirror)
        //                    {
        //                        mirrorStr += (-(mousePosition.y - 540)).ToString("0");
        //                    }
        //                    else if (b_XMirror && !b_YMirror)
        //                    {
        //                        mirrorStr += (mousePosition.y - 540).ToString("0");
        //                    }
        //                }

        //                handStr += ",";
        //                if (b_XMirror || b_YMirror)
        //                {
        //                    mirrorStr += ",";
        //                }

        //                handStr += musicTime;
        //                if (b_XMirror || b_YMirror)
        //                {
        //                    mirrorStr += musicTime;
        //                }

        //                if (b_LeftHand && !b_RightHand)
        //                {
        //                    handStr += ",1,0,0,0";

        //                    noteList.Add(handStr);

        //                    if (b_XMirror || b_YMirror)
        //                    {
        //                        mirrorStr += ",0,1,0,0";
        //                        noteList.Add(mirrorStr);
        //                    }
        //                }
        //                else if (b_RightHand && !b_LeftHand)
        //                {
        //                    handStr += ",0,1,0,0";

        //                    noteList.Add(handStr);

        //                    if (b_XMirror || b_YMirror)
        //                    {
        //                        mirrorStr += ",1,0,0,0";
        //                        noteList.Add(mirrorStr);
        //                    }
        //                }
        //            }
        //        }
        //    }
    }

    void CreateLine(string hand)
    {
        leftHandLineStr = string.Empty;
        rightHandLineStr = string.Empty;
        lineStartTime = string.Empty;

        if (currentLineLeft != null)
        {
            Destroy(currentLineLeft);
        }
        if(currentLineRight != null)
        {
            Destroy(currentLineRight);
        }

        if(hand.Equals("left"))
        {
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

            float time = music.time * 1000;
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");
            lineStartTime = timeStr;

            fingerPositionLeft.Add(leftHandPosi);
            fingerPositionLeft.Add(leftHandPosi);
            lineRendererLeft.SetPosition(0, fingerPositionLeft[0]);
            lineRendererLeft.SetPosition(1, fingerPositionLeft[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            leftHandLineStr += string.Format("{0},{1}", leftHandPosi.x.ToString("N0"), leftHandPosi.y.ToString("N0"));
        }
        else if(hand.Equals("right"))
        {
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

            float time = music.time * 1000;
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");
            lineStartTime = timeStr;

            fingerPositionRight.Add(rightHandPosi);
            fingerPositionRight.Add(rightHandPosi);
            lineRendererRight.SetPosition(0, fingerPositionRight[0]);
            lineRendererRight.SetPosition(1, fingerPositionRight[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            rightHandLineStr += string.Format("{0},{1}", rightHandPosi.x.ToString("N0"), rightHandPosi.y.ToString("N0"));
        }
        else if(hand.Equals("Both"))
        {
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

            float time = music.time * 1000;
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");
            lineStartTime = timeStr;

            fingerPositionLeft.Add(leftHandPosi);
            fingerPositionLeft.Add(leftHandPosi);
            lineRendererLeft.SetPosition(0, fingerPositionLeft[0]);
            lineRendererLeft.SetPosition(1, fingerPositionLeft[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            leftHandLineStr += string.Format("{0},{1}", leftHandPosi.x.ToString("N0"), leftHandPosi.y.ToString("N0"));

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

            float time2 = music.time * 1000;
            string timeStr2 = time2.ToString("N0");
            timeStr2 = timeStr2.Replace(",", "");
            lineStartTime = timeStr2;

            fingerPositionRight.Add(rightHandPosi);
            fingerPositionRight.Add(rightHandPosi);
            lineRendererRight.SetPosition(0, fingerPositionRight[0]);
            lineRendererRight.SetPosition(1, fingerPositionRight[1]);
            //edgeCollider.points = fingerPosition.ToArray();

            rightHandLineStr += string.Format("{0},{1}", rightHandPosi.x.ToString("N0"), rightHandPosi.y.ToString("N0"));
        }
    }

    void SetNote()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //anim.Play("K-POP Dance 1", 0, 0.09840505f);
            // 포지션, 각도 값 같이 저장해서 넣어야될듯
            if (b_Pause)
            {
                anim.speed = 1.0f;
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

        if (music.isPlaying)
        {
            time = music.time * 1000;
            timeText.text = time.ToString("N0");
            timeView = time.ToString("N0");
            //realTimeVal = realTimeVal + Time.deltaTime;
            //realTimeText.text = realTimeVal.ToString();

            if(Input.GetKeyDown(KeyCode.A))
            {
                AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0);
                AnimatorClipInfo[] myAnimatorClip = anim.GetCurrentAnimatorClipInfo(0);
                float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
                //Debug.Log(myTime);
                Debug.Log(animationState.normalizedTime % 1);
            }
            

            if (b_Dance && !calBPM.Equals(0))
            {
                if (leftHand != null && rightHand != null)
                {
                    if (time >= calBPM)
                    {
                        string timeStr = calBPM.ToString();
                        //timeStr.Replace(",","");
                        leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                        rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                        string leftStr = string.Empty;
                        string rightStr = string.Empty;

                        // --------------------------------------------------------------------------------------------------------------------------------------------------------------
                        // --------------------------------------------------------------------------- 왼손 ---------------------------------------------------------------------------
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
                        leftStr += ",";

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
                        leftStr += ",";

                        leftStr += timeStr;
                        leftStr += ",1,0,0,0";

                        noteList.Add(leftStr);

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
                        rightStr += ",";

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
                        rightStr += ",";

                        rightStr += timeStr;
                        rightStr += ",0,1,0,0";

                        noteList.Add(rightStr);

                        //Debug.Log(string.Format("L : {0} , R : {1}", leftHandPosi, rightHandPosi));
                        calBPM += offsetBeat;
                    }
                }
            }
            else if (b_Self)
            {
                
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                string rightStr = string.Empty;

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
                rightStr += "/";

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
                rightStr += "/";

                rightStr += timeStr;
                rightStr += "/0/1/0/0";

                noteList.Add(rightStr);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                string leftStr = string.Empty;

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
                leftStr += "/";

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
                leftStr += "/";

                leftStr += timeStr;
                leftStr += "/1/0/0/0";

                noteList.Add(leftStr);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                //timeStr.Replace(",","");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                string leftStr = string.Empty;
                string rightStr = string.Empty;

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
                leftStr += "/";

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
                leftStr += "/";

                leftStr += timeStr;
                leftStr += "/1/0/0/0";

                noteList.Add(leftStr);


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
                rightStr += "/";

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
                rightStr += "/";

                rightStr += timeStr;
                rightStr += "/0/1/0/0";

                noteList.Add(rightStr);
            }
        }

        // 왼쪽 롱노트
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CreateLine("left");
        }
        if (Input.GetKey(KeyCode.Z))
        {
            leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);

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

            Vector2 tempFingerPos = leftHandPosi;
            //Debug.Log(string.Format("tempFingerPos : {0}, fingerPositionLeft[fingerPositionLeft.Count - 1] : {1}", tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]));
            //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
            if (Vector2.Distance(tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos, "left");
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");

            leftHandLineStr += "/";
            leftHandLineStr += string.Format("{0},{1}", lineStartTime, timeStr); ;
            leftHandLineStr += "/1/0/1/0";

            noteList.Add(leftHandLineStr);
            //lineList.Add(leftHandLineStr);
        }

        // 오른손 롱노트
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateLine("right");
        }
        if (Input.GetKey(KeyCode.X))
        {
            rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);

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

            Vector2 tempFingerPos = rightHandPosi;
            //Debug.Log(string.Format("tempFingerPos : {0}, fingerPosition[fingerPosition.Count - 1] : {1}", tempFingerPos, fingerPosition[fingerPosition.Count - 1]));
            //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
            if (Vector2.Distance(tempFingerPos, fingerPositionRight[fingerPositionRight.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos, "right");
            }
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");

            rightHandLineStr += "/";
            rightHandLineStr += string.Format("{0},{1}", lineStartTime, timeStr); ;
            rightHandLineStr += "/0/1/1/0";

            noteList.Add(rightHandLineStr);
            //lineList.Add(leftHandLineStr);
        }

        // 양손 롱노트
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateLine("Both");
        }
        if (Input.GetKey(KeyCode.C))
        {
            leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);

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

            Vector2 tempFingerPos = leftHandPosi;
            //Debug.Log(string.Format("tempFingerPos : {0}, fingerPositionLeft[fingerPositionLeft.Count - 1] : {1}", tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]));
            //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
            if (Vector2.Distance(tempFingerPos, fingerPositionLeft[fingerPositionLeft.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos, "left");
            }

            // -----------------------------------------------------------------------------------------------------------------

            rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);

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

            Vector2 tempFingerPos2 = rightHandPosi;
            //Debug.Log(string.Format("tempFingerPos : {0}, fingerPosition[fingerPosition.Count - 1] : {1}", tempFingerPos, fingerPosition[fingerPosition.Count - 1]));
            //Debug.Log(string.Format("tempFingerPos : {0}", tempFingerPos));
            if (Vector2.Distance(tempFingerPos2, fingerPositionRight[fingerPositionRight.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos2, "right");
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            string timeStr = time.ToString("N0");
            timeStr = timeStr.Replace(",", "");

            leftHandLineStr += "/";
            leftHandLineStr += string.Format("{0},{1}", lineStartTime, timeStr); ;
            leftHandLineStr += "/1/0/1/0";

            noteList.Add(leftHandLineStr);

            // -----------------------------------------------------------------------------------------------------------------

            string timeStr2 = time.ToString("N0");
            timeStr2 = timeStr2.Replace(",", "");

            rightHandLineStr += "/";
            rightHandLineStr += string.Format("{0},{1}", lineStartTime, timeStr2); ;
            rightHandLineStr += "/0/1/1/0";

            noteList.Add(rightHandLineStr);
        }

        // 왼손 Press
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                leftPressTime = 0;
                leftPressStr = string.Empty;

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

                leftPressStr += leftHandPosi.x.ToString("0");
                leftPressStr += "/";

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

                leftPressStr += leftHandPosi.y.ToString("0");
                leftPressStr += "/";

                leftPressStr += timeStr;
            }
        }
        if (Input.GetKey(KeyCode.Delete))
        {
            leftPressTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Delete))
        {
            leftPressStr += "/1/0/0/1/";
            leftPressStr += leftPressTime.ToString("N1");

            noteList.Add(leftPressStr);
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                rightPressTime = 0;
                rightPressStr = string.Empty;

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

                rightPressStr += rightHandPosi.x.ToString("0");
                rightPressStr += "/";

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

                rightPressStr += rightHandPosi.y.ToString("0");
                rightPressStr += "/";

                rightPressStr += timeStr;
            }
        }
        if (Input.GetKey(KeyCode.End))
        {
            rightPressTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.End))
        {
            rightPressStr += "/0/1/0/1/";
            rightPressStr += rightPressTime.ToString("N1");

            noteList.Add(rightPressStr);
        }

        // Press 양손
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (leftHand != null && rightHand != null)
            {
                string timeStr = time.ToString("N0");
                timeStr = timeStr.Replace(",", "");
                leftHandPosi = Camera.main.WorldToScreenPoint(leftHand.position);
                leftPressTime = 0;
                leftPressStr = string.Empty;

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

                leftPressStr += leftHandPosi.x.ToString("0");
                leftPressStr += "/";

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

                leftPressStr += leftHandPosi.y.ToString("0");
                leftPressStr += "/";

                leftPressStr += timeStr;

                // ------------------------------------------------------------------------------------------------------------------------------

                string timeStr2 = time.ToString("N0");
                timeStr2 = timeStr2.Replace(",", "");
                rightHandPosi = Camera.main.WorldToScreenPoint(rightHand.position);
                rightPressTime = 0;
                rightPressStr = string.Empty;

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

                rightPressStr += rightHandPosi.x.ToString("0");
                rightPressStr += "/";

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

                rightPressStr += rightHandPosi.y.ToString("0");
                rightPressStr += "/";

                rightPressStr += timeStr2;
            }

        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            leftPressTime += Time.deltaTime;
            rightPressTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            leftPressStr += "/1/0/0/1/";
            leftPressStr += leftPressTime.ToString("N1");

            noteList.Add(leftPressStr);

            // ------------------------------------------------------------------------------------------------------------------------------

            rightPressStr += "/0/1/0/1/";
            rightPressStr += rightPressTime.ToString("N1");

            noteList.Add(rightPressStr);
        }
    }

    void UpdateLine(Vector2 newFingerPos, string hand)
    {
        if(hand.Equals("left"))
        {
            fingerPositionLeft.Add(newFingerPos);

            lineRendererLeft.positionCount++;
            lineRendererLeft.SetPosition(lineRendererLeft.positionCount - 1, newFingerPos);

            leftHandLineStr += "@" + string.Format("{0},{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
        else if(hand.Equals("right"))
        {
            fingerPositionRight.Add(newFingerPos);

            lineRendererRight.positionCount++;
            lineRendererRight.SetPosition(lineRendererRight.positionCount - 1, newFingerPos);

            rightHandLineStr += "@" + string.Format("{0},{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
        else if(hand.Equals("Bote"))
        {
            lineRendererLeft.positionCount++;
            lineRendererLeft.SetPosition(lineRendererLeft.positionCount - 1, newFingerPos);

            leftHandLineStr += "@" + string.Format("{0},{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));

            // -----------------------------------------------------------------------------------------------------------------

            lineRendererRight.positionCount++;
            lineRendererRight.SetPosition(lineRendererRight.positionCount - 1, newFingerPos);

            rightHandLineStr += "@" + string.Format("{0},{1}", newFingerPos.x.ToString("N0"), newFingerPos.y.ToString("N0"));
        }
    }

    public void SetBPM()
    {
        if(currentBPM.Equals(BPM))
        {
            return;
        }

        if(!BPM.Equals(0))
        {
            currentBPM = BPM;
            offsetBeat = Mathf.FloorToInt((60 / currentBPM) * 1000);
        }
        else
        {
            currentBPM = 0;
            offsetBeat = 0;
        }

        beatText.text = offsetBeat.ToString();
    }

    public void PlusBeat()
    {
        musicTime += offsetBeat;
        beatTimeText.text = musicTime.ToString();
    }

    public void MinusBeat()
    {
        musicTime -= offsetBeat;
        
        if (musicTime < 0)
        {
            musicTime = 0;
        }

        beatTimeText.text = musicTime.ToString();
    }

    public void SetSubBPM()
    {
        offsetBeat = subBPM;
        beatText.text = offsetBeat.ToString();
    }

    public void StartSong()
    {
        if(music.clip != null && !music.isPlaying && !b_Dance)
        {
            music.Play();
        }
        else if(music.clip != null && !music.isPlaying && b_Dance)
        {
            anim.SetInteger("Dance", 1);
            b_Pause = false;
            calBPM = offsetBeat + musicTime;
        }
    }

    public void PlaySong()
    {
        if (!music.isPlaying)
        {
            music.Play();
        }
    }

    public void PauseSong()
    {
        if (music.isPlaying)
        {
            music.Pause();
        }
    }

    public void StopSong()
    {
        if(music.isPlaying)
        {
            music.Stop();
            timeText.text = "0";
        }
    }

    public void SaveList()
    {
        //Debug.Log(string.Format("Path : {0}", Application.streamingAssetsPath));
        if(b_LeftHand && b_RightHand && noteList.Count != 0)
        {
            for (int i = 0; i < noteList.Count; i++)
            {
                string filePath = Path.Combine(string.Format("{0}", Application.streamingAssetsPath), "Notedata.txt");
                string message = noteList[i];

                WriteTxt(filePath, message);
            }
        }
    }

    public void ClearList()
    {
        if (noteList.Count != 0)
        {
            noteList.Clear();
        }
    }

    public void RemoveList()
    {
        if (noteList.Count != 0)
        {
            noteList.Remove(noteList[noteList.Count - 1]);
        }
    }

    public void SetMusicTime()
    {
        beatTimeText.text = musicTime.ToString();
    }

    public void PlusMusicTime()
    {
        if(b_Dance)
        {
            return;
        }

        musicTime += changeMusicTime;
        beatTimeText.text = musicTime.ToString();
    }

    public void MinusMusicTime()
    {
        if (b_Dance)
        {
            return;
        }

        musicTime -= changeMusicTime;

        if(musicTime < 0)
        {
            musicTime = 0;
        }

        beatTimeText.text = musicTime.ToString();
    }

    void WriteTxt(string filePath, string message)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if(!directoryInfo.Exists)
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

        if(fileInfo.Exists)
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

    void Save()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "Name";
        rowDataTemp[1] = "ID";
        rowDataTemp[2] = "Income";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < 10; i++)
        {
            rowDataTemp = new string[3];

            rowDataTemp[0] = "Sushanta" + i; // name
            rowDataTemp[1] = "" + i; // ID
            rowDataTemp[2] = "$" + UnityEngine.Random.Range(5000, 10000); // Income

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

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR

        Debug.Log(Application.dataPath + "/CSV/" + "Saved_data.csv");
        return Application.dataPath + "/CSV/" + "Saved_data.csv";

#elif UNITY_ANDROID

        return Application.persistentDataPath+"Saved_data.csv";

#elif UNITY_IPHONE

        return Application.persistentDataPath+"/"+"Saved_data.csv";

#else

        return Application.dataPath +"/"+"Saved_data.csv";

#endif
    }
}

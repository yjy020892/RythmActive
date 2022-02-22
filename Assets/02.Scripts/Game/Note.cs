using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums_Game;
using System;

public class Note : MonoBehaviour
{
    public int posiX;
    public int posiY;
    public int noteTime;
    public int longNoteEndTime;
    public NoteBody noteBody;
    public NoteType noteType;

    public GameObject pinkTailEffect, blueTailEffect, pinkPressEffect, bluePressEffect, pinkPressBeforeEffect, bluePressBeforeEffect;

    public Transform leftObj;
    public Transform rightObj;
    public Transform lineEnd;
    Vector2 leftPosi;
    Vector2 rightPosi;

    RectTransform rt;
    public CircleNote circleNote;

    Vector2 singleBrokenSize = new Vector2(200.0f, 200.0f);
    public Animator singleAnim;
    Animator anim;
    SphereCollider sc;
    LineRenderer lr;
    Vector3 lrStartPosition, lrLastPosition;

    Transform tr;
    public RectTransform markRT;
    public Image outerImg;
    public Image markImg;
    public Image innerImg;
    public Text numberTxt;
    public bool b_Fade;
    bool b_LongMove = false;
    [HideInInspector] public bool b_Perfect, b_Good, b_Ready, b_Long, b_Press = false;

    [Header("Object Sprite")]
    public Sprite[] outlineColor;
    public Sprite[] objectColor;
    public Sprite[] objectInnerColor;

    public string[] longNotePosi;

    Vector3 vec;
    Transform imgTransform;
    
    float noteTimer = 0.0f;
    [HideInInspector] public float pressTime, pressStartTime, pressOntime = 0.0f;

    int longCnt = 0;
    int perfect = 1000;
    int good = 300;
    //public Vector3 viewToWorldPosi;

    void Start()
    {
        TryGetComponent(out rt);
        TryGetComponent(out tr);
        TryGetComponent(out sc);
        TryGetComponent(out anim);
        if (TryGetComponent(out lr))
        {
            rt = outerImg.GetComponent<RectTransform>();
            sc = outerImg.GetComponent<SphereCollider>();
            outerImg.tag = gameObject.tag;
            circleNote.noteType = noteType;

            lr.positionCount = longNotePosi.Length;

            for (int i = 0; i < longNotePosi.Length; i++)
            {
                float.TryParse(longNotePosi[i].Split(',')[0], out vec.x);
                float.TryParse(longNotePosi[i].Split(',')[1], out vec.y);

                lr.SetPosition(i, vec);
                
                if(i.Equals(0))
                {
                    lrStartPosition = vec;
                }
                else if(i.Equals(longNotePosi.Length - 1))
                {
                    lrLastPosition = vec;
                }
            }

            //float percentSize = (float)(0.4f / Vector3.Distance(lrStartPosition, lrLastPosition));
            //float percentSize = 0.15f;
            
            lr.Simplify(1);

            //Debug.Log(string.Format("{0}", percentSize));
            //Debug.Log(string.Format("{0},{1}", lrStartPosition, lrLastPosition));

            //lr.widthCurve = new AnimationCurve(
            //    new Keyframe(0, 0.4f),
            //    new Keyframe(0.999f - percentSize, 0.4f),
            //    //new Keyframe(1 - percentSize, 0.4f),
            //    new Keyframe(1 - percentSize, 1f),
            //    new Keyframe(1, 0f));

            outerImg.transform.localPosition = lr.GetPosition(0);

            //lr.SetPositions(new Vector3[]
            //    {
            //        lrStartPosition,
            //        Vector3.Lerp(lrStartPosition, lrLastPosition, 0.999f - percentSize),
            //        Vector3.Lerp(lrStartPosition, lrLastPosition, 1f - percentSize),
            //        lrLastPosition
            //    });

            lineEnd.localPosition = lr.GetPosition(lr.positionCount - 1);
            imgTransform = outerImg.transform;
        }

        //viewToWorldPosi = Camera.main.ViewportToWorldPoint(transform.position);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(GameManager.instance.gameState.Equals(GameState.Play) && noteType.Equals(NoteType.Press))
        {
            numberTxt.text = GameManager.instance.noteCnt.ToString();

            if (gameObject.CompareTag("HandLeft"))
            {
                pinkPressBeforeEffect.SetActive(true);
            }
            else if(gameObject.CompareTag("HandRight"))
            {
                bluePressBeforeEffect.SetActive(true);
            }
        }

        if(GameManager.instance.gameState.Equals(GameState.Play) && noteType.Equals(NoteType.Single))
        {
            numberTxt.text = GameManager.instance.noteCnt.ToString();

            // 애니매이션
            //if (gameObject.CompareTag("HandLeft"))
            //{
            //    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "SingleRedBornEffect");
            //    obj.SetActive(true);
            //    singleAnim.SetBool("RedNote", true);
            //}
            //else if (gameObject.CompareTag("HandRight"))
            //{
            //    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "SingleBlueBornEffect");
            //    obj.SetActive(true);
            //    singleAnim.SetBool("BlueNote", true);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_Ready)
        {
            SetReady();
        }
        else
        {
            if (noteType.Equals(NoteType.Single))
            {
                Fade();

                if (b_Fade)
                {
                    if (DataManager.instance.songData._Difficult == SongDifficult.Easy)
                    {
                        rt.sizeDelta = new Vector2(rt.sizeDelta.x - 250 * Time.deltaTime, rt.sizeDelta.y - 250 * Time.deltaTime);

                        if (rt.sizeDelta.x > markRT.sizeDelta.x + 35 && rt.sizeDelta.x <= markRT.sizeDelta.x + 50)
                        {
                            sc.enabled = true;
                            b_Good = true;
                        }
                        else if (rt.sizeDelta.x <= markRT.sizeDelta.x + 35)
                        {
                            sc.enabled = true;
                            b_Perfect = true;
                            b_Good = false;
                        }

                        if (rt.sizeDelta.x <= markRT.sizeDelta.x - 3 && rt.sizeDelta.y <= markRT.sizeDelta.y - 3)
                        {
                            //gameObject.SetActive(false);
                            //Destroy(gameObject);
                            sc.enabled = false;

                            GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                            GameManager.instance.comboCnt = 0;
                            GameManager.instance.missCnt += 1;

                            //if(!GameManager.instance.b_Fever)
                            //{
                            //    GameManager.instance.SetGage(0.02f);
                            //}

                            obj.SetActive(true);

                            // 애니메이션
                            //markRT.sizeDelta = singleBrokenSize;
                            //singleAnim.SetBool("Hit", true);
                            outerImg.color = Color.clear;
                            b_Fade = false;
                        }
                    }
                    else if (DataManager.instance.songData._Difficult == SongDifficult.Normal)
                    {
                        rt.sizeDelta = new Vector2(rt.sizeDelta.x - 120 * Time.deltaTime, rt.sizeDelta.y - 120 * Time.deltaTime);

                        if (rt.sizeDelta.x <= markRT.sizeDelta.x && rt.sizeDelta.y <= markRT.sizeDelta.y)
                        {
                            //gameObject.SetActive(false);
                            //Destroy(gameObject);
                            sc.enabled = false;
                            b_Fade = false;
                        }
                    }
                    else if (DataManager.instance.songData._Difficult == SongDifficult.Hard)
                    {
                        rt.sizeDelta = new Vector2(rt.sizeDelta.x - 250 * Time.deltaTime, rt.sizeDelta.y - 250 * Time.deltaTime);

                        if (rt.sizeDelta.x > markRT.sizeDelta.x + 30 && rt.sizeDelta.x <= markRT.sizeDelta.x + 45)
                        {
                            sc.enabled = true;
                            b_Good = true;
                        }
                        else if (rt.sizeDelta.x <= markRT.sizeDelta.x + 30)
                        {
                            sc.enabled = true;
                            b_Perfect = true;
                            b_Good = false;
                        }

                        if (rt.sizeDelta.x <= markRT.sizeDelta.x - 3 && rt.sizeDelta.y <= markRT.sizeDelta.y - 3)
                        {
                            //gameObject.SetActive(false);
                            //Destroy(gameObject);
                            sc.enabled = false;

                            GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                            GameManager.instance.comboCnt = 0;
                            GameManager.instance.missCnt += 1;

                            //if(!GameManager.instance.b_Fever)
                            //{
                            //    GameManager.instance.SetGage(0.02f);
                            //}

                            obj.SetActive(true);

                            markRT.sizeDelta = singleBrokenSize;
                            // 애니메이션
                            //singleAnim.SetBool("Hit", true);
                            outerImg.color = Color.clear;
                            b_Fade = false;
                        }
                    }
                }
            }
            else if (noteType.Equals(NoteType.Long))
            {
                Fade();

                if (b_Fade)
                {
                    if (DataManager.instance.songData._Difficult == SongDifficult.Easy)
                    {
                        if (!b_LongMove)
                        {
                            rt.sizeDelta = new Vector2(rt.sizeDelta.x - 250 * Time.deltaTime, rt.sizeDelta.y - 250 * Time.deltaTime);

                            if (rt.sizeDelta.x > markRT.sizeDelta.x + 35 && rt.sizeDelta.x <= markRT.sizeDelta.x + 50)
                            {
                                sc.enabled = true;
                                b_Good = true;
                            }
                            else if (rt.sizeDelta.x <= markRT.sizeDelta.x + 35)
                            {
                                sc.enabled = true;
                                b_Perfect = true;
                                b_Good = false;
                            }

                            if (rt.sizeDelta.x <= markRT.sizeDelta.x - 3 && rt.sizeDelta.y <= markRT.sizeDelta.y - 3)
                            {
                                if (!b_Long)
                                {
                                    sc.enabled = false;

                                    GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                                    GameManager.instance.comboCnt = 0;
                                    GameManager.instance.missCnt += 1;

                                    //if(!GameManager.instance.b_Fever)
                                    //{
                                    //    GameManager.instance.SetGage(0.02f);
                                    //}

                                    obj.SetActive(true);

                                    Destroy(gameObject);
                                }
                                else
                                {
                                    if (!b_LongMove)
                                    {
                                        b_LongMove = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (noteBody.Equals(NoteBody.HandLeft))
                            {
                                leftPosi = Camera.main.WorldToScreenPoint(leftObj.position);

                                if (leftPosi.x <= 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                                    leftPosi.x = -(960 - leftPosi.x);
                                }
                                else if (leftPosi.x > 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                                    leftPosi.x -= 960;
                                }

                                if (leftPosi.y <= 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                                    leftPosi.y = -(540 - leftPosi.y);
                                }
                                else if (leftPosi.y > 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                                    leftPosi.y -= 540;
                                }

                                imgTransform.localPosition = leftPosi;

                                //Debug.Log("localPosition : " + imgTransform.localPosition);
                                //Debug.Log("positionCount : " + lr.GetPosition(lr.positionCount - 1));
                                if (imgTransform.localPosition.Equals(lr.GetPosition(lr.positionCount - 1)))
                                {
                                    Destroy(gameObject);
                                }
                            }
                            else if (noteBody.Equals(NoteBody.HandRight))
                            {
                                rightPosi = Camera.main.WorldToScreenPoint(rightObj.position);

                                if (rightPosi.x <= 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                                    rightPosi.x = -(960 - rightPosi.x);
                                }
                                else if (rightPosi.x > 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                                    rightPosi.x -= 960;
                                }

                                if (rightPosi.y <= 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                                    rightPosi.y = -(540 - rightPosi.y);
                                }
                                else if (rightPosi.y > 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                                    rightPosi.y -= 540;
                                }

                                imgTransform.localPosition = rightPosi;
                            }
                        }
                    }
                    else if (DataManager.instance.songData._Difficult == SongDifficult.Normal)
                    {
                        rt.sizeDelta = new Vector2(rt.sizeDelta.x - 120 * Time.deltaTime, rt.sizeDelta.y - 120 * Time.deltaTime);

                        if (rt.sizeDelta.x <= markRT.sizeDelta.x && rt.sizeDelta.y <= markRT.sizeDelta.y)
                        {
                            //gameObject.SetActive(false);
                            //Destroy(gameObject);
                            sc.enabled = false;
                            b_Fade = false;
                        }
                    }
                    else if (DataManager.instance.songData._Difficult == SongDifficult.Hard)
                    {
                        if (!b_LongMove)
                        {
                            rt.sizeDelta = new Vector2(rt.sizeDelta.x - 250 * Time.deltaTime, rt.sizeDelta.y - 250 * Time.deltaTime);

                            if (rt.sizeDelta.x > markRT.sizeDelta.x + 30 && rt.sizeDelta.x <= markRT.sizeDelta.x + 45)
                            {
                                sc.enabled = true;
                                b_Good = true;
                            }
                            else if (rt.sizeDelta.x <= markRT.sizeDelta.x + 30)
                            {
                                sc.enabled = true;
                                b_Perfect = true;
                                b_Good = false;
                            }

                            if (rt.sizeDelta.x <= markRT.sizeDelta.x -3 && rt.sizeDelta.y <= markRT.sizeDelta.y -3)
                            {
                                if (!b_Long)
                                {
                                    sc.enabled = false;

                                    GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                                    GameManager.instance.comboCnt = 0;
                                    GameManager.instance.missCnt += 1;

                                    //if(!GameManager.instance.b_Fever)
                                    //{
                                    //    GameManager.instance.SetGage(0.02f);
                                    //}

                                    obj.SetActive(true);

                                    Destroy(gameObject);
                                }
                                else
                                {
                                    if (!b_LongMove)
                                    {
                                        b_LongMove = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (noteBody.Equals(NoteBody.HandLeft))
                            {
                                leftPosi = Camera.main.WorldToScreenPoint(leftObj.position);

                                if (leftPosi.x <= 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                                    leftPosi.x = -(960 - leftPosi.x);
                                }
                                else if (leftPosi.x > 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                                    leftPosi.x -= 960;
                                }

                                if (leftPosi.y <= 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                                    leftPosi.y = -(540 - leftPosi.y);
                                }
                                else if (leftPosi.y > 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                                    leftPosi.y -= 540;
                                }

                                imgTransform.localPosition = leftPosi;

                                //Debug.Log("localPosition : " + imgTransform.localPosition);
                                //Debug.Log("positionCount : " + lr.GetPosition(lr.positionCount - 1));
                                //if (imgTransform.localPosition.Equals(lr.GetPosition(lr.positionCount - 1)))
                                //{
                                //    Destroy(gameObject);
                                //}
                            }
                            else if (noteBody.Equals(NoteBody.HandRight))
                            {
                                rightPosi = Camera.main.WorldToScreenPoint(rightObj.position);

                                if (rightPosi.x <= 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", -(960 - leftHandPosi.x)));
                                    rightPosi.x = -(960 - rightPosi.x);
                                }
                                else if (rightPosi.x > 960.0f)
                                {
                                    //Debug.Log(string.Format("x : {0}", leftHandPosi.x - 960));
                                    rightPosi.x -= 960;
                                }

                                if (rightPosi.y <= 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", -(540 - leftHandPosi.y)));
                                    rightPosi.y = -(540 - rightPosi.y);
                                }
                                else if (rightPosi.y > 540.0f)
                                {
                                    //Debug.Log(string.Format("y: {0}", leftHandPosi.y - 540));
                                    rightPosi.y -= 540;
                                }

                                imgTransform.localPosition = rightPosi;
                            }
                        }
                    }
                }

                //if(b_Long)
                //{
                //    StartCoroutine(longNoteActive());
                //}
            }
            else if(noteType.Equals(NoteType.Press))
            {
                if (b_Press)
                {
                    pressOntime = innerImg.fillAmount - pressStartTime;
                    GameManager.instance.b_Press = true;
                    //GameManager.instance.MoveCamera(true, tr);

                    if (pressOntime >= 0.1f && pressOntime < 0.7f)
                    {
                        b_Good = true;
                    }
                    else if(pressOntime >= 0.7f)
                    {
                        b_Good = false;
                        b_Perfect = true;
                    }
                }
                
                if (b_Fade)
                {
                    innerImg.fillAmount += (1.0f / pressTime) * Time.deltaTime;

                    if (innerImg.fillAmount.Equals(1))
                    {
                        if (pressOntime >= 0.1f)
                        {
                            GameManager.instance.SetScore(this, tr);
                            GameManager.instance.b_Press = false;
                            //GameManager.instance.MoveCamera(false, tr);

                            if (gameObject.CompareTag("HandLeft"))
                            {
                                //Instantiate(effectRedTest, transform.position, Quaternion.identity);
                                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "PressRedEffect");
                                obj.SetActive(true);
                            }
                            else if (gameObject.CompareTag("HandRight"))
                            {
                                //Instantiate(effectBlueTest, transform.position, Quaternion.identity);
                                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "PressBlueEffect");
                                obj.SetActive(true);
                            }
                        }
                        else if (pressOntime < 0.1f)
                        {
                            GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                            GameManager.instance.comboCnt = 0;
                            GameManager.instance.missCnt += 1;

                            //if(!GameManager.instance.b_Fever)
                            //{
                            //    GameManager.instance.SetGage(0.02f);
                            //}

                            obj.SetActive(true);
                        }

                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    Vector2 markVec;
    Vector2 vec2 = new Vector2(0.7f, 0.7f);

    void SetReady()
    {
        //noteTimer += Time.deltaTime;

        if(noteType.Equals(NoteType.Single) || noteType.Equals(NoteType.Long))
        {
            if (outerImg.color.a != 1.0f || markImg.color.a != 1.0f)
            {
                //outerImg.color = new Color(1.0f, 1.0f, 1.0f, outerImg.color.a + 3f * Time.deltaTime);
                markImg.color = new Color(1.0f, 1.0f, 1.0f, markImg.color.a + (0.6f * Time.deltaTime));
               //innerImg.color = new Color(1.0f, 1.0f, 1.0f, innerImg.color.a + 3f * Time.deltaTime);
            }

            markVec = markImg.transform.localScale;
            markVec += vec2 * Time.deltaTime;
            markImg.transform.localScale = markVec;

            if(markImg.transform.localScale.x >= 1.0f)
            {
                markImg.color = Color.white;
                markImg.transform.localScale = Vector2.one;
                b_Ready = true;
            }

            //if (noteTimer >= 0.4f)
            //{
            //    outerImg.color = new Color32(255, 255, 255, 80);
            //    b_Ready = true;
            //}
        }
        else if(noteType.Equals(NoteType.Press))
        {
            if (noteTimer >= 0.4f)
            {
                if (gameObject.CompareTag("HandLeft"))
                {
                    pinkPressBeforeEffect.SetActive(false);
                }
                else if (gameObject.CompareTag("HandRight"))
                {
                    bluePressBeforeEffect.SetActive(false);
                }

                markImg.color = new Color32(255, 255, 255, 255);
                outerImg.color = new Color32(255, 255, 255, 255);
                innerImg.color = new Color32(255, 255, 255, 255);

                sc.enabled = true;

                b_Ready = true;
            }
        }
    }

    public void Fade()
    {
        if (b_Fade)
        {
            if (outerImg.color.a != 1.0f || markImg.color.a != 1.0f)
            {
                outerImg.color = new Color(1.0f, 1.0f, 1.0f, outerImg.color.a + 3f * Time.deltaTime);
                //markImg.color = new Color(1.0f, 1.0f, 1.0f, markImg.color.a + 3f * Time.deltaTime);
                //innerImg.color = new Color(1.0f, 1.0f, 1.0f, innerImg.color.a + 3f * Time.deltaTime);
            }
        }
        else
        {
            if(noteType.Equals(NoteType.Single))
            {
                // 애니메이션
                //img.color = Color.clear;

                //if (singleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                //{
                    Destroy(gameObject);
                //}
            }
        }
    }

    IEnumerator longNoteActive()
    {
        b_Long = false;

        if (!longCnt.Equals(lr.positionCount - 1))
        {
            while (!imgTransform.localPosition.Equals(lr.GetPosition(longCnt + 1)))
            {
                imgTransform.localPosition = Vector3.Lerp(lr.GetPosition(longCnt), lr.GetPosition(longCnt + 1), Vector2.Distance(lr.GetPosition(longCnt), lr.GetPosition(longCnt + 1)));

                yield return null;
            }

            longCnt++;
            b_Long = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PressOn()
    {
        pressStartTime = innerImg.fillAmount;
        anim.SetTrigger("PressOn");
    }

    public void OnMouseEnter()
    {
//#if UNITY_EDITOR
        if(noteType.Equals(NoteType.Single))
        {
            GameManager.instance.SetScore(this, tr);

            if (gameObject.CompareTag("HandLeft"))
            {
                //Instantiate(effectRedTest, transform.position, Quaternion.identity);
                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "SingleRedEffect");
                obj.SetActive(true);

                obj = PooledManager.instance.GetPooledObject_NoteEffect(GameManager.instance.bigEffectSpawn, "NoteBigEffect");
                obj.SetActive(true);
            }
            else if (gameObject.CompareTag("HandRight"))
            {
                //Instantiate(effectBlueTest, transform.position, Quaternion.identity);
                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "SingleBlueEffect");
                obj.SetActive(true);

                obj = PooledManager.instance.GetPooledObject_NoteEffect(GameManager.instance.bigEffectSpawn, "NoteBigEffect");
                obj.SetActive(true);
            }

            GameManager.instance.SetRankEffect();
            GameManager.instance.SpawnHeart();
            SoundManager.instance.PlayBPM();
            b_Fade = false;
            markRT.sizeDelta = singleBrokenSize;
            // 애니메이션
            //singleAnim.SetBool("Hit", true);

            Destroy(gameObject);
        }
        else if(noteType.Equals(NoteType.Press))
        {
            b_Press = true;
            PressOn();
            
            //GameManager.instance.MoveCamera(true, transform);

            //GameManager.instance.SetScore(this, transform);

            if (gameObject.CompareTag("HandLeft"))
            {
                pinkPressEffect.SetActive(true);
            }
            else if (gameObject.CompareTag("HandRight"))
            {
                bluePressEffect.SetActive(true);
            }
        }
//#endif
    }

    public void OnMouseExit()
    {
        if (noteType.Equals(NoteType.Press))
        {
            if (pressOntime >= 0.1f)
            {
                GameManager.instance.SetScore(this, tr);
                
                if (gameObject.CompareTag("HandLeft"))
                {
                    //Instantiate(effectRedTest, transform.position, Quaternion.identity);
                    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "PressRedEffect");
                    obj.SetActive(true);
                }
                else if (gameObject.CompareTag("HandRight"))
                {
                    //Instantiate(effectBlueTest, transform.position, Quaternion.identity);
                    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "PressBlueEffect");
                    obj.SetActive(true);
                }
            }
            else if (pressOntime < 0.1f)
            {
                GameObject obj = PooledManager.instance.GetPooledObject_GameText(tr, "MISS");
                GameManager.instance.comboCnt = 0;
                GameManager.instance.missCnt += 1;

                //if(!GameManager.instance.b_Fever)
                //{
                //    GameManager.instance.SetGage(0.02f);
                //}

                obj.SetActive(true);
            }

            GameManager.instance.b_Press = false;
            //GameManager.instance.MoveCamera(false);

            Destroy(gameObject);
        }
    }
}
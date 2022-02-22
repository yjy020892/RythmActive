using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Enums_Game;
//using System;

[System.Serializable]
public class noteObjArray
{
    public GameObject[] notes;
}

public class SheetParser : MonoBehaviour
{
    public Transform canvasUI;
    public noteObjArray[] noteObjs;

    public Transform leftObj;
    public Transform rightObj;

    public Material pinkLineMat;
    public Material blueLineMat;

    TextAsset textAsset;
    StringReader strReader;

    [SerializeField] Sheet sheet;
    //SongManager songManager;

    string sheetText = string.Empty;
    string songName;
    string songDiff;
    string[] textSplit;

    void Awake()
    {
        //sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        //songManager = GameObject.Find("SongSelect").GetComponent<SongManager>();

        //songName = DataManager.instance.songData._SongName;
        songName = DataManager.instance.songData._AudioFileName;
        songDiff = DataManager.instance.songData._Difficult.ToString();
        textAsset = Resources.Load(string.Format("Song/{0}/{1}_{2}_Data", songName, songName, songDiff)) as TextAsset;
        strReader = new StringReader(textAsset.text);

        GameManager.instance.SongStart += ParseSheet;
        //ParseSheet();
    }

    public void ParseSheet()
    {
        int _PosiX = 0;
        int _PosiY = 0;
        int _NoteTime = 1;
        float _pressTime = 0.0f;
        NoteBody _NoteBody = NoteBody.None;

        while (sheetText != null)
        {
            sheetText = strReader.ReadLine();
            textSplit = sheetText.Split('=');

            if (textSplit[0] == "AudioFileName")
            {
                sheet.AudioFileName = textSplit[1];
            }
            //else if (textSplit[0] == "AudioViewTime") sheet.AudioViewTime = textSplit[1];
            //else if (textSplit[0] == "ImageFileName") sheet.ImageFileName = textSplit[1];
            else if (textSplit[0] == "BPM")
            {
                int.TryParse(textSplit[1], out DataManager.instance.songData._BPM);
                //sheet.Bpm = float.Parse(textSplit[1]);
            }
            else if (textSplit[0] == "Offset")
            {
                int.TryParse(textSplit[1], out DataManager.instance.songData._Offset);
                //sheet.Offset = float.Parse(textSplit[1]);
            }
            //else if (textSplit[0] == "Beat") sheet.Beat = int.Parse(textSplit[1]);
            //else if (textSplit[0] == "Bit") sheet.Bit = int.Parse(textSplit[1]);
            //else if (textSplit[0] == "Bar") sheet.BarCnt = int.Parse(textSplit[1]);
            else if (textSplit[0] == "Title") sheet.Title = textSplit[1];
            else if (textSplit[0] == "Artist") sheet.Artist = textSplit[1];
            //else if (textSplit[0] == "Source") sheet.Source = textSplit[1];
            //else if (textSplit[0] == "Sheet") sheet.SheetBy = textSplit[1];
            else if (textSplit[0] == "Difficult") sheet.Difficult = textSplit[1];
            else if (sheetText == string.Format("[NoteInfo_{0}]", songDiff))
            {
                while ((sheetText = strReader.ReadLine()) != null)
                {
                    textSplit = sheetText.Split('/');

                    // single note
                    if (int.Parse(textSplit[5]).Equals(0) && int.Parse(textSplit[6]).Equals(0))
                    {
                        int.TryParse(textSplit[0], out _PosiX);
                        int.TryParse(textSplit[1], out _PosiY);
                        int.TryParse(textSplit[2], out _NoteTime);

                        if (int.Parse(textSplit[3]).Equals(1))
                        {
                            _NoteBody = Enums_Game.NoteBody.HandLeft;
                        }
                        else if (int.Parse(textSplit[4]).Equals(1))
                        {
                            _NoteBody = Enums_Game.NoteBody.HandRight;
                        }

                        GameObject obj = Instantiate(noteObjs[0].notes[0], new Vector2(_PosiX, _PosiY), Quaternion.identity);
                        Note note = obj.GetComponent<Note>();
                        obj.name = _NoteTime.ToString();
                        note.posiX = _PosiX;
                        note.posiY = _PosiY;
                        note.noteTime = _NoteTime;
                        note.noteBody = _NoteBody;
                        note.noteType = NoteType.Single;
                        note.tag = _NoteBody.ToString();

                        obj.transform.SetParent(canvasUI, false);

                        if (_NoteBody == Enums_Game.NoteBody.HandLeft)
                        {
                            // 외각 IN
                            //note.img.sprite = note.objectColor[0];
                            //note.markImg.sprite = note.outlineColor[0];

                            // 외각 OUT
                            note.outerImg.sprite = note.outlineColor[0];
                            note.markImg.sprite = note.objectColor[0];
                            //note.innerImg.sprite = note.objectInnerColor[0];

                            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                        }
                        else if (_NoteBody == Enums_Game.NoteBody.HandRight)
                        {
                            // 외각 IN
                            //note.img.sprite = note.objectColor[1];
                            //note.markImg.sprite = note.outlineColor[1];

                            // 외각 OUT
                            note.outerImg.sprite = note.outlineColor[1];
                            note.markImg.sprite = note.objectColor[1];
                            //note.innerImg.sprite = note.objectInnerColor[1];

                            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                        }

                        sheet.SetNote(_NoteBody, obj);
                    }
                    // stop note
                    else if(int.Parse(textSplit[5]).Equals(1))
                    {

                    }
                    //// long note
                    //else if(int.Parse(textSplit[5]).Equals(1))
                    //{
                    //    string[] longNotePosiSplit = textSplit[0].Split('@');

                    //    GameObject obj = Instantiate(noteObjs[0].notes[1], new Vector2(0, 0), Quaternion.identity);

                    //    if (int.Parse(textSplit[3]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.HandLeft;
                    //        obj.GetComponent<LineRenderer>().material = pinkLineMat;
                    //    }
                    //    else if (int.Parse(textSplit[4]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.HandRight;
                    //        obj.GetComponent<LineRenderer>().material = blueLineMat;
                    //    }

                    //    Note longNote = obj.GetComponent<Note>();

                    //    longNote.leftObj = leftObj;
                    //    longNote.rightObj = rightObj;
                    //    longNote.noteType = NoteType.Long;
                    //    longNote.name = textSplit[1];
                    //    longNote.longNotePosi = longNotePosiSplit;
                    //    longNote.tag = _NoteBody.ToString();
                    //    longNote.noteBody = _NoteBody;
                    //    int.TryParse(textSplit[1], out longNote.noteTime);
                    //    int.TryParse(textSplit[2], out longNote.longNoteEndTime);

                    //    obj.transform.SetParent(canvasUI, false);

                    //    if (_NoteBody == Enums_Game.NoteBody.HandLeft)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[0];
                    //        //note.markImg.sprite = note.outlineColor[0];

                    //        // 외각 OUT
                    //        longNote.img.sprite = longNote.outlineColor[0];
                    //        longNote.markImg.sprite = longNote.objectColor[0];
                    //        longNote.innerImg.sprite = longNote.objectInnerColor[0];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                    //    }
                    //    else if (_NoteBody == Enums_Game.NoteBody.HandRight)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[1];
                    //        //note.markImg.sprite = note.outlineColor[1];

                    //        // 외각 OUT
                    //        longNote.img.sprite = longNote.outlineColor[1];
                    //        longNote.markImg.sprite = longNote.objectColor[1];
                    //        longNote.innerImg.sprite = longNote.objectInnerColor[1];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                    //    }

                    //    sheet.SetNote(_NoteBody, obj);
                    //}
                    // press note
                    else if(int.Parse(textSplit[6]).Equals(1))
                    {
                        int.TryParse(textSplit[0], out _PosiX);
                        int.TryParse(textSplit[1], out _PosiY);
                        int.TryParse(textSplit[2], out _NoteTime);
                        float.TryParse(textSplit[7], out _pressTime);

                        if (int.Parse(textSplit[3]).Equals(1))
                        {
                            _NoteBody = Enums_Game.NoteBody.HandLeft;
                        }
                        else if (int.Parse(textSplit[4]).Equals(1))
                        {
                            _NoteBody = Enums_Game.NoteBody.HandRight;
                        }

                        GameObject obj = Instantiate(noteObjs[0].notes[2], new Vector2(_PosiX, _PosiY), Quaternion.identity);
                        Note pressNote = obj.GetComponent<Note>();
                        obj.name = _NoteTime.ToString();
                        pressNote.posiX = _PosiX;
                        pressNote.posiY = _PosiY;
                        pressNote.pressTime = _pressTime;
                        pressNote.innerImg.fillAmount = 0;
                        pressNote.noteTime = _NoteTime;
                        pressNote.noteBody = _NoteBody;
                        pressNote.noteType = NoteType.Press;
                        pressNote.tag = _NoteBody.ToString();

                        obj.transform.SetParent(canvasUI, false);

                        if (_NoteBody == Enums_Game.NoteBody.HandLeft)
                        {
                            // 외각 IN
                            //note.img.sprite = note.objectColor[0];
                            //note.markImg.sprite = note.outlineColor[0];

                            // 외각 OUT
                            pressNote.outerImg.sprite = pressNote.outlineColor[0];
                            pressNote.markImg.sprite = pressNote.objectColor[0];
                            pressNote.innerImg.sprite = pressNote.objectInnerColor[0];

                            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                        }
                        else if (_NoteBody == Enums_Game.NoteBody.HandRight)
                        {
                            // 외각 IN
                            //note.img.sprite = note.objectColor[1];
                            //note.markImg.sprite = note.outlineColor[1];

                            // 외각 OUT
                            pressNote.outerImg.sprite = pressNote.outlineColor[1];
                            pressNote.markImg.sprite = pressNote.objectColor[1];
                            pressNote.innerImg.sprite = pressNote.objectInnerColor[1];

                            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                        }

                        sheet.SetNote(_NoteBody, obj);
                    }

                    // ------------------------------------------------------------------------- 구 노트 -------------------------------------------------------------------------

                    //if (textSplit.Length.Equals(7))
                    //{
                    //    // single
                    //    if(int.Parse(textSplit[6]).Equals(0))
                    //    {
                    //        float.TryParse(textSplit[0], out _PosiX);
                    //        float.TryParse(textSplit[1], out _PosiY);
                    //        float.TryParse(textSplit[2], out _NoteTime);

                    //        if (int.Parse(textSplit[3]).Equals(1))
                    //        {
                    //            _NoteBody = Enums_Game.NoteBody.LeftHand;
                    //        }
                    //        else if (int.Parse(textSplit[4]).Equals(1))
                    //        {
                    //            _NoteBody = Enums_Game.NoteBody.RightHand;
                    //        }

                    //        GameObject obj = Instantiate(noteObjs[0].notes[0], new Vector2(_PosiX, _PosiY), Quaternion.identity);
                    //        Note note = obj.GetComponent<Note>();
                    //        obj.name = _NoteTime.ToString();
                    //        note.posiX = _PosiX;
                    //        note.posiY = _PosiY;
                    //        note.noteTime = _NoteTime;
                    //        note.noteBody = _NoteBody;
                    //        note.noteType = NoteType.Single;
                    //        note.tag = _NoteBody.ToString();

                    //        obj.transform.SetParent(canvasUI, false);

                    //        if (_NoteBody == Enums_Game.NoteBody.LeftHand)
                    //        {
                    //            // 외각 IN
                    //            //note.img.sprite = note.objectColor[0];
                    //            //note.markImg.sprite = note.outlineColor[0];

                    //            // 외각 OUT
                    //            note.img.sprite = note.outlineColor[0];
                    //            note.markImg.sprite = note.objectColor[0];
                    //            note.innerImg.sprite = note.objectInnerColor[0];

                    //            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                    //        }
                    //        else if (_NoteBody == Enums_Game.NoteBody.RightHand)
                    //        {
                    //            // 외각 IN
                    //            //note.img.sprite = note.objectColor[1];
                    //            //note.markImg.sprite = note.outlineColor[1];

                    //            // 외각 OUT
                    //            note.img.sprite = note.outlineColor[1];
                    //            note.markImg.sprite = note.objectColor[1];
                    //            note.innerImg.sprite = note.objectInnerColor[1];

                    //            //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                    //        }

                    //        sheet.SetNote(_NoteBody, obj);
                    //    }
                    //}
                    //// long
                    //else if(textSplit.Length.Equals(6))
                    //{
                    //    string[] longNotePosiSplit = textSplit[0].Split('@');
                    //    string[] longNoteTimeSplit = textSplit[1].Split(',');

                    //    GameObject obj = Instantiate(noteObjs[0].notes[1], new Vector2(0, 0), Quaternion.identity);

                    //    if (int.Parse(textSplit[2]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.LeftHand;
                    //        obj.GetComponent<LineRenderer>().material = pinkLineMat;
                    //    }
                    //    else if (int.Parse(textSplit[3]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.RightHand;
                    //        obj.GetComponent<LineRenderer>().material = blueLineMat;
                    //    }

                    //    Note longNote = obj.GetComponent<Note>();

                    //    longNote.leftObj = leftObj;
                    //    longNote.rightObj = rightObj;
                    //    longNote.noteType = NoteType.Long;
                    //    longNote.name = longNoteTimeSplit[0];
                    //    longNote.longNotePosi = longNotePosiSplit;
                    //    longNote.tag = _NoteBody.ToString();
                    //    longNote.noteBody = _NoteBody;
                    //    float.TryParse(longNoteTimeSplit[0], out longNote.noteTime);
                    //    float.TryParse(longNoteTimeSplit[1], out longNote.longNoteEndTime);

                    //    obj.transform.SetParent(canvasUI, false);

                    //    if (_NoteBody == Enums_Game.NoteBody.LeftHand)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[0];
                    //        //note.markImg.sprite = note.outlineColor[0];

                    //        // 외각 OUT
                    //        longNote.img.sprite = longNote.outlineColor[0];
                    //        longNote.markImg.sprite = longNote.objectColor[0];
                    //        longNote.innerImg.sprite = longNote.objectInnerColor[0];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                    //    }
                    //    else if (_NoteBody == Enums_Game.NoteBody.RightHand)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[1];
                    //        //note.markImg.sprite = note.outlineColor[1];

                    //        // 외각 OUT
                    //        longNote.img.sprite = longNote.outlineColor[1];
                    //        longNote.markImg.sprite = longNote.objectColor[1];
                    //        longNote.innerImg.sprite = longNote.objectInnerColor[1];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                    //    }

                    //    sheet.SetNote(_NoteBody, obj);
                    //}
                    //// press
                    //else if (textSplit.Length.Equals(8))
                    //{
                    //    float.TryParse(textSplit[0], out _PosiX);
                    //    float.TryParse(textSplit[1], out _PosiY);
                    //    float.TryParse(textSplit[2], out _NoteTime);
                    //    float.TryParse(textSplit[7], out _pressTime);

                    //    if (int.Parse(textSplit[3]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.LeftHand;
                    //    }
                    //    else if (int.Parse(textSplit[4]).Equals(1))
                    //    {
                    //        _NoteBody = Enums_Game.NoteBody.RightHand;
                    //    }

                    //    GameObject obj = Instantiate(noteObjs[0].notes[2], new Vector2(_PosiX, _PosiY), Quaternion.identity);
                    //    Note pressNote = obj.GetComponent<Note>();
                    //    obj.name = _NoteTime.ToString();
                    //    pressNote.posiX = _PosiX;
                    //    pressNote.posiY = _PosiY;
                    //    pressNote.pressTime = _pressTime;
                    //    pressNote.innerImg.fillAmount = 0;
                    //    pressNote.noteTime = _NoteTime;
                    //    pressNote.noteBody = _NoteBody;
                    //    pressNote.noteType = NoteType.Press;
                    //    pressNote.tag = _NoteBody.ToString();

                    //    obj.transform.SetParent(canvasUI, false);

                    //    if (_NoteBody == Enums_Game.NoteBody.LeftHand)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[0];
                    //        //note.markImg.sprite = note.outlineColor[0];

                    //        // 외각 OUT
                    //        pressNote.img.sprite = pressNote.outlineColor[0];
                    //        pressNote.markImg.sprite = pressNote.objectColor[0];
                    //        pressNote.innerImg.sprite = pressNote.objectInnerColor[0];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L");
                    //    }
                    //    else if (_NoteBody == Enums_Game.NoteBody.RightHand)
                    //    {
                    //        // 외각 IN
                    //        //note.img.sprite = note.objectColor[1];
                    //        //note.markImg.sprite = note.outlineColor[1];

                    //        // 외각 OUT
                    //        pressNote.img.sprite = pressNote.outlineColor[1];
                    //        pressNote.markImg.sprite = pressNote.objectColor[1];
                    //        pressNote.innerImg.sprite = pressNote.objectInnerColor[1];

                    //        //note.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R");
                    //    }

                    //    sheet.SetNote(_NoteBody, obj);
                    //}

                    // --------------------------------------------------------------------------------------------------------------------------------------------------
                }
            }
        }
    }
}
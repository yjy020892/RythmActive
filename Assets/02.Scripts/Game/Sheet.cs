using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Game;
using DATA;

public class Sheet : MonoBehaviour
{
    // [SheetInfo]
    public string AudioFileName { set; get; }
    public string AudioViewTime { set; get; }
    public string ImageFileName { set; get; }
    public float Bpm { set; get; }
    public float Offset { set; get; }
    public int Beat { set; get; }
    public int Bit { set; get; }
    public int BarCnt { set; get; }

    // [ContentInfo]
    public string Title { set; get; }
    public string Artist { set; get; }
    public string Source { set; get; }
    public string SheetBy { set; get; }
    public string Difficult { set; get; }

    // [NoteInfo]
    public List<GameObject> leftHandList = new List<GameObject>();
    public List<GameObject> rightHandList = new List<GameObject>();
    //public List<GameObject> leftAnkleList = new List<GameObject>();
    //public List<GameObject> rightAnkleList = new List<GameObject>();

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void SetNote(Enums_Game.NoteBody noteBody, GameObject noteObj)
    {
        if (noteBody.Equals(Enums_Game.NoteBody.HandLeft))
        {
            leftHandList.Add(noteObj);
        }
        else if (noteBody.Equals(Enums_Game.NoteBody.HandRight))
        {
            rightHandList.Add(noteObj);
        }
        //else if (noteType.Equals(Enums_Game.NoteType.LeftAnkle))
        //{
        //    leftAnkleList.Add(noteObj);
        //}
        //else if (noteType.Equals(Enums_Game.NoteType.RightAnkle))
        //{
        //    rightAnkleList.Add(noteObj);
        //}
    }

    void showInfo()
    {
        Debug.Log(AudioFileName);
        Debug.Log(Title);
        Debug.Log(Artist);
        Debug.Log(Difficult);
        Debug.Log(Bpm);
    }
}

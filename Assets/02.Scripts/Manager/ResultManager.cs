using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums_Game;

public class ResultManager : MonoBehaviour
{
    //ResultState resultState = ResultState.Title;

    public Text songNameText, perfectText, goodText, missText, comboText, totalScoreText;
    Text currentText;

    public Image albumImg;
    public Image kioskAlbumImg;
    public Image musicLevelBarImg;
    public Image kioskMusicLevelBarImg;

    public Sprite[] musicLevelBars;

    private string audioFileName, songName;
    private int goodNumber, perfectNumber, missNumber, comboNumber, scoreNumber;
    private bool b_FullCombo;

    public float slidingTime = 2.5f;

    private float desiredNumber, initialNumber, currentNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        SetResult();
    }

    // Update is called once per framew
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneController.instance.GotoScene(Enums_Common.SceneType.Rank);
        }

        SlidingNumber();
    }

    void SlidingNumber()
    {
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

            currentText.text = currentNumber.ToString("N0");

            if (desiredNumber.Equals(currentNumber))
            {
                //switch (resultState)
                //{
                //    case ResultState.Perfect:
                //        desiredNumber = 0;
                //        initialNumber = 0;
                //        currentNumber = 0;
                //        resultState = ResultState.Good;
                //        AddToNumber(goodNumber, goodText);
                //        break;

                //    case ResultState.Good:
                //        desiredNumber = 0;
                //        initialNumber = 0;
                //        currentNumber = 0;
                //        resultState = ResultState.Miss;
                //        AddToNumber(missNumber, missText);
                //        break;

                //    case ResultState.Miss:
                //        desiredNumber = 0;
                //        initialNumber = 0;
                //        currentNumber = 0;
                //        resultState = ResultState.Combo;
                //        AddToNumber(comboNumber, comboText);
                //        break;

                //    case ResultState.Combo:
                //        desiredNumber = 0;
                //        initialNumber = 0;
                //        currentNumber = 0;
                //        resultState = ResultState.Score;
                //        AddToNumber(scoreNumber, totalScoreText);
                //        break;

                //    case ResultState.Score:
                //        resultState = ResultState.End;
                //        break;
                //}
            }
        }
        else
        {
            //switch (resultState)
            //{
            //    case ResultState.Perfect:
            //        desiredNumber = 0;
            //        initialNumber = 0;
            //        currentNumber = 0;
            //        resultState = ResultState.Good;
            //        AddToNumber(goodNumber, goodText);
            //        break;

            //    case ResultState.Good:
            //        desiredNumber = 0;
            //        initialNumber = 0;
            //        currentNumber = 0;
            //        resultState = ResultState.Miss;
            //        AddToNumber(missNumber, missText);
            //        break;

            //    case ResultState.Miss:
            //        desiredNumber = 0;
            //        initialNumber = 0;
            //        currentNumber = 0;
            //        resultState = ResultState.Combo;
            //        AddToNumber(comboNumber, comboText);
            //        break;

            //    case ResultState.Combo:
            //        desiredNumber = 0;
            //        initialNumber = 0;
            //        currentNumber = 0;
            //        resultState = ResultState.Score;
            //        AddToNumber(scoreNumber, totalScoreText);
            //        break;

            //    case ResultState.Score:
            //        resultState = ResultState.End;
            //        break;
            //}
        }
    }

    void AddToNumber(float value, Text text)
    {
        initialNumber = currentNumber;
        desiredNumber += value;

        currentText = text;
    }

    void Init()
    {
        audioFileName = DataManager.instance.songData._AudioFileName;
        songName = DataManager.instance.songData._SongName;
        //goodNumber = DataManager.instance.songData._Good;
        //perfectNumber = DataManager.instance.songData._Perfect;
        //missNumber = DataManager.instance.songData._Miss;
        comboNumber = DataManager.instance.songData._Combo;
        b_FullCombo = DataManager.instance.songData._FullCombo;
        scoreNumber = DataManager.instance.songData._Score;
    }

    void SetResult()
    {
        albumImg.sprite = Resources.Load<Sprite>(string.Format("Song/{0}/{1}_Img", audioFileName, audioFileName));
        songNameText.text = songName;

        if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Easy))
        {
            musicLevelBarImg.sprite = musicLevelBars[0];
        }
        else if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Normal))
        {
            musicLevelBarImg.sprite = musicLevelBars[1];
        }
        else if (DataManager.instance.songData._Difficult.Equals(SongDifficult.Hard))
        {
            musicLevelBarImg.sprite = musicLevelBars[2];
        }

        //resultState = ResultState.Perfect;
        AddToNumber(perfectNumber, perfectText);
    }
}
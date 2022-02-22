using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Enums_Game;

public class RankManager : MonoBehaviour
{
    RankingState rankingState = RankingState.None;

    public Text firstText, secondText, thirdText, scoreText;
    public Text kioskFirstText, kioskSecondText, kioskThirdText, kioskScoreText;

    Text currentText, currentKioskText;

    private int firstScore, secondScore, thirdScore, myScore;
    public int saveNumber = 100;
    private int[] rankNum;

    public float slidingTime = 1f;

    private float desiredNumber, initialNumber, currentNumber = 0;

    private string rankStr;
    private string[] rankStrLines;

    StreamWriter sw;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        SetData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneController.instance.GotoScene(Enums_Common.SceneType.Intro);
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
            currentKioskText.text = currentNumber.ToString("N0");

            if (desiredNumber.Equals(currentNumber))
            {
                switch (rankingState)
                {
                    case RankingState.first:
                        desiredNumber = 0;
                        initialNumber = 0;
                        currentNumber = 0;
                        rankingState = RankingState.second;
                        AddToNumber(secondScore, secondText, kioskSecondText);
                        break;

                    case RankingState.second:
                        desiredNumber = 0;
                        initialNumber = 0;
                        currentNumber = 0;
                        rankingState = RankingState.third;
                        AddToNumber(thirdScore, thirdText, kioskThirdText);
                        break;

                    case RankingState.third:
                        desiredNumber = 0;
                        initialNumber = 0;
                        currentNumber = 0;
                        rankingState = RankingState.score;

                        scoreText.text = myScore.ToString();
                        kioskScoreText.text = myScore.ToString();
                        SaveData();
                        break;
                }
            }
        }
        else
        {
            switch (rankingState)
            {
                case RankingState.first:
                    desiredNumber = 0;
                    initialNumber = 0;
                    currentNumber = 0;
                    rankingState = RankingState.second;
                    AddToNumber(secondScore, secondText, kioskSecondText);
                    break;

                case RankingState.second:
                    desiredNumber = 0;
                    initialNumber = 0;
                    currentNumber = 0;
                    rankingState = RankingState.third;
                    AddToNumber(thirdScore, thirdText, kioskThirdText);
                    break;

                case RankingState.third:
                    desiredNumber = 0;
                    initialNumber = 0;
                    currentNumber = 0;
                    rankingState = RankingState.score;

                    scoreText.text = myScore.ToString();
                    SaveData();
                    break;
            }
        }
    }

    void AddToNumber(float value, Text text, Text kiosk)
    {
        initialNumber = currentNumber;
        desiredNumber += value;

        currentText = text;
        currentKioskText = kiosk;
    }

    void Init()
    {
        rankStr = Path.Combine(string.Format("{0}", Application.streamingAssetsPath), "Data/RankData.txt");
        sw = new StreamWriter(rankStr, true);

        myScore = DataManager.instance.songData._Score;
    }

    void SetData()
    {
        if(!myScore.Equals(0))
        {
            sw.WriteLine(myScore);
            sw.Close();
        }
        else
        {
            sw.Close();
        }

        rankStrLines = File.ReadAllLines(rankStr);

        rankNum = new int[rankStrLines.Length];
        for (int i = 0; i < rankStrLines.Length; i++)
        {
            //Debug.Log("rankStrLines : " + rankStrLines[i]);
            rankNum[i] = int.Parse(rankStrLines[i]);
        }

        int temp;
        for(int i = 0; i < rankNum.Length - 1; i++)
        {
            for(int j = i + 1; j < rankNum.Length; j++)
            {
                if(rankNum[i] < rankNum[j])
                {
                    temp = rankNum[j];
                    rankNum[j] = rankNum[i];
                    rankNum[i] = temp;
                }
            }
        }

        firstScore = rankNum[0];
        secondScore = rankNum[1];
        thirdScore = rankNum[2];

        rankingState = RankingState.first;
        AddToNumber(firstScore, firstText, kioskFirstText);
    }

    void SaveData()
    {
        StreamWriter sw2 = new StreamWriter(rankStr, false);
        
        for(int i = 0; i < rankNum.Length; i++)
        {
            sw2.WriteLine(rankNum[i]);

            if(i.Equals(99))
            {
                break;
            }
        }

        sw2.Close();
    }
}

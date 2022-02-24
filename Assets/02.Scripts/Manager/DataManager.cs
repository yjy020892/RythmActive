using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums_Game;
using System.IO;
using DATA;

/// <summary>
/// 데이터 관리 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
	public ResultState resultState = ResultState.None;

	private string line;

	private static DataManager _instance;

	[SerializeField] public UIValue uiValue;
	[SerializeField] public GameData gameData;
	[SerializeField] public Pay payData;
	[SerializeField] public SongData songData = new SongData();

	public List<string> chatList = new List<string>();
	public List<string> songList = new List<string>();
	//public List<string> songName = new List<string>();
	public List<string> songArtist = new List<string>();
	public List<string> songBPM = new List<string>();
	public List<string> songLevel = new List<string>();
	//public List<string> songDate = new List<string>();
	public List<string> songContent = new List<string>();
	//public List<string> songSource = new List<string>();

	public static DataManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<DataManager>();

				if (_instance == null)
				{
					GameObject obj = new GameObject("DataManager", typeof(DataManager));
					_instance = obj.GetComponent<DataManager>();
				}
			}

			return _instance;
		}
	}

	/// <summary>
	/// 데이터 불러오기
	/// </summary>
	public void LoadConfigureData()
    {
		string pathBasic = Application.dataPath + "/StreamingAssets/";
		string path = String.Empty;
		string[] textSplit;

		uiValue = new UIValue();

		path = "Data/UIValue.ini";
		using (StreamReader file = new StreamReader(@pathBasic + path))
        {
			while((line = file.ReadLine()) != null)
            {
				if(line.Contains(";") || string.IsNullOrEmpty(line))
                {
					continue;
                }

				if(line.Contains("MAXVALUE"))
                {
					uiValue._MaxValue = float.Parse(line.Split('=')[1]);
				}

				if(line.Contains("SPEED"))
                {
					uiValue._Speed = float.Parse(line.Split('=')[1]);
				}
            }

			file.Close();
			line = string.Empty;
        }

		path = "Data/Pay.ini";
		using (StreamReader file = new StreamReader(@pathBasic + path))
		{
			while ((line = file.ReadLine()) != null)
			{
				if (line.Contains(";") || string.IsNullOrEmpty(line))
				{
					continue;
				}

				if (line.Contains("MONEY"))
				{
					payData._Money = int.Parse(line.Split('=')[1]);
				}
			}

			file.Close();
			line = string.Empty;
		}

		path = "Data/SongList.ini";
		using (StreamReader file = new StreamReader(pathBasic + path))
        {
			while((line = file.ReadLine()) != null)
            {
				if (line.Contains(";") || string.IsNullOrEmpty(line))
				{
					continue;
				}

				textSplit = line.Split(',');

				songList.Add(textSplit[0]);
				songArtist.Add(textSplit[1]);
				songBPM.Add(textSplit[2]);
				songLevel.Add(textSplit[3]);
				//songDate.Add(textSplit[4]);
				songContent.Add(textSplit[4]);
				//songSource.Add(textSplit[6]);
			}

			file.Close();
			line = string.Empty;
		}

		path = "Data/ChatData.ini";
		using (StreamReader file = new StreamReader(pathBasic + path))
		{
			while ((line = file.ReadLine()) != null)
			{
				if (line.Contains(";") || string.IsNullOrEmpty(line))
				{
					continue;
				}

				chatList.Add(line);
			}

			file.Close();
			line = string.Empty;
		}

		gameData = new GameData();
		path = "Data/GameData.ini";
		using (StreamReader file = new StreamReader(@pathBasic + path))
		{
			while ((line = file.ReadLine()) != null)
			{
				if (line.Contains(";") || string.IsNullOrEmpty(line))
				{
					continue;
				}

				switch(line.Split('=')[0])
                {
					case "Silver":
						int.TryParse(line.Split('=')[1], out gameData._Silver);
						break;

					case "Gold":
						int.TryParse(line.Split('=')[1], out gameData._Gold);
						break;

					case "Diamond":
						int.TryParse(line.Split('=')[1], out gameData._Diamond);
						break;

					case "Ruby":
						int.TryParse(line.Split('=')[1], out gameData._Ruby);
						break;

					case "Life":
						int.TryParse(line.Split('=')[1], out gameData._Life);
						break;

					case "Goal":
						int.TryParse(line.Split('=')[1], out gameData._Goal);
						break;

					case "SelectTime":
						int.TryParse(line.Split('=')[1], out gameData._SelectTime);
						break;

					case "WarningTime":
						int.TryParse(line.Split('=')[1], out gameData._WarningTime);
						break;

					case "EndTime":
						int.TryParse(line.Split('=')[1], out gameData._EndTime);
						break;
				}

				//if (line.Contains("Silver"))
				//{
				//	int.TryParse(line.Split('=')[1], out uiData.Silver);
				//}

				//if (line.Contains("Gold"))
				//{
				//	int.TryParse(line.Split('=')[1], out uiData.Gold);
				//}

				//if (line.Contains("Diamond"))
				//{
				//	int.TryParse(line.Split('=')[1], out uiData.Diamond);
				//}

				//if (line.Contains("Ruby"))
				//{
				//	int.TryParse(line.Split('=')[1], out uiData.Silver);
				//}
			}

			file.Close();
			line = string.Empty;
		}
	}
}

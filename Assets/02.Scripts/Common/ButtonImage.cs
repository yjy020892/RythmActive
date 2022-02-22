using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    Image buttonImage;
    public Sprite[] buttonSprite;
    public Text buttonText;

    public bool b_Text;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void ChangeButtonImage(bool b, string colorCode = null, string text = null)
    {
        if(b)
        {
            buttonImage.sprite = buttonSprite[0];
        }
        else
        {
            buttonImage.sprite = buttonSprite[1];
        }
        
        if(b_Text)
        {
            if (colorCode != null || text != null)
            {
                ColorUtility.TryParseHtmlString(colorCode, out Color newColor);
                buttonText.color = newColor;

                if(text != null)
                {
                    buttonText.text = text;
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnMouseDown()
    {
        if(gameObject.CompareTag("LeftArrow"))
        {
            ChangeButtonImage(true);
        }
        else if(gameObject.CompareTag("RightArrow"))
        {
            ChangeButtonImage(true);
        }
        else if(gameObject.CompareTag("Difficult"))
        {
            if (buttonText.text.Equals("EASY"))
            {
                ChangeButtonImage(true, "#E06535");
            }
            else if (buttonText.text.Equals("NORMAL"))
            {
                ChangeButtonImage(true, "#8C90F9");
            }
            else if (buttonText.text.Equals("HARD"))
            {
                ChangeButtonImage(true, "#DC4D66");
            }
        }
        else if(gameObject.CompareTag("Play"))
        {
            ChangeButtonImage(true);
        }
    }

    void OnMouseUp()
    {
        if (gameObject.CompareTag("LeftArrow"))
        {
            if (SongSelectManager.instance.songListNumber.Equals(0))
            {
                SongSelectManager.instance.songListNumber = DataManager.instance.songList.Count - 1;
            }
            else
            {
                SongSelectManager.instance.songListNumber -= 1;
            }

            SongSelectManager.instance.SetSong();
            ChangeButtonImage(false);
        }
        else if (gameObject.CompareTag("RightArrow"))
        {
            if (SongSelectManager.instance.songListNumber.Equals(DataManager.instance.songList.Count - 1))
            {
                SongSelectManager.instance.songListNumber = 0;
            }
            else
            {
                SongSelectManager.instance.songListNumber += 1;
            }

            SongSelectManager.instance.SetSong();
            ChangeButtonImage(false);
        }
        else if (gameObject.CompareTag("Difficult"))
        {
            if (buttonText.text.Equals("EASY"))
            {
                DataManager.instance.songData._Difficult = Enums_Game.SongDifficult.Normal;
                ChangeButtonImage(false, "#8CB3F9", "NORMAL");
            }
            else if (buttonText.text.Equals("NORMAL"))
            {
                DataManager.instance.songData._Difficult = Enums_Game.SongDifficult.Hard;
                ChangeButtonImage(false, "#FF6062", "HARD");
            }
            else if (buttonText.text.Equals("HARD"))
            {
                DataManager.instance.songData._Difficult = Enums_Game.SongDifficult.Easy;
                ChangeButtonImage(false, "#F7944E", "EASY");
            }
        }
        else if (gameObject.CompareTag("Play"))
        {
            ChangeButtonImage(false);

            SongSelectManager.instance.PlaySong();
        }
    }
#endif
}

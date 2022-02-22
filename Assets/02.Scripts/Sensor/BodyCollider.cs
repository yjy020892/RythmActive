using UnityEngine;
using Enums_Game;

public class BodyCollider : MonoBehaviour
{
    Vector2 singleBrokenSize = new Vector2(200.0f, 200.0f);

    void OnTriggerEnter(Collider col)
    {
        if (gameObject.name.Equals("HandLeft"))
        {
            if (col.CompareTag("HandLeft"))
            {
                Note note = col.GetComponentInParent<Note>();

                if (note.noteType.Equals(NoteType.Single))
                {
                    GameManager.instance.SetScore(note, col.transform);

                    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(col.transform, "SingleRedEffect");
                    obj.SetActive(true);

                    obj = PooledManager.instance.GetPooledObject_NoteEffect(GameManager.instance.bigEffectSpawn, "NoteBigEffect");
                    obj.SetActive(true);

                    //note.enabled = false;

                    GameManager.instance.SetRankEffect();
                    GameManager.instance.SpawnHeart();

                    note.b_Fade = false;
                    // 애니메이션
                    //note.singleAnim.SetBool("Hit", true);
                    note.markRT.sizeDelta = singleBrokenSize;
                    Destroy(col.gameObject);
                }
                else if(note.noteType.Equals(NoteType.Long))
                {
                    if(!note.b_Long)
                    {
                        GameManager.instance.SetScore(note, col.transform);

                        GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(transform, "LongRedEffect");
                        obj.SetActive(true);

                        note.b_Long = true;
                        note.outerImg.color = Color.clear;
                        note.pinkTailEffect.SetActive(true);
                        //note.enabled = false;
                        //Destroy(col.gameObject);
                    }
                }
                else if(note.noteType.Equals(NoteType.Press))
                {
                    note.b_Press = true;
                    note.PressOn();
                    GameManager.instance.SetScore(note, col.transform);
                    GameManager.instance.b_Press = false;
                    //GameManager.instance.MoveCamera(true, transform);

                    note.pinkPressEffect.SetActive(true);
                }
            }
        }
        else if(gameObject.name.Equals("HandRight"))
        {
            if (col.CompareTag("HandRight"))
            {
                Note note = col.GetComponentInParent<Note>();

                if(note.noteType.Equals(NoteType.Single))
                {
                    GameManager.instance.SetScore(note, col.transform);

                    GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(col.transform, "SingleBlueEffect");
                    obj.SetActive(true);

                    obj = PooledManager.instance.GetPooledObject_NoteEffect(GameManager.instance.bigEffectSpawn, "NoteBigEffect");
                    obj.SetActive(true);

                    //note.enabled = false;

                    GameManager.instance.SetRankEffect();
                    GameManager.instance.SpawnHeart();

                    note.b_Fade = false;
                    // 애니메이션
                    //note.singleAnim.SetBool("Hit", true);
                    note.markRT.sizeDelta = singleBrokenSize;
                    Destroy(col.gameObject);
                }
                else if(note.noteType.Equals(NoteType.Long))
                {
                    if (!note.b_Long)
                    {
                        GameManager.instance.SetScore(note, col.transform);

                        GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(transform, "LongBlueEffect");
                        obj.SetActive(true);

                        note.b_Long = true;
                        note.outerImg.color = Color.clear;
                        note.blueTailEffect.SetActive(true);
                        //note.enabled = false;
                        //Destroy(col.gameObject);
                    }
                }
                else if (note.noteType.Equals(NoteType.Press))
                {
                    note.b_Press = true;
                    note.PressOn();
                    GameManager.instance.SetScore(note, col.transform);
                    GameManager.instance.b_Press = false;
                    //GameManager.instance.MoveCamera(true, transform);

                    note.bluePressEffect.SetActive(true);
                }
            }
        }

        //if(col.tag.Equals("LeftArrow"))
        //{
        //    col.GetComponent<ButtonImage>().ChangeButtonImage(true);
        //}
        //else if(col.tag.Equals("RightArrow"))
        //{
        //    col.GetComponent<ButtonImage>().ChangeButtonImage(true);
        //}
        //else if(col.tag.Equals("Difficult"))
        //{
        //    SongSelectManager.instance.difficultCount += 1;

        //    if(SongSelectManager.instance.difficultCount > 1)
        //    {
        //        SongSelectManager.instance.difficultCount = 2;

        //        ButtonImage buttonImage = col.GetComponent<ButtonImage>();

        //        if(buttonImage.buttonText.text.Equals("EASY"))
        //        {
        //            buttonImage.ChangeButtonImage(true, "#E06535");
        //        }
        //        else if(buttonImage.buttonText.text.Equals("NORMAL"))
        //        {
        //            buttonImage.ChangeButtonImage(true, "#8C90F9");
        //        }
        //        else if(buttonImage.buttonText.text.Equals("HARD"))
        //        {
        //            buttonImage.ChangeButtonImage(true, "#DC4D66");
        //        }
        //    }
        //}
        //else if (col.tag.Equals("Play"))
        //{
        //    SongSelectManager.instance.playCount += 1;

        //    if (SongSelectManager.instance.playCount > 1)
        //    {
        //        SongSelectManager.instance.playCount = 2;
        //        col.GetComponent<ButtonImage>().ChangeButtonImage(true);
        //    }
        //}
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    if (col.tag.Equals("LeftArrow"))
    //    {
    //        if(SongSelectManager.instance.songListNumber.Equals(0))
    //        {
    //            SongSelectManager.instance.songListNumber = DataManager.instance.songList.Count - 1;
    //        }
    //        else
    //        {
    //            SongSelectManager.instance.songListNumber -= 1;
    //        }

    //        SongSelectManager.instance.SetSong();
    //        col.GetComponent<ButtonImage>().ChangeButtonImage(false);
    //    }
    //    else if (col.tag.Equals("RightArrow"))
    //    {
    //        if(SongSelectManager.instance.songListNumber.Equals(DataManager.instance.songList.Count - 1))
    //        {
    //            SongSelectManager.instance.songListNumber = 0;
    //        }
    //        else
    //        {
    //            SongSelectManager.instance.songListNumber += 1;
    //        }

    //        SongSelectManager.instance.SetSong();
    //        col.GetComponent<ButtonImage>().ChangeButtonImage(false);
    //    }
    //    else if (col.tag.Equals("Difficult"))
    //    {
    //        if(SongSelectManager.instance.difficultCount.Equals(2))
    //        {
    //            SongSelectManager.instance.difficultCount = 0;

    //            ButtonImage buttonImage = col.GetComponent<ButtonImage>();

    //            if (buttonImage.buttonText.text.Equals("EASY"))
    //            {
    //                DataManager.instance.songData._Difficult = SongDifficult.Normal;
    //                buttonImage.ChangeButtonImage(false, "#8CB3F9", "NORMAL");
    //            }
    //            else if (buttonImage.buttonText.text.Equals("NORMAL"))
    //            {
    //                DataManager.instance.songData._Difficult = SongDifficult.Hard;
    //                buttonImage.ChangeButtonImage(false, "#FF6062", "HARD");
    //            }
    //            else if (buttonImage.buttonText.text.Equals("HARD"))
    //            {
    //                DataManager.instance.songData._Difficult = SongDifficult.Easy;
    //                buttonImage.ChangeButtonImage(false, "#F7944E", "EASY");
    //            }
    //        }
    //        else if(SongSelectManager.instance.difficultCount.Equals(1))
    //        {
    //            SongSelectManager.instance.difficultCount = 0;
    //        }
    //    }
    //    else if (col.tag.Equals("Play"))
    //    {
    //        if(SongSelectManager.instance.playCount.Equals(2))
    //        {
    //            SongSelectManager.instance.playCount = 0;

    //            col.GetComponent<ButtonImage>().ChangeButtonImage(false);

    //            SongSelectManager.instance.PlaySong();
    //        }
    //        else if (SongSelectManager.instance.playCount.Equals(1))
    //        {
    //            SongSelectManager.instance.playCount = 0;
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider col)
    {
        Note note = col.GetComponentInParent<Note>();

        if(note != null)
        {
            if (note.noteType.Equals(NoteType.Press))
            {
                if (note.pressOntime >= 0.1f)
                {
                    GameManager.instance.SetScore(note, transform);

                    if (gameObject.CompareTag("HandLeft"))
                    {
                        //Instantiate(effectRedTest, transform.position, Quaternion.identity);
                        GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(transform, "PressRedEffect");
                        obj.SetActive(true);
                    }
                    else if (gameObject.CompareTag("HandRight"))
                    {
                        //Instantiate(effectBlueTest, transform.position, Quaternion.identity);
                        GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(transform, "PressBlueEffect");
                        obj.SetActive(true);
                    }
                }
                else if (note.pressOntime < 0.1f)
                {
                    GameObject obj = PooledManager.instance.GetPooledObject_GameText(transform, "MISS");
                    GameManager.instance.comboCnt = 0;
                    GameManager.instance.missCnt += 1;

                    //if(!GameManager.instance.b_Fever)
                    //{
                    //    GameManager.instance.SetGage(0.02f);
                    //}

                    obj.SetActive(true);
                }

                //GameManager.instance.MoveCamera(false, transform);

                Destroy(col.gameObject);
            }
        }
    }
}

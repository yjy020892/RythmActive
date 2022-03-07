using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameText : MonoBehaviour
{
    [SerializeField] private GameObject readyUI, startUI, blackWall, startBar, completeUI, completeFire, completeSparke;
    [SerializeField] private Text titleText, levelText, bpmText;
    private Animator anim;

    float timer = 0.8f;
    int life;

    private void Awake()
    {
        TryGetComponent(out anim);

        if (gameObject.name.Equals("StartBar"))
        {
            titleText.text = string.Format("{0} {1}", DataManager.instance.songData._SongName, DataManager.instance.songData._Artist);
            levelText.text = string.Format("Level : {0}", DataManager.instance.songData._Difficult.ToString());
            bpmText.text = string.Format("BPM : {0}", DataManager.instance.songData._BPM);
        }
        else if(gameObject.name.Equals("CompleteUI"))
        {
            completeFire.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.CompareTag("Note"))
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0.8f;
                PooledManager.instance.poolObjs_GameText.Remove(gameObject);
                transform.SetAsLastSibling();
                PooledManager.instance.poolObjs_GameText.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    void CompleteEffect()
    {
        completeSparke.SetActive(true);
    }

    void OffsetUI()
    {
        if (gameObject.name.Equals("ReadyUI"))
        {
            readyUI.SetActive(false);
            startUI.SetActive(true);
        }
        else if(gameObject.name.Equals("StartUI"))
        {
            startUI.SetActive(false);
            blackWall.SetActive(false);
            GameManager.instance.gameState = Enums_Game.GameState.Play;
        }
        else if(gameObject.name.Equals("CompleteUI"))
        {
            life = DataManager.instance.gameData._Life;

            //SceneController.instance.GotoScene(Enums_Common.SceneType.Result);

            if (BaseManager.instance.GameCnt.Equals(life))
            {
                DataManager.instance.resultState = Enums_Game.ResultState.LifeEnd;
                SceneController.instance.GotoScene(Enums_Common.SceneType.GameOver);
            }
            else if(BaseManager.instance.GameCnt < life)
            {
                if(DataManager.instance.songData._Rank >= DataManager.instance.gameData._Goal)
                {
                    SceneController.instance.GotoScene(Enums_Common.SceneType.SongSelect);
                }
                else
                {
                    DataManager.instance.resultState = Enums_Game.ResultState.ScoreEnd;
                    SceneController.instance.GotoScene(Enums_Common.SceneType.GameOver);
                }
            }
        }
        else if(gameObject.name.Equals("StartBlackWall"))
        {
            startBar.SetActive(true);
            blackWall.SetActive(false);
            GameManager.instance.gameState = Enums_Game.GameState.Play;
        }
        else if (gameObject.name.Equals("StartBar"))
        {
            if(!anim.GetBool("Sky") && !anim.GetBool("Blue"))
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        anim.SetBool("Sky", true);
                        break;

                    case 1:
                        anim.SetBool("Blue", true);
                        break;
                }
            }

            if(!DataManager.instance.resultState.Equals(Enums_Game.ResultState.None))
            {
                //completeEffect.SetActive(true);
                completeUI.SetActive(true);
            }
        }
        else if(gameObject.name.Equals("back"))
        {
            if(anim.GetBool("Title"))
            {
                anim.SetBool("Title", false);
                anim.SetBool("Badge", true);
            }
            else if(anim.GetBool("Badge"))
            {
                anim.SetBool("Badge", false);
                anim.SetBool("Content", true);

                SongSelectManager.instance.AddToNumber(DataManager.instance.songData._Score);
            }
        }
    }
}

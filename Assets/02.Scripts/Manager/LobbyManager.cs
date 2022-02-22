using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums_Common;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject drawColor;

    [SerializeField]
    private GameObject skeleton;

    [SerializeField]
    private Transform cameraUI;

    [SerializeField]
    private Button testSingleBtn;

    static private LobbyManager _instance;

    public static LobbyManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LobbyManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        testSingleBtn.onClick.AddListener(GotoScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(string.Format("Screen Width : {0}, Height : {1}", Screen.width, Screen.height));

        //cameraUI.localPosition = new Vector3(8.41f, 5.3f);

        if (Screen.width == 1920 && Screen.height == 1200)
        {
            Debug.Log("1920x1200");
            cameraUI.localPosition = new Vector3(8.41f, 5.3f);
        }
        else if (Screen.width == 1920 && Screen.height == 1080)
        {
            Debug.Log("1920x1080");
            cameraUI.localPosition = new Vector3(8.41f, 4.6f);
        }
    }

    public void GotoScene()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;

        switch (btn.name)
        {
            case "Back":
                Debug.Log("Back");
                //drawColor.SetActive(false);
                //Destroy(drawColor);
                //Destroy(skeleton);
                //NuitrackManager.Instance.CloseUserGen();

                SceneController.instance.GotoScene(SceneType.Game_Single);
                break;
        }
    }
}

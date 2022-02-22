using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Game;

public class BaseManager : MonoBehaviour
{
    public GameMode gameMode = GameMode.None;

    private static BaseManager _instance;
    [HideInInspector] public int gameCnt = 0;

    public GameObject poolObj_TouchEffect;
    public GameObject group_TouchEffect;
    public int poolAmount_TouchEffect;
    [HideInInspector] public List<GameObject> poolObjs_TouchEffect = new List<GameObject>();

    [HideInInspector] public Vector3 mousePosition;

    public static BaseManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BaseManager>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < poolAmount_TouchEffect; i++)
        {
            GameObject obj_TouchEffect = Instantiate(poolObj_TouchEffect, group_TouchEffect.transform, false);

            obj_TouchEffect.name = "TouchEffect";
            //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

            obj_TouchEffect.SetActive(false);
            poolObjs_TouchEffect.Add(obj_TouchEffect);
        }

        //Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 마우스 포지션값 리턴
    /// </summary>
    /// <param name="cam">카메라</param>
    public Vector3 SetMousePosition(Camera cam)
    {
        mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);

        return mousePosition;
    }

    public GameObject GetPooledObject_TouchEffect(Camera cam)
    {
        Vector3 vec = SetMousePosition(cam);
        vec.z = 0;

        for (int i = 0; i < poolObjs_TouchEffect.Count; i++)
        {
            if (!poolObjs_TouchEffect[i].activeInHierarchy)
            {
                poolObjs_TouchEffect[i].transform.SetPositionAndRotation(vec, Quaternion.identity);

                return poolObjs_TouchEffect[i];
            }
        }

        return null;
    }
}

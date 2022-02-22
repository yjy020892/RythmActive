using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledManager : MonoBehaviour
{
    public static PooledManager instance;

    // ----- GameText -----
    public GameObject[] poolObj_GameText;
    public GameObject group_GameText;
    public int poolAmount_GameText;
    [HideInInspector] public List<GameObject> poolObjs_GameText = new List<GameObject>();

    public Transform spawnGameTextPoint;
    // --------------------

    // ----- Note Effect -----
    public GameObject[] poolObj_NoteEffect;
    public GameObject group_NoteEffect;
    public GameObject group_BigNoteEffect;
    public int poolAmount_NoteEffect;
    [HideInInspector] public List<GameObject> poolObjs_NoteEffect = new List<GameObject>();

    public Transform spawnNoteEffectPoint;
    // --------------------

    // ----- Note Effect -----
    public GameObject group_PreviewEffect;
    // --------------------

    // ----- Heart Effect -----
    public GameObject[] poolObj_HeartEffect;
    public GameObject group_HeartEffect;
    public GameObject group_KioskHeartEffect;
    public int poolAmount_HeartEffect;
    [HideInInspector] public List<GameObject> poolObjs_HeartEffect = new List<GameObject>();
    // --------------------

    // ----- ChatBox -----
    public GameObject poolObj_ChatBox;
    public GameObject group_ChatBox;
    public int poolAmount_ChatBox;
    [HideInInspector] public List<GameObject> poolObjs_ChatBox = new List<GameObject>();

    public Transform spawnChatBoxPoint;
    // --------------------

    void Awake()
    {
        if (PooledManager.instance == null)
        {
            PooledManager.instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolAmount_GameText; i++)
        {
            for(int j = 0; j < poolObj_GameText.Length; j++)
            {
                GameObject obj_GameText = Instantiate(poolObj_GameText[j], spawnGameTextPoint.position, Quaternion.identity);
                
                switch(j)
                {
                    case 0:
                        obj_GameText.name = "MISS";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);
                        break;

                    case 1:
                        obj_GameText.name = "GOOD";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);
                        break;

                    case 2:
                        obj_GameText.name = "PERFECT";
                        //obj_GameText.transform.localScale = Vector3.one;
                        break;
                }

                //obj_GameText.transform.SetParent(group_GameText.transform);
                obj_GameText.transform.parent = group_GameText.transform;
                obj_GameText.SetActive(false);
                poolObjs_GameText.Add(obj_GameText);
            }
        }

        for (int i = 0; i < poolAmount_NoteEffect; i++)
        {
            for(int j = 0; j < poolObj_NoteEffect.Length; j++)
            {
                GameObject obj_NoteEffect = Instantiate(poolObj_NoteEffect[j], spawnNoteEffectPoint.position, Quaternion.identity);

                switch (j)
                {
                    case 0:
                        obj_NoteEffect.name = "SingleRedEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 1:
                        obj_NoteEffect.name = "SingleBlueEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 2:
                        obj_NoteEffect.name = "LongRedEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 3:
                        obj_NoteEffect.name = "LongBlueEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 4:
                        obj_NoteEffect.name = "PressRedEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 5:
                        obj_NoteEffect.name = "PressBlueEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 6:
                        obj_NoteEffect.name = "PressBeforeRedEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 7:
                        obj_NoteEffect.name = "PressBeforeBlueEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 8:
                        obj_NoteEffect.name = "SingleRedBornEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 9:
                        obj_NoteEffect.name = "SingleBlueBornEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_NoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 10:
                        obj_NoteEffect.name = "PreviewEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_PreviewEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;

                    case 11:
                        obj_NoteEffect.name = "NoteBigEffect";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_NoteEffect.transform.parent = group_BigNoteEffect.transform;
                        obj_NoteEffect.transform.localScale = Vector3.one;
                        obj_NoteEffect.transform.localPosition = Vector3.zero;
                        obj_NoteEffect.SetActive(false);
                        poolObjs_NoteEffect.Add(obj_NoteEffect);
                        break;
                }

                //obj_GameText.transform.SetParent(group_GameText.transform);
            }
        }

        for (int i = 0; i < poolAmount_HeartEffect; i++)
        {
            for (int j = 0; j < poolObj_HeartEffect.Length; j++)
            {
                GameObject obj_HeartEffect = Instantiate(poolObj_HeartEffect[j]);

                switch (j)
                {
                    case 0:
                        obj_HeartEffect.name = "HeartEffectScreen";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_HeartEffect.transform.SetParent(group_HeartEffect.transform);
                        obj_HeartEffect.transform.localScale = Vector3.one;
                        obj_HeartEffect.SetActive(false);
                        poolObjs_HeartEffect.Add(obj_HeartEffect);
                        break;

                    case 1:
                        obj_HeartEffect.name = "HeartEffectKiosk";
                        //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                        obj_HeartEffect.transform.SetParent(group_KioskHeartEffect.transform);
                        obj_HeartEffect.transform.localScale = Vector3.one;
                        obj_HeartEffect.SetActive(false);
                        poolObjs_HeartEffect.Add(obj_HeartEffect);
                        break;
                }

                //obj_GameText.transform.SetParent(group_GameText.transform);
            }
        }

        for (int i = 0; i < poolAmount_ChatBox; i++)
        {
            GameObject obj_ChatBox = Instantiate(poolObj_ChatBox, group_ChatBox.transform, false);

            obj_ChatBox.name = "ChatBox";
            //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

            obj_ChatBox.SetActive(false);
            poolObjs_ChatBox.Add(obj_ChatBox);
        }
    }

    public GameObject GetPooledObject_GameText(Transform posi, string gameText)
    {
        for (int i = 0; i < poolObjs_GameText.Count; i++)
        {
            if (poolObjs_GameText[i].name.Equals(gameText) && !poolObjs_GameText[i].activeInHierarchy)
            {
                poolObjs_GameText[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);

                return poolObjs_GameText[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject_NoteEffect(Transform posi, string colorStr)
    {
        for (int i = 0; i < poolObjs_NoteEffect.Count; i++)
        {
            if (poolObjs_NoteEffect[i].name.Equals(colorStr) && !poolObjs_NoteEffect[i].activeInHierarchy)
            {
                poolObjs_NoteEffect[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);
                //poolObjs_NoteEffect[i].transform.localScale = Vector3.one;

                return poolObjs_NoteEffect[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject_NoteEffect(string colorStr)
    {
        for (int i = 0; i < poolObjs_NoteEffect.Count; i++)
        {
            if (poolObjs_NoteEffect[i].name.Equals(colorStr) && !poolObjs_NoteEffect[i].activeInHierarchy)
            {
                //poolObjs_NoteEffect[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);
                //poolObjs_NoteEffect[i].transform.localScale = Vector3.one;

                return poolObjs_NoteEffect[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject_HeartEffect(Transform posi, string kind)
    {
        for (int i = 0; i < poolObjs_HeartEffect.Count; i++)
        {
            if (poolObjs_HeartEffect[i].name.Equals(kind) && !poolObjs_HeartEffect[i].activeInHierarchy)
            {
                poolObjs_HeartEffect[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);
                //poolObjs_NoteEffect[i].transform.localScale = Vector3.one;

                return poolObjs_HeartEffect[i];
            }
        }

        return null;
    }

    public GameObject GetPooledObject_ChatBox()
    {
        for (int i = 0; i < poolObjs_ChatBox.Count; i++)
        {
            if (!poolObjs_ChatBox[i].activeInHierarchy)
            {
                //poolObjs_ChatBox[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);

                return poolObjs_ChatBox[i];
            }
        }

        return null;
    }
}

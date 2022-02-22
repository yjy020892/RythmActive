using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Game;

public class CircleNote : MonoBehaviour
{
    public Note note;

    [HideInInspector] public NoteType noteType;

    SphereCollider sc;
    Transform tr;

    private void Start()
    {
        tr = GetComponent<Transform>();
        sc = GetComponent<SphereCollider>();
    }

    public void OnMouseEnter()
    {
//#if UNITY_EDITOR
        if(!note.b_Long)
        {
            GameManager.instance.SetScore(note, tr);

            //note.b_Fade = false;
            note.b_Long = true;
            //sc.enabled = false;
            note.outerImg.color = Color.clear;

            if (gameObject.CompareTag("HandLeft"))
            {
                //Instantiate(effectRedTest, transform.position, Quaternion.identity);
                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "LongRedEffect");
                obj.SetActive(true);
                note.pinkTailEffect.SetActive(true);

                Debug.Log("LeftLongNote");
            }
            else if (gameObject.CompareTag("HandRight"))
            {
                //Instantiate(effectBlueTest, transform.position, Quaternion.identity);
                GameObject obj = PooledManager.instance.GetPooledObject_NoteEffect(tr, "LongBlueEffect");
                obj.SetActive(true);
                note.blueTailEffect.SetActive(true);

                Debug.Log("RightLongNote");
            }

            //Destroy(gameObject);
        }


        //#endif
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("LineEnd"))
        {
            Destroy(note.gameObject);
        }
    }
}

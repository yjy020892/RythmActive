using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private bool b_Pooled = true;

    float t;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if(timer <= t)
        {
            t = 0;

            if(b_Pooled)
            {
                if (gameObject.name.Equals("HeartEffectScreen") || gameObject.name.Equals("HeartEffectKiosk"))
                {
                    PooledManager.instance.poolObjs_HeartEffect.Remove(gameObject);
                    transform.SetAsLastSibling();
                    PooledManager.instance.poolObjs_HeartEffect.Add(gameObject);
                    gameObject.SetActive(false);
                }
                else if(gameObject.name.Equals("TouchEffect"))
                {
                    BaseManager.instance.poolObjs_TouchEffect.Remove(gameObject);
                    transform.SetAsLastSibling();
                    BaseManager.instance.poolObjs_TouchEffect.Add(gameObject);
                    gameObject.SetActive(false);
                }
                else
                {
                    PooledManager.instance.poolObjs_NoteEffect.Remove(gameObject);
                    transform.SetAsLastSibling();
                    PooledManager.instance.poolObjs_NoteEffect.Add(gameObject);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if(gameObject.name.Equals("preview_effect"))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopNote : MonoBehaviour
{
    [SerializeField] RectTransform oriPosi;
    [SerializeField] RectTransform centerPosi;
    public string _dir;

    RectTransform tr;

    [SerializeField] float _speed;
    float moveTimer;
    float songSPB = 0.0f;

    bool b_Move = false;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if(songSPB.Equals(0.0f))
        {
            songSPB = DataManager.instance.songData._SPB8;
            Debug.Log(songSPB);
        }
        moveTimer = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {
        //if(!b_Move)
        //{
        //    StartCoroutine(UpdateMove(oriPosi.anchoredPosition, centerPosi.anchoredPosition, songSPB));
        //}

        //if (moveTimer <= 0.3f)
        //{
        //    moveTimer += Time.deltaTime;
        //}
        //else
        //{
            switch (_dir)
            {
                case "Left":
                    tr.Translate(Vector3.right * _speed * Time.deltaTime);

                    if (tr.anchoredPosition.x >= 0)
                    {
                        //tr.position = oriPosi.position;
                        //gameObject.SetActive(false);
                        //moveTimer = 0.0f;
                    }
                    break;

                case "Right":
                    tr.Translate(Vector3.left * _speed * Time.deltaTime);

                    if (tr.anchoredPosition.x <= 0)
                    {
                        //tr.position = oriPosi.position;
                        //gameObject.SetActive(false);
                        //moveTimer = 0.0f;
                    }
                    break;
            }
        //}
    }

    private IEnumerator UpdateMove(Vector2 startPos, Vector2 targetPos, float duration)
    {
        float timer = 0f;

        // 이동 시작 위치 설정
        Vector2 position = startPos;
        tr.anchoredPosition = position;

        // 시간에 따른 위치 설정
        while (timer < duration)
        {
            timer += Time.deltaTime;

            position.x = Mathf.Lerp(startPos.x, targetPos.x, timer / duration);
            position.y = Mathf.Lerp(startPos.y, targetPos.y, timer / duration);

            tr.anchoredPosition = position;

            yield return null;
        }

        // 이동 종료 위치 설정
        //position = targetPos;
        position = startPos;
        tr.anchoredPosition = position;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] Text chatTxt;

    private void OnEnable()
    {
        chatTxt.text = DataManager.instance.chatList[Random.Range(0, DataManager.instance.chatList.Count)];
    }

    private void OnDisable()
    {
        chatTxt.text = string.Empty;
    }
}

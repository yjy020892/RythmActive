using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iSceneChangeReceiver
{
    /// <summary>
    /// 씬이 바뀐후 Awake, OnEnable <  > Start 사이에 호출됨
    /// 이벤트 받을 오브젝트에 "SceneChangeReceiver" Tag 필요함
    /// </summary>
    /// <param name="param">(Enums_Common.SceneType)param</param>
    void OnSceneChange(object param);
}

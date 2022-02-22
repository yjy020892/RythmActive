using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iSceneChangeReceiver
{
    /// <summary>
    /// ���� �ٲ��� Awake, OnEnable <  > Start ���̿� ȣ���
    /// �̺�Ʈ ���� ������Ʈ�� "SceneChangeReceiver" Tag �ʿ���
    /// </summary>
    /// <param name="param">(Enums_Common.SceneType)param</param>
    void OnSceneChange(object param);
}

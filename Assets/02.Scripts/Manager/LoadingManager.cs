using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums_Common;

public class LoadingManager : MonoBehaviour, iSceneChangeReceiver
{
    public SceneType nextLoadScene;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        //Debug.Log("Loading NextScene : " + nextLoadScene.ToString());
        SceneController.instance.GotoScene(nextLoadScene);
    }

    /// <summary>
	/// SceneController에서 호출됨(Awake, OnEnable 과 Start 사이에 호출됨)
	/// </summary>
	/// <param name="param">param[0] = Next SceneType</param>
	public void OnSceneChange(object param)
    {
        if (param != null)
        {
            nextLoadScene = (SceneType)param;
        }
    }
}

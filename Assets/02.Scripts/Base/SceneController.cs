using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums_Common;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    private SceneType _preScene;
    private SceneType _currentScene;
    private SceneType _nextScene;
    public ScreenEffect _screenEffect;

    private bool _isLoading;

    public GameObject baseCamera;
    public GameObject baseUICamera;

    public SceneType CURRENT_SCENETYPE
    {
        get { return _currentScene; }
    }

    public static SceneController instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SceneController>();

                if(_instance == null)
                {
                    return null;
                }

                _instance.Init();
            }

            return _instance;
        }
    }

    private void Init(Action callback = null)
    {
        //이벤트 등록
        SceneManager.activeSceneChanged += EventSceneChanged;
        SceneManager.sceneLoaded += EventSceneLoaded;
        SceneManager.sceneUnloaded += EventSceneUnloaded;

        _isLoading = false;
    }

    public void GotoScene(SceneType sceneNext)
    {
        if (_isLoading)
            return;

        if (baseCamera.activeInHierarchy/* || baseUICamera.activeInHierarchy*/)
        {
            baseCamera.SetActive(false);
            //baseUICamera.SetActive(false);
        }

        _isLoading = true;

        //Debug.Log("[00] GotoScene [" + sceneNext + "]");

        _preScene = _currentScene;  // 로딩
        _currentScene = (_preScene != SceneType.Loading) ? SceneType.Loading : sceneNext;
        _nextScene = sceneNext;

        //페이드 아웃 이펙트(화면가림)
        _screenEffect.ScreenFadeEffect(Color.black, false, 0.0f, 0.5f, 1.0f, result =>
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(_currentScene.ToString(), LoadSceneMode.Additive);
            //씬 로딩 완료후 호출됨
            StartCoroutine(WaitForSceneAsync(operation, () =>
            {
                //Debug.Log("[00][LoadingScene]로드 완료됨");

                if (_preScene != SceneType.Base)
                {
                    operation = SceneManager.UnloadSceneAsync(_preScene.ToString());
                    //이전씬 언로드 완료후 호출됨
                    StartCoroutine(WaitForSceneAsync(operation, () =>
                    {
                        _screenEffect.ScreenFadeEffectByLastSetting(true);
                        _isLoading = false;
                    }));
                }
                else
                {
                    _screenEffect.ScreenFadeEffectByLastSetting(true);
                    _isLoading = false;
                }
            }));
        });
    }

    /// <summary>
	/// 씬을 언로드 될때까지 대기후 콜백 실행함
	/// </summary>
	private IEnumerator WaitForSceneAsync(AsyncOperation operation, Action callback = null)
    {
        while (!operation.isDone)
        {
            yield return null;
        }

        if (callback != null)
        {
            callback();
        }
    }

    #region Scene Callback Event
    /// <summary>
    /// 씬로딩 완료후 호출
    /// </summary>
    /// <param name="loadScene">로드된 씬</param>
    /// <param name="mode">씬모드 정보</param>
    private void EventSceneLoaded(Scene loadScene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(loadScene);

        //Debug.Log("[01] Loaded " + loadScene.name + " Scene");

        GameObject[] objs = GameObject.FindGameObjectsWithTag("SceneChangeReceiver");
        int count = objs.Length;
        for (int i = 0; i < count; i++)
        {
            objs[i].gameObject.SendMessage("OnSceneChange", (object)_nextScene, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// 씬 변경후 호출
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    private void EventSceneChanged(Scene before, Scene after)
    {
        //Debug.Log("[02] ChangeScene [" + before.name + "]" + "  =====>  " + "[" + after.name + "]");
    }

    /// <summary>
    /// 씬 언로드 완료후 호출
    /// </summary>
    /// <param name="unloadScene"></param>
    private void EventSceneUnloaded(Scene unloadScene)
    {
        //Debug.Log("[03] UnloadScene [" + unloadScene.name + "]");
    }
    #endregion
}

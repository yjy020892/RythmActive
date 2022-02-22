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
        //�̺�Ʈ ���
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

        _preScene = _currentScene;  // �ε�
        _currentScene = (_preScene != SceneType.Loading) ? SceneType.Loading : sceneNext;
        _nextScene = sceneNext;

        //���̵� �ƿ� ����Ʈ(ȭ�鰡��)
        _screenEffect.ScreenFadeEffect(Color.black, false, 0.0f, 0.5f, 1.0f, result =>
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(_currentScene.ToString(), LoadSceneMode.Additive);
            //�� �ε� �Ϸ��� ȣ���
            StartCoroutine(WaitForSceneAsync(operation, () =>
            {
                //Debug.Log("[00][LoadingScene]�ε� �Ϸ��");

                if (_preScene != SceneType.Base)
                {
                    operation = SceneManager.UnloadSceneAsync(_preScene.ToString());
                    //������ ��ε� �Ϸ��� ȣ���
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
	/// ���� ��ε� �ɶ����� ����� �ݹ� ������
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
    /// ���ε� �Ϸ��� ȣ��
    /// </summary>
    /// <param name="loadScene">�ε�� ��</param>
    /// <param name="mode">����� ����</param>
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
    /// �� ������ ȣ��
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    private void EventSceneChanged(Scene before, Scene after)
    {
        //Debug.Log("[02] ChangeScene [" + before.name + "]" + "  =====>  " + "[" + after.name + "]");
    }

    /// <summary>
    /// �� ��ε� �Ϸ��� ȣ��
    /// </summary>
    /// <param name="unloadScene"></param>
    private void EventSceneUnloaded(Scene unloadScene)
    {
        //Debug.Log("[03] UnloadScene [" + unloadScene.name + "]");
    }
    #endregion
}

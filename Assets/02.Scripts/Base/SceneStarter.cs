using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums_Common;

public class SceneStarter : MonoBehaviour
{
    private Camera _mainCam;

    private void Awake()
    {
        //SetResolution();
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();

        //Load Setting configure
        DataManager.instance.LoadConfigureData();
        
        // 외부로 빌드시 주석풀기
        //Cursor.visible = false;

        BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
        SceneController.instance.GotoScene(SceneType.FirstLoading);
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            _mainCam.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            _mainCam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}

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
        
        // �ܺη� ����� �ּ�Ǯ��
        //Cursor.visible = false;

        BaseManager.instance.gameMode = Enums_Game.GameMode.Single;
        SceneController.instance.GotoScene(SceneType.FirstLoading);
    }

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution()
    {
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            _mainCam.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            _mainCam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}

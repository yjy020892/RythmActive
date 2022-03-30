using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KioskWebCam : MonoBehaviour
{
    public RawImage _target = null;

    protected WebCamTexture textureWebCam = null;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        int selectedCameraIndex = -1;

        for(int i = 0; i < devices.Length; i++)
        {
            Debug.Log("Available Webcam: " + devices[i].name + ((devices[i].isFrontFacing) ? "(Front)" : "(Back)"));

            if(devices[i].isFrontFacing)
            {
                if(!devices[i].name.Contains("Astra"))
                {
                    selectedCameraIndex = i;
                    break;
                }
            }
        }

        if(selectedCameraIndex >= 0)
        {
            textureWebCam = new WebCamTexture(devices[selectedCameraIndex].name, 1280, 720, 60);

            //if(textureWebCam)
            //{
            //    textureWebCam.requestedFPS = 60;
            //}
        }

        if(textureWebCam)
        {
            //Renderer render = _target.GetComponent<Renderer>();

            //render.material.mainTexture = textureWebCam;
            _target.texture = textureWebCam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(textureWebCam)
            {
                if(!textureWebCam.isPlaying)
                {
                    textureWebCam.Play();
                }
                else
                {
                    textureWebCam.Stop();
                }
            }
        }
    }

    void OnDestroy()
    {
        if (textureWebCam != null)
        {
            textureWebCam.Stop();
            WebCamTexture.Destroy(textureWebCam);
            textureWebCam = null;
        }
    }
}

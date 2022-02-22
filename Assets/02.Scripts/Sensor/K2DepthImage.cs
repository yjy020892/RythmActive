using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K2DepthImage : MonoBehaviour
{
	[Tooltip("RawImage used to display the depth image.")]
	public UnityEngine.UI.RawImage backgroundImage;

	[Tooltip("Camera used to display the background image.")]
	public Camera backgroundCamera;

	[Tooltip("Whether to use the texture-2d option of the user image (may lower the performance).")]
	public bool useTexture2D = false;

	public GameObject particleObj;
	public Material particleMat;

	void Start()
	{
		if (backgroundImage == null)
		{
			backgroundImage = GetComponent<UnityEngine.UI.RawImage>();
		}
	}

	void Update()
	{
        //if (GameManager.instance.gameState.Equals(Enums_Game.GameState.Sensor))
        //{
        //    if (!backgroundImage.rectTransform.offsetMin.x.Equals(0) || !backgroundImage.rectTransform.offsetMin.y.Equals(0))
        //    {
        //        backgroundImage.rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
        //        backgroundImage.rectTransform.offsetMax = new Vector2(0.0f, 0.0f);
        //    }
        //}

        KinectManager manager = KinectManager.Instance;

		if (manager && manager.IsInitialized())
		{
			if (backgroundImage && (backgroundImage.texture == null))
			{
				backgroundImage.texture = !useTexture2D ? manager.GetUsersLblTex() : manager.GetUsersLblTex2D();

				backgroundImage.color = new Color32(255, 255, 255, 200);
				//backgroundImage.color = Color.white;

				if(particleObj)
                {
					particleObj.SetActive(true);
					particleMat.SetTexture("_MainTex", backgroundImage.texture);
				}
				
				KinectInterop.SensorData sensorData = manager.GetSensorData();
				if (sensorData != null && sensorData.sensorInterface != null && backgroundCamera != null)
				{
					// get depth image size
					int depthImageWidth = sensorData.depthImageWidth;
					int depthImageHeight = sensorData.depthImageHeight;

					// calculate insets
					Rect cameraRect = backgroundCamera.pixelRect;
					float rectWidth = cameraRect.width;
					float rectHeight = cameraRect.height;

					if (rectWidth > rectHeight)
						rectWidth = rectHeight * depthImageWidth / depthImageHeight;
					else
						rectHeight = rectWidth * depthImageHeight / depthImageWidth;

					float deltaWidth = cameraRect.width - rectWidth;
					float deltaHeight = cameraRect.height - rectHeight;

					//					float leftX = deltaWidth / 2;
					//					float rightX = -deltaWidth;
					//					float bottomY = -deltaHeight / 2;
					//					float topY = deltaHeight;
					//
					//					backgroundImage.pixelInset = new Rect(leftX, bottomY, rightX, topY);

					RectTransform rectImage = backgroundImage.GetComponent<RectTransform>();
					if (rectImage)
					{
						rectImage.sizeDelta = new Vector2(-deltaWidth, -deltaHeight);
					}

					rectImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920);
					rectImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);
				}
			}
		}
	}
}

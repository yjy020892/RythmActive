using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K2SkeletonTest : MonoBehaviour
{
	KinectInterop.JointType[] jointsInfo = new KinectInterop.JointType[]
	{
		KinectInterop.JointType.Head,
        //nuitrack.JointType.Neck,
        //nuitrack.JointType.LeftCollar,
        //nuitrack.JointType.Torso,
        //nuitrack.JointType.Waist,
        //nuitrack.JointType.LeftShoulder,
        //nuitrack.JointType.RightShoulder,
        //nuitrack.JointType.LeftElbow,
        //nuitrack.JointType.RightElbow,
        //nuitrack.JointType.LeftWrist,
        //nuitrack.JointType.RightWrist,
        KinectInterop.JointType.HandLeft,
		KinectInterop.JointType.HandRight
        //nuitrack.JointType.LeftHip,
        //nuitrack.JointType.RightHip,
        //nuitrack.JointType.LeftKnee,
        //nuitrack.JointType.RightKnee,
        //nuitrack.JointType.LeftAnkle,
        //nuitrack.JointType.RightAnkle
    };

	//	[Tooltip("GUI-texture used to display the color camera feed on the scene background.")]
	//	public GUITexture backgroundImage;

	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("Game object used to represent the body joints.")]
	public GameObject jointPrefab;

	[Tooltip("Line object used to represent the bones between joints.")]
	//public LineRenderer linePrefab;
	public GameObject linePrefab;

	[Tooltip("Camera that will be used to represent the Kinect-sensor's point of view in the scene.")]
	public Camera kinectCamera;

	[Tooltip("Body scale factors in X,Y,Z directions.")]
	public Vector3 scaleFactors = Vector3.one;


	//public UnityEngine.UI.Text debugText;

	//private LineRenderer[] lines = null;
	//private GameObject[] lines = null;

	//private Quaternion initialRotation = Quaternion.identity;

	public Transform canvas;
	Dictionary<KinectInterop.JointType, RectTransform> jointsDic;

	RectTransform parentRect;

	void Start()
	{
		KinectManager manager = KinectManager.Instance;
		parentRect = transform.parent.GetComponent<RectTransform>();

		CreateSkeletonParts(manager);

		// always mirrored
		//initialRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
	}

	void CreateSkeletonParts(KinectManager manager)
	{
		jointsDic = new Dictionary<KinectInterop.JointType, RectTransform>();

		if (manager && manager.IsInitialized())
		{
			int jointsCount = manager.GetJointCount();

			if (jointPrefab)
			{
				for (int i = 0; i < jointsInfo.Length; i++)
				{
					GameObject joint = Instantiate(jointPrefab, transform);
					joint.SetActive(false);
					joint.transform.SetParent(canvas);
					//joint.transform.parent = transform;
					joint.name = jointsInfo[i].ToString();

					RectTransform jointRectTransform = joint.GetComponent<RectTransform>();
					jointsDic.Add(jointsInfo[i], jointRectTransform);
				}
			}

			// array holding the skeleton lines
			//lines = new LineRenderer[jointsCount];
			//lines = new GameObject[jointsCount];
		}
	}

	
	void Update()
	{
		KinectManager manager = KinectManager.Instance;

		if (manager && manager.IsInitialized())
		{
			if (manager.IsUserDetected(playerIndex))
			{
				long userId = manager.GetUserIdByIndex(playerIndex);
				int jointsCount = manager.GetJointCount();

				for (int i = 0; i < jointsCount; i++)
				{
					for (int j = 0; j < jointsInfo.Length; j++)
					{
						if (jointsDic[jointsInfo[j]].gameObject.name.Equals(((KinectInterop.JointType)i).ToString()))
						{
							if (manager.GetJointData(userId, i).Confidence > 0.01f)
							{
								Vector3 posJoint = !kinectCamera ? manager.GetJointPosition(userId, i) : manager.GetTestPosition(userId, i);

								jointsDic[jointsInfo[j]].gameObject.SetActive(true);

								Vector2 newPosition = new Vector2(
											parentRect.rect.width * (posJoint.x - 0.5f),
											parentRect.rect.height * (0.5f - posJoint.y));

								jointsDic[jointsInfo[j]].anchoredPosition = newPosition;
							}
							else
							{
								jointsDic[jointsInfo[j]].gameObject.SetActive(false);
							}
						}
					}
				}
			}

		}
	}
}

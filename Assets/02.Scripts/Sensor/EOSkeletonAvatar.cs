using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOSkeletonAvatar : MonoBehaviour
{
    public bool autoProcessing = true;

    [SerializeField] GameObject jointPrefab = null/*, connectionPrefab = null*/;

    [SerializeField] GameObject sensorUIPrefab = null;

    [SerializeField] GameObject leftHandPrefab = null;
    [SerializeField] GameObject rightHandPrefab = null;

    [SerializeField] Vector3 handSize = new Vector3(100f, 100f, 100f);

    nuitrack.JointType[] jointsInfo = new nuitrack.JointType[]
    {
        nuitrack.JointType.Head,
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
        nuitrack.JointType.LeftHand,
        nuitrack.JointType.RightHand
        //nuitrack.JointType.LeftHip,
        //nuitrack.JointType.RightHip,
        //nuitrack.JointType.LeftKnee,
        //nuitrack.JointType.RightKnee,
        //nuitrack.JointType.LeftAnkle,
        //nuitrack.JointType.RightAnkle
    };

    //nuitrack.JointType[,] connectionsInfo = new nuitrack.JointType[,]
    //{ //Right and left collars are currently located at the same point, that's why we use only 1 collar,
    //    //it's easy to add rightCollar, if it ever changes
    //    {nuitrack.JointType.Neck,           nuitrack.JointType.Head},
    //    {nuitrack.JointType.LeftCollar,     nuitrack.JointType.Neck},
    //    {nuitrack.JointType.LeftCollar,     nuitrack.JointType.LeftShoulder},
    //    {nuitrack.JointType.LeftCollar,     nuitrack.JointType.RightShoulder},
    //    {nuitrack.JointType.LeftCollar,     nuitrack.JointType.Torso},
    //    {nuitrack.JointType.Waist,          nuitrack.JointType.Torso},
    //    {nuitrack.JointType.Waist,          nuitrack.JointType.LeftHip},
    //    {nuitrack.JointType.Waist,          nuitrack.JointType.RightHip},
    //    {nuitrack.JointType.LeftShoulder,   nuitrack.JointType.LeftElbow},
    //    {nuitrack.JointType.LeftElbow,      nuitrack.JointType.LeftWrist},
    //    {nuitrack.JointType.LeftWrist,      nuitrack.JointType.LeftHand},
    //    {nuitrack.JointType.RightShoulder,  nuitrack.JointType.RightElbow},
    //    {nuitrack.JointType.RightElbow,     nuitrack.JointType.RightWrist},
    //    {nuitrack.JointType.RightWrist,     nuitrack.JointType.RightHand},
    //    {nuitrack.JointType.LeftHip,        nuitrack.JointType.LeftKnee},
    //    {nuitrack.JointType.LeftKnee,       nuitrack.JointType.LeftAnkle},
    //    {nuitrack.JointType.RightHip,       nuitrack.JointType.RightKnee},
    //    {nuitrack.JointType.RightKnee,      nuitrack.JointType.RightAnkle}
    //};

    //List<RectTransform> connections;
    Dictionary<nuitrack.JointType, RectTransform> joints;
    Dictionary<nuitrack.JointType, RectTransform> sensorJoints;

    RectTransform parentRect;

    void Start()
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
        CreateSkeletonParts();
        CreateSkeletonSensorParts();
    }

    void CreateSkeletonParts()
    {
        joints = new Dictionary<nuitrack.JointType, RectTransform>();

        for (int i = 0; i < jointsInfo.Length; i++)
        {
            if (jointPrefab != null)
            {
                GameObject joint = Instantiate(jointPrefab, transform);
                joint.SetActive(false);

                RectTransform jointRectTransform = joint.GetComponent<RectTransform>();
                joints.Add(jointsInfo[i], jointRectTransform);

                if (leftHandPrefab != null && rightHandPrefab != null)
                {
                    switch (jointsInfo[i].ToString())
                    {
                        //case "Head":
                        //    GameObject sensorUIPrefab = Instantiate(leftHandPrefab, joint.transform, false);
                        //    //leftHand.transform.localPosition = Vector3.zero;
                        //    sensorUIPrefab.transform.localScale = handSize;
                        //    sensorUIPrefab.name = jointsInfo[i].ToString();

                        //    //leftHand.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L_RED");

                        //    //leftHand.SetActive(false);
                        //    break;

                        case "LeftHand":
                            GameObject leftHand = Instantiate(leftHandPrefab, joint.transform, false);
                            //leftHand.transform.localPosition = Vector3.zero;
                            leftHand.transform.localScale = handSize;
                            leftHand.name = jointsInfo[i].ToString();

                            //leftHand.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_L_RED");

                            //leftHand.SetActive(false);
                            break;

                        case "RightHand":
                            GameObject rightHand = Instantiate(rightHandPrefab, joint.transform, false);
                            //rightHand.transform.localPosition = Vector3.zero;
                            rightHand.transform.localScale = handSize;
                            rightHand.name = jointsInfo[i].ToString();

                            //rightHand.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/HAND_R_BLUE");

                            //rightHand.SetActive(false);
                            break;

                        case "LeftAnkle":
                            //joint.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/FOOT_L");
                            break;

                        case "RightAnkle":
                            //joint.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Game/Stamp/FOOT_R");
                            break;
                    }
                }
            }
        }

        //connections = new List<RectTransform>();

        //for (int i = 0; i < connectionsInfo.GetLength(0); i++)
        //{
        //    if (connectionPrefab != null)
        //    {
        //        GameObject connection = Instantiate(connectionPrefab, transform);
        //        connection.SetActive(false);

        //        RectTransform connectionRectTransform = connection.GetComponent<RectTransform>();
        //        connections.Add(connectionRectTransform);
        //    }
        //}
    }

    void CreateSkeletonSensorParts()
    {
        sensorJoints = new Dictionary<nuitrack.JointType, RectTransform>();

        for (int i = 0; i < jointsInfo.Length; i++)
        {
            if (sensorUIPrefab != null)
            {
                //GameObject joint = Instantiate(sensorUIPrefab, transform);
                //joint.SetActive(false);

                //RectTransform jointRectTransform = joint.GetComponent<RectTransform>();
                //sensorJoints.Add(jointsInfo[i], jointRectTransform);

                switch (jointsInfo[i].ToString())
                {
                    case "Head":
                        GameObject head = Instantiate(sensorUIPrefab, transform);
                        //leftHand.transform.localPosition = Vector3.zero;
                        head.name = string.Format("{0}{1}", jointsInfo[i].ToString(), "Sensor");

                        head.SetActive(false);

                        RectTransform jointRectTransform = head.GetComponent<RectTransform>();
                        sensorJoints.Add(jointsInfo[i], jointRectTransform);
                        break;

                    case "LeftHand":
                        GameObject leftHand = Instantiate(sensorUIPrefab, transform);
                        //leftHand.transform.localPosition = Vector3.zero;
                        leftHand.name = string.Format("{0}{1}", jointsInfo[i].ToString(), "Sensor");

                        leftHand.SetActive(false);

                        RectTransform jointRectTransform1 = leftHand.GetComponent<RectTransform>();
                        sensorJoints.Add(jointsInfo[i], jointRectTransform1);
                        break;

                    case "RightHand":
                        GameObject rightHand = Instantiate(sensorUIPrefab, transform);
                        //rightHand.transform.localPosition = Vector3.zero;
                        rightHand.name = string.Format("{0}{1}", jointsInfo[i].ToString(), "Sensor");

                        rightHand.SetActive(false);

                        RectTransform jointRectTransform2 = rightHand.GetComponent<RectTransform>();
                        sensorJoints.Add(jointsInfo[i], jointRectTransform2);
                        break;
                }
            }
        }
    }

    void Update()
    {
        if (autoProcessing)
            ProcessSkeleton(CurrentUserTracker.CurrentSkeleton);
    }

    public void ProcessSkeleton(nuitrack.Skeleton skeleton)
    {
        if (skeleton == null)
            return;

        if(GameManager.instance.gameState.Equals(Enums_Game.GameState.Sensor))
        {
            for (int i = 0; i < jointsInfo.Length; i++)
            {
                nuitrack.Joint j = skeleton.GetJoint(jointsInfo[i]);
                if (j.Confidence > 0.01f)
                {
                    sensorJoints[jointsInfo[i]].gameObject.SetActive(true);

                    Vector2 newPosition = new Vector2(
                        parentRect.rect.width * (j.Proj.X - 0.5f),
                        parentRect.rect.height * (0.5f - j.Proj.Y));

                    sensorJoints[jointsInfo[i]].anchoredPosition = newPosition;
                }
                else
                {
                    sensorJoints[jointsInfo[i]].gameObject.SetActive(false);
                }
            }
        }
        else if(GameManager.instance.gameState.Equals(Enums_Game.GameState.Play))
        {
            for (int i = 0; i < jointsInfo.Length; i++)
            {
                nuitrack.Joint j = skeleton.GetJoint(jointsInfo[i]);
                if (j.Confidence > 0.01f)
                {
                    joints[jointsInfo[i]].gameObject.SetActive(true);

                    Vector2 newPosition = new Vector2(
                        parentRect.rect.width * (j.Proj.X - 0.5f),
                        parentRect.rect.height * (0.5f - j.Proj.Y));

                    joints[jointsInfo[i]].anchoredPosition = newPosition;
                }
                else
                {
                    joints[jointsInfo[i]].gameObject.SetActive(false);
                }
            }
        }

        //for (int i = 0; i < connectionsInfo.GetLength(0); i++)
        //{
        //    RectTransform startJoint = joints[connectionsInfo[i, 0]];
        //    RectTransform endJoint = joints[connectionsInfo[i, 1]];

        //    if (startJoint.gameObject.activeSelf && endJoint.gameObject.activeSelf)
        //    {
        //        connections[i].gameObject.SetActive(true);

        //        connections[i].anchoredPosition = startJoint.anchoredPosition;
        //        connections[i].transform.right = endJoint.position - startJoint.position;
        //        float distance = Vector3.Distance(endJoint.anchoredPosition, startJoint.anchoredPosition);
        //        connections[i].transform.localScale = new Vector3(distance, 1f, 1f);
        //    }
        //    else
        //    {
        //        connections[i].gameObject.SetActive(false);
        //    }
        //}
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BodyController : MonoBehaviour
{
    public nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject PrefabJoint;
    public Transform body1;
    public Transform body2;

    void Start()
    {
        //NuitrackManager.DepthSensor.SetMirror(true);

        CreatedJoint = new GameObject[typeJoint.Length];
        for (int q = 0; q < typeJoint.Length; q++)
        {
            CreatedJoint[q] = Instantiate(PrefabJoint);
            CreatedJoint[q].name = ((nuitrack.JointType)q).ToString();
            CreatedJoint[q].transform.SetParent(body1);
        }
    }

    void Update()
    {
        //if (CurrentUserTracker.CurrentUser != 0)
        //{
        //    nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;

        //    for (int q = 0; q < typeJoint.Length; q++)
        //    {
        //        nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);
        //        Vector3 newPosition = 0.6f * joint.ToVector3();
        //        //Vector3 newPosition = 0.6f * ToVector3Flipped(joint);
        //        CreatedJoint[q].transform.localPosition = new Vector3(newPosition.x, newPosition.y, body1.transform.localPosition.z);
        //    }
        //}
    }

    public static Vector3 ToVector3(nuitrack.Joint joint)
    {
        return new Vector3(joint.Real.X, joint.Real.Y, joint.Real.Z);
    }

    public static Vector3 ToVector3Flipped(nuitrack.Joint joint)
    {
        return new Vector3(joint.Real.X * -1f, joint.Real.Y, joint.Real.Z);
    }
}

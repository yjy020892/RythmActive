using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorJointCollider : MonoBehaviour
{
    int cnt = 0;

    void OnTriggerEnter(Collider col)
    {
        if(gameObject.name.Equals("HeadSensor"))
        {
            if(col.CompareTag("HeadSensor"))
            {
                GameManager.instance.sensorCnt += 1;
            }
            //if (col.tag.Equals("HeadSensor"))
            //{
            //    GameManager.instance.sensorCnt += 1;
            //}
        }
        else if (gameObject.name.Equals("HandLeftSensor"))
        {
            if (col.CompareTag("HandLeftSensor"))
            {
                GameManager.instance.sensorCnt += 1;
            }
            //if (col.tag.Equals("HandLeftSensor"))
            //{
            //    GameManager.instance.sensorCnt += 1;
            //}
        }
        else if (gameObject.name.Equals("HandRightSensor"))
        {
            if (col.CompareTag("HandRightSensor"))
            {
                GameManager.instance.sensorCnt += 1;
            }
            //if (col.tag.Equals("HandRightSensor"))
            //{
            //    GameManager.instance.sensorCnt += 1;
            //}
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (gameObject.name.Equals("HeadSensor"))
        {
            if (col.CompareTag("HeadSensor"))
            {
                GameManager.instance.sensorCnt -= 1;
            }
            //if (col.tag.Equals("HeadSensor"))
            //{
            //    GameManager.instance.sensorCnt -= 1;
            //    //Debug.Log("HeadSensor");
            //}
        }
        else if (gameObject.name.Equals("HandLeftSensor"))
        {
            if (col.CompareTag("HandLeftSensor"))
            {
                GameManager.instance.sensorCnt -= 1;
            }
            //if (col.tag.Equals("HandLeftSensor"))
            //{
            //    GameManager.instance.sensorCnt -= 1;
            //    //Debug.Log("LeftHandSensor");
            //}
        }
        else if (gameObject.name.Equals("HandRightSensor"))
        {
            if (col.CompareTag("HandRightSensor"))
            {
                GameManager.instance.sensorCnt -= 1;
            }
            //if (col.tag.Equals("HandRightSensor"))
            //{
            //    GameManager.instance.sensorCnt -= 1;
            //    //Debug.Log("RightHandSensor");
            //}
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNote : MonoBehaviour
{
    public Transform leftObj;
    public Transform rightObj;
    public Animator anim;

    Vector2 mousePosition;
    Vector2 leftHands;
    Vector2 rightHands;
    public Camera cam;

    public AudioSource audioSource;

    public void PlayMusic()
    {
        if(this.enabled)
        {
            if (!audioSource.isPlaying)
            {
                Debug.Log("Play");
                audioSource.Play();
            }
        }
    }

    void Start()
    {
        if(!audioSource)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        if (leftObj != null && rightObj != null)
        {
            leftHands = Camera.main.WorldToScreenPoint(leftObj.position);
            rightHands = Camera.main.WorldToScreenPoint(rightObj.position);

            if (leftHands.x <= 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", -(960 - leftHands.x)));
                leftHands.x = -(960 - leftHands.x);
            }
            else if (leftHands.x > 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", leftHands.x - 960));
                leftHands.x = leftHands.x - 960;
            }

            if (leftHands.y <= 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", -(540 - leftHands.y)));
                leftHands.y = -(540 - leftHands.y);
            }
            else if (leftHands.y > 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", leftHands.y - 540));
                leftHands.y = leftHands.y - 540;
            }

            if (rightHands.x <= 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", -(960 - leftHands.x)));
                rightHands.x = -(960 - rightHands.x);
            }
            else if (rightHands.x > 960.0f)
            {
                //Debug.Log(string.Format("x : {0}", leftHands.x - 960));
                rightHands.x = rightHands.x - 960;
            }

            if (rightHands.y <= 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", -(540 - leftHands.y)));
                rightHands.y = -(540 - rightHands.y);
            }
            else if (rightHands.y > 540.0f)
            {
                //Debug.Log(string.Format("y: {0}", leftHands.y - 540));
                rightHands.y = rightHands.y - 540;
            }

            //Debug.Log(string.Format("L : {0} , R : {1}", leftHands, rightHands));
        }

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        mousePosition = Input.mousePosition;
        //        //mousePosition = cam.ScreenToWorldPoint(mousePosition);
        //        if (mousePosition.x <= 960.0f)
        //        {
        //            Debug.Log(string.Format("x : {0}", -(960 - mousePosition.x)));
        //        }
        //        else if (mousePosition.x > 960.0f)
        //        {
        //            Debug.Log(string.Format("x : {0}", mousePosition.x - 960));
        //        }

        //        if (mousePosition.y <= 540.0f)
        //        {
        //            Debug.Log(string.Format("y: {0}", -(540 - mousePosition.y)));
        //        }
        //        else if (mousePosition.y > 540.0f)
        //        {
        //            Debug.Log(string.Format("y: {0}", mousePosition.y - 540));
        //        }

        //        //Debug.Log(mousePosition);
        //    }
    }
}

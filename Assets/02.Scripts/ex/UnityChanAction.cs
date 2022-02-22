using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanAction : MonoBehaviour
{
    public CharacterController characterController;
    Animator anim;

    public float walkSpeed = 1.0f;
    public float runSpeed = 2.5f;
    public float gravity = -9.82f;

    public Toggle walkToggle;
    public Toggle winToggle;
    public Toggle loseToggle;
    public Toggle damageToggle;
    public Toggle jumpToggle;
    public Toggle slideToggle;

    float a;
    float b;

    Vector3 move;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         
        a = Input.GetAxis("Vertical");
        b = Input.GetAxis("Horizontal");

        move = a * Vector3.forward + b * Vector3.right;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(move); // 이것

            //transform.rotation = Quaternion.LookRotation(a * Vector3.forward + b * Vector3.right);
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        //transform.rotation = Quaternion.LookRotation(move);

        //transform.Translate(new Vector3(-b, 0f, -a).normalized * Time.deltaTime);

        if (walkToggle.isOn)
        {
            characterController.Move(move * walkSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(move * runSpeed * Time.deltaTime);
        }

        if (a != 0 || b != 0)
        {
            if(walkToggle.isOn)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("WAIT"))
                {
                    anim.SetInteger("AnimationState", 3);
                }
            }
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("WAIT"))
                {
                    anim.SetInteger("AnimationState", 1);
                }
            }
        }
        else if(a == 0 && b == 0)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("RUN") || anim.GetCurrentAnimatorStateInfo(0).IsName("WALK"))
            {
                anim.SetInteger("AnimationState", 0);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("WAIT"))
            {
                anim.SetInteger("AnimationState", 0);
            }

            if (Input.GetKeyDown(KeyCode.Q) && anim.GetCurrentAnimatorStateInfo(0).IsName("WAIT"))
            {
                if(winToggle.isOn)
                {
                    anim.SetInteger("AnimationState", 2);
                }
                else if (loseToggle.isOn)
                {
                    anim.SetInteger("AnimationState", 5);
                }
                else if (damageToggle.isOn)
                {
                    anim.SetInteger("AnimationState", 4);
                }
                else if (jumpToggle.isOn)
                {
                    anim.SetInteger("AnimationState", 6);
                }
                else if (slideToggle.isOn)
                {
                    anim.SetInteger("AnimationState", 7);
                }
            }
        }

        */
        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}

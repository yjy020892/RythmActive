using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform leftObj;
    public Transform rightObj;
    public Animator anim;

    Vector2 mousePosition;
    Vector2 leftHands;
    Vector2 rightHands;
    Vector3 gravity;

    public Camera cam;

    public AudioSource audioSource;

    [SerializeField] float gravityScale = -1.0f;
    [SerializeField] private Rigidbody rigid;

    //private void Start()
    //{
    //    audioSource = GameObject.Find("Sync").GetComponent<AudioSource>();

    //    if (audioSource == null)
    //    {
    //        Debug.Log("audioSource null");
    //    }
    //}

    public void PlayMusic()
    {
        if(!audioSource.isPlaying)
        {
            Debug.Log("play");
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    //private void Update()
    //{
    //    if(rigid)
    //    {
    //        gravity = gravityScale * Vector3.up;
    //        rigid.AddForce(gravity, ForceMode.Acceleration);
    //    }

    //    if (anim != null && GameManager.instance.gameState.Equals(Enums_Game.GameState.Play) && anim.GetInteger("Dance").Equals(0))
    //    {
    //        anim.SetInteger("Dance", 1);
    //    }
    //}
}

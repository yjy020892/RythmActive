using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator anim;

    public AudioSource audioSource;

    bool b_Dance = false;
    private void Start()
    {
        audioSource = GameObject.Find("Sync").GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.Log("audioSource null");
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
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

    private void Update()
    {
        if (anim != null && GameManager.instance.gameState.Equals(Enums_Game.GameState.Play) && !b_Dance)
        {
            b_Dance = true;

            anim.SetBool(DataManager.instance.songData._SongName, true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(audioSource.clip)
            {
                audioSource.Play();
            }
        }
    }

    public void SongStart()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

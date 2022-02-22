using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource myAudio;

    public AudioClip payMoneySound;
    public AudioClip difficultSound;
    public AudioClip musicSelectSound;
    public AudioClip arrowSound;
    public AudioClip gameFinishSound;
    public AudioClip sensorSound;
    public AudioClip bpmSound;
    

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
    }

    public void PlayDifficult()
    {
        myAudio.PlayOneShot(difficultSound);
    }

    public void PlayMusicSelect()
    {
        myAudio.PlayOneShot(musicSelectSound);
    }

    public void PlayArrow()
    {
        myAudio.PlayOneShot(arrowSound);
    }

    public void PlayGameFinish()
    {
        myAudio.PlayOneShot(gameFinishSound);
    }

    public void PlayPayMoney()
    {
        myAudio.PlayOneShot(payMoneySound);
    }

    public void PlaySensor()
    {
        myAudio.PlayOneShot(sensorSound);
    }

    public void PlayBPM()
    {
        myAudio.PlayOneShot(bpmSound);
    }
}

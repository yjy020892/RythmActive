using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{
    AudioSource music;

    AudioSource playTik;
    public AudioClip tikClip;

    Note note;

    float musicBPM;
    float stdBPM = 60.0f;
    //float musicBeat = 4.0f;
    //float stdBeat = 4.0f;

    public float oneBeatTime = 0f;
    public float beatPerSample = 0f;

    public float bitPerSec = 0f;
    public float bitPerSample = 0f;

    public float barPerSec = 0f;
    public float barPerSample = 0f;

    float frequency = 0f;
    float nextSample = 0f;

    public float offset; // ������ ������(��)
    public float offsetForSample; // ������ ������(����)

    public float scrollSpeed; //�� bpm�� ���� �⺻ ���
    public float userSpeedRate;

    private void Start()
    {
        GameManager.instance.SongStart += SetSync;
    }

    void SetSync()
    {
        //player = FindObjectOfType<Player>();

        //playTik = GetComponent<AudioSource>();
        //music = FindObjectOfType<SongManager>().GetComponent<AudioSource>();
        //music = FindObjectOfType<Sync>().GetComponent<AudioSource>();
        //sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        //generator = GameObject.Find("GeneratorNote").GetComponent<GeneratorNote>();

        //scrollSpeed = 10.0f;
        //userSpeedRate = 1f;

        //musicBPM = sheet.Bpm;
        //musicBPM = DataManager.instance.songData._BPM;
        // ������� ���ļ���
        //frequency = music.clip.frequency;
        // ������
        //offset = sheet.Offset;
        offset = 1.6426f;
        // ������ �ʸ� ���÷� ��ȯ
        offsetForSample = frequency * offset;
        // �ѹ��� �ð���
        oneBeatTime = (stdBPM / musicBPM);// * (musicBeat / stdBeat);
        // ù���� ���ð�(������)
        nextSample += offsetForSample;
        // 32��Ʈ���� 1��Ʈ�� �ð���
        //bitPerSec = stdBPM / (8 * musicBPM);
        // 32��Ʈ���� 1��Ʈ�� ���ð�
        //bitPerSample = bitPerSec * playMusic.clip.frequency;
        // 1�� �ð���
        barPerSec = oneBeatTime * 4.0f;
        // 1�� ���ð�
        //barPerSample = barPesrSec * playMusic.clip.frequency; 
    }

    IEnumerator PlayTik()
    {
        // �ʴ� 44100 ���ð� ���� �������ð��� 44100������ ��Ȯ�� ����������
        if (music.timeSamples >= nextSample)
        {
            playTik.PlayOneShot(tikClip); // ���� ���
            beatPerSample = oneBeatTime * frequency;
            nextSample += beatPerSample;
        }
        yield return null;
    }

}

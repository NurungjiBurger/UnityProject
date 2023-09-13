using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/*
// ����� �ҽ� �����ؼ� �߰�
AudioSource audioSource = gameObject.AddComponent<AudioSource>();

// ��Ʈ: true�� ��� �Ҹ��� ���� ����
audioSource.mute = false;

// ����: true�� ��� �ݺ� ���
audioSource.loop = false;

// �ڵ� ���: true�� ��� �ڵ� ���
audioSource.playOnAwake = false;

// ����� ���
audioSource.Play();

// ����� ����
audioSource.Stop();
*/

public class AudioController : MonoBehaviour
{

    private string NowControl = "";
    private AudioSource AudioSource = null;

    public AudioMixer MasterMixer;

    public Slider AudioSlider;


    public void SetNowControl(string str)
    {
        NowControl = str;
    }
    public void SetLevel(float val)
    {
        switch(NowControl)
        {
            case "BGM":
                MasterMixer.SetFloat("BGM", Mathf.Log10(val) * 20);
                break;
            case "SFX":
                MasterMixer.SetFloat("SFX", Mathf.Log10(val) * 20);
                break;
            case "Master":
                // ������ ������ ��� ������� ȿ���� ��θ� �����ؾ���
                MasterMixer.SetFloat("Master", Mathf.Log10(val) * 20);
                MasterMixer.SetFloat("BGM", Mathf.Log10(val) * 20);
                MasterMixer.SetFloat("SFX", Mathf.Log10(val) * 20);
                GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("BGMSlider").GetComponent<Slider>().value = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("MasterSlider").GetComponent<Slider>().value;
                GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("SFXSlider").GetComponent<Slider>().value = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("MasterSlider").GetComponent<Slider>().value;
                break;
            default:
                break;
        }
        
        if (GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("BGMSlider").GetComponent<Slider>().value == 0.0001f) AudioStop(null);
        else 
        {
            if (!AudioSource.isPlaying) AudioPlay(null);
        }

        NowControl = "";
    }
    public void AudioStop(AudioSource source)
    {
        if (source == null) AudioSource.Stop();
        else source.Stop();
    }
    public void AudioPlay(AudioSource source)
    {
        // ������� ������ �ٸ� �����Ŭ������ �ش� ��ũ��Ʈ�� Ŭ���� ��Ź ����ϴ� �����̱� ������ �Ķ���ͷ� �ҽ��� ���°��� �ش� ��ũ��Ʈ���� ȣ���� ���� ����
        if (source == null) AudioSource.Play();
        else source.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

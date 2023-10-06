using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/*
// 오디오 소스 생성해서 추가
AudioSource audioSource = gameObject.AddComponent<AudioSource>();

// 뮤트: true일 경우 소리가 나지 않음
audioSource.mute = false;

// 루핑: true일 경우 반복 재생
audioSource.loop = false;

// 자동 재생: true일 경우 자동 재생
audioSource.playOnAwake = false;

// 오디오 재생
audioSource.Play();

// 오디오 정지
audioSource.Stop();
*/


public class AudioController : MonoBehaviour
{

    private string NowControl = "";
    private AudioSource AudioSource = null;

    public AudioMixer MasterMixer;

    public Slider AudioSlider;

    private Data GameData;
    private bool GameStart = false;


    public void AdaptVolume()
    {
        if (GameData != null)
        {
            MasterMixer.SetFloat("BGM", Mathf.Log10(GameData.BGMVolume * 20));
            MasterMixer.SetFloat("SFX", Mathf.Log10(GameData.SFXVolume * 20));
            MasterMixer.SetFloat("Master", Mathf.Log10(GameData.MasterVolume * 20));

            GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("MasterSlider").GetComponent<Slider>().value = GameData.MasterVolume;
            GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("BGMSlider").GetComponent<Slider>().value = GameData.BGMVolume;
            GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("SFXSlider").GetComponent<Slider>().value = GameData.SFXVolume;

            GameStart = true;
        }
    }
    public void SetNowControl(string str)
    {
        NowControl = str;
    }
    public void SetLevel(float val)
    {
        if (GameData != null)
        {
            switch (NowControl)
            {
                case "BGM":
                    GameData.BGMVolume = val;
                    break;
                case "SFX":
                    GameData.SFXVolume = val;
                    break;
                case "Master":
                    // 마스터 볼륨의 경우 배경음과 효과음 모두를 조절해야함
                    GameData.MasterVolume = val;
                    GameData.BGMVolume = val;
                    GameData.SFXVolume = val;
                    GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("BGMSlider").GetComponent<Slider>().value = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("MasterSlider").GetComponent<Slider>().value;
                    GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("SFXSlider").GetComponent<Slider>().value = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("MasterSlider").GetComponent<Slider>().value;
                    break;
                default:
                    break;
            }

            AdaptVolume();

            if (GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Setting").Find("BGMSlider").GetComponent<Slider>().value == 0.0001f) AudioStop(null);
            else
            {
                if (!AudioSource.isPlaying) AudioPlay(null);
            }

            NowControl = "";
        }
    }
    public void AudioStop(AudioSource source)
    {
        if (source == null) AudioSource.Stop();
        else source.Stop();
    }
    public void AudioPlay(AudioSource source)
    {
        // 배경음을 제외한 다른 오디오클립들은 해당 스크립트에 클립을 위탁 재생하는 형식이기 때문에 파라미터로 소스가 없는경우는 해당 스크립트에서 호출한 경우로 만듬
        if (source == null) AudioSource.Play();
        else source.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        AudioSource = GetComponent<AudioSource>();

        GameData = GameObject.Find("Data").GetComponent<DataController>().GAMEDATA;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData == null) GameData = GameObject.Find("Data").GetComponent<DataController>().GAMEDATA;
        else
        {
            if (!GameStart) AdaptVolume();
        }
    }
}

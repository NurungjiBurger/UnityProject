using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetValure : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float SliderVal)
    {
        // 최소 최대 0.0001 ~ 1 대입시 -80 ~ 0
        mixer.SetFloat("masterVol", Mathf.Log10(SliderVal) * 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

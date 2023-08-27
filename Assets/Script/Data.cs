using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Data
{
    // 기본 데이터
    public string EndTime = "";

    public float NeedClickCount = 5.0f;
    public float NowClickCount = 0.0f;
    public int ClickPower = 1;

    public int EggLevel = 1;
    public int NowGold = 0;
    public int NowGem = 0;

    // 강화나 업적 데이터를 위한 기본 데이터
    public int CharacterNum = 0;
    public int PersonNum = 0;
    public int AnimalNum = 0;
    public int EtcNum = 0;
    public int EnhanceNum = 0;

    public int Gold = 0;
    public int Gem = 0;
    public int TimeStackNum = 0;
    public int TImeStack = 0;


    // 강화 데이터
    public bool ActivateStack = false;
    public bool FirstEnhanceStack = false;
    public bool SecondEnhanceStack = false;


    // 업적 데이터
    public bool FirstCharacter = false;
    public bool AcquireFirstCharacter = false;
    public bool GetFirstPerson = false;
    public bool AcquireGetFirstPerson = false;
    public bool FirstEnhance = false;
    public bool AcquireFirstEnhance = false;
    public bool UseGold1000 =false;
    public bool AcquireUseGold1000 = false;
    public bool UseTimeStack = false;
    public bool AcquireUseTimeStack = false;
    public bool GetCharacter10 = false;
    public bool AcquireGetCharacter10 = false;

    // 컬렉션 업적


    // 설정 데이터
    // 배경소리
    // 효과음

}

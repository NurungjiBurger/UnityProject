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

    public int EggLevel = 1;
    public int NowGold = 0;
    public int NowGem = 0;

    // 캐릭터 데이터
    public List<CharacterData> CharacterDatas = new List<CharacterData>();

    // 강화나 업적 데이터를 위한 기본 데이터

    public int HatchProbability = 10;

    public int ClickPower = 1;
    public int CostForClickPower = 100;

    public int MovedCharacterNum = 0;
    public int CharacterNum = 0;
    public int PersonNum = 0;
    public int AnimalNum = 0;
    public int EtcNum = 0;
    public int EnhanceNum = 0;

    public int Gold = 0;
    public int Gem = 0;

    public int TimeInterval = 600;
    public int TimeStackNum = 0;
    public int TImeStack = 0;


    // 강화 데이터
    public bool EnhanceClickPower = false;
    public bool ActivateStack = false;
    public bool FirstEnhanceStack = false;
    public bool SecondEnhanceStack = false;
    public bool ThirdEnhanceStack = false;
    public bool FirstHatchProbabilityEnhance = false;
    public bool SecondHatchProbabilityEnhance = false;
    public bool ThirdHatchProbabilityEnhance = false;


    // 업적 데이터
    public bool FirstCharacter = false;
    public bool AcquireFirstCharacter = false;

    public bool GetFirstPerson = false;
    public bool AcquireGetFirstPerson = false;

    public bool FirstEnhance = false;
    public bool AcquireFirstEnhance = false;

    public bool UseGold1000 = false;
    public bool AcquireUseGold1000 = false;

    public bool UseGold100000 = false;
    public bool AcquireUseGold100000 = false;

    public bool UseGem100 = false;
    public bool AcquireUseGem100 = false;

    public bool UseGem2000 = false;
    public bool AcquireUseGem2000 = false;

    public bool UseTimeStack = false;
    public bool AcquireUseTimeStack = false;

    public bool GetCharacter10 = false;
    public bool AcquireGetCharacter10 = false;

    public bool GetCharacter100 = false;
    public bool AcquireGetCharacter100 = false;

    public bool GetFirstAnimal = false;
    public bool AcquireGetFirstAnimal = false;

    public bool GetFirstEtc = false;
    public bool AcquireGetFirstEtc = false;


    // 컬렉션 업적



}

[Serializable]
public class CharacterData
{
    public string Name;
    public float[] Position = new float[3];
    public int PrfNum;
    public int Index;

    public CharacterData(string name, int num, int idx, float[] pos)
    {
        Name = name;
        PrfNum = num;
        Index = idx;
        Position = pos;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Data
{
    // �⺻ ������
    public string EndTime = "";

    public float NeedClickCount = 5.0f;
    public float NowClickCount = 0.0f;

    public int EggLevel = 1;
    public int NowGold = 0;
    public int NowGem = 0;

    // ĳ���� ������
    public List<CharacterData> CharacterDatas = new List<CharacterData>();

    // ��ȭ�� ���� �����͸� ���� �⺻ ������

    public int HatchProbability = 100;

    public int ClickPower = 1;
    public int CostForClickPower = 100;

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


    // ��ȭ ������
    public bool EnhanceClickPower = false;
    public bool ActivateStack = false;
    public bool FirstEnhanceStack = false;
    public bool SecondEnhanceStack = false;
    public bool ThirdEnhanceStack = false;


    // ���� ������
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

    // �÷��� ����


    // ���� ������
    // ���Ҹ�
    // ȿ����

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
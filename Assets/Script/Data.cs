using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Data
{
    // �⺻ ������
    public string EndTime = "";

    public int EggLevel = 1;
    public int NeedClickCount = 1;
    public int NowClickCount = 0;
    public int ClickPower = 1;

    public int Gold = 0;
    public int Gem = 0;

    // ��ȭ ������
    public bool ActivateStack = false;
    public bool FirstEnhanceStack = false;
    public bool SecondEnhanceStack = false;



    // ���� ������
    // ��ȭȽ�� ����
    public bool FirstHatch;
    public bool HundredHatch;

    // �÷��� ����
    public bool FirstPerson;
    public bool FirstFood;
    public bool FirstThing;
    public bool FirstAnimal;


    // ���� ������
    // ���Ҹ�
    // ȿ����

}

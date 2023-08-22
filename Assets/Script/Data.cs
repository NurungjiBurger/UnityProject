using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Data
{
    // 기본 데이터
    public string EndTime = "";

    public int EggLevel = 1;
    public int NeedClickCount = 1;
    public int NowClickCount = 0;
    public int ClickPower = 1;

    public int Gold = 0;
    public int Gem = 0;

    // 강화 데이터
    public bool ActivateStack = false;
    public bool FirstEnhanceStack = false;
    public bool SecondEnhanceStack = false;



    // 업적 데이터
    // 부화횟수 업적
    public bool FirstHatch;
    public bool HundredHatch;

    // 컬렉션 업적
    public bool FirstPerson;
    public bool FirstFood;
    public bool FirstThing;
    public bool FirstAnimal;


    // 설정 데이터
    // 배경소리
    // 효과음

}

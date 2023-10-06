using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class EnhanceAchievement : MonoBehaviour
{
    [SerializeField]
    GameObject GameController;
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject Data;

    private Data GameData;

    Type type;

    /////////////////////////////////////////////
    ///// Enhance

    public void EnhanceClickPower(GameObject obj)
    {
        GameData.ClickPower += 1;
        GameData.CostForClickPower += (int)(GameData.CostForClickPower * 0.1f);
        if (GameData.ClickPower > GameData.NeedClickCount) GameData.EggLevel++;
    }
    private void EnhanceTimeStack(int interval)
    {
        GameData.TimeInterval /= interval;
    }
    public void FirstEnhanceStack(GameObject obj)
    {
        EnhanceTimeStack(10);
    }
    public void SecondEnhanceStack(GameObject obj)
    {
        EnhanceTimeStack(10);
    }
    public void ThirdEnhanceStack(GameObject obj)
    {
        EnhanceTimeStack(2);
    }

    public void FirstHatchProbabilityEnhance(GameObject obj)
    {
        GameData.HatchProbability += 10;
    }
    
    public void SecondHatchProbabilityEnhance(GameObject obj)
    {
        GameData.HatchProbability += 5;
    }
    public void ThirdHatchProbabilityEnhance(GameObject obj)
    {
        GameData.HatchProbability += 5;
    }

    /////////////////////////////////////////////
    ///// Achievement
    public bool FirstCharacter()
    {
        if (GameData.CharacterNum != 0) return true;
        return false;
    }
    public bool GetFirstPerson()
    {
        if (GameData.PersonNum != 0) return true;
        return false;
    }

    public bool GetFirstAnimal()
    {
        if (GameData.AnimalNum != 0) return true;
        return false;
    }

    public bool GetFirstEtc()
    {
        if (GameData.EtcNum != 0) return true;
        return false;
    }

    public bool FirstEnhance()
    {
        if (GameData.EnhanceNum != 0) return true;
        return false;
    }
    public bool UseGold1000()
    {
        if (GameData.Gold - GameData.NowGold >= 1000) return true;
        return false;
    }
    public bool UseGold100000()
    {
        if (GameData.Gold - GameData.NowGold >= 100000) return true;
        return false;
    }
    public bool UseGem100()
    {
        if (GameData.Gem - GameData.NowGem >= 100) return true;
        return false;
    }
    public bool UseGem2000()
    {
        if (GameData.Gem - GameData.NowGem >= 2000) return true;
        return false;
    }
    public bool UseTimeStack()
    {
        if (GameData.TimeStackNum != 0) return true;
        return false;
    }
    public bool GetCharacter10()
    {
        if (GameData.CharacterNum >= 10) return true;
        return false;
    }
    public bool GetCharacter100()
    {
        if (GameData.CharacterNum >= 100) return true;
        return false;
    }


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController == null) GameController = GameObject.Find("GameController").GetComponent<GameController>().gameObject;
        else
        {
            Data = GameObject.Find("Data").gameObject;
            Canvas = GameObject.Find("Canvas").gameObject;
            GameData = Data.GetComponent<DataController>().GAMEDATA;
        }
    }
}

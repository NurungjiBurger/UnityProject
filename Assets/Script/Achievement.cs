using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Achievement : MonoBehaviour
{
    [SerializeField]
    GameObject GameController;
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject Data;

    private Data GameData;

    Type type;

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
    public bool UseTimeStack()
    {
        if (GameData.TimeStackNum != 0) return true;
        return false;
    }
    public bool GetChracter10()
    {
        if (GameData.CharacterNum >= 10) return true;
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameData = Data.GetComponent<DataController>().GameData;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController == null) GameController = GameObject.Find("GameController").GetComponent<GameController>().gameObject;
        else
        {
            Canvas = GameObject.Find("Canvas").gameObject;
            GameData = Data.GetComponent<DataController>().GameData;
        }
    }
}

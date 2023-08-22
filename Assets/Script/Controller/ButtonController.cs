using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ButtonController : MonoBehaviour
{
    GameObject GameController;
    private GameObject Canvas;

    private bool IsCreateCharacter;
    private GameObject Character;
    private Data GameData;
    private int TimeStack;

    Type type;

    public void EnhanceManager(GameObject obj)
    {
        String CostType = obj.transform.Find("Cost").transform.Find("Image").Find("CostType").GetComponent<TextMeshProUGUI>().text;
        int Cost = Convert.ToInt32(obj.transform.Find("Cost").Find("Quantity").GetComponent<TextMeshProUGUI>().text);

        int DepositGold = Convert.ToInt32(GameData.Gold);
        int DepositGem = Convert.ToInt32(GameData.Gem);

        bool result;

        switch (CostType)
        {
            case "GOLD":
                result = DepositGold >= Cost;
                GameData.Gold = result ? DepositGold - Cost  : DepositGold - 0;
                break;
            case "GEM":
                result = DepositGem >= Cost;
                GameData.Gem = result ? DepositGem - Cost : DepositGem - 0;
                break;
            default:
                result = false;
                break;
        }

        type = GameData.GetType();
        type.GetField(obj.gameObject.name).SetValue(GameData, result);
    }
    private void CharacterInform(GameObject obj)
    {
        // 캐릭터 이미지
        Canvas.transform.Find("ScreenPanels").transform.Find("Inform").transform.Find("Image").GetComponent<Image>().sprite = obj.transform.Find("ObjImage").GetComponent<SpriteRenderer>().sprite;
        // 캐릭터 이름
        Canvas.transform.Find("ScreenPanels").transform.Find("Inform").transform.Find("Text").GetComponent<TextMeshProUGUI>().text = obj.name;
    }
    public void EggManager(int times)
    {
        GameObject obj;

        if (Canvas.transform.GetChild(1).gameObject.name == "EggButton")
        {
            obj = Canvas.transform.Find("EggButton").gameObject;
            if (GameData != null)
            {
                GameData.NowClickCount += times * GameData.ClickPower;
            }
            if (GameData.NeedClickCount <= GameData.NowClickCount)
            {
                // egg obj 삭제
                Destroy(obj);
                GameData.Gold += 100;
                // 캐릭터 생성
                Character = GameController.GetComponent<GameController>().CreatePrefab("Character", -1, new Vector3(0.0f, 0.0f, 0.0f));
                Character.transform.SetParent(Canvas.transform);
                Character.transform.SetSiblingIndex(1);
                Character.name = Character.name.Substring(0, Character.name.IndexOf('('));

                GameData.NowClickCount = 0;
                //GameData.NeedClickCount *= 2;
                Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(true);
                // 캐릭터 정보 넘기기
                CharacterInform(Character);
            }
        }
        else
        {
            // 생성된 캐릭터 삭제
            Character = Canvas.transform.GetChild(1).gameObject;
            Destroy(Character);

            // egg obj 생성
            obj = GameController.GetComponent<GameController>().CreatePrefab("Egg", 0, new Vector3(0.0f, 0.0f, 0.0f));
            obj.transform.SetParent(Canvas.transform);
            obj.transform.SetSiblingIndex(1);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.GetComponent<Button>().onClick.AddListener(delegate { EggManager(1); });
            obj.gameObject.name = "EggButton";
        }
    }
    public void AdaptStackDamage()
    {
        EggManager(TimeStack);
        TimeStack = 0;
        Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
        Canvas.transform.Find("UpPanels").transform.Find("StackButton").GetComponent<Button>().interactable = false;
    }
    public void CameraModeManager()
    {

    }
    public void ChangeSceneManager(String name)
    {
        SceneManager.LoadScene(name);
    }
    public void ScreenOnOff(GameObject obj)
    {
        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        else obj.gameObject.SetActive(true);
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
            Canvas = GameObject.Find("Canvas").gameObject;
            GameData = GameController.GetComponent<GameController>().GAMEDATA;
            TimeStack = GameController.GetComponent<GameController>().TIMESTACK;
            IsCreateCharacter = GameController.GetComponent<GameController>().ISCREATECHARACTER;

        }

        if (this.name == "Button")
        {
            type = GameData.GetType();

            if (type.GetField(transform.parent.name).GetValue(GameData) != null)
            {
                bool val = Convert.ToBoolean(type.GetField(transform.parent.name).GetValue(GameData));

                transform.parent.transform.Find("Complete").gameObject.SetActive(val);
                GetComponent<Button>().interactable = !(val);

                if (transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text != "None")
                {
                    bool preval = Convert.ToBoolean(type.GetField(transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text).GetValue(GameData));
                    transform.parent.transform.Find("CanNotUse").gameObject.SetActive(!preval);
                    GetComponent<Button>().interactable = preval;
                }
            }
        }
    }
}

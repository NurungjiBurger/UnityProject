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
    [SerializeField]
    private GameObject Data;

    private bool IsCreateCharacter;
    private GameObject Character;
    private Data GameData;
    private int TimeStack;

    Type GameDataType;
    Type ScriptType;

    private void AchieveManager(GameObject obj)
    {
        String CostType = obj.transform.Find("Reward").transform.Find("Image").Find("Type").GetComponent<TextMeshProUGUI>().text;
        int Reward = Convert.ToInt32(obj.transform.Find("Reward").Find("Quantity").GetComponent<TextMeshProUGUI>().text);

        switch(CostType)
        {
            case "GOLD":
                GameData.NowGold += Reward;
                GameData.Gold += Reward;
                break;
            case "GEM":
                GameData.NowGem += Reward;
                GameData.Gem += Reward;
                break;
            default:
                break;
        }

        GameDataType = GameData.GetType();
        GameDataType.GetField("Acquire" + obj.gameObject.name).SetValue(GameData, true);
    }

    private void EnhanceManager(GameObject obj)
    {
        String CostType = obj.transform.Find("Cost").transform.Find("Image").Find("Type").GetComponent<TextMeshProUGUI>().text;
        int Cost = Convert.ToInt32(obj.transform.Find("Cost").Find("Quantity").GetComponent<TextMeshProUGUI>().text);

        int DepositGold = Convert.ToInt32(GameData.NowGold);
        int DepositGem = Convert.ToInt32(GameData.NowGem);

        bool result;

        switch (CostType)
        {
            case "GOLD":
                result = DepositGold >= Cost;
                GameData.NowGold = result ? DepositGold - Cost  : DepositGold - 0;
                break;
            case "GEM":
                result = DepositGem >= Cost;
                GameData.NowGem = result ? DepositGem - Cost : DepositGem - 0;
                break;
            default:
                result = false;
                break;
        }

        if (result) GameData.EnhanceNum++;
        GameDataType = GameData.GetType();
        GameDataType.GetField(obj.gameObject.name).SetValue(GameData, result);
    }
    public void PerformRequest()
    {
        GameObject RequestObject;

        if (GameController != null)
        {
            RequestObject = GameController.GetComponent<GameController>().REQUESTOBJECT;

            if (RequestObject.transform.parent.parent.parent.name == "Achievement") AchieveManager(RequestObject);
            else if (RequestObject.transform.parent.parent.parent.name == "Enhance") EnhanceManager(RequestObject);
        }
    }
    private void CharacterInform(GameObject obj)
    {
        // ĳ���� �̹���
        Canvas.transform.Find("ScreenPanels").transform.Find("Inform").transform.Find("Image").GetComponent<Image>().sprite = obj.transform.Find("ObjImage").GetComponent<SpriteRenderer>().sprite;
        // ĳ���� �̸�
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
                // egg obj ����
                Destroy(obj);
                GameData.Gold += 100;
                GameData.NowGold += 100;
                // ĳ���� ����
                Character = GameController.GetComponent<GameController>().CreatePrefab("Character", -1, new Vector3(0.0f, 0.0f, 0.0f));
                Character.transform.SetParent(Canvas.transform);
                Character.transform.SetSiblingIndex(1);
                Character.name = Character.name.Substring(0, Character.name.IndexOf('('));
                switch(Character.transform.tag)
                {
                    case "Person":
                        GameData.PersonNum++;
                        break;
                    case "Animal":
                        GameData.AnimalNum++;
                        break;
                    case "Etc":
                        GameData.EtcNum++;
                        break;
                    default:
                        break;
                }
                GameData.CharacterNum++;

                GameData.NowClickCount = 0;
                //GameData.NeedClickCount *= 2;
                Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(true);
                // ĳ���� ���� �ѱ��
                CharacterInform(Character);
            }
        }
        else
        {
            // ������ ĳ���� ����
            Character = Canvas.transform.GetChild(1).gameObject;
            Destroy(Character);

            // egg obj ����
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
        GameData.TimeStackNum++;
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
        GameController = GameObject.Find("GameController").GetComponent<GameController>().gameObject;
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

        if (GameData != null)
        {
            if (this.name == "Button" && this.transform.parent.parent.parent.parent.name == "Enhance")
            {
                // �����Ϳ��� ��ȭ ������ ���÷������� ���� ���� �Ǵ��ϰ� UI ��Ƽ�� ����
                GameDataType = GameData.GetType();

                if (GameDataType.GetField(transform.parent.name).GetValue(GameData) != null)
                {

                    bool Clear = Convert.ToBoolean(GameDataType.GetField(transform.parent.name).GetValue(GameData));
                    // ���������� �ʿ��� ��ȭ�� ��� ó���� ��Ȱ��ȭ
                    if (transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text != "None")
                    {
                        if (!Clear)
                        {
                            bool PreClear = Convert.ToBoolean(GameDataType.GetField(transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text).GetValue(GameData));
                            transform.parent.transform.Find("CanNotUse").gameObject.SetActive(!PreClear);
                            GetComponent<Button>().interactable = PreClear;
                        }
                        else
                        {
                            transform.parent.transform.Find("Complete").gameObject.SetActive(Clear);
                            GetComponent<Button>().interactable = !Clear;
                        }

                    }
                    else
                    {
                        transform.parent.transform.Find("Complete").gameObject.SetActive(Clear);
                        GetComponent<Button>().interactable = !Clear;
                    }
                }
            }
            else if (this.name == "Button" && this.transform.parent.parent.parent.parent.name == "Achievement")
            {
                GameDataType = GameData.GetType();
                ScriptType = GameController.GetComponent<Achievement>().GetType();

                // �����Ϳ��� ���� ������ ���÷���
                if (GameDataType.GetField(transform.parent.name) != null)
                {
                    bool Clear = Convert.ToBoolean(GameDataType.GetField(transform.parent.name).GetValue(GameData));
                    bool Acquire = Convert.ToBoolean(GameDataType.GetField("Acquire" + transform.parent.name).GetValue(GameData));

                    if (Clear)
                    {
                        if (Acquire)
                        {
                            transform.parent.transform.Find("Complete").gameObject.SetActive(Clear);
                            GetComponent<Button>().interactable = !Clear;
                        }
                        else
                        {
                            transform.parent.transform.Find("Complete").gameObject.SetActive(!Clear);
                            GetComponent<Button>().interactable = Clear;
                        }
                    }
                    else
                    {
                        bool result = false;

                        if (ScriptType.GetMethod(transform.parent.name) != null)
                        {
                            // ��ũ��Ʈ���� �ش� ������ ������ �̸��� �Լ��� ���÷������� ����
                            // ���� ���� �޾Ƽ� ���� ������ ����
                            result = Convert.ToBoolean(ScriptType.GetMethod(transform.parent.name).Invoke(GameController.GetComponent<Achievement>(), null));
                            GameDataType.GetField(transform.parent.name).SetValue(GameData, result);
                        }

                        GetComponent<Button>().interactable = (result);
                    }
                }
            }
        }
        
    }
}

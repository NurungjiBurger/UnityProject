using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

using Random = UnityEngine.Random;
public class ButtonController : MonoBehaviour
{
    GameObject GameController;
    private GameObject Canvas;
    private GameObject Caffe;

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

        ScriptType = GameController.GetComponent<EnhanceAchievement>().GetType();

        
        if (result && obj.transform.Find("IsRecursive").Find("Value").GetComponent<TextMeshProUGUI>().text == "True")
        {
            ScriptType.GetMethod(obj.name).Invoke(GameController.GetComponent<EnhanceAchievement>(), new object[] { obj });
        }
        else
        {
            GameDataType = GameData.GetType();
            GameDataType.GetField(obj.gameObject.name).SetValue(GameData, result);

            if (ScriptType.GetMethod(obj.name) != null) ScriptType.GetMethod(obj.name).Invoke(GameController.GetComponent<EnhanceAchievement>(), new object[] { obj });
        }
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
                GameData.NowGold += 100;
                // 확률에 따라서 캐릭터 생성
                if (Random.Range(0, 100) <= GameData.HatchProbability)
                {
                    Character = GameController.GetComponent<GameController>().CreatePrefab("Character", -1, new Vector3(0.0f, 0.0f, 0.0f));
                    Character.transform.SetParent(Canvas.transform);
                    Character.transform.SetSiblingIndex(1);
                    switch (Character.transform.tag)
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

                    Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(true);
                    CharacterInform(Character);
                }
                else
                {
                    // egg obj 생성
                    obj = GameController.GetComponent<GameController>().CreatePrefab("Egg", 0, new Vector3(0.0f, 0.0f, 0.0f));
                    obj.transform.SetParent(Canvas.transform);
                    obj.transform.SetSiblingIndex(1);
                    obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    obj.gameObject.name = "EggButton";
                }

                GameData.NowClickCount = 0;
                //GameData.NeedClickCount *= 2;
            }
        }
        else
        {
            // 중복된 캐릭터 생성이면 삭제
            Character = Canvas.transform.GetChild(1).gameObject;
            int i = 0;

            for (i = 0; i < GameData.CharacterDatas.Count - 1; i++)
            {
                if (GameData.CharacterDatas[i].PrfNum == GameData.CharacterDatas[GameData.CharacterDatas.Count - 1].PrfNum)
                {
                    GameData.CharacterDatas.RemoveAt(GameData.CharacterDatas.Count - 1);
                    Destroy(Character);
                    Character = null;
                    break;
                }
            }
            // 생성된 캐릭터 이동
            if (Character != null)
            {
                Character.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
                Character.transform.parent = Caffe.transform.Find("ScreenPanels").Find("Field");
                Character.transform.position = Caffe.transform.position;
            }

            // egg obj 생성
            obj = GameController.GetComponent<GameController>().CreatePrefab("Egg", 0, new Vector3(0.0f, 0.0f, 0.0f));
            obj.transform.SetParent(Canvas.transform);
            obj.transform.SetSiblingIndex(1);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.gameObject.name = "EggButton";
        }
    }
    public void AdaptStackDamage()
    {
        EggManager(TimeStack);
        GameData.TimeStackNum += TimeStack;
        TimeStack = 0;
        Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
        Canvas.transform.Find("UpPanels").transform.Find("StackButton").GetComponent<Button>().interactable = false;
    }
    public void CameraModeManager()
    {

    }
    public void ChangeSceneManager(String name)
    {
        if (name == "Caffe")
        {
            GameObject.Find("Main Camera").GetComponent<Camera>().depth = -2;
            GameObject.Find("Caffe Camera").GetComponent<Camera>().depth = -1;
        }
        else if (name == "Game")
        {
            GameObject.Find("Main Camera").GetComponent<Camera>().depth = -1;
            GameObject.Find("Caffe Camera").GetComponent<Camera>().depth = -2;
        }
    }
    public void ScreenOnOff(GameObject obj)
    {
        if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
        else obj.gameObject.SetActive(true);
    }
    public void SaveRequestObject(GameObject obj)
    {
        GameController.GetComponent<GameController>().SaveRequestObejct(obj);
    }
    private void Awake()
    {

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
            Caffe = GameObject.Find("Caffe").gameObject;
            GameData = GameController.GetComponent<GameController>().GAMEDATA;
            TimeStack = GameController.GetComponent<GameController>().TIMESTACK;
            IsCreateCharacter = GameController.GetComponent<GameController>().ISCREATECHARACTER;            
        }

        if (GameData != null)
        {
            if (this.name == "Button" && this.transform.parent.parent.parent.parent.name == "Enhance")
            {
                // 데이터에서 강화 변수를 리플렉션으로 값을 얻어와 판단하고 UI 액티브 설정
                GameDataType = GameData.GetType();

                if (GameDataType.GetField(transform.parent.name).GetValue(GameData) != null)
                {

                    bool Clear = Convert.ToBoolean(GameDataType.GetField(transform.parent.name).GetValue(GameData));

                    // 반복적으로 강화가 가능한 경우 계속해서 업데이트 해줘야함

                    if (transform.parent.Find("IsRecursive").Find("Value").GetComponent<TextMeshProUGUI>().text == "True")
                    {
                        transform.parent.Find("Cost").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.CostForClickPower);
                    }
                    else
                    {
                        // 선행조건이 필요한 강화의 경우 처음에 비활성화
                        if (transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text != "None")
                        {
                            bool PreClear = Convert.ToBoolean(GameDataType.GetField(transform.parent.Find("Prerequisites").GetComponent<TextMeshProUGUI>().text).GetValue(GameData));

                            transform.parent.Find("CanNotUse").gameObject.SetActive(!PreClear);

                            if (!PreClear)
                            {
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
            }
            else if (this.name == "Button" && this.transform.parent.parent.parent.parent.name == "Achievement")
            {
                GameDataType = GameData.GetType();
                ScriptType = GameController.GetComponent<EnhanceAchievement>().GetType();

                // 데이터에서 업적 변수를 리플렉션
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
                            // 스크립트에서 해당 변수와 동일한 이름의 함수를 리플렉션으로 실행
                            // 값을 리턴 받아서 업적 데이터 갱신
                            result = Convert.ToBoolean(ScriptType.GetMethod(transform.parent.name).Invoke(GameController.GetComponent<EnhanceAchievement>(), null));
                            GameDataType.GetField(transform.parent.name).SetValue(GameData, result);
                        }

                        GetComponent<Button>().interactable = (result);
                    }
                }
            }
        }
        
    }
}

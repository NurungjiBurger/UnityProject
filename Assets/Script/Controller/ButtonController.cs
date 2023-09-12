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

    public void AudioPlay()
    {
        // 오디오 위탁 재생
        GameObject.Find("Audio").GetComponent<AudioController>().AudioPlay(GetComponent<AudioSource>());
    }
    private void AchieveManager(GameObject obj)
    {
        String CostType = obj.transform.Find("Reward").transform.Find("Image").Find("Type").GetComponent<TextMeshProUGUI>().text;
        int Reward = Convert.ToInt32(obj.transform.Find("Reward").Find("Quantity").GetComponent<TextMeshProUGUI>().text);

        // 강화와 달리 업적의 경우 소지금을 계산해서 가능 불가능 여부를 따질 필요가 없이 소지금을 추가해주기만하면 됨
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

        // 소지금을 통해서 강화를 진행하는 것이므로 소지금을 확인할 수 있어야함
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

        // 클릭파워와 같이 여러번 강화 가능한 오브젝트인지 확인할 수 있는 오브젝트를 추가해둠
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

            if (RequestObject.transform.parent.name == "Setting") GameObject.Find("Data").GetComponent<DataController>().DeleteGameData();
            else if (RequestObject.transform.parent.parent.parent.name == "Achievement") AchieveManager(RequestObject);
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

        // 알의 위치는 계층구조에서 항상 고정
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

                // 현재 클릭횟수 초기화 및 알 체력 증가
                GameData.NowClickCount = 0;
                GameData.NeedClickCount *= 2;
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
                GameData.MovedCharacterNum++;

            }

            // egg obj 생성
            obj = GameController.GetComponent<GameController>().CreatePrefab("Egg", 0, new Vector3(0.0f, 0.0f, 0.0f));
            obj.transform.SetParent(Canvas.transform);
            obj.transform.SetSiblingIndex(1);
            obj.transform.localPosition = new Vector3(-30.0f, -70.0f, 0.0f);
            obj.transform.localScale = new Vector3(4.0f, 2.0f, 2.0f);
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
        string tag = GameController.GetComponent<GameController>().REQUESTOBJECT.tag;

        // 캐릭터인 경우에만 카메라 모드에 진입할 수 있음
        if (tag == "Person" || tag == "Animal" || tag == "Etc") GameController.GetComponent<GameController>().SetIsCameraOn(true);
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
        // 작업을 요청한 오브젝트를 GameController에 저장
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
            if (this.name == "Button" && this.transform.parent.name == "Setting")
            {

            }
            else
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
}

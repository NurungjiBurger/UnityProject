using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    private int TimeStack = -1;
    private Data GameData;
    private bool Restore = false;

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject UserInform;
    [SerializeField]
    private GameObject Collection;
    [SerializeField]
    private GameObject SelectArrow;
    [SerializeField]
    private GameObject Gem;
    [SerializeField]
    private GameObject Gold;
    [SerializeField]
    private GameObject Level;
    [SerializeField]
    private GameObject CharacterCamera;

    [SerializeField]
    private GameObject PrefabEgg;
    [SerializeField]
    private GameObject[] PrefabObjects;
    [SerializeField]
    private Sprite[] EggImage;

    private bool IsCreateCharacter = false;
    private GameObject Character = null;
    private GameObject Egg = null;

    private bool IsCameraOn = false;

    private GameObject RequestObejct = null;

    private static GameController Instance = null;


    /////////////////////////////////

    public int TIMESTACK { get { return TimeStack; } }
    public Data GAMEDATA { get { return GameData; } }
    public bool ISCREATECHARACTER { get { return IsCreateCharacter; } }
    public GameObject NOWCHARACTER { get { return Character; } }
    public GameObject REQUESTOBJECT { get { return RequestObejct; } }
    public void SetIsCameraOn(bool val) { IsCameraOn = val; }    
    public static GameController INSTANCE { get { return Instance; } }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void SaveRequestObejct(GameObject obj)
    {
        // 캐릭터 오브젝트의 경우 클릭시 표시가 OnOff 되기 위해 한번 더 체크
        if (RequestObejct == obj && (obj.tag == "Person" || obj.tag == "Animal" || obj.tag == "Etc") ) RequestObejct = null;
        else RequestObejct = obj;
    }
    public GameObject CreatePrefab (String Type, int prefabnum, Vector3 pos)
    {
        if (prefabnum < 0) prefabnum = Random.Range(0, PrefabObjects.Length);
        switch(Type)
        {
            case "Egg":
                IsCreateCharacter = false;
                Character = null;
                return Instantiate(PrefabEgg, pos, Quaternion.identity);
            case "Character":
                Character = Instantiate(PrefabObjects[prefabnum], pos, Quaternion.identity);
                Character.name = Character.name.Substring(0, Character.name.IndexOf('('));
                float[] Position = new float[3];
                Position[0] = pos.x; Position[1] = pos.y; Position[2] = pos.z;
                GameData.CharacterDatas.Add(new CharacterData(Character.name, prefabnum, GameData.CharacterDatas.Count, Position));
                Character.GetComponent<Character>().Index = GameData.CharacterDatas.Count - 1;
                IsCreateCharacter = true;
                return Character;
            default:
                return null;
        }
    }
    private void UpdateUI()
    {
        Canvas.transform.Find("UpPanels").Find("Gold").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.NowGold);
        Canvas.transform.Find("UpPanels").Find("Gem").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.NowGem);
        Canvas.transform.Find("UpPanels").Find("Level").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.EggLevel);
        Canvas.transform.Find("UpPanels").Find("HPRatio").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString((1.0 - (GameData.NowClickCount / GameData.NeedClickCount)) * 100) + "%";
        Canvas.transform.Find("UpPanels").Find("HP").Find("NowHP").GetComponent<Image>().fillAmount = 1.0f - (GameData.NowClickCount / GameData.NeedClickCount);

        // 남은 HP에 따라 EGG 의 이미지 변경
        if (Egg != null)
        {
            switch ((1.0 - (GameData.NowClickCount / GameData.NeedClickCount)) * 100)
            {
                case <=20:
                    Egg.GetComponent<Image>().sprite = EggImage[0];
                    break;
                case <=60:
                    Egg.GetComponent<Image>().sprite = EggImage[1];
                    break;
                case >=80:
                    Egg.GetComponent<Image>().sprite = EggImage[2];
                    break;
                default:
                    break;
            }
        }
    }
    private void ManageUserInform()
    {
        UserInform.transform.Find("EggLevel").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.EggLevel);
        UserInform.transform.Find("EggHP").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.NeedClickCount - GameData.NowClickCount);
        UserInform.transform.Find("ClickPower").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.ClickPower);
        UserInform.transform.Find("TimeStackNum").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.TimeStackNum);
        UserInform.transform.Find("Gold").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.Gold);
        UserInform.transform.Find("Gem").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.Gem);
        UserInform.transform.Find("UpgradeCount").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.EnhanceNum);
        UserInform.transform.Find("CharacterCount").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.CharacterNum);
        UserInform.transform.Find("HatchProbabilty").Find("Value").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.HatchProbability) + " %";
    }
    private void ManageColletionInform()
    {
        if (GameData.CharacterDatas.Count != 0)
        {
            GameObject tmp = Collection.transform.Find("Viewport").Find("Content").gameObject;
            for (int i = 0; i < tmp.transform.childCount; i++)
            {
                if (GameData.CharacterDatas.Count != 0)
                {
                    for (int j = 0; j < GameData.CharacterDatas.Count; j++)
                    {
                        if (GameData.CharacterDatas[j].Name == tmp.transform.GetChild(i).gameObject.name)
                        {
                            tmp.transform.GetChild(i).Find("Image").GetComponent<Image>().color = new Vector4(255.0f, 255.0f, 255.0f, 255.0f);
                            tmp.transform.GetChild(i).Find("Complete").gameObject.SetActive(true);
                            tmp.transform.GetChild(i).Find("Button").gameObject.SetActive(true);
                        }
                    }
                }
            }

            if (RequestObejct != null)
            {
                Collection.transform.Find("Details").Find("Image").GetComponent<Image>().sprite = RequestObejct.transform.Find("Image").GetComponent<Image>().sprite;
                Collection.transform.Find("Details").Find("Name").GetComponent<TextMeshProUGUI>().text = RequestObejct.name;
                Collection.transform.Find("Details").Find("Description").GetComponent<TextMeshProUGUI>().text = RequestObejct.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>().text;
            }
        }
    }
    private void SetResolution()
    {
        // 9 : 16 
        //Screen.SetResolution((3 / 4) * 1920, 1920, true);

        //Default 해상도 비율
        float fixedAspectRatio = 9f / 16f;

        //현재 해상도의 비율
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        //현재 해상도 가로 비율이 더 길 경우
        if (currentAspectRatio > fixedAspectRatio)
        {
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            GameObject.Find("Caffe").GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        //현재 해상도의 세로 비율이 더 길 경우
        else if (currentAspectRatio < fixedAspectRatio)
        {
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            GameObject.Find("Caffe").GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }

    private void CalculateTimeStack()
    {
        // 스택을 활성화한 상태라면
        if (GameData.ActivateStack)
        {
            // 처음 게임이 시작된경우에는 이전 종료 타임이 없으므로 누적된 스택은 0
            if (GameData.EndTime == "")
            {
                TimeStack = 0;
            }
            // 누적 스택 계산
            else
            {
                DateTime StartTime = DateTime.Now;
                TimeSpan TimeDiff = StartTime - Convert.ToDateTime(GameData.EndTime);
                TimeStack = Convert.ToInt32(TimeDiff.TotalSeconds);
                // 누적되는 스택은 걍화단계에 따라 다름
                TimeStack /= GameData.TimeInterval;
            }
            Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
        }
        else TimeStack = 0;

        GameData.TImeStack += TimeStack;
    }

    private void RestoreCharacter()
    {
        if (GameData.CharacterDatas.Count != 0)
        {
            CharacterData tmp;
            for (int i = 0; i < GameData.CharacterDatas.Count; i++)
            {
                tmp = GameData.CharacterDatas[i];

                Character = Instantiate(PrefabObjects[tmp.PrfNum], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                Character.name = Character.name.Substring(0, Character.name.IndexOf('('));
                Character.transform.parent = GameObject.Find("Caffe").transform.Find("ScreenPanels").Find("Field");
                Character.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
                Character.transform.localPosition = new Vector3(tmp.Position[0], tmp.Position[1], tmp.Position[2]);
                Character.GetComponent<Character>().Index = i;
            }
        }
        Restore = true;
        Character = null;
    }
    private void Awake()
    {
        // 싱글톤
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetResolution();
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            // 게임 종료 시점 저장
            if (GameData != null) GameData.EndTime = DateTime.Now.ToString();
            ExitGame();
        }
    }

    private void FixedUpdate()
    {
        if (GameData == null)
        {
            GameData = GameObject.Find("Data").GetComponent<DataController>().GAMEDATA;
        }
        else
        {
            // 누적된 스택 계산
            if (TimeStack < 0) CalculateTimeStack();
            // 카페에 있는 캐릭터 복구
            if (!Restore) RestoreCharacter();
        }

        if (Canvas == null) Canvas = GameObject.Find("Canvas").gameObject;

        if (GameObject.Find("Main Camera").GetComponent<Camera>().depth == -1)
        {
            if (UserInform == null) UserInform = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("UserInformation").gameObject;
            if (Collection == null) Collection = GameObject.Find("Canvas").transform.Find("ScreenPanels").Find("Collection").gameObject;
            if (Gem == null) Gem = GameObject.Find("Canvas").transform.Find("UpPanels").Find("Gem").gameObject;
            if (Gold == null) Gold = GameObject.Find("Canvas").transform.Find("UpPanels").Find("Gold").gameObject;
            if (Level == null) Level = GameObject.Find("Canvas").transform.Find("UpPanels").Find("Level").gameObject;
            if (Egg == null && Character == null) Egg = GameObject.Find("Canvas").transform.Find("EggButton").gameObject;

            if (GameData != null)
            {
                // UI 업데이트
                if (UserInform.gameObject.activeSelf) ManageUserInform();
                if (Collection.gameObject.activeSelf) ManageColletionInform();
                UpdateUI();
            }
            if (IsCreateCharacter)
            {
                Character.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
            }


        }
        else if (GameObject.Find("Main Camera").GetComponent<Camera>().depth == -2)
        {
            if (CharacterCamera == null) CharacterCamera = GameObject.Find("Character Camera").gameObject;
            else
            {
                // 현재 캐릭터를 선택한 상태인지 확인
                if (SelectArrow == null) SelectArrow = GameObject.Find("Caffe").transform.Find("ScreenPanels").Find("Filed").Find("SelectArrow").gameObject;
                else
                {
                    if (RequestObejct != null)
                    {
                        // 카메라 모드로 진입
                        if (IsCameraOn)
                        {
                            SelectArrow.SetActive(false);
                            CharacterCamera.GetComponent<Camera>().depth = -1;
                            GameObject.Find("Main Camera").GetComponent<Camera>().depth = -2;
                            GameObject.Find("Caffe Camera").GetComponent<Camera>().depth = -3;

                            CharacterCamera.GetComponent<Camera>().transform.localPosition = new Vector3(RequestObejct.transform.localPosition.x, RequestObejct.transform.localPosition.y, RequestObejct.transform.localPosition.z - 100.0f);
                            CharacterCamera.GetComponent<Camera>().transform.LookAt(RequestObejct.transform);
                            CharacterCamera.GetComponent<Camera>().orthographicSize = 1;

                            // 관찰용이므로 클릭하거나 터치하게되면 모드 종료
                            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) IsCameraOn = !IsCameraOn;

                        }
                        else
                        {
                            SelectArrow.SetActive(true);
                            SelectArrow.transform.localPosition = RequestObejct.transform.localPosition + new Vector3(0.0f, 100.0f, 0.0f);
                            GameObject.Find("Main Camera").GetComponent<Camera>().depth = -2;
                            GameObject.Find("Caffe Camera").GetComponent<Camera>().depth = -1;
                            CharacterCamera.GetComponent<Camera>().depth = -3;
                        }
                    }
                    else
                    {
                        SelectArrow.SetActive(false);
                    }
                }
            }
        }
    }
}

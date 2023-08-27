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

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject PrefabEgg;
    [SerializeField]
    private GameObject[] PrefabObjects;
    [SerializeField]
    private GameObject Gem;
    [SerializeField]
    private GameObject Gold;
    [SerializeField]
    private GameObject Level;

    private bool IsCreateCharacter = false;
    private GameObject Character;

    private GameObject RequestObejct = null;


    /////////////////////////////////

    public int TIMESTACK { get { return TimeStack; } }
    public Data GAMEDATA { get { return GameData; } }
    public bool ISCREATECHARACTER { get { return IsCreateCharacter; } }
    public GameObject REQUESTOBJECT { get { return RequestObejct; } }

    public void SaveRequestObejct(GameObject obj)
    {
        RequestObejct = obj;
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public GameObject CreatePrefab (String Type, int prefabnum, Vector3 pos)
    {
        if (prefabnum < 0) prefabnum = Random.Range(0, PrefabObjects.Length);
        switch(Type)
        {
            case "Egg":
                IsCreateCharacter = false;
                return Instantiate(PrefabEgg, pos, Quaternion.identity);
                break;
            case "Character":
                Character = Instantiate(PrefabObjects[prefabnum], pos, Quaternion.identity);
                IsCreateCharacter = true;
                return Character;
                break;
            default:
                return null;
                break;
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
        //if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1;
        //현재 해상도의 세로 비율이 더 길 경우
        //else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 0;
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

            }
            Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
        }
        else TimeStack = 0;

        GameData.TImeStack += TimeStack;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        GameData = GameObject.Find("Data").GetComponent<DataController>().GameData;
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
            GameData = GameObject.Find("Data").GetComponent<DataController>().GameData;
        }
        else
        {
            // 누적된 스택 계산
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (IsCreateCharacter)
        {
            Character.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }

        Canvas.transform.Find("UpPanels").Find("Gold").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.NowGold);
        Canvas.transform.Find("UpPanels").Find("Gem").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.NowGem);
        Canvas.transform.Find("UpPanels").Find("Level").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.EggLevel);
        Canvas.transform.Find("UpPanels").Find("HPRatio").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString((1.0 - (GameData.NowClickCount / GameData.NeedClickCount)) * 100) + "%";
        Canvas.transform.Find("UpPanels").Find("HP").Find("NowHP").GetComponent<Image>().fillAmount = 1.0f - (GameData.NowClickCount / GameData.NeedClickCount);
    }
}

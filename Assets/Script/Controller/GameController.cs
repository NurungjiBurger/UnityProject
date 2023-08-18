using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    private Data GameData;
    private int TimeStack = -1;

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject PrefabEgg;

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    private void CharacterInform(GameObject obj)
    {
        // 캐릭터 이미지
        // 캐릭터 이름
    }
    private void EggManager()
    {
        GameObject obj;

        if (Canvas.transform.GetChild(1).gameObject.name == "EggButton")
        {
            obj = Canvas.transform.Find("EggButton").gameObject;
            if (GameData != null)
            {
                GameData.NowClickCount += GameData.ClickPower;
                //Debug.Log(GameData.EggLevel + " /// " + GameData.NeedClickCount + " /// " + GameData.NowClickCount + " /// " + GameData.ClickPower);
            }
            if (GameData.NeedClickCount <= GameData.NowClickCount)
            {
                Destroy(obj);
                // 캐릭터 생성
                GameData.NowClickCount = 0;
                GameData.NeedClickCount *= 2;
                Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(true);
                // 캐릭터 정보 넘기기
                //CharacterInform(obj);
            }
        }
        else
        {
            obj = Instantiate(PrefabEgg, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            obj.transform.SetParent(Canvas.transform);
            obj.transform.SetSiblingIndex(1);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            obj.GetComponent<Button>().onClick.AddListener(delegate { ButtonControl(obj); });
            obj.gameObject.name = "EggButton";
        }
    }
    public void ButtonControl(GameObject obj)
    {
        // 버튼의 종류는 두가지
        // 1. 버튼으로 기능 동작
        if (obj.name.Contains("Button"))
        {
            switch (obj.transform.name)
            {
                case "StackButton":
                    // 한반데미지 적용
                    TimeStack = 0;
                    Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
                    break;
                case "CameraButton":
                    // 카메라모드 진입
                    // 모든 UI OFF  카메라 버튼만 남아야함
                    break;
                case "BackRoomButton":
                    // 백룸 씬 전환
                    SceneManager.LoadScene("BackRoom");
                    break;
                case "GameRoomButton":
                    SceneManager.LoadScene("GameScene");
                    break;
                case "EggButton":
                    EggManager();
                    break;
                case "ConfirmButton":
                    Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(false);
                    EggManager();
                    break;
                default:
                    break;
            }
        }
        // 2. 버튼으로 스크린 띄워주기
        else
        {
            if (obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
            else obj.gameObject.SetActive(true);
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
        if (true)//GameData.ActivateStack)
        {
            // 처음 게임이 시작된경우에는 이전 종료 타임이 없으므로 누적된 스택은 0
            if (GameData.EndTime == "")
            {
                TimeStack = 0;
            }
            // 누적 스택 계산
            else {
                DateTime StartTime = DateTime.Now;
                TimeSpan TimeDiff = StartTime - Convert.ToDateTime(GameData.EndTime);
                TimeStack = Convert.ToInt32(TimeDiff.TotalSeconds);
            }
            Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
        }

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
        if (GameData == null)
        {
            GameData = GameObject.Find("Data").GetComponent<DataController>().GameData;
        }
        else
        {
            // 누적된 스택 계산
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (Input.GetKey("escape"))
        {
            // 게임 종료 시점 저장
            if (GameData != null) GameData.EndTime = DateTime.Now.ToString();
            ExitGame();
        }        
    }

    private void FixedUpdate()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    private Data GameData;
    private int TimeStack = -1;

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
                    GameObject.Find("Canvas").transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
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
            GameObject.Find("Canvas").transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
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

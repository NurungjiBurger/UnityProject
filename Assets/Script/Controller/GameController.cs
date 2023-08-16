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
        // ��ư�� ������ �ΰ���
        // 1. ��ư���� ��� ����
        if (obj.name.Contains("Button"))
        {
            switch (obj.transform.name)
            {
                case "StackButton":
                    // �ѹݵ����� ����
                    TimeStack = 0;
                    GameObject.Find("Canvas").transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
                    break;
                case "CameraButton":
                    // ī�޶��� ����
                    // ��� UI OFF  ī�޶� ��ư�� ���ƾ���

                    break;
                case "BackRoomButton":
                    // ��� �� ��ȯ
                    SceneManager.LoadScene("BackRoom");
                    break;
                case "GameRoomButton":
                    SceneManager.LoadScene("GameScene");
                    break;
                default:
                    break;
            }
        }
        // 2. ��ư���� ��ũ�� ����ֱ�
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

        //Default �ػ� ����
        float fixedAspectRatio = 9f / 16f;

        //���� �ػ��� ����
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        //���� �ػ� ���� ������ �� �� ���
        //if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1;
        //���� �ػ��� ���� ������ �� �� ���
        //else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 0;
    }

    private void CalculateTimeStack()
    {
        // ������ Ȱ��ȭ�� ���¶��
        if (true)//GameData.ActivateStack)
        {
            // ó�� ������ ���۵Ȱ�쿡�� ���� ���� Ÿ���� �����Ƿ� ������ ������ 0
            if (GameData.EndTime == "")
            {
                TimeStack = 0;
            }
            // ���� ���� ���
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
            // ������ ���� ���
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (Input.GetKey("escape"))
        {
            // ���� ���� ���� ����
            if (GameData != null) GameData.EndTime = DateTime.Now.ToString();
            ExitGame();
        }        
    }

    private void FixedUpdate()
    {
        
    }
}

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
    private Data GameData;
    private int TimeStack = -1;

    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject PrefabEgg;
    [SerializeField]
    private GameObject[] PrefabObjects;

    private bool IsCreateCharacter = false;
    private GameObject Character;

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
        // ĳ���� �̹���
        Canvas.transform.Find("ScreenPanels").transform.Find("Inform").transform.Find("Image").GetComponent<Image>().sprite = obj.transform.Find("ObjImage").GetComponent<SpriteRenderer>().sprite;
        // ĳ���� �̸�
        Canvas.transform.Find("ScreenPanels").transform.Find("Inform").transform.Find("Text").GetComponent<TextMeshProUGUI>().text = obj.name;
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
                // egg obj ����
                Destroy(obj);
                // ĳ���� ����
                //GameObject tmp
                Character = Instantiate(PrefabObjects[Random.Range(0, PrefabObjects.Length)], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                Character.transform.SetParent(Canvas.transform);
                Character.transform.SetSiblingIndex(1);
                Character.name = Character.name.Substring(0, Character.name.IndexOf('('));
                IsCreateCharacter = true;

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
            IsCreateCharacter = false;

            // egg obj ����
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
        // ��ư�� ������ �ΰ���
        // 1. ��ư���� ��� ����
        if (obj.name.Contains("Button"))
        {
            switch (obj.transform.name)
            {
                case "StackButton":
                    // �ѹݵ����� ����
                    TimeStack = 0;
                    Canvas.transform.Find("UpPanels").transform.Find("StackButton").transform.Find("Stack").GetComponent<TextMeshProUGUI>().text = Convert.ToString(TimeStack);
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
                case "EggButton":
                    EggManager();
                    break;
                case "ConfirmButton":
                    Canvas.transform.Find("ScreenPanels").transform.Find("Inform").gameObject.SetActive(false);
                    // Cafe�� ĳ���� �̵�
                    EggManager();
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
            // ������ ���� ���
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (Input.GetKey("escape"))
        {
            // ���� ���� ���� ����
            if (GameData != null) GameData.EndTime = DateTime.Now.ToString();
            ExitGame();
        }        

        if (IsCreateCharacter)
        {
            Character.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        
    }
}

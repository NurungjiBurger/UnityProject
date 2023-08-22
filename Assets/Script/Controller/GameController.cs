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


    /////////////////////////////////

    public int TIMESTACK { get { return TimeStack; } }
    public Data GAMEDATA { get { return GameData; } }
    public bool ISCREATECHARACTER { get { return IsCreateCharacter; } }

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
        if (GameData.ActivateStack)
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
                // �����Ǵ� ������ ��ȭ�ܰ迡 ���� �ٸ�
                
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
        if (Input.GetKey("escape"))
        {
            // ���� ���� ���� ����
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
            // ������ ���� ���
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (IsCreateCharacter)
        {
            Character.transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }

        Canvas.transform.Find("UpPanels").transform.Find("Gold").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.Gold);
        Canvas.transform.Find("UpPanels").transform.Find("Gem").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.Gem);
        Canvas.transform.Find("UpPanels").transform.Find("Level").Find("Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(GameData.EggLevel);
    }
}

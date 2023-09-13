using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    private string DataFile = "database.json";
    private string FilePath;

    private Data GameData;

    private bool IsEnd = false;

    private static DataController Instance = null;

    public Data GAMEDATA { get { return GameData; } }
    public static DataController INSTANCE { get { return Instance; } }
    // ���ӵ����� ����
    public void DeleteGameData()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
        IsEnd = !IsEnd;

        GameObject.Find("GameController").GetComponent<GameController>().ExitGame();
    }

    // ���ӵ����� ����
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(GameData);
        File.WriteAllText(FilePath, jsonData);
    }

    // ���ο� ���ӵ����� ����
    public void NewGameData()
    {
        GameData = new Data();
    }

    // ����� ���ӵ����� �ε�
    public void LoadGameData()
    {
        if (File.Exists(FilePath))
        {
            string jsonData = File.ReadAllText(FilePath);
            GameData = JsonUtility.FromJson<Data>(jsonData);
        }
        else NewGameData();
    }

    private void OnApplicationQuit()
    {
        if (!IsEnd) SaveGameData();
    }

    private void Awake()
    {
        // �ȵ���̵�� �н�
        FilePath = Application.persistentDataPath + DataFile;
        // PC�� �н� ( �׽�Ʈ�� )
        //FilePath = Path.Combine(Application.dataPath, DataFile);

        
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
        LoadGameData();
    }

    void Update()
    {
        if (GameData == null && !IsEnd) LoadGameData();
    }
}

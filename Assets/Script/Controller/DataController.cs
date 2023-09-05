using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    private string DataFile = "database.json";
    private string FilePath;

    private Data GameData;

    private static DataController Instance = null;

    public Data GAMEDATA { get { return GameData; } }
    public static DataController INSTANCE { get { return Instance; } }
    // ���ӵ����� ����
    public void DeleteGameData()
    {
        File.Delete(FilePath);
    }

    // ���ӵ����� ����
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(GameData);
        File.WriteAllText(FilePath, jsonData);
        Debug.Log(jsonData);

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
        SaveGameData();
    }

    private void Awake()
    {
        // �ȵ���̵�� �н�
        //filePath = Application.persistentDataPath + dataFile;
        // PC�� �н� ( �׽�Ʈ�� )
        FilePath = Path.Combine(Application.dataPath, DataFile);

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
        if (GameData == null) LoadGameData();
    }
}

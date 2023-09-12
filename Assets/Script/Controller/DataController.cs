using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    private string DataFile = "Data/database.json";
    private string FilePath;

    private Data GameData;

    private static DataController Instance = null;

    public Data GAMEDATA { get { return GameData; } }
    public static DataController INSTANCE { get { return Instance; } }
    // ���ӵ����� ����
    public void DeleteGameData()
    {
        FileInfo file_info = new FileInfo(FilePath);

        if(File.Exists(FilePath))
        {
            Debug.Log(FilePath + " /// " + DataFile + " /// " + GameData);
            //FileUtil.DeleteFileOrDirectory(FilePath);
            //file_info.Delete();
            File.Delete(FilePath);
        }
        GameObject.Find("GameController").GetComponent<GameController>().ExitGame();
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
        Debug.Log("������ ����");
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
        //FilePath = Application.persistentDataPath + DataFile;
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

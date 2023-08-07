using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    private string dataFile = "database.json";
    private string filePath;

    private Data gameData;

    public Data GameData { get { return gameData; } }

    // ���ӵ����� ����
    public void DeleteGameData()
    {
        File.Delete(filePath);
    }

    // ���ӵ����� ����
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log(jsonData);

    }

    // ���ο� ���ӵ����� ����
    public void NewGameData()
    {
        gameData = new Data();
    }

    // ����� ���ӵ����� �ε�
    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<Data>(jsonData);
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
        filePath = Path.Combine(Application.dataPath, dataFile);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadGameData();
    }

    void Update()
    {

    }
}

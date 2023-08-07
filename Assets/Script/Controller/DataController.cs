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

    // 게임데이터 삭제
    public void DeleteGameData()
    {
        File.Delete(filePath);
    }

    // 게임데이터 저장
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log(jsonData);

    }

    // 새로운 게임데이터 생성
    public void NewGameData()
    {
        gameData = new Data();
    }

    // 저장된 게임데이터 로드
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
        // 안드로이드용 패스
        //filePath = Application.persistentDataPath + dataFile;
        // PC용 패스 ( 테스트용 )
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

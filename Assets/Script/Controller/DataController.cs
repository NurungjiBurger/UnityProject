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
    // 게임데이터 삭제
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

    // 게임데이터 저장
    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(GameData);
        File.WriteAllText(FilePath, jsonData);
        Debug.Log(jsonData);

    }

    // 새로운 게임데이터 생성
    public void NewGameData()
    {
        Debug.Log("새게임 생성");
        GameData = new Data();
    }

    // 저장된 게임데이터 로드
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
        // 안드로이드용 패스
        //FilePath = Application.persistentDataPath + DataFile;
        // PC용 패스 ( 테스트용 )
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

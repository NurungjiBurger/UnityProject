using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void CalculateTimeStack()
    {
        // ������ ó�� �����Ұ�쿡�� ���� ���� Ÿ���� �����Ƿ� ������ ������ 0
        if (GameData.EndTime == "")
        {
            TimeStack = 0;
            return;
        }

        // ���� ���� ���
        DateTime StartTime = DateTime.Now;
        TimeSpan TimeDiff = StartTime - Convert.ToDateTime(GameData.EndTime);
        TimeStack = Convert.ToInt32(TimeDiff.TotalSeconds);

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

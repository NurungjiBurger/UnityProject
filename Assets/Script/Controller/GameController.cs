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
        // 게임이 처음 시작할경우에는 이전 종료 타임이 없으므로 누적된 스택은 0
        if (GameData.EndTime == "")
        {
            TimeStack = 0;
            return;
        }

        // 누적 스택 계산
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
            // 누적된 스택 계산
            if (TimeStack < 0) CalculateTimeStack();
        }

        if (Input.GetKey("escape"))
        {
            // 게임 종료 시점 저장
            if (GameData != null) GameData.EndTime = DateTime.Now.ToString();
            ExitGame();
        }        
    }

    private void FixedUpdate()
    {
        
    }
}

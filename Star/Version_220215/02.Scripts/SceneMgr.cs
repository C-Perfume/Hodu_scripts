using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMgr : MonoBehaviour
{
    static SceneMgr instance = null;

    //User's JoinedDate from UserInfo(uncreated)
    DateTime joinedDate;
    //count and day should not be changed by other class
    int weekCnt = 0;
    int weekDay = 0;
    // whether the game is from all games or not
    bool isWeekGame;

    public int WeekCnt { get => weekCnt; }
    public int WeekDay { get => weekDay; }
    public bool IsWeekGame { get => isWeekGame; }

    string[] dateTxts = new string[3];
    public string[] DateTxts { get => dateTxts; }

    //for Mainitems
    string[] w1st = { "초성게임", "점과 점을 연결하기", "버튼 외워 누르기", "같은 그림 찾기", "지는 가위 바위 보", "영상 재활" };
    string[] w2nd = { "모아라 단어", "퍼즐 맞추기", "카드 짝 맞추기", "블록 구하기", "더하기? 빼기?", "체조 하기" };
    string[] w3rd = { "단어 만들기", "다른 그림 찾기", "장바구니 물건 기억하기", "계산하기", "반대로 동글 세모 네모", "운동 하기" };
    string[] w4th = { "짧은 글 이해하기", "그림 방향 맞추기", "블록 사진 속 물건", "같은 그림 모두 찾기", "동전 계산하기", "4주차 체조" };
    string[] w5th = { "끝말잇기", "가려진 그림 찾기", "인물 기억하기", "제시 단어 듣기", "저울 무게 맞추기", "5주차 운동" };
    string[] w6th = { "끝말잇기2", "가려진 그림 찾기2", "인물 기억하기2", "제시 단어 듣기2", "저울 무게 맞추기2", "6주차 운동" };
    string[] type = { "L", "S", "M", "A", "F", "H" };
    Dictionary<string, string> sceneMatch = new Dictionary<string, string>();
    string currScene = "";
    public string CurrScene { get => currScene; }

    public static SceneMgr Instance 
    { 
        get 
        {
            if (null == instance) 
            {
                return null; 
            }
            return instance; 
        } 
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneMatches();

        DateTime today = DateTime.Now;
        weekDay = (int)today.DayOfWeek;

        //randomly set the joined day for develope
        //-UnityEngine.Random.Range(1, 43)
        joinedDate = today.AddDays(-5);
        print($"{joinedDate.ToString("yyMMdd")}");
        var joinedWDay = (int)joinedDate.DayOfWeek;

        //Match the JoinedDate to Monday of the week
        joinedDate = joinedDate.AddDays(1 - joinedWDay);
        //Count days from Monday of the week
        var jDays = (today- joinedDate).Days;
        //if the week day of joined Date is really Mon and Sun then the Training week is directly 1
        weekCnt = joinedWDay <= 1? (jDays / 7) + 1 : jDays / 7;

        print($"{joinedDate.ToString("yyMMdd")} {jDays}일 {weekCnt}주차");
        dateTxts[0] = $"{weekCnt}주 {weekDay}일차";
        dateTxts[1] = today.ToString(string.Format("ddd요일"));
        dateTxts[2] = today.ToString(string.Format("yyyy년 MM월 dd일"));
    }
    
    /// <summary>
    /// Match SceneName and GameName with KeyValue
    /// </summary>
    void SceneMatches() 
    {
        for (int i = 1; i < 7; i++)
        {
            string[] temp = GameName(i);
            for (int j = 0; j < temp.Length; j++)
            {
                //L S M A F H + w1~6
                string typetxt = $"{type[j]}w{i}";
                //.ToString("0##")
                sceneMatch.Add(temp[j], typetxt);
            }
        }
    } 
    public string[] GameName(int i) 
    { 
        if (i == 6) return w6th;  
        else if (i == 2) return w2nd;
        else if (i == 3) return w3rd; 
        else if (i == 4) return w4th; 
        else if (i == 5) return w5th;
        else return w1st;
    }

    public void Leave() 
    {
        Application.Quit();
    }

    //only for MainMenu
    public void SceneChange(string key) => SceneManager.LoadSceneAsync(key);
    public void SceneChange(string key, bool isWG) 
    {
        //key(GameName) and value(SceneName) should match each other
        currScene = key;
        var value = sceneMatch[key];
        if (value.Contains("L") || value.Contains("M") || value.Contains("A") || value.Contains("F"))
            SceneManager.LoadSceneAsync(value);
        else Debug.Log(value + key);
        //if the game is not in the week game list, isWeekGame should be false
        isWeekGame = isWG;
        //otherwise the game Name should be thrown to next scene Mgr.cs
    }
}

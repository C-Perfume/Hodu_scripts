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
    string[] w1st = { "�ʼ�����", "���� ���� �����ϱ�", "��ư �ܿ� ������", "���� �׸� ã��", "���� ���� ���� ��", "���� ��Ȱ" };
    string[] w2nd = { "��ƶ� �ܾ�", "���� ���߱�", "ī�� ¦ ���߱�", "��� ���ϱ�", "���ϱ�? ����?", "ü�� �ϱ�" };
    string[] w3rd = { "�ܾ� �����", "�ٸ� �׸� ã��", "��ٱ��� ���� ����ϱ�", "����ϱ�", "�ݴ�� ���� ���� �׸�", "� �ϱ�" };
    string[] w4th = { "ª�� �� �����ϱ�", "�׸� ���� ���߱�", "��� ���� �� ����", "���� �׸� ��� ã��", "���� ����ϱ�", "4���� ü��" };
    string[] w5th = { "�����ձ�", "������ �׸� ã��", "�ι� ����ϱ�", "���� �ܾ� ���", "���� ���� ���߱�", "5���� �" };
    string[] w6th = { "�����ձ�2", "������ �׸� ã��2", "�ι� ����ϱ�2", "���� �ܾ� ���2", "���� ���� ���߱�2", "6���� �" };
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

        print($"{joinedDate.ToString("yyMMdd")} {jDays}�� {weekCnt}����");
        dateTxts[0] = $"{weekCnt}�� {weekDay}����";
        dateTxts[1] = today.ToString(string.Format("ddd����"));
        dateTxts[2] = today.ToString(string.Format("yyyy�� MM�� dd��"));
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

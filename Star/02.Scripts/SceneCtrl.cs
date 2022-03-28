using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    static SceneCtrl instance = null;

    //User's JoinedDate from UserInfo(uncreated)
    DateTime joinedDate;
    int weekCnt = 0;
    string weekDayTxt;
    public int WeekCnt { get => weekCnt; }
    public string WeekDay { get => weekDayTxt; }
    public string main = "Main_0322";

    protected int sceneIdx;
    public int SceneIdx { get => sceneIdx; }

    public static SceneCtrl Instance
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


        DateTime today = DateTime.Now;
        weekDayTxt = today.ToString(string.Format("ddd요일"));
        //randomly set the joined day for develope
        //-UnityEngine.Random.Range(1, 43)
        joinedDate = today.AddDays(-5);
        print($"{joinedDate.ToString("yyMMdd")}");
        var joinedWDay = (int)joinedDate.DayOfWeek;

        //Match the JoinedDate to Monday of the week
        joinedDate = joinedDate.AddDays(1 - joinedWDay);
        //Count days from Monday of the week
        var jDays = (today - joinedDate).Days;
        //if the week day of joined Date is really Mon and Sun then the Training week is directly 1
        weekCnt = joinedWDay <= 1 ? (jDays / 7) + 1 : jDays / 7;

        print($"{joinedDate.ToString("yyMMdd")} {jDays}일 {weekCnt}주차");

    }

    //Data Save & Load method needed
    public void IndexChange() => sceneIdx++;

    public void SceneChange(string key) => SceneManager.LoadSceneAsync(key);
    public void Leave() => Application.Quit();
}

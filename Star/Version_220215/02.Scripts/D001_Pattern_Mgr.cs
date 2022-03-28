using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class D001_Pattern_Mgr : MonoBehaviour
{
    #region general attributes
    //need to get this from sceneMgr
    bool isWeekGame;
    int allRandom;

    bool isEasy = true;
    bool isStarted;
    float playTime;
    float savedPlayTime;
    GameObject btnPlay;
    GameObject panel;
    GameObject pResult;
    int moveCnt;
    string[] topTxts = { "", "", "" };
    [SerializeField] List<TextMeshProUGUI> tops;
    string[] bottomTxts = { "", "", "" };
    [SerializeField] List<TextMeshProUGUI> bottoms;
    int hintCnt;
    #endregion

    #region game attributes
    [SerializeField] Image originIMG;
    [SerializeField] Image[] answerIMGs;
    [SerializeField] Sprite[] easySprites;
    [SerializeField] Sprite[] hardSprites;
    int correctIdx;
    int gameCnt = 10;
    [SerializeField] int resultCnt;

    List<int> indexs = new List<int>();
    #endregion

     void Awake()
    {
        //init PlayMode
        panel = originIMG.transform.parent.parent.gameObject;
        pResult = panel.transform.parent.GetChild(3).gameObject;
        btnPlay = panel.transform.parent.GetChild(1).gameObject;
    }

     void OnEnable()
    {
        pResult.SetActive(false);
        panel.SetActive(isStarted);
        btnPlay.SetActive(!isStarted);

        isWeekGame = SceneMgr.Instance ? SceneMgr.Instance.IsWeekGame : false;
        //if allGame, given the random number for QnA sprite image
        if (!isWeekGame) allRandom = Random.Range(0, 8);
        //for TopBar
        tops[0].text = isWeekGame ?
                $"<size=50>{SceneMgr.Instance.WeekCnt}</size>" +
                $"{SceneMgr.Instance.DateTxts[0][1..]} " +
                //the sceneName should be changed by KeyValue
                $"<#FFF247>{SceneMgr.Instance.CurrScene}</color>" :
                //this will be null if the game scene play its own
                $"{tops[0].text}<size=50><#FFF247>{SceneMgr.Instance?.CurrScene}</color></size>";

        int cnt = isStarted ? --gameCnt : gameCnt;
        tops[2].text = $"잔여: {cnt}";

        //for QnA Panel
        Init_IMGs(0, SceneMgr.Instance? SceneMgr.Instance.WeekDay:0);
    }
    void OnDisable()
    {
        savedPlayTime = 0;
        gameCnt = 10;
        tops[2].text = $"잔여: {gameCnt}";
        resultCnt = 0;
        indexs.Clear();
        isEasy = true;
        isStarted = true;
    }
    void Update()
    {
        isStarted = panel.activeSelf;
        //time format - if playtime is over ??mins then the game should shut down itself?
        int time = (int)(playTime = isStarted ? playTime + Time.deltaTime : 0);
        int min = time % 3600 / 60;
        int sec = time % 3600 % 60;
        tops[1].text = isStarted ? (min != 0 ? $"{min}분 {sec}초" : $"{sec}초") : "0초";
        if(pResult.activeSelf) enabled = false;
    }

    void Init_IMGs(int multiply, int weekday)
    {
        int mode = isEasy ? 3 : 0;
        Sprite[] s_mode = isEasy ? easySprites : hardSprites;
        weekday = isWeekGame ? weekday : allRandom;

        foreach (var item in answerIMGs)
        {
            var obj = item.transform.parent.gameObject;
            obj.SetActive(false);
        }

        //for main - easy version 
        for (int i = 0; i < answerIMGs.Length - mode; i++)
        {
            var obj = answerIMGs[i].transform.parent.gameObject;
            obj.SetActive(true);
            //init Number Value
            indexs.Add(i);
            if (i != answerIMGs.Length - mode - 1) continue;
            //suffle at the very last
            for (int j = 0; j < indexs.Count; j++)
            {
                int rand = Random.Range(0, indexs.Count);
                int temp = indexs[j];
                indexs[j] = indexs[rand];
                indexs[rand] = temp;
            }

            //place sprite according to number value
            int k = 0;
            foreach (var idx in indexs) answerIMGs[k++].sprite = s_mode[idx + ((9 - mode) * (multiply + weekday))];
            //foreach (var idx in indexs) answerIMGs[k++].sprite = s_mode[idx + ((9 - mode) * multiply)];
        }
        correctIdx = Random.Range(0, indexs.Count);
        originIMG.sprite = answerIMGs[correctIdx].sprite;
    }
    public void CheckAnswer(int a)
    {
        resultCnt = a != correctIdx ? resultCnt : ++resultCnt;

        //reset_IMGs
        indexs.Clear();
        if (gameCnt == 0)
        {
            if (!isEasy)
            {
                savedPlayTime = playTime;
                panel.SetActive(false);
                //change the p_result into User's Last Play
                TMP_Text[] t = pResult.transform.GetComponentsInChildren<TMP_Text>()??null;
                int[] timeSpan = { 0, 0 };
                timeSpan[0] = (int)savedPlayTime % 3600 / 60;
                timeSpan[1] = (int)savedPlayTime % 3600 % 60;
                string ms = timeSpan[0] != 0 ? $"{timeSpan[0]}분 {timeSpan[1]}초" : $"{timeSpan[1]}초";
                t[0].text = $"<size=200>{resultCnt}/20\n</size> 플레이타임 = {ms}";
                t[1].text = isWeekGame ? "메인메뉴" : "재도전";
                pResult.SetActive(true);
            }
            else
            {
                isEasy = !isEasy; 
                gameCnt = 10;
            }
        }
        Init_IMGs((10 - gameCnt) < 5 ? 0 : 1, SceneMgr.Instance ? SceneMgr.Instance.WeekDay : 0);
        UpdateGameCnt();
    }
    public void UpdateGameCnt() => tops[2].text = gameCnt >= 0 ? $"잔여: {--gameCnt}" : "잔여: 0";
    public void GotoMain() => SceneMgr.Instance?.SceneChange("Main_S");
}

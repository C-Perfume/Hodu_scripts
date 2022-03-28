using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[Serializable]
public class MissionCountofDay
{
    public int done;
    public int total;

    public MissionCountofDay Mcnt(int done, int total)
    {
        this.done = done;
        this.total = total;
        return this;
    }
    public void Plus() { this.done++; }

}

[Serializable]
public class GamesofDay
{
    public string vocab;
    public string cognition;
    public string memory;
    public string perception;
    public string math;
    public string health;

    public GamesofDay GameName(string v, string c, string mm, string p, string mt, string h)
    {
        this.vocab = v;
        this.cognition = c;
        this.memory = mm;
        this.perception = p;
        this.math = mt;
        this.health = h;

        return this;
    }

    public string SetName(int n)
    {
        string temp = this.vocab;
        if (n == 1) temp = this.cognition;
        if (n == 2) temp = this.memory;
        if (n == 3) temp = this.perception;
        if (n == 4) temp = this.math;
        if (n == 5) temp = this.health;
        return temp;
    }
}

[Serializable]
public class IsWeekDay
{
  public bool[] isDone = { false, false, false, false, false, false };
}

public class DOTweenManager : MonoBehaviour
{
    //the week of training session
    int weekDay = 0;
    int chosenDay = 0;
    [SerializeField] List<TextMeshProUGUI> dateTexts;

    //for MissionCount
    [SerializeField] List<TextMeshProUGUI> missionTexts;
    [SerializeField] List<MissionCountofDay> mcDays = new List<MissionCountofDay>();
    [SerializeField] Transform buttonIcon;

    //for Mainitems
    [SerializeField] List<TextMeshProUGUI> topTexts;
    [SerializeField] List<GamesofDay> gameNames = new List<GamesofDay>();
    public List<string> chosenGNs = new List<string>();

    //0 doneName 1 doneType
    [SerializeField] List<Material> gNameMats;

    //backpanel
    //0 default 1 yellow 2 purple 3 orange 4 blue 5 pink 6 green
    [SerializeField] List<Sprite> backPanels;
    [SerializeField] List<Image> bgImages;

    //bool[] isdone list
    [SerializeField] List<IsWeekDay> isWDs = new List<IsWeekDay>();
    //playerprefs
    string[] p_isdone = new string[30];

    void Awake()
    {
        //initialize the Mission Count of weekday
        for (int i = 0; i < missionTexts.Count; i++)
        {
            MissionCountofDay mcDay = new MissionCountofDay();
            mcDays.Add(mcDay.Mcnt(0, 6));
            IsWeekDay wd = new IsWeekDay();
            isWDs.Add(wd);
        }

        //Match gameNames with the training week from 0 to 6
        for (int i = 0; i < 7; i++)
        {
            if (SceneMgr.Instance.WeekCnt != i) continue;
            string[] wday = SceneMgr.Instance.GameName(i);
            for (int j = 0; j < 5; j++)
            { GameNameMatch(wday); }
            break;
        }
    }
    
    void GameNameMatch(string[] wday)
    {
        GamesofDay names = new GamesofDay();
        names.GameName(wday[0], wday[1], wday[2], wday[3], wday[4], wday[5]);
        gameNames.Add(names);
    }

    void Start()
    {
        //init isdone with playerPrefs
        GetIsDone();

        //setting for the left bars info
        weekDay = SceneMgr.Instance.WeekDay;
        for (int i = 0; i < dateTexts.Count; i++) 
        { dateTexts[i].text = SceneMgr.Instance.DateTxts[i]; }

        //set chosenday = today
        chosenDay = weekDay - 1;
        
        //MainItem
        for (int j = 0; j < topTexts.Count; j++)
        {
            topTexts[j].text = gameNames[chosenDay].SetName(j);
            var t = topTexts[j].transform.parent;
            var downTxt = t.GetChild(3).GetComponent<TMP_Text>();
            //font material color change
            topTexts[j].fontMaterial = isWDs[chosenDay].isDone[j] ? gNameMats[0] : gNameMats[2];
            downTxt.fontMaterial = isWDs[chosenDay].isDone[j] ? gNameMats[1] : gNameMats[3];
            //set the back of Mainitems gray
            bgImages[j].sprite = isWDs[chosenDay].isDone[j] ? backPanels[j + 1] : backPanels[0];
        }

        for (int i = 0; i < missionTexts.Count; i++)
        {
            //if today is friday, then other days needed to show the crown should be seen at first
            if (i <= chosenDay) UpdateMCnt(i);
            else break;
        }

        foreach (var c in contents) origins.Add(c.position);
    }

    void UpdateMCnt(int i)
    {
        var t = missionTexts[i].transform;
        bool temp = mcDays[i].done == 6;
        t.gameObject.SetActive(!temp);
        
        var crown = t.parent.GetChild(3).gameObject;
        crown.SetActive(temp);
        for (int j = 0; j < missionTexts.Count; j++)
        {
            var other = missionTexts[j].transform;
            other.gameObject.SetActive(j == chosenDay);
            var otherDay = other.parent.GetChild(0).gameObject;
            otherDay.SetActive(j == chosenDay);
        }

        missionTexts[i].text = $"{mcDays[i].done}/{mcDays[i].total}";
        missionTexts[i].gameObject.SetActive(!temp);
        buttonIcon.SetParent(t.parent);
        buttonIcon.position = t.parent.GetChild(4).position;
    }

    #region DOTween variables and methods

    //contents needed to be movable with DOTween
    [SerializeField] List<Transform> contents;
    [SerializeField] Ease ease;
    //contents' original position
    List<Vector3> origins = new List<Vector3>();

    bool isAllGameMoved = false;
    Vector3 goal = new Vector3(200f, 0, 0);
    public void MoveAllGames() => contents[0].DOMove((isAllGameMoved = !isAllGameMoved) ? goal : origins[0], .5f).SetEase(ease);
    public void LeaveAllGames() { if (isAllGameMoved) MoveAllGames(); else SceneMgr.Instance.Leave(); }
    public void OnClickPlay(Image b) { }
        // => ChooseGame(b);
        //StartCoroutine(DoColor(b));
    public void OnClickPlay(int i) => ChooseGame(i);

    //need to do from here ! why Null ????
    void ChooseGame(int i)
    {
        //comment this out for development
        //mon-fri game should not be played if today is not the day 
        //if (chosenDay != weekDay - 1) yield break;

        //int i = back.transform.parent.GetSiblingIndex();
        if (isWDs[chosenDay].isDone[i]) return;
        isWDs[chosenDay].isDone[i] = true;

        //MissionCount +1
        mcDays[chosenDay].Plus();
        UpdateMCnt(chosenDay);
        SetIsDone();
        //the bool is for the isWeekGame or AllGames
        SceneMgr.Instance.SceneChange(chosenGNs[0], true);
        chosenGNs.RemoveAt(0);

        //BackPanel color change
        Image back = transform.parent.GetComponentInChildren<Image>();
        back.sprite = backPanels[i + 1];

        //font material color change
        topTexts[i].fontMaterial = gNameMats[0];
        var t = topTexts[i].transform.parent;
        var downTxt = t.GetChild(3).GetComponent<TMP_Text>();
        downTxt.fontMaterial = gNameMats[1];
    }

    void ChooseGame(Image back) 
    {
        //comment this out for development
        //mon-fri game should not be played if today is not the day 
        //if (chosenDay != weekDay - 1) yield break;

        int i = back.transform.parent.GetSiblingIndex();
        if (isWDs[chosenDay].isDone[i]) return;
        isWDs[chosenDay].isDone[i] = true;

        //MissionCount +1
        mcDays[chosenDay].Plus();
        UpdateMCnt(chosenDay);
        SetIsDone();
        //the bool is for the isWeekGame or AllGames
        SceneMgr.Instance.SceneChange(chosenGNs[0], true);
        chosenGNs.RemoveAt(0);

        //BackPanel color change
        back.sprite = backPanels[i + 1];

        //font material color change
        topTexts[i].fontMaterial = gNameMats[0];
        var t = topTexts[i].transform.parent;
        var downTxt = t.GetChild(3).GetComponent<TMP_Text>();
        downTxt.fontMaterial = gNameMats[1];
    }

    //this was for the Btn Click Animation but replaced into ChooseGame
    IEnumerator DoColor(Image back)
    {
        //comment this out for development
        //mon-fri game should not be played if today is not the day 
        //if (chosenDay != weekDay - 1) yield break;

        int i = back.transform.parent.GetSiblingIndex();
        if (isWDs[chosenDay].isDone[i]) yield break;
        isWDs[chosenDay].isDone[i] = true;

        //BackPanel color change
        back.DOFade(0, .3f);
        while (back.color.a > 0) yield return null;
        back.sprite = backPanels[i + 1];
        back.DOFade(1, .3f);
  
        //font material color change
        topTexts[i].fontMaterial = gNameMats[0];
        var t = topTexts[i].transform.parent;
        var downTxt = t.GetChild(3).GetComponent<TMP_Text>();
        downTxt.fontMaterial = gNameMats[1];
        
        //MissionCount +1
        mcDays[chosenDay].Plus();
        UpdateMCnt(chosenDay);
        while (back.color.a < 1) yield return null;
        SetIsDone();
        //the bool is for the isWeekGame or AllGames
        SceneMgr.Instance.SceneChange(chosenGNs[0], true);
        chosenGNs.RemoveAt(0);
    }
    #endregion

    void SetIsDone() 
    {
        //save isDone[?] as playerprefs
        //j = 5 / k = 6
        for (int j = 1; j < isWDs.Count + 1; j++)
        {
            for (int k = 1; k < isWDs[j - 1].isDone.Length + 1; k++)
            {
                p_isdone[j - 1] = $"{(DayOfWeek)j}{(k - 1).ToString("0#")}";
                PlayerPrefs.SetInt(p_isdone[j - 1], Convert.ToInt32(isWDs[j - 1].isDone[k-1]));
            }
        }
    }

    void GetIsDone() 
    {
        for (int j = 1; j < isWDs.Count + 1; j++)
        {
            for (int k = 1; k < isWDs[j - 1].isDone.Length + 1; k++)
            {
                p_isdone[j - 1] = $"{(DayOfWeek)j}{(k - 1).ToString("0#")}";
                int done = PlayerPrefs.GetInt(p_isdone[j - 1]);
                isWDs[j - 1].isDone[k - 1] = Convert.ToBoolean(done);
                mcDays[j - 1].done += done;
                //Debug.Log($"{p_isdone[j - 1]}_{isWDs[j - 1].isDone[k - 1]}");
            }
        }
    }

    public void ResetAllPrefs() => PlayerPrefs.DeleteAll();

    public void OnClickDayChange(int day) 
    {
        chosenDay = day;
        for (int i = 0; i < topTexts.Count; i++)
        {
            topTexts[i].text = gameNames[day].SetName(i);
            var t = topTexts[i].transform.parent;
            var downTxt = t.GetChild(3).GetComponent<TMP_Text>();
            //font material color change
            topTexts[i].fontMaterial = isWDs[day].isDone[i] ? gNameMats[0] : gNameMats[2];
            downTxt.fontMaterial = isWDs[day].isDone[i] ? gNameMats[1] : gNameMats[3];
            bgImages[i].sprite = isWDs[day].isDone[i] ? backPanels[i+1] : backPanels[0];
        }
        UpdateMCnt(day);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCtrl : MonoBehaviour
{
    enum button   
    { today, setting, exit, trial }

    [SerializeField] TMP_Text txtStage;
    int sceneIdx = 0; // 0~7 game category
    int weekCnt;
    string wDay;

    void Start()
    {
        Button[] b = GetComponentsInChildren<Button>();
        weekCnt = SceneCtrl.Instance.WeekCnt;
        wDay = SceneCtrl.Instance.WeekDay;
        sceneIdx = SceneCtrl.Instance.SceneIdx;

        
        b[(int)button.exit].onClick.AddListener(() => SceneCtrl.Instance.Leave());

        if (weekCnt > 1)
        { 
            txtStage.text = $"1주차 진행 전";
            b[(int)button.today].onClick.AddListener(() => Debug.Log("훈련 시작 주간이 아닙니다."));
        } 
        else 
        {
            if (sceneIdx == 6) txtStage.text = $"{weekCnt}주차 {wDay} 진행 완료";
            else 
            {
                b[(int)button.today].onClick.AddListener(() => SceneCtrl.Instance.SceneChange("RoadMap"));
                if (sceneIdx == 0) txtStage.text = $"{weekCnt}주차 {wDay} 진행 전"; 
                else txtStage.text = $"{weekCnt}주차 {wDay} 진행 중";
            }
        }
    }

}

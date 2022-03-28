using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoadMapCtrl : MonoBehaviour
{
    [SerializeField] float waitTime = 2;
    [SerializeField] TMP_Text txtInfo;
    [SerializeField] Transform[] icones;
    [SerializeField] Sprite iconeColors;
    int sceneIdx;
    int weekCnt;
    string wDay;
    string[] types = { "어휘력", "시지각력", "기억력", "주의력", "전두엽훈련", "영상시청" };
    void Start()
    {
        weekCnt = SceneCtrl.Instance.WeekCnt;
        wDay = SceneCtrl.Instance.WeekDay;
        sceneIdx = SceneCtrl.Instance.SceneIdx;

        txtInfo.text = $"{weekCnt}주차 {wDay} 훈련 진행 중입니다.\n현재 훈련하실 항목은 '{types[sceneIdx]}'입니다.";

        for (int i = 0; i < icones.Length; i++)
        {
            if (sceneIdx >= i) icones[i].GetComponent<Image>().sprite = iconeColors;
            else break;
        }

        StartCoroutine(GotoGameInfo(waitTime));
    }

    IEnumerator GotoGameInfo(float time)
    {
        yield return new WaitForSeconds(time);
        SceneCtrl.Instance.SceneChange("GameInfo");
    }
}

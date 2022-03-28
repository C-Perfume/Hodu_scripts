using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameInfoCtrl : MonoBehaviour
{
    enum button { start, exit }

    int sceneIdx = 0; // 0~7 game category
    int weekCnt;

    [SerializeField] Image arrow;
    [SerializeField] Sprite[] arrows;
    [SerializeField] TMP_Text gameName;
    [SerializeField] Button[] b;
    string[] nameLists = {
        "초성으로 단어 맞추기","그림 따라 그리기", "순서 외워서 누르기", "같은 그림 찾기", "지는 가위바위보", "영상시청",
        "조합해 단어 만들기"," 다른 그림 찾기", "장바구니 물건 기억하기", "계산해서 맞추기", "반대로 동그라미 세모 네모", "영상시청",
        "짧은 문장 이해하기"," 그림 방향 맞추기", "블록 위치 기억하기", "같은 그림 모두 찾기", "동전 계산하기", "영상시청",
        "초성으로 단어 맞추기","그림 따라 그리기", "순서 외워서 누르기", "같은 그림 찾기", "지는 가위바위보", "영상시청",
        "조합해 단어 만들기"," 다른 그림 찾기", "장바구니 물건 기억하기", "계산해서 맞추기", "반대로 동그라미 세모 네모", "영상시청",
        "짧은 문장 이해하기"," 그림 방향 맞추기", "블록 위치 기억하기", "같은 그림 모두 찾기", "동전 계산하기", "영상시청"};

    void Start()
    {
        weekCnt = SceneCtrl.Instance.WeekCnt;
        sceneIdx = SceneCtrl.Instance.SceneIdx;

        b[(int)button.start].onClick.AddListener(() => SceneCtrl.Instance.SceneChange($"W{weekCnt}_{sceneIdx}"));
        b[(int)button.exit].onClick.AddListener(() => SceneCtrl.Instance.SceneChange(SceneCtrl.Instance.main));

        arrow.sprite = arrows[sceneIdx];
        if(weekCnt == 1) gameName.text = nameLists[sceneIdx];
        else gameName.text = nameLists[(weekCnt-1 * 6) + sceneIdx];
    }

}

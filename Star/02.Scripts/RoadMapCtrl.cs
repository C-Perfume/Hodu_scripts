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
    string[] types = { "���ַ�", "��������", "����", "���Ƿ�", "���ο��Ʒ�", "�����û" };
    void Start()
    {
        weekCnt = SceneCtrl.Instance.WeekCnt;
        wDay = SceneCtrl.Instance.WeekDay;
        sceneIdx = SceneCtrl.Instance.SceneIdx;

        txtInfo.text = $"{weekCnt}���� {wDay} �Ʒ� ���� ���Դϴ�.\n���� �Ʒ��Ͻ� �׸��� '{types[sceneIdx]}'�Դϴ�.";

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

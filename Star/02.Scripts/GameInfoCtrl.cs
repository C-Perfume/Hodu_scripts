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
        "�ʼ����� �ܾ� ���߱�","�׸� ���� �׸���", "���� �ܿ��� ������", "���� �׸� ã��", "���� ����������", "�����û",
        "������ �ܾ� �����"," �ٸ� �׸� ã��", "��ٱ��� ���� ����ϱ�", "����ؼ� ���߱�", "�ݴ�� ���׶�� ���� �׸�", "�����û",
        "ª�� ���� �����ϱ�"," �׸� ���� ���߱�", "��� ��ġ ����ϱ�", "���� �׸� ��� ã��", "���� ����ϱ�", "�����û",
        "�ʼ����� �ܾ� ���߱�","�׸� ���� �׸���", "���� �ܿ��� ������", "���� �׸� ã��", "���� ����������", "�����û",
        "������ �ܾ� �����"," �ٸ� �׸� ã��", "��ٱ��� ���� ����ϱ�", "����ؼ� ���߱�", "�ݴ�� ���׶�� ���� �׸�", "�����û",
        "ª�� ���� �����ϱ�"," �׸� ���� ���߱�", "��� ��ġ ����ϱ�", "���� �׸� ��� ã��", "���� ����ϱ�", "�����û"};

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

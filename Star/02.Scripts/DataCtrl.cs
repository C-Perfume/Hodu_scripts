using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DataCtrl : MonoBehaviour
{
    enum button { retry, exit }

    int sceneIdx;
    int weekCnt;
    string wDay;
    [SerializeField] Button[] b;

    UnityAction[] actions = new UnityAction[2];
    void Start()
    {
        weekCnt = SceneCtrl.Instance.WeekCnt;
        wDay = SceneCtrl.Instance.WeekDay;
        sceneIdx = SceneCtrl.Instance.SceneIdx;


        actions[(int)button.retry] += new UnityAction(() => Debug.Log("실패 문제 재시작"));
        b[(int)button.retry].onClick.AddListener(actions[(int)button.retry]);

        actions[(int)button.exit] += new UnityAction(SceneCtrl.Instance.IndexChange);
        actions[(int)button.exit] += new UnityAction(() => SceneCtrl.Instance.SceneChange(SceneCtrl.Instance.main));
        b[(int)button.exit].onClick.AddListener(actions[(int)button.exit]);
    }
}

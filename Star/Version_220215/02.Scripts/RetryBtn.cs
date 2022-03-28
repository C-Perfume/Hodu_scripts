using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryBtn : MonoBehaviour
{
    [SerializeField] D001_Pattern_Mgr d001;
    bool isWG;

    void Start()
    {
        isWG = SceneMgr.Instance ? SceneMgr.Instance.IsWeekGame : false;
        Button b = GetComponent<Button>();
        if (!isWG) b.onClick.AddListener(() => d001.enabled = true);
        else b.onClick.AddListener(() => SceneMgr.Instance.SceneChange("Main_S"));
    }

   
}

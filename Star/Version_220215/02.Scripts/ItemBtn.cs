using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBtn : MonoBehaviour
{
    [SerializeField] DOTweenManager dotween;
    int i;
    void Start()
    {
        i = transform.parent.GetSiblingIndex();
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => SceneNameChange());
    }

    void SceneNameChange()
    {
        TMPro.TMP_Text[] a = transform.parent.GetComponentsInChildren<TMPro.TMP_Text>();
        dotween.chosenGNs.Add(a[0].text);
        dotween.OnClickPlay(i);
    }

}

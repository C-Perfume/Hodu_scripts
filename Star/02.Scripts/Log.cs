using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [SerializeField] TextAsset text;
    List<Dictionary<string, object>> txtList;

    void Start()
    {
        txtList = CSVReader.Read(text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            for (var i = 0; i < txtList.Count; i++)
            {
                print("text " + txtList[i]["text"]);
            }
        }
    }
}

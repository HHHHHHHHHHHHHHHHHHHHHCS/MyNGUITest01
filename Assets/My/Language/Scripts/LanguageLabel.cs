using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class LanguageLabel : MonoBehaviour
{
    [SerializeField]
    private string stringID;


    private void Start()
    {
        GetComponent<UILabel>().text =
        LanguageFile.Instance.LanguageTextDic[stringID];
    }
}

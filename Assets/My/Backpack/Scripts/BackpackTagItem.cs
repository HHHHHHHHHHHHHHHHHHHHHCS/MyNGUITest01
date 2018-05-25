using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackTagItem  : MonoBehaviour
{
    [SerializeField]
    private BackpackDataManager.BackpackTag backpackTag;

    private void Awake()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, OnClickButton);

    }

    private void OnClickButton()
    {
        BackpackDataManager.Instance.ChangeTag(backpackTag);

    }
}

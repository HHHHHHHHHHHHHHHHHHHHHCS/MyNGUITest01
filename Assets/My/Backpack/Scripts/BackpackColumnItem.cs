using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackColumnItem : MonoBehaviour
{
    [SerializeField]
    private BackpackDataManager.BackpackColumn backpackColumn;

    private void Awake()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, OnClickButton);

    }

    private void OnClickButton()
    {
        BackpackDataManager.Instance.ChangeColumn(backpackColumn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBase : MonoBehaviour, IClickEvent
{
    [SerializeField]
    private int cloudID;

    private TowerBase[] towerBaseArray;

    private void Start()
    {
        CloudManager.Instance.cloudDic.Add(cloudID, this);
        towerBaseArray = transform.parent.GetComponentsInChildren<TowerBase>();
    }

    public void OnClick()
    {
        CloudManager.Instance.UnlockCloud(cloudID);
    }

    public void HideCloud()
    {
        gameObject.SetActive(false);
        foreach (var item in towerBaseArray)
        {
            item.CreateDeaultTower();
        }
    }
}

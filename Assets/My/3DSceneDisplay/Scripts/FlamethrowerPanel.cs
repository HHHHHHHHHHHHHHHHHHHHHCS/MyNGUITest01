using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerPanel : MonoBehaviour
{
    private TowerBase.TowerInfo towerInfo;

    private void Awake()
    {
        GameObject upButton = transform.Find("UpButton").gameObject;
        GameObject closeButton = transform.Find("CloseButton").gameObject;

        UIEventListener.Get(upButton).onClick += OnClickUp;
        UIEventListener.Get(closeButton).onClick += OnClickClose;
    }

    public void OnShow(TowerBase.TowerInfo _towerInfo)
    {
        gameObject.SetActive(true);
        towerInfo = _towerInfo;
    }

    private void OnClickUp(GameObject go)
    {
        gameObject.SetActive(false);
        if (TowerBase.current)
        {
            TowerBase.current.UIEventCallBack(towerInfo,true);
        }
    }


    private void OnClickClose(GameObject go)
    {
        gameObject.SetActive(false);
    }
}

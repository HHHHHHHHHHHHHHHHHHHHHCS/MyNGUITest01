using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour, ITowerEvent
{
    public struct TowerInfo
    {
        public int id;
        public int lv;
        public string modelName;
    }


    public static TowerBase current;

    private BoxCollider boxCol;
    private Transform modelTS;

    private void Awake()
    {
        Transform root = transform;
        boxCol = gameObject.GetComponent<BoxCollider>();
        modelTS = transform.Find("Model");
        OnInit();
    }

    protected virtual void OnInit()
    {
        if(modelTS.childCount>0)
        {
            //删除原来的model  然后添加新的模型 重新复制boxCollider
            var box = modelTS.GetChild(0).GetComponent<BoxCollider>();
            boxCol.center = box.center;
            boxCol.size = box.size;
            box.enabled = false;
        }
    }

    public virtual void OnClick()
    {
        current = this;
        UIManager_3DScene.Instance.TowerBuildPanel.OnShow();
    }

    public virtual void UIEventCallBack(BuildTowerUI.BuildTowerInfo towerInfo)
    {

    }
}

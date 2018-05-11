using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerUI : MonoBehaviour
{
    public struct BuildTowerInfo
    {
        public int id;
        public string towerIcon;
        public string towerName;
        public string towerDes;
    }

    private UISprite towerIconSprite;
    private UILabel towerNameText;
    private UILabel towerDesText;

    private BuildTowerInfo towerInfo;

    private void Awake()
    {
        Transform root = transform;
        towerIconSprite = root.Find("TowerIconBg/TowerIconSprite").GetComponent<UISprite>();
        towerNameText = root.Find("TowerNameBg/TowerNameText").GetComponent<UILabel>();
        towerDesText = root.Find("TowerDesText").GetComponent<UILabel>();

        UIEventListener.Get(gameObject).onClick += OnClick;
    }

    public void OnInit(BuildTowerInfo towerInfo)
    {
        towerIconSprite.spriteName = towerInfo.towerIcon;
        towerNameText.text = towerInfo.towerName;
        towerDesText.text = towerInfo.towerDes;
    }

    private void OnClick(GameObject go)
    {
        if(TowerBase.current)
        {
            TowerBase.current.UIEventCallBack(towerInfo);
        }
    }
}

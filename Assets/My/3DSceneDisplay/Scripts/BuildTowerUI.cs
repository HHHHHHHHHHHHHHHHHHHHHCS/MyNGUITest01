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

        UIEventListener.Get(gameObject).onClick += OnClickButton;
    }

    public void OnInit(BuildTowerInfo _towerInfo)
    {
        towerInfo = _towerInfo;
        towerIconSprite.spriteName = _towerInfo.towerIcon;
        towerNameText.text = _towerInfo.towerName;
        towerDesText.text = _towerInfo.towerDes;
    }

    private void OnClickButton(GameObject go)
    {
        UIManager_3DScene.Instance.TowerBuildPanel.OnClose(null);
        if (TowerBase.current)
        {
            TowerBase.current.UIEventCallBack(towerInfo);
        }
    }
}

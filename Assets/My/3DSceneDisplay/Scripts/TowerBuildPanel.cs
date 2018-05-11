using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildPanel : MonoBehaviour
{
    private const string buildInfo = @"1|[#1]|喷火塔|我是会喷火的，hoho!
2|[#2]|炮塔|来呀，我是会打炮的！
3|[#3]|卫星塔|卫星滴滴滴滴，老司机滴滴滴滴！
4|[#4]|冰塔|pol ice。
5|[#5]|激光塔|激激激激激，bei bei bei。";

    [SerializeField]
    private BuildTowerUI buildTowerUIPrefab;

    private Transform autoSortGrid;

    private void Awake()
    {
        Transform root = transform;
        autoSortGrid = root.Find("ScrollContent/AutoSortGrid");
        GameObject closeButton = root.Find("CloseButton").gameObject;
        UIEventListener.Get(closeButton).onClick += OnClose;

        OnInitBuildInfo(buildInfo);
    }

    public void OnShow()
    {
        gameObject.SetActive(true);
    }

    public void OnClose(GameObject go)
    {
        gameObject.SetActive(false);
    }

    private void OnInitBuildInfo(string buildInfos)
    {
        string[] infos = buildInfos.Split('\n');
        foreach (var detailInfo in infos)
        {
            string[] buildInfoDetail = detailInfo.Split('|');
            BuildTowerUI.BuildTowerInfo info
                = new BuildTowerUI.BuildTowerInfo()
                {
                    id = int.Parse(buildInfoDetail[0]),
                    towerIcon = buildInfoDetail[1],
                    towerName = buildInfoDetail[2],
                    towerDes = buildInfoDetail[3],
                };
            var item = Instantiate(buildTowerUIPrefab, autoSortGrid);
            item.OnInit(info);
        }
    }
}

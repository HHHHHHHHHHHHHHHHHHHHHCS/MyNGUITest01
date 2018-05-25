using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackMono : MonoBehaviour
{
    private BackpackDataManager bdm;
    private Transform scrollViewTS, itemContentTS;
    private UIPanel scrollViewPanel;
    private UIWrapContent uiWrap;

    private List<BackpackItem> itemList;
    private List<BackpackItemInfo> nowInfoList;

    private void Awake()
    {
        bdm = BackpackDataManager.Instance;//用于初始化
        bdm.RefreshViewEvent += SetView;

        scrollViewTS = transform.Find("ContentBg/ScrollView");
        scrollViewPanel = scrollViewTS.GetComponent<UIPanel>();
        itemContentTS = scrollViewTS.Find("ItemContent");
        uiWrap = itemContentTS.GetComponent<UIWrapContent>();
        uiWrap.onInitializeItem += OnInitializeItem;

        itemList = new List<BackpackItem>();
        foreach (Transform item in itemContentTS)
        {
            itemList.Add(item.GetComponent<BackpackItem>());
        }

        bdm.RefreshView();
    }


    public void ResetView()
    {
        uiWrap.enabled = false;
        Vector3 vec3 = scrollViewTS.localPosition;
        vec3.y = 0;
        scrollViewTS.localPosition = vec3;
        scrollViewPanel.clipOffset = new Vector2(0, 0);
        nowInfoList = null;
    }

    private void SetView(List<BackpackItemInfo> infoList
        , BackpackDataManager.BackpackColumn column)
    {
        ResetView();
        nowInfoList = infoList;

        if (infoList.Count > itemList.Count)
        {
            uiWrap.enabled = true;
            uiWrap.minIndex = -(infoList.Count - 1);
            uiWrap.maxIndex = 0;
        }
        else
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (i < infoList.Count)
                {
                    itemList[i].SetInfo(nowInfoList[i]);
                    itemList[i].gameObject.SetActive(true);
                }
                else
                {
                    itemList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (nowInfoList != null && nowInfoList.Count > -realIndex)
        {
            itemList[wrapIndex].SetInfo(nowInfoList[-realIndex]);
        }

    }
}

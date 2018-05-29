using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BannerManagerMono : MonoBehaviour
{
    public static BannerManagerMono Instance { get; private set; }

    [SerializeField]
    private BannerMessageItem itemPrefab;
    [SerializeField]
    private int startPosY = 0;

    private Transform parent;
    private int length = 0;

    private const int maxLength = 10;
    private Queue<BannerMessageItem> bannerCacheQueue;
    private List<BannerMessageItem> bannerQueue;

    private void Awake()
    {
        Instance = this;
        parent = transform;
        length = itemPrefab.GetComponent<UISprite>().height;
        bannerCacheQueue = new Queue<BannerMessageItem>();
        bannerQueue = new List<BannerMessageItem>();
    }

    public void CreateMessageItem(BannerMessageItem.BannerType _type, string data)
    {
        BannerMessageItem item;
        if (bannerCacheQueue.Count <= 0)
        {
            item = Instantiate(itemPrefab,parent).OnInit(_type, data);
        }
        else
        {
            item = bannerCacheQueue.Dequeue().OnInit(_type, data);
        }
        bannerQueue.Add(item);
        SortListPos();
    }

    private void SortListPos()
    {
        int count = bannerQueue.Count - 1;
        foreach (var item in bannerQueue)
        {
            item.transform.localPosition = new Vector3(0, length * count--, 0);
        }
    }

    public void HideItem(BannerMessageItem item)
    {
        bannerQueue.Remove(item);
        if (bannerCacheQueue.Count <= maxLength)
        {
            item.gameObject.SetActive(false);
            bannerCacheQueue.Enqueue(item);
        }
        else
        {
            Destroy(item.gameObject);
        }
    }
}

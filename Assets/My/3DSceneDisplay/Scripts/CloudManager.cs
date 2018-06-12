using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public static CloudManager Instance { get; private set; }

    public Dictionary<int,CloudBase> cloudDic;

    private int nextCloud;
    private ScreenEvent screenEvent;

    private void Awake()
    {
        Instance = this;
        cloudDic = new Dictionary<int, CloudBase>();
        nextCloud = 1;
        screenEvent = GetComponent<ScreenEvent>();
    }

    private void Start()
    {
        UnlockCloud(1);
    }

    public void UnlockCloud(int id)
    {
        if(id==nextCloud)
        {
            if(cloudDic.ContainsKey(id))
            {
                cloudDic[id].HideCloud();
                nextCloud++;
            }
        }
        else
        {
            screenEvent.NavigationBuild(cloudDic[nextCloud].transform.position);
        }
    }
}

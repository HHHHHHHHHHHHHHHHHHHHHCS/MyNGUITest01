using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_3DScene : MonoBehaviour
{
    public static UIManager_3DScene Instance { get; private set; }

    public TowerBuildPanel TowerBuildPanel { get; private set; }
    public FlamethrowerPanel FlamethrowerPanel { get; private set; }

    private void Awake()
    {
        Instance = this;

        Transform root = transform;
        TowerBuildPanel = root.Find("TowerBuildPanel").GetComponent<TowerBuildPanel>();
        FlamethrowerPanel = root.Find("FlamethrowerPanel").GetComponent<FlamethrowerPanel>();
    }
}

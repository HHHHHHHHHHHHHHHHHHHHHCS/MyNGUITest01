using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    private Material level_TenMat, level_UnitMat;

    private void Awake()
    {
        level_TenMat = transform.Find("Level_Ten").GetComponent<MeshRenderer>()
            .material;
        level_UnitMat = transform.Find("Level_Unit").GetComponent<MeshRenderer>()
            .material;

        level_TenMat.SetTextureOffset("_MainTex", new Vector2(Random.Range(0, 10) / 10f, 0));
        level_UnitMat.SetTextureOffset("_MainTex", new Vector2(Random.Range(0, 10) / 10f, 0));
    }
}

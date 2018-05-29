using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerTest : MonoBehaviour
{
    public TweenAlpha ta;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int rd = Random.Range(0, 5);
            BannerManagerMono.Instance.CreateMessageItem((BannerMessageItem.BannerType)rd, Random.Range(100000, 10000000).ToString());

        }

    }
}

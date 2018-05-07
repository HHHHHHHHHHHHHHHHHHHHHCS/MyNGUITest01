using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform targetTS;

    private void Awake()
    {
        UIEventListener linstener = UIEventListener.Get(gameObject);
        linstener.onDrag += ScrollHero;
    }

    private void ScrollHero(GameObject go, Vector2 delta)
    {
        if (delta.x != 0)
        {
            targetTS.localEulerAngles += (delta.x > 0 ? -1 : 1) * Vector3.up * 180 * Time.deltaTime;
        }
    }
}

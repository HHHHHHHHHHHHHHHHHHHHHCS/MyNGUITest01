using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLTextEvent : MonoBehaviour
{
    void OnClick()
    {
        UILabel lbl = GetComponent<UILabel>();

        if (lbl != null)
        {
            string url = lbl.GetSurlAtPosition(UICamera.lastWorldPosition);
            if (!string.IsNullOrEmpty(url)) Debug.Log(url);
        }
    }
}

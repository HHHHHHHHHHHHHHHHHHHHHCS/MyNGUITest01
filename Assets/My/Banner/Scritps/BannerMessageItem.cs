using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerMessageItem : MonoBehaviour
{
    public enum BannerType
    {
        Message,
        Warrning,
        Error,
        Battle,
        Help,
    }

    private UILabel messageText;
    private UISprite messageBg, messgaeSprite;
    private TweenAlpha ta;
    private TweenScale ts;

    private void Awake()
    {
        ta = GetComponent<TweenAlpha>();
        EventDelegate.Add(ta.onFinished, OnAnimFinish);

        messageText = transform.Find("MessageText").GetComponent<UILabel>();
        messageBg = transform.GetComponent<UISprite>();
        messgaeSprite = transform.Find("MessageSprite").GetComponent<UISprite>();
        ts = messgaeSprite.GetComponent<TweenScale>();
    }

    public BannerMessageItem OnInit(BannerType _type, string data)
    {
        string spriteName = "";
        switch (_type)
        {
            case BannerType.Message:
                spriteName = "Emoticon - Angry";
                break;
            case BannerType.Warrning:
                spriteName = "Emoticon - Annoyed";
                break;
            case BannerType.Error:
                spriteName = "Emoticon - Dead";
                break;
            case BannerType.Battle:
                spriteName = "Emoticon - Frown";
                break;
            case BannerType.Help:
                spriteName = "Emoticon - Skull";
                break;
            default:
                break;
        }
        messgaeSprite.spriteName = spriteName;
        messageText.text = data;
        messageBg.alpha = 1;
        gameObject.SetActive(true);
        ta.ResetToBeginning();
        ta.PlayForward();
        ts.ResetToBeginning();
        ts.PlayForward();


        return this;
    }


    private void OnAnimFinish()
    {

        BannerManagerMono.Instance.HideItem(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChatMessagesItem : MonoBehaviour
{
    public struct ChatMessagesStruct
    {
        public string headSpriteName;
        public bool isSelf;
        public int vipLevel;
        public string playerName;
        public string chatContentMessage;

        public float posY;
    }

    public class ChatMessagesInfo
    {
        public ChatMessagesStruct chatMessagesStruct;
        public float posY;
        public bool isShow;
    }
    public ChatMessagesInfo CurrentInfo { private set; get; }

    private UIWidget widget;
    private UISprite headSprite;
    private UILabel vipText;
    private UILabel nameText;
    private UIButton functionButton;
    private UILabel chatContentText;



    public void OnCreate()
    {
        Transform root = transform;
        widget = root.GetComponent<UIWidget>();
        headSprite = root.Find("HeadMask/HeadSprite").GetComponent<UISprite>();
        vipText = root.Find("VIPText").GetComponent<UILabel>();
        nameText = root.Find("NameText").GetComponent<UILabel>();
        functionButton = root.Find("FunctionButton").GetComponent<UIButton>();
        chatContentText = root.Find("ChatContentText").GetComponent<UILabel>();

        gameObject.SetActive(false);
    }

    public void OnInit(ChatMessagesInfo _currentInfo)
    {
        CurrentInfo = _currentInfo;
        var _struct = _currentInfo.chatMessagesStruct;

        _currentInfo.isShow = true;
        transform.localPosition = new Vector3(0, _currentInfo.posY, 0);

        headSprite.spriteName = _struct.headSpriteName;
        vipText.text = "VIP" + _struct.vipLevel;
        nameText.text = _struct.playerName;
        chatContentText.text = _struct.chatContentMessage;
        widget.height = 80 + chatContentText.height;

        SetIsSelfStyle(_struct.isSelf);
        gameObject.SetActive(true);
        //return widget.height;
    }

    private void SetIsSelfStyle(bool _isSelf)
    {
        if (_isSelf)
        {
            headSprite.transform.parent.localPosition = new Vector3(234, -54, 0);
            vipText.transform.localPosition = new Vector3(-275, -17, 0);
            nameText.transform.localPosition = new Vector3(-170, -14, 0);

            chatContentText.transform.localPosition = new Vector3(-271, -69, 0);
            functionButton.gameObject.SetActive(false);
        }
        else
        {
            headSprite.transform.parent.localPosition = new Vector3(-234, -54, 0);
            vipText.transform.localPosition = new Vector3(-170, -17, 0);
            nameText.transform.localPosition = new Vector3(-65, -14, 0);

            chatContentText.transform.localPosition = new Vector3(-143, -69, 0);
            functionButton.gameObject.SetActive(true);
        }
    }

    public int GetFontSize()
    {
        if (chatContentText == null)
        {
            chatContentText = transform.Find("ChatContentText").GetComponent<UILabel>();
        }
        return chatContentText.fontSize;
    }

    public void UpdateNGUIFont()
    {
        if (chatContentText == null)
        {
            chatContentText = transform.Find("ChatContentText").GetComponent<UILabel>();
        }
        chatContentText.UpdateNGUIText();
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
        CurrentInfo.isShow = false;
        CurrentInfo = null;
    }
}

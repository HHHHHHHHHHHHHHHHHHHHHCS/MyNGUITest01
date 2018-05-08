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
    }

    public float posY;

    private UIWidget widget;
    private UISprite headSprite;
    private UILabel vipText;
    private UILabel nameText;
    private UIButton functionButton;
    private UILabel chatContentText;

    private void Awake()
    {
        Transform root = transform;
        widget = root.GetComponent<UIWidget>();
        headSprite = root.Find("HeadMask/HeadSprite").GetComponent<UISprite>();
        vipText = root.Find("VIPText").GetComponent<UILabel>();
        nameText = root.Find("NameText").GetComponent<UILabel>();
        functionButton = root.Find("FunctionButton").GetComponent<UIButton>();
        chatContentText = root.Find("ChatContentText").GetComponent<UILabel>();
    }

    public int OnInit(ChatMessagesStruct _struct)
    {
        headSprite.spriteName = _struct.headSpriteName;
        vipText.text = "VIP" + _struct.vipLevel;
        nameText.text = _struct.playerName;

        chatContentText.text = _struct.chatContentMessage;
        widget.height = 80 + chatContentText.height;
        return widget.height;
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

}

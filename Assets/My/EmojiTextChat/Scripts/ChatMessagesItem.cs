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
        public string spriteMessgae;
    }
    public class ChatMessagesInfo
    {
        public ChatMessagesStruct chatMessagesStruct;
        public float posY;
        public bool isShow;
    }

    public ChatMessagesInfo CurrentInfo { private set; get; }

    private const int widgeBaseHeight = 80;

    private UIWidget widget;
    private UISprite headSprite;
    private UILabel vipText;
    private UILabel nameText;
    private UIButton functionButton;
    private UILabel chatContentText;
    private UISprite emojiSpriteBg;
    private UISprite emojiSprite;

    public void OnCreate()
    {
        Transform root = transform;
        widget = root.GetComponent<UIWidget>();
        headSprite = root.Find("HeadMask/HeadSprite").GetComponent<UISprite>();
        vipText = root.Find("VIPText").GetComponent<UILabel>();
        nameText = root.Find("NameText").GetComponent<UILabel>();
        functionButton = root.Find("FunctionButton").GetComponent<UIButton>();
        chatContentText = root.Find("ChatContentText").GetComponent<UILabel>();
        emojiSpriteBg = root.Find("EmojiSpriteBg").GetComponent<UISprite>();
        emojiSprite = root.Find("EmojiSpriteBg/EmojiSprite").GetComponent<UISprite>();

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


        bool isSpriteMessage = !string.IsNullOrEmpty(_struct.spriteMessgae);
        if (isSpriteMessage)
        {
            emojiSprite.spriteName= _struct.spriteMessgae;
        }
        else
        {
            chatContentText.text = _struct.chatContentMessage;
            widget.height = widgeBaseHeight + chatContentText.height;
        }

        SetIsStyle(_struct.isSelf, isSpriteMessage);
        gameObject.SetActive(true);
    }

    private void SetIsStyle(bool _isSelf,bool _isSprite)
    {
        emojiSpriteBg.gameObject.SetActive(_isSprite);
        chatContentText.gameObject.SetActive(!_isSprite);
        functionButton.gameObject.SetActive(!_isSelf);
        if (_isSelf)
        {
            headSprite.transform.parent.localPosition = new Vector3(234, -54, 0);
            vipText.transform.localPosition = new Vector3(-275, -17, 0);
            nameText.transform.localPosition = new Vector3(-170, -14, 0);
            chatContentText.transform.localPosition = new Vector3(-271, -69, 0);
            emojiSpriteBg.transform.localPosition = new Vector3(-54, -97, 0);
            emojiSprite.transform.localPosition = new Vector3(180, 0, 0);
        }
        else
        {
            headSprite.transform.parent.localPosition = new Vector3(-234, -54, 0);
            vipText.transform.localPosition = new Vector3(-170, -17, 0);
            nameText.transform.localPosition = new Vector3(-65, -14, 0);
            chatContentText.transform.localPosition = new Vector3(-143, -69, 0);
            emojiSpriteBg.transform.localPosition = new Vector3(73, -97, 0);
            emojiSprite.transform.localPosition = new Vector3(-180, 0, 0);

        }
    }

    public int GetSpriteBgHeight()
    {
        if(!emojiSpriteBg)
        {
            emojiSpriteBg = transform.Find("EmojiSpriteBg").GetComponent<UISprite>();
        }
        return emojiSpriteBg.height;
    }

    public int GetFontSize()
    {
        if (!chatContentText)
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

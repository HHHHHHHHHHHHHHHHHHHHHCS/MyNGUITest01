using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.UI;

        public class AutoUICodePanel : MonoBehaviour{
private UISprite sprite;
private UILabel nameText;
private UIButton test01Button;
private UIButton test02Button;
private UISlider hPSlider;

private void Awake (){
Transform root = transform;
sprite = root.Find("Bg/HPSlider/Sprite").GetComponent<UISprite>();
nameText = root.Find("Bg/NameText").GetComponent<UILabel>();
test01Button = root.Find("Bg/Test01Button").GetComponent<UIButton>();
test02Button = root.Find("Bg/Test02Button").GetComponent<UIButton>();
hPSlider = root.Find("Bg/HPSlider").GetComponent<UISlider>();

}}

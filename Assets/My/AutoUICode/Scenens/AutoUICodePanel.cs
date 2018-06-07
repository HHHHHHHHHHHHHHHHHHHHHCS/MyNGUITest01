using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.UI;

        public class AutoUICodePanel : MonoBehaviour{
private UILabel nameText;
private UIButton test01Button;
private UIButton test02Button;
private UISlider hPSlider;

private void Awake (){
Transform root = transform;
UILabel nameText = root.Find("Bg/NameText").GetComponent<UILabel>();
UIButton test01Button = root.Find("Bg/Test01Button").GetComponent<UIButton>();
UIButton test02Button = root.Find("Bg/Test02Button").GetComponent<UIButton>();
UISlider hPSlider = root.Find("Bg/HPSlider").GetComponent<UISlider>();

}}

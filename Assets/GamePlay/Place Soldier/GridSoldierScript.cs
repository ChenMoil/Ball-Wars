using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSoldierScript : MonoBehaviour
{
    Image image;                   //显示士兵图像的ui
    public Image Image{ get { return image; } }
    Toggle toggle;                //士兵是否被选择的ui
    public Toggle Toggle{ get { return toggle; } }
    Text text;                     //显示士兵消耗金钱的Ui
    public Text Text{ get { return text; } }
    Text nameText;                     //显示小球名字的UI
    public Text Name{ get { return nameText; } }
    public SummonBall ball;        //小球
    PlaceSoliderScript placeSoliderScript;  //放置士兵功能主控脚本
    void Awake(){
        image = GetComponent<Image>();
        toggle = GetComponent<Toggle>();
        text = transform.Find("CoinText").gameObject.GetComponent<Text>();
        nameText = transform.Find("NameText").gameObject.GetComponent<Text>();
        placeSoliderScript = FindObjectOfType<PlaceSoliderScript>();
    }
    public void SelectBall(bool selected){               //根据toggle组件的选中与否改变所选中小球
        if (selected){
            placeSoliderScript.ChangeBall(ball);
            image.sprite=ball.pressedImage;
        }else{
            if(placeSoliderScript.Ball== ball){
                placeSoliderScript.ChangeBall();
            }
            image.sprite=ball.ballImage;
        }
    }
}

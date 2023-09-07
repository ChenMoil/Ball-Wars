using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Ball", menuName ="SummonBall")]
public class SummonBall:ScriptableObject
{
    [System.Serializable]
    public enum Category
    {
        all,
        human,
        elf,
        orc,
        dwarf
    }
    public Category category;
    public int coin;   //该小球所消耗的金钱
    public GameObject ball;  //该小球的prefab
    public Sprite ballImage;  //该小球的图片
    public Sprite pressedImage;  //该小球按下时的图片
}
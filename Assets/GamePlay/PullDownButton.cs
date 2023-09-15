using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullDownButton : MonoBehaviour
{
    public List<GameObject> objects;  //下拉的对象
    private float objectHeight; //下拉对象的高度(需要所有对象高度一样)
    public int SelectedObject;  //被选择的物体

    private bool isPullDown;  //是否已经下拉
    private bool isMove;      //下拉的对象是否正在移动
    public float PullDownTime = 0.5f;  //下拉所需要的时间

    public static PullDownButton instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        objectHeight = objects[0].GetComponent<RectTransform>().rect.height;
    }

    public void PullDown()
    {
        if (isPullDown == false && isMove == false) 
        {
            isMove = true;
            StopAllCoroutines();
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(true);
                objects[i].transform.DOLocalMoveY(-objectHeight * (i + 1), PullDownTime);  //让元素下移
            }
            isPullDown = true;
            isMove = false;
        }
        else if (isPullDown == true && isMove == false)
        {
            isMove = true;
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].transform.DOLocalMoveY(0, PullDownTime);  //让元素回到初始位置
                StartCoroutine(FalseGameobject(objects[i]));
            }
            isMove = false;
            isPullDown = false;
        }
    }

    public void ChooseObject(int number)
    {
        SelectedObject = number;
        GameObject.Find("Level").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/map" + number);
    }
    IEnumerator FalseGameobject(GameObject gameObject)
    {
        yield return new WaitForSeconds(PullDownTime);
        gameObject.SetActive(false);
    }
}

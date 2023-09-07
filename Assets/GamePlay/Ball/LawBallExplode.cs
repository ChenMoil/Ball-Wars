using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LawBallExplode : MonoBehaviour
{
    public float maxSize = 4f;
    private float explodeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ResizeLawBall());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void StartCon()
    {
        StartCoroutine(ResizeLawBall());
    }
    IEnumerator ResizeLawBall() //改变法球的大小
    {
        transform.DOScale(maxSize, explodeTime / 2);
        yield return new WaitForSeconds(explodeTime / 2);
        transform.DOScale(0, explodeTime / 2);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPlotManagerScript : MonoBehaviour
{
    public Text text;                //��������ui
    public List<string> lines = new List<string>();   //�Ի��б�
    public Queue<string> dialogues;             //�ݴ�Ի��Ķ���
    public GameObject canvas;                  //���黭��
    private void Awake()
    {
        dialogues = new Queue<string>(lines);
        ShowDialogue();

    }
    public void ShowDialogue()         //��ʾ�Ի�
    {
        text.text = "";
        if (dialogues.Count==0)
        {
            canvas.SetActive(false);
        }
        text.DOText(dialogues.Peek(), dialogues.Peek().Length / 5);
        dialogues.Dequeue();
    }
}

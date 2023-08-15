using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPlotManagerScript : MonoBehaviour
{
    public Text text;                //剧情文字ui
    public List<string> lines = new List<string>();   //对话列表
    public Queue<string> dialogues;             //暂存对话的队列
    public GameObject canvas;                  //剧情画布
    private void Awake()
    {
        dialogues = new Queue<string>(lines);
        ShowDialogue();

    }
    public void ShowDialogue()         //显示对话
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

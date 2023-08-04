using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPlotManagerScript : MonoBehaviour
{
    public Text text;
    public List<string> lines = new List<string>();
    public Queue<string> dialogues;
    public GameObject canvas;
    private void Awake()
    {
        dialogues = new Queue<string>(lines);
        ShowDialogue();

    }
    public void ShowDialogue()
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

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogues           //对话类
{
    public string dialogue;
    public characterDirection direction;
    public enum characterDirection
    {
        Left,
        Right,
    }
}
public class PlayPlotManagerScript : MonoBehaviour
{
    public List<Dialogues> lines = new List<Dialogues>();   //对话列表
    public Queue<Dialogues> dialogues;             //暂存对话的队列
    public GameObject canvas;                  //剧情画布
    public GameObject leftCharacter;        //左边的人物及对话框
    public GameObject rightCharacter;       //右边的人物及对话框
    public Text leftText;                //左边剧情文字ui
    public Text rightText;               //右边剧情文字Ui
    Dialogues.characterDirection direction; //当前说话的人物
    Text currentText;                   //当前文字ui
    private void Awake()
    {
        dialogues = new Queue<Dialogues>(lines);//初始化
        currentText = leftText;
        direction = Dialogues.characterDirection.Left;

        ShowDialogue();
    }
    public void ShowDialogue()         //显示对话
    {
        currentText.text = "";
        currentText.DOKill();                  //结束之前的对话显示
        if (dialogues.Count==0)         //对话播放完后禁用对话框
        {
            canvas.SetActive(false);
            return;
        }

        if (dialogues.Peek().direction!=direction)       //切换显示的人物
        {
            switch (dialogues.Peek().direction)
            {
                case Dialogues.characterDirection.Left:
                    rightCharacter.SetActive(false);
                    leftCharacter.SetActive(true);
                    currentText=leftText;
                    direction = dialogues.Peek().direction;
                    break;
                case Dialogues.characterDirection.Right:
                    leftCharacter.SetActive(false);
                    rightCharacter.SetActive(true);
                    currentText = rightText;
                    direction=dialogues.Peek().direction;
                    break;
                default:
                    break;
            }
        }

        currentText.DOText(dialogues.Peek().dialogue, dialogues.Peek().dialogue.Length / 10).SetEase(Ease.Linear);
        dialogues.Dequeue();
    }
}

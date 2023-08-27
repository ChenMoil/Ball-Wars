using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogues           //�Ի���
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
    public List<Dialogues> lines = new List<Dialogues>();   //�Ի��б�
    public Queue<Dialogues> dialogues;             //�ݴ�Ի��Ķ���
    public GameObject canvas;                  //���黭��
    public GameObject leftCharacter;        //��ߵ����Ｐ�Ի���
    public GameObject rightCharacter;       //�ұߵ����Ｐ�Ի���
    public Text leftText;                //��߾�������ui
    public Text rightText;               //�ұ߾�������Ui
    Dialogues.characterDirection direction; //��ǰ˵��������
    Text currentText;                   //��ǰ����ui
    private void Awake()
    {
        dialogues = new Queue<Dialogues>(lines);//��ʼ��
        currentText = leftText;
        direction = Dialogues.characterDirection.Left;

        ShowDialogue();
    }
    public void ShowDialogue()         //��ʾ�Ի�
    {
        currentText.text = "";
        currentText.DOKill();                  //����֮ǰ�ĶԻ���ʾ
        if (dialogues.Count==0)         //�Ի����������öԻ���
        {
            canvas.SetActive(false);
            return;
        }

        if (dialogues.Peek().direction!=direction)       //�л���ʾ������
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

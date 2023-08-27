using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogues           //�Ի���
{
    public string dialogue;       //�Ի�
    public characterDirection direction;      //˵���ķ���
    public string name;                       //����������
    public Sprite image;                       //������ͷ��
    public enum characterDirection
    {
        Left,
        Right,
    }
}
public class PlayPlotManagerScript : MonoBehaviour
{
    public List<Dialogues> prelines = new List<Dialogues>();   //ս��ǰ�Ի��б�
    public List<Dialogues> postlines = new List<Dialogues>();  //ս����Ի��б�
    public Queue<Dialogues> dialogues;             //�ݴ�Ի��Ķ���
    public GameObject canvas;                  //���黭��
    public List<GameObject> enabledGameObjects ;            //�Ի����������õ�����
    public GameObject leftCharacter;        //��ߵ����Ｐ�Ի���
    public GameObject rightCharacter;       //�ұߵ����Ｐ�Ի���
    public Text leftText;                //��߾�������ui
    public Text rightText;               //�ұ߾�������Ui
    public Text leftName;                //�����������ui
    public Text rightName;               //�ұ���������ui
    public Image leftImage;              //�������ͷ��
    public Image rightImage;             //�ұ�����ͷ��
    Dialogues.characterDirection direction; //��ǰ˵��������
    Text currentText;                   //��ǰ����ui
    Text currentName;                   //��ǰ����ui
    Image currentImage;                 //��ǰͷ��
    Tweener currentTweener;             //��ǰ���ֶ���
    private void Awake()
    {
        dialogues = new Queue<Dialogues>(prelines);//��ʼ��
        currentText = leftText;
        direction = Dialogues.characterDirection.Left;
        currentName = leftName;
        currentImage = leftImage;

        ShowDialogue();
    }
    public void ShowDialogue()         //��ʾ�Ի�
    {
        if (currentTweener != null && !currentTweener.IsComplete())//����֮ǰ�ĶԻ���ʾ
        {
            currentText.DOComplete();
            currentText.DOKill();
            return;
        }                       
        if (dialogues.Count==0)         //�Ի����������öԻ���
        { 
            canvas.SetActive(false);
            foreach (var item in enabledGameObjects)
            {
                item.SetActive(true);
            }
            return;
        }
        
        if (dialogues.Peek().direction!=direction)       //�л���ʾ������
        {
            switch (dialogues.Peek().direction)
            {
                case Dialogues.characterDirection.Left:
                    rightCharacter.GetComponent<Canvas>().sortingOrder=0;
                    leftCharacter.GetComponent<Canvas>().sortingOrder = 10;
                    currentText=leftText;
                    direction = dialogues.Peek().direction;
                    currentName=leftName;
                    currentImage=leftImage;
                    break;
                case Dialogues.characterDirection.Right:
                    leftCharacter.GetComponent<Canvas>().sortingOrder=0;
                    rightCharacter.GetComponent<Canvas>().sortingOrder=10;
                    currentText = rightText;
                    direction=dialogues.Peek().direction;
                    currentName=rightName;
                    currentImage=rightImage;
                    break;
                default:
                    break;
            }
        }
        else
        {
            currentText.text = "";
        }
        
        currentName.text=dialogues.Peek().name;             //���ŶԻ�
        currentImage.sprite=dialogues.Peek().image;
        currentTweener = currentText.DOText(dialogues.Peek().dialogue, dialogues.Peek().dialogue.Length / 10).SetEase(Ease.Linear);
        currentTweener.OnComplete(() => currentTweener = null);
        dialogues.Dequeue();
    }
    public void PostPlayDialog()         //����ս���󶯻�
    {
        if (postlines != null)
        {
            dialogues = new Queue<Dialogues>(postlines);
            postlines = null;
            canvas.SetActive(true);
            ShowDialogue();
        }
    }
}

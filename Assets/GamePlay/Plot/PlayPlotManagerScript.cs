using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogues           //对话类
{
    public string dialogue;       //对话
    public characterDirection direction;      //说话的方向
    public string name;                       //发言人名字
    public Sprite image;                       //发言人头像
    public enum characterDirection
    {
        Left,
        Right,
    }
}
public class PlayPlotManagerScript : MonoBehaviour
{
    [SerializeField] List<Dialogues> prelines = new List<Dialogues>();   //战斗前对话列表
    [SerializeField] List<Dialogues> postlines = new List<Dialogues>();  //战斗后对话列表
    Queue<Dialogues> dialogues;             //暂存对话的队列
    bool _isPostPlay;                        //是否播放战斗后对话
    public bool IsPostPlay { get { return _isPostPlay; } }
    [SerializeField] GameObject canvas;                  //剧情画布
    [SerializeField] GameObject leftCharacter;        //左边的人物及对话框
    [SerializeField] GameObject rightCharacter;       //右边的人物及对话框
    [SerializeField] Text leftText;                //左边剧情文字ui
    [SerializeField] Text rightText;               //右边剧情文字Ui
    [SerializeField] Text leftName;                //左边人物名字ui
    [SerializeField] Text rightName;               //右边人物名字ui
    [SerializeField] Image leftImage;              //左边人物头像
    [SerializeField] Image rightImage;             //右边任务头像
    Dialogues.characterDirection direction; //当前说话的人物
    Text currentText;                   //当前文字ui
    Text currentName;                   //当前人名ui
    Image currentImage;                 //当前头像
    Tweener currentTweener;             //当前文字动画
    private void Awake()
    {
        dialogues = new Queue<Dialogues>(prelines);//初始化
        currentText = leftText;
        direction = Dialogues.characterDirection.Left;
        currentName = leftName;
        currentImage = leftImage;

        ShowDialogue();
    }
    public void ShowDialogue()         //显示对话
    {
        if (currentTweener != null && !currentTweener.IsComplete())//结束之前的对话显示
        {
            currentText.DOComplete();
            currentText.DOKill();
            return;
        }                       
        if (dialogues.Count==0)         //对话播放完后禁用对话框
        {
            if (!IsPostPlay)
            {
                Transform list = GameObject.Find("BallList").transform;
                for(int i = 0; i < list.childCount; i++)
                {
                    list.GetChild(i).gameObject.SetActive(true);
                }
            }
            if (_isPostPlay)
            {
                _isPostPlay = false;
            }
            canvas.SetActive(false);

            OtherButton.instance.DisplayUI();  //显示UI

            return;
        }
        
        if (dialogues.Peek().direction!=direction)       //切换显示的人物
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
                    currentText.text = "";
                    break;
                case Dialogues.characterDirection.Right:
                    leftCharacter.GetComponent<Canvas>().sortingOrder=0;
                    rightCharacter.GetComponent<Canvas>().sortingOrder=10;
                    currentText = rightText;
                    direction=dialogues.Peek().direction;
                    currentName=rightName;
                    currentImage=rightImage;
                    currentText.text = "";
                    break;
                default:
                    break;
            }
        }
        else
        {
            currentText.text = "";
        }
        
        currentName.text=dialogues.Peek().name;             //播放对话
        currentImage.sprite=dialogues.Peek().image;
        currentTweener = currentText.DOText(dialogues.Peek().dialogue, dialogues.Peek().dialogue.Length / 10).SetEase(Ease.Linear);
        currentTweener.OnComplete(() => currentTweener = null);
        dialogues.Dequeue();
    }
    public void PostPlayDialog()         //播放战斗后动画
    {
        if (postlines.Count!=0)
        {
            dialogues = new Queue<Dialogues>(postlines);
            postlines.Clear();
            canvas.SetActive(true);
            _isPostPlay = true;
            leftText.text = "";
            rightText.text = "";
            leftName.text = "";
            rightName.text = "";
            ShowDialogue();
        }
    }
}

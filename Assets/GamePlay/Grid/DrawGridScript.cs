using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridScript : MonoBehaviour
{
    [SerializeField] int width;  //地图宽度
    [SerializeField] int height;//地图高度
    public GridMap map;                //地图
    GameObject linePrefab;            //边界线预制体
    Vector3 startPoint;         //起始点
    public Vector2 gridSize;           //格子尺寸
    Transform lineParent;         //边界线的父物体
    List<GameObject> lines = new List<GameObject>(); //边界线列表
    public static DrawGridScript instance;
    void Start()
    {
        if (instance == null)
            instance = this;

        map = new GridMap(width, height);                                         //初始化
        linePrefab = Resources.Load<GameObject>("Prefabs/Line");
        lineParent = transform.Find("GridList");
        transform.position = GameObject.FindGameObjectWithTag("Map").transform.position;

        Sprite sprite = GameObject.FindGameObjectWithTag("Map").GetComponent<SpriteRenderer>().sprite;
        Vector3 scale = GameObject.FindGameObjectWithTag("Map").transform.localScale;
        startPoint = new Vector3(sprite.bounds.min.x * scale.x, sprite.bounds.min.y * scale.y, sprite.bounds.min.z * scale.z);
        gridSize = new Vector2(sprite.bounds.size.x * scale.x / width, sprite.bounds.size.y * scale.y / height);

        Draw();
    }
    void Draw()//画边界线
    {
            for (int i = 0; i <= width; i++)
            {
                GameObject line = Instantiate(linePrefab, lineParent);
                line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(startPoint.x + i * gridSize.x, startPoint.y, startPoint.z), new Vector3(startPoint.x + i * gridSize.x, startPoint.y+height*gridSize.y, startPoint.z) });
                lines.Add(line);
            }
            for (int i = 0; i <= height; i++)
            {
            GameObject line = Instantiate(linePrefab, lineParent);
            line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(startPoint.x, startPoint.y+i*gridSize.y, startPoint.z), new Vector3(startPoint.x+width*gridSize.x , startPoint.y+i*gridSize.y, startPoint.z) });
            lines.Add(line);
            }
    }
    public Vector2Int GetTouchGrid(Vector2 pos)       //获取指定格子
    {
        if(pos.x < startPoint.x || pos.x > startPoint.x + width * gridSize.x || pos.y<startPoint.y || pos.y> startPoint.y + height * gridSize.y)  //
        {
            Debug.LogError(pos + "不在地图范围内");
            return Vector2Int.zero;
        }
        return new Vector2Int((int)((pos.x - startPoint.x) / gridSize.x), (int)((pos.y - startPoint.y) / gridSize.y));
    }
    public Vector2 GetGridPos(Vector2 index)         //获取格子中心点的位置
    {
        return new Vector2(startPoint.x + (index.x + (float)0.5) * gridSize.x, startPoint.y + (index.y + (float)0.5) * gridSize.y);
    }
    public Vector2 TargetGrid(Vector2 pos)
    {
        return GetGridPos(GetTouchGrid(pos));
        
    }
    public bool isInCamera(Vector2 pos)      //格子是否在摄像机视口内
    {
        pos = GetGridPos(pos);
        pos = Camera.main.WorldToViewportPoint(pos);
        if(pos.x < 0 || pos.x >1 || pos.y < 0 || pos.y >1) return false;
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridScript : MonoBehaviour
{
    [SerializeField] int width;  //��ͼ���
    [SerializeField] int height;//��ͼ�߶�
    GridMap map;                //��ͼ
    GameObject linePrefab;            //�߽���Ԥ����
    Vector3 startPoint;         //��ʼ��
    public Vector2 gridSize;           //���ӳߴ�
    Transform lineParent;         //�߽��ߵĸ�����
    List<GameObject> lines = new List<GameObject>(); //�߽����б�
    public static DrawGridScript instance;
    void Start()
    {
        if (instance == null)
            instance = this;

        map = new GridMap(width, height);                                         //��ʼ��
        linePrefab = Resources.Load<GameObject>("Prefabs/Line");
        lineParent = transform.Find("GridList");
        transform.position = GameObject.FindGameObjectWithTag("Map").transform.position;

        Sprite sprite = GameObject.FindGameObjectWithTag("Map").GetComponent<SpriteRenderer>().sprite;
        Vector3 scale = GameObject.FindGameObjectWithTag("Map").transform.localScale;
        startPoint = new Vector3(sprite.bounds.min.x * scale.x, sprite.bounds.min.y * scale.y, sprite.bounds.min.z * scale.z);
        gridSize = new Vector2(sprite.bounds.size.x * scale.x / width, sprite.bounds.size.y * scale.y / height);

        Draw();
    }
    void Draw()//���߽���
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
    Vector2Int GetTouchGrid(Vector2 pos)       //��ȡָ������
    {
        if(pos.x < startPoint.x || pos.x > startPoint.x + width * gridSize.x || pos.y<startPoint.y || pos.y> startPoint.y + height * gridSize.y)  //
        {
            Debug.LogError(pos + "���ڵ�ͼ��Χ��");
            return Vector2Int.zero;
        }
        return new Vector2Int((int)((pos.x - startPoint.x) / gridSize.x), (int)((pos.y - startPoint.y) / gridSize.y));
    }
    Vector2 GetGridPos(Vector2 index)         //��ȡ����λ��
    {
        return new Vector2(startPoint.x + (index.x + (float)0.5) * gridSize.x, startPoint.y + (index.y + (float)0.5) * gridSize.y);
    }
    public Vector2 TargetGrid(Vector2 pos)
    {
        return GetGridPos(GetTouchGrid(pos));
        
    }
}

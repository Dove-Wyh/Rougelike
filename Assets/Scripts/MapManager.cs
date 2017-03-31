using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public GameObject[] wallArrary;
    public GameObject[] floorArrary;
    public GameObject[] obstacleArrary;
    public GameObject[] resourceArrary;
    public GameObject[] enemyArray;
    public GameObject exitPrefab;
    public GameObject playerPrefab;

    private Transform Map;
    private GameManager GameManager;

    public int rows = 10;//行
    public int cols = 10;//列
    private List<Vector2> positionList = new List<Vector2>();

    private void Awake()
    {
        
    }

    void Start () {
        
        GameManager = gameObject.GetComponent<GameManager>();
        InitMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitMap()
    {
        //创建空物体，为父物体
        Map = new GameObject("Map").transform;

        //创建地板和围墙
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0||y ==0||x == cols - 1||y == rows -1)
                {
                    //在地图边缘随机创建一个围墙
                    Instantiate(new Vector2(x, y), wallArrary);
                }
                else
                {
                    //在地图中间随机创建地板
                    Instantiate(new Vector2(x, y), floorArrary);
                }
            }
        }
        positionList.Clear();
        //遍历中间的位置信息，随机取位置放置
        for(int x = 2;x < cols -2 ;x++)
        {
            for (int y = 2; y < rows -2; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }

        
        

        //创建障碍物 敌人 资源

        //创建障碍物
        //随机创建障碍物的个数，最少0个，最多level个
        int obstacleNumber = Random.Range(0, GameManager.level + 1);
        Create(obstacleNumber,obstacleArrary);

        //创建资源
        //随机创建资源数量 最少两个，最多Level个
        int resourcesNumber = Random.Range(2, GameManager.level + 1);
        Create(resourcesNumber, resourceArrary);

        //创建敌人
        //创建敌人数量 为level个
        int enemyNumber = GameManager.level;
        Create(enemyNumber, enemyArray);

        //创建主角与出口
        Instantiate(new Vector2(1, 1), playerPrefab);
        //Instantiate(new Vector2(7, 7), playerPrefab);
        Instantiate(new Vector2(8, 8), exitPrefab);
    }

    public void Create(int number,GameObject[] gameObject)
    {
        for (int i = 0; i < number; i++)
        {
            //随机取得一个中心位置的下标
            int positionIndex = Random.Range(0, positionList.Count);
            //获取到这个位置的坐标信息
            Vector2 position = positionList[positionIndex];
            //把这个位置信息移除
            positionList.RemoveAt(positionIndex);
            //随机创建资源
            Instantiate(position, gameObject);
        }
    }

    public void Instantiate(Vector2 pos, GameObject[] gameObject)
    {
        GameObject go = GameObject.Instantiate(gameObject[Random.Range(0, gameObject.Length)], pos, Quaternion.identity);
        go.transform.SetParent(Map);
    }

    //重写方法
    public void Instantiate(Vector2 pos, GameObject gameObject)
    {
        GameObject go = GameObject.Instantiate(gameObject, pos, Quaternion.identity);
        go.transform.SetParent(Map);
    }
}





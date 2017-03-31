using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //内部变量
    //开始关卡等级为1
    public int level;
    public int health = 20;
    public bool canMove = true;
    private bool pauseStep = true;
    private bool isOver = false;
    private bool isPass = false;
    private float r = 0;
    private float g = 0;
    private float b = 0;
    private float a = 0;

    public List<Enemy> enemyList = new List<Enemy>();

    private MapManager mapManager;

    //组件
    public Text healthText;
    public Text DayText;
    public GameObject addText;
    public GameObject subtractText;
    public GameObject GameOverUI;
    public Image GameOverImage;
    public GameObject PassGameUI;
    public Image PassGameImage;
    public Text PassGameText;

    //单例模式
    public static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Start()
    {
        _instance = this;
        //获取到这个组件，在过关后调用其中的方法
        mapManager = this.GetComponent<MapManager>();
        //动态获取到图片颜色，直接赋值了，所以把这里注释了
        //r = GameOverImage.color.r;
        //b = GameOverImage.color.b;
        //g = GameOverImage.color.g;
    }

    void Update()
    {
        if (isOver)
        {
            a = Mathf.Lerp(a, 1, Time.deltaTime / 2);
            GameOverImage.color = new Color(r, g, b, a);
        }
        if (isPass)
        {
            a = Mathf.Lerp(a, 1, Time.deltaTime);
            Debug.Log(a);
            PassGameImage.color = new Color(r, g, b, 1-a);
        }
    }

    //游戏结束方法，显示游戏结束UI
    public void GameOver()
    {
        GameOverUI.SetActive(true);
        isOver = true;
    }

    //过关方法，显示过关UI，更新天数（等级），重新生成地图
    public void PassGame(GameObject map)
    {
        canMove = false;
        pauseStep = true;
        PassGameText.text = "恭喜你活过了第" + level + "天";
        PassGameUI.SetActive(true);
        isPass = true;

        GameObject.Destroy(map);
        GameManager.Instance.level++;
        GameManager.Instance.enemyList.Clear();
        mapManager.InitMap();

        Invoke("HidePassGameUI", 2);
    }

    //隐藏过关UI
    public void HidePassGameUI()
    {
        canMove = true;
        DayText.text = "第" + level + "天";
        PassGameUI.SetActive(false);
        isPass = false;
        a = 0;
    }

    //再来一次按钮的点击事件
    public void OnAgainTextClick()
    {
        SceneManager.LoadScene("01");
    }

    //隐藏增加或者减少生命值提示文字
    public void HideText()
    {
        addText.SetActive(false);
        subtractText.SetActive(false);
    }

    //增加食物，并更新生命值
    public void AddFood(int foodNumber)
    {
        health += foodNumber;

        healthText.text = "生命值:" + health;
        addText.SetActive(true);

        addText.GetComponent<Text>().text = "生命值+" + foodNumber;
        Invoke("HideText", 1.5f);
    }

    //减少食物，并更新生命值
    public void SubtractFood(int DamageNumber)
    {
        health -= DamageNumber;

        healthText.text = "生命值:" + health;
        if (DamageNumber != 1)
        {
            subtractText.SetActive(true);
            subtractText.GetComponent<Text>().text = "生命值-" + DamageNumber;
            Invoke("HideText", 1.5f);
        }
    }

    //判断敌人是否可以移动，如果可以，让每个敌人都执行移动方法
    public void EnemyMove()
    {
        if (pauseStep == true)
        {
            pauseStep = false;
        }
        else
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Move();
            }
            pauseStep = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //组件
    private Transform enemytransform;
    private Transform playerTransform;
    private Rigidbody2D enemyRigidbody;
    private Animator Animator;
    private BoxCollider2D BoxCollider2D;

    //内部变量
    private Vector2 enemyTargetPos;
    public int ATK = 3;

    void Awake()
    {
        
        
    }

    private void Start()
    {//注意获取时间不要太早，防止获取不到该组件
        enemytransform = this.gameObject.GetComponent<Transform>();
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();

        BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        Animator = gameObject.GetComponent<Animator>();
        //赋个初始值
        enemyTargetPos = enemytransform.position;
        //获取到所有的enemy，添加到list中，控制移动
        GameManager.Instance.enemyList.Add(this);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        enemyRigidbody.MovePosition(Vector2.Lerp(enemytransform.position, enemyTargetPos, 0.25f));
    }

    public void Move()
    {
        Invoke("Move1", 0.25f); //等待人物移动结束
        Invoke("Move2", 0.5f); //等待敌人移动结束
    }

    public void Attack()
    {
        Animator.SetTrigger("Attack");
        //playerTransform.SendMessage("TakeDamage", ATK);
        GameManager.Instance.SubtractFood(ATK);
    }

    public void Move2()
    {
        
        //人物移动后，敌人也移动后的距离
        Vector2 offset = enemytransform.position - playerTransform.position;
        //如果距离小，攻击
        if (offset.magnitude < 1.1f)
        {
            //攻击
            Attack();
        }
    }

    //如果自己走到敌人脸上，受到双倍攻击，若敌人自己走过来，受到一次攻击
    public void Move1()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //人物移动后，敌人未移动的距离
        Vector2 offset = enemytransform.position - playerTransform.position;
        
        //如果距离小，攻击
        if (offset.magnitude < 1.1f)
        {
            //攻击
            Attack();
        }
        //移动
        else
        {
            BoxCollider2D.enabled = false;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                if (offset.x > 0)
                {
                    if (Mathf.Abs(offset.y - 0) < 0.1f)
                    {
                        Left();
                    }
                    if (offset.y > 0.9f)
                    {
                        if (!Left())
                        {
                            Down();
                        }
                    }
                    if (offset.y < -0.9f)
                    {
                        if (!Left())
                        {
                            Up();
                        }
                    }
                }
                if (offset.x < 0)
                {
                    if (Mathf.Abs(offset.y - 0) < 0.1f)
                    {
                        Right();
                    }
                    if (offset.y > 0.9f)
                    {
                        if (!Right())
                        {
                            Down();
                        }
                    }
                    if (offset.y < -0.9f)
                    {
                        if (!Right())
                        {
                            Up();
                        }
                    }
                }
            }

            if (Mathf.Abs(offset.x) < Mathf.Abs(offset.y))
            {
                if (offset.y > 0)
                {
                    if (Mathf.Abs(offset.x - 0) < 0.1f)
                    {
                        Down();
                    }
                    if (offset.x > 0.9f)
                    {
                        if (!Down())
                        {
                            Left();
                        }
                    }
                    if (offset.x < -0.9f)
                    {
                        if (!Down())
                        {
                            Right();
                        }
                    }
                }
                if (offset.y < 0)
                {
                    if (Mathf.Abs(offset.x - 0) < 0.1f)
                    {
                        Up();
                    }
                    if (offset.x > 0.9f)
                    {
                        if (!Up())
                        {
                            Left();
                        }
                    }
                    if (offset.x < -0.9f)
                    {
                        if (!Up())
                        {
                            Right();
                        }
                    }
                }
            }

            if (Mathf.Abs(offset.x) == Mathf.Abs(offset.y))
            {
                if (offset.x == offset.y && offset.x > 0)
                {
                    if (!Left())
                    {
                        Down();
                    }
                }
                if (offset.x < offset.y)
                {
                    if (!Right())
                    {
                        Down();
                    }
                }
                if (offset.x > offset.y)
                {
                    if (!Left())
                    {
                        Up();
                    }
                }
                if (offset.x == offset.y && offset.x < 0)
                {
                    if (!Up())
                    {
                        Right();
                    }
                }
            }
            BoxCollider2D.enabled = true;
        }
    }

    public bool Left()
    {
        RaycastHit2D hit = Physics2D.Linecast(enemytransform.position, (Vector2)enemytransform.position + new Vector2(-1, 0));
        if (hit.transform == null || hit.collider.tag == "Food" || hit.collider.tag == "Water")
        {
            enemyTargetPos += new Vector2(-1, 0);
            return true;
        }
        else
            return false;
    }

    public bool Right()
    {
        RaycastHit2D hit = Physics2D.Linecast(enemytransform.position, (Vector2)enemytransform.position + new Vector2(1, 0));
        if (hit.transform == null || hit.collider.tag == "Food" || hit.collider.tag == "Water")
        {
            enemyTargetPos += new Vector2(1, 0);
            return true;
        }
        else
            return false;
    }

    public bool Up()
    {
        RaycastHit2D hit = Physics2D.Linecast(enemytransform.position, (Vector2)enemytransform.position + new Vector2(0, 1));
        if (hit.transform == null || hit.collider.tag == "Food" || hit.collider.tag == "Water")
        {
            enemyTargetPos += new Vector2(0, 1);
            return true;
        }
        else
            return false;
    }

    public bool Down()
    {
        RaycastHit2D hit = Physics2D.Linecast(enemytransform.position, (Vector2)enemytransform.position + new Vector2(0, -1));
        if (hit.transform == null || hit.collider.tag == "Food" || hit.collider.tag == "Water")
        {
            enemyTargetPos += new Vector2(0, -1);
            return true;
        }
        else
            return false;
    }
}

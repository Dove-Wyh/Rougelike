using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //本物体组件
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private BoxCollider2D collider;
    //游戏物体
    private GameObject map;

    //内部变量
    private Vector2 playerTargetpos;
    public float moveCD = 0;
    public float time = 1;
    public AudioClip step;
    public AudioClip food;
    public AudioClip water;
    public AudioClip chop;

    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
        Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponent<Animator>();
        playerTargetpos = transform.position;
    }

    void Update()
    {
        /*if (canMove)
        {
            Rigidbody2D.MovePosition(Vector2.Lerp(transform.position, playerTargetpos, 0.25f));
            if (((Vector2)transform.position - playerTargetpos).magnitude < 0.1f)
            {
                canMove = false;
            }
        }*/
        

        Rigidbody2D.MovePosition(Vector2.Lerp(transform.position, playerTargetpos, 0.25f));

        if (GameManager.Instance.health <= 0)
        {
            GameManager.Instance.GameOver();
            return;
        }
        if (GameManager.Instance.canMove == false)
        {
            return;
        }
        time += Time.deltaTime;
        //当时间超过CD开始检测
        if (time > moveCD)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            //如果同时按下，把水平设为0
            if (h * v != 0)
            {
                v = 0;
            }
            if (h != 0 || v != 0)
            {
                //从当前位置
                collider.enabled = false;
                RaycastHit2D hit = Physics2D.Linecast(playerTargetpos, playerTargetpos + new Vector2(h, v));
                collider.enabled = true;

                if (hit.transform == null)
                {
                    playerTargetpos += new Vector2(h, v);
                    //AudioSource.PlayOneShot(step, 1);
                    AudioSource.PlayClipAtPoint(step, this.transform.position);
                }
                else
                {
                    switch (hit.collider.tag)
                    {
                        case "Exit":
                            playerTargetpos += new Vector2(h, v);
                            break;
                        case "Wall":
                            Animator.SetTrigger("Attack");
                            AudioSource.PlayClipAtPoint(chop, this.transform.position);
                            break;
                        case "Obstacle":
                            AudioSource.PlayClipAtPoint(chop, this.transform.position);
                            Animator.SetTrigger("Attack");
                            hit.collider.SendMessage("TakeDamage");
                            break;
                        case "Food":
                            AudioSource.PlayClipAtPoint(food, this.transform.position);
                            GameManager._instance.AddFood(15);
                            playerTargetpos += new Vector2(h, v);
                            Destroy(hit.transform.gameObject);
                            break;
                        case "Water":
                            AudioSource.PlayClipAtPoint(water, this.transform.position);
                            GameManager._instance.AddFood(10);
                            playerTargetpos += new Vector2(h, v);
                            Destroy(hit.transform.gameObject);
                            break;
                    }
                }
                //无论攻击还是移动都需要休息，所以归零放在外面
                time = 0;
                GameManager.Instance.SubtractFood(1);
                GameManager.Instance.EnemyMove();
            }
        }
    }

    public void TakeDamage(int ATK)
    {
        GameManager.Instance.health -= ATK;
        Animator.SetTrigger("Damage");
    }
}

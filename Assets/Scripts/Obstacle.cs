using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    private SpriteRenderer SpriteRenderer;
    public Sprite destorySprite;

    private int HP = 2;

    void Start()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

	public void TakeDamage()
    {
        HP -= 1;
        SpriteRenderer.sprite = destorySprite;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

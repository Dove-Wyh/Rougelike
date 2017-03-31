using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

    private GameObject map;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag == "Player")
        {
            GameManager.Instance.PassGame(this.transform.parent.gameObject);
        }
    }  

    public void cnm(Collider2D collider)
    {
        GameManager.Instance.PassGame(collider.transform.parent.gameObject);
    }
}

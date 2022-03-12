using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private GameObject gameManager;

    public GameObject sfx;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.gameObject.tag == "blasternaut") {
            EndObjCollect();
        }
    }
    
    public void EndObj()
    {
        Destroy(gameObject);
    }

    //End object and add SFX object
    //Also sends point to Game Manager
    public void EndObjCollect()
    {
        if(sfx) {
            Instantiate(sfx, gameManager.transform.position, Quaternion.identity, gameManager.transform.parent);
        }

        gameManager.GetComponent<GameManagerSystem>().PointCollected(1);

        Destroy(gameObject);
    }
}
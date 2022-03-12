using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPackControl : MonoBehaviour
{
    public GameObject[] spritesQR;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        int rand = Random.Range(0, spritesQR.Length);
        spritesQR[rand].SetActive(true);

    }
    
    public void RetrieveWinnings()
    {
        //retrieve HTML here
        //HTML CLASS
        bool result = true;

        if(true) {
            //Replace with RedPacket
            //

            //Raise Packet
            anim.SetBool("win", true);
        } else {
            //Raise with loss panel anyway
            anim.SetBool("win", true);
        }
    }
}
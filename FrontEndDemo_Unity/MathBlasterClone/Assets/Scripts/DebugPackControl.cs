using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPackControl : MonoBehaviour
{
    public GameObject[] spritesQR;
    public GameObject spriteDownload;
    public GameObject spriteError;
    public GameObject spriteNoWin;
    private Animator anim;

    public ConnectToWeb connection;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public void RetrieveWinnings()
    {
        //Attempt to Retrieve HTML/Image
        //HTML CLASS
        connection.ConnectAndRetrieve();
    }

    public void NoWinnings()
    {
        //Setup a debug image
        //int rand = Random.Range(0, spritesQR.Length);
        //spritesQR[rand].SetActive(true);

        //disable unloaded sprite
        spriteDownload.SetActive(false);

        //No winnings were returned
        spriteNoWin.SetActive(true);
        
        //Raise Packet
        anim.SetBool("win", true);
    }

    public void ErrorReturned()
    {
        //disable unloaded sprite
        spriteDownload.SetActive(false);

        //No winnings were returned
        spriteError.SetActive(true);
        
        //Raise Packet
        anim.SetBool("win", true);
    }

    public void WinningsReturned()
    {
        //Raise Packet
        anim.SetBool("win", true);
    }

}
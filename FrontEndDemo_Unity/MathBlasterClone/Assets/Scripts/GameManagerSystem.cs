using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSystem : MonoBehaviour
{
    //Score tracking
    private static int score = 0;
    public int winningScore = 1;
    
    public GameObject fireWorksFinal;
    public GameObject redPacket;
    public GameObject logo;

    private AudioSource audioS;

    private float timerCountdown = 3f;
    
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //On win condition, trigger events
        if ((score >= winningScore) && timerCountdown > 0) {
            timerCountdown -= Time.deltaTime;

            //Trigger win sfx
            if (timerCountdown < 2) {
                if (!audioS.isPlaying) {
                    audioS.Play();
                }
            }
            //Trigger win packet and fireworks
            if (timerCountdown < 1f) {
                logo.SetActive(true);
                fireWorksFinal.SetActive(true);

                //Request winning packet
                redPacket.GetComponent<DebugPackControl>().RetrieveWinnings();
            }

        }

    }

    //Called by Collectable when collected
    //Increments score
    public void PointCollected(int points)
    {
        score += points;
    }
}
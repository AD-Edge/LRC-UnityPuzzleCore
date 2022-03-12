using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip[] clips;
    public float vol = 1;
    private AudioSource audioS;
    private bool activated;
    
    void Start()
    {
        //Play given sound on Start
        audioS = GetComponent<AudioSource>();
        audioS.clip = clips[Random.Range(0, clips.Length)];
        audioS.volume = vol;

        audioS.Play();
        activated = true;
    }

    void Update()
    {
        //Destroy when finished playing
        if(activated) {
            if (!audioS.isPlaying) {
                Destroy(gameObject);
            }
        }
    }
}
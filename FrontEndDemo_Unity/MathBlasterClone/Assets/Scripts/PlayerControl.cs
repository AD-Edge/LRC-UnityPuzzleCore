using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Physics/Player effecting settings
    public float speed;
    public float speedJetpack;
    public float gravity;
    public bool grounded = false;
    public bool stunned = false;
    
    //Audio clips for Blasternaut
    //Theres probably 100x better ways to do this...
    public AudioClip step1;
    public AudioClip step2;
    public AudioClip whistle;
    public AudioClip click;
    public AudioClip jetpack;
    public AudioClip impact;
    public AudioClip rattle;
    public AudioClip touchgrass;
    public AudioClip ohno;

    //Misc variables
    private float hitForce = 0;
    private float vertVel;
    private float scaleMag;
    private Vector3 localScale;

    //Boring stuff
    private Rigidbody2D rBody;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        rBody = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
        audioSource = transform.GetComponent<AudioSource>();

        //Grab initial scale to toggle later
        scaleMag = transform.localScale.x;
    }

    void Update()
    {
        //Handle Inputs
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        localScale = transform.localScale;

        float vert = 0;

        //If not stunned, player actions
        //Bad state machine, replace all this with a better one
        if(!stunned) {
            //Jetpacking/Vertical movement
            if(y > 0) {
                anim.SetBool("jetpack", true);
                vert = speedJetpack * y;

                audioSource.clip = jetpack;
                audioSource.volume = 0.4f;
                if(!audioSource.isPlaying) {
                    audioSource.loop = true;
                    audioSource.Play();
                }
            } else if (y <= 0) { 
                //Reset jeckpacking on ground
                if(grounded && audioSource.loop) {
                    anim.SetBool("jetpack", false);
                    audioSource.loop = false;
                    audioSource.Stop();
                    audioSource.volume = 1f;
                }
                vert = 0;
            }

            //Custom gravity calc
            CalcGravity(vert);

            //Check grounded state (ray check)
            CheckGrounded();

            //Horizontal movement
            if (x != 0) {
                //Animation and movement
                anim.SetBool("running", true);

                //Flip Sprite scale
                if (x > 0) {
                    localScale.x = scaleMag * 1;
                    transform.localScale = localScale;
                } else if (x < 0) {
                    localScale.x = scaleMag * -1;
                    transform.localScale = localScale;
                }
            } else {
                //Reset movement/idle
                anim.SetBool("running", false);
            }

            //Apply all calculated velocities
            rBody.velocity = new Vector3(x * speed, vertVel, 0);

        } else { //Stunned state (hacky)
            
            //Still need to calc gravity because probably falling
            CalcGravity(vert);

            //Handle airborn forces
            if(!grounded) { //Apply given *horizontal* hit-force until grounded
                //This is a little detail that allows the character to be ..
                // .. hit in the direction of travel of the object that hit them
                rBody.velocity = new Vector3(hitForce, vertVel, 0);
            } else { //No more hit force, grounded
                rBody.velocity = new Vector3(0, vertVel, 0);
            }
        }
    }

    //Sort out custom gravity force calculation
    private void CalcGravity(float vert)
    {
        if(!grounded) {
            vertVel = gravity + vert;
        } else {
            vertVel = 0 + vert;
        }
    }
    
    //Ray check if grounded
    //This works along with 
    private void CheckGrounded()
    {
        Vector2 castPos = new Vector2(transform.position.x + 0.6f, transform.position.y);
        RaycastHit2D downCast = Physics2D.Raycast(castPos, Vector2.down, 0.8f);
        bool contact = true;
        
        //Check Down
        if (downCast.collider != null) {
            GameObject hitObject = downCast.collider.gameObject;

            //if ((rightHitObject != null)) {
            if ((hitObject.tag == "surface")) {
                grounded = true;
                contact = true;
                anim.SetBool("jetpack", false);
            }
        } else {
            contact = false;
            //Debug.DrawRay(castPos, Vector2.down * 0.6f, Color.green);
        }
        
        //Extra condition to toggle grounded off if made airborne
        if (grounded) {
            if (!contact) {
                grounded = false;
                anim.SetBool("jetpack", true);
                
                audioSource.clip = jetpack;
                audioSource.volume = 0.4f;
                if (!audioSource.isPlaying) {
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collide)
    {
        //Old/Basic ground collision (buggy)
        //Still needed for return to ground after head smash
        //For some reason
        if (collide.gameObject.tag == "surface") {
            grounded = true;
            if (stunned) {
                anim.SetBool("grounded", true);
            }
        }

        //If hit by something with an imacter tag, trigger state change
        if (collide.gameObject.tag == "impacter") {
            if (!stunned) {
                anim.SetTrigger("impact");
                stunned = true;

                //Currently assigning a random direction
                //can change this to be based on direction and ..
                // ..magnitude of object we're being hit by
                int rand = Random.Range(0, 2);
                if(rand == 0) {
                    HitForce(1, true);
                } else {
                    HitForce(1, false);
                }
            }
        }
    }

    //Old/Basic ground un-collision (buggy)
    //private void OnCollisionExit2D(Collision2D collide)
    //{
    //    if (collide.gameObject.tag == "surface") {
    //        grounded = false;
    //    }
    //}

    //Leave stunned state
    public void ExitStun()
    {
        stunned = false;
    }

    //When hit by an object, get smashed in a direction of given force
    public void HitForce(float strength, bool left)
    {
        if(left) {
            hitForce = -strength;
            //Debug.Log("hit from the left!");
        } else {
            hitForce = strength;
            //Debug.Log("hit from the right!");
        }
        //Trigger impact sfx
        ImpactSound();
    }

    //Handle All Audio Methods
    //Random step sounds
    public void StepSound()
    {
        int rdm = Random.Range(-1, 2);
        if (rdm == 0) {
            audioSource.clip = step1;
            audioSource.Play();
        } else if (rdm == 1) {
            audioSource.clip = step2;
            audioSource.Play();
        }
    }
    //Impatient click, triggered by animation event
    public void ClickSound()
    {
        audioSource.clip = click;
        audioSource.Play();
    }
    //Impatient whiste, triggered by animation event
    public void WhistleSound()
    {
        audioSource.clip = whistle;
        audioSource.Play();
    }
    //When your head gets smashed
    public void ImpactSound()
    {
        //Exit jetpack sfx mode
        if (audioSource.clip == jetpack) {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.volume = 1.0f;
        }
        //Easter egg, for 1-in-100 impact sound
        int rnd = Random.Range(0, 100);
        if (rnd == 0) {
            //90s kids will understand
            audioSource.clip = ohno;
        } else {
            audioSource.clip = impact;
        }
        audioSource.Play();
    }
    //When you go outside, triggered by animation event
    public void TouchGrass()
    {
        audioSource.clip = touchgrass;
        audioSource.Play();
    }
    //Head rattle, triggered by animation event
    public void RattleSound()
    {
        audioSource.clip = rattle;
        audioSource.Play();

        //Just deactivate states now we're recovering, making sure reset
        anim.SetBool("grounded", false);
        anim.SetBool("jetpack", false);
        anim.SetBool("running", false);
    }
}
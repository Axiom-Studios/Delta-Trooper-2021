using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Bone_Rex : MonoBehaviour
{
    public bool running = false;
    public bool ready = false;
    public float upSpeed = 0;
    public float horizSpeed = 0;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ready && transform.position.x > 9){
            rb.MovePosition(rb.position + (new Vector2(-8, 0) * Time.fixedDeltaTime));
        }
        else if (!ready){
            ready = true;
            startTime = Time.time;
            anim.Play("T-Bone-Idle");
        }
        else if (running)
        {
            if (sr.flipX)
            {
                if(rb.position.x > 9)
                {
                    sr.flipX = false;
                    running = false;
                    horizSpeed = 0f;
                    anim.Play("T-Bone-Idle");
                    startTime = Time.time;
                }
                else
                {
                    horizSpeed = 15f;
                }
            }
            else{
                if(rb.position.x < -9)
                {
                    sr.flipX = true;
                    running = false;
                    horizSpeed = 0f;
                    anim.Play("T-Bone-Idle");
                    startTime = Time.time;
                }
                else
                {
                    horizSpeed = -15f;
                }
            }

            if (rb.position.y > -2)
            {
                upSpeed -= 50f * Time.fixedDeltaTime;
            }

            else if (upSpeed != 0)
            {
                transform.position = new Vector2(rb.position.x, -2f);
                upSpeed = 0;
            }
        }

        else if (Time.time - startTime > 2){
            if (Mathf.RoundToInt(Random.Range(0f, 1f)) == 0)
            {
                upSpeed = 30f;
                transform.position = new Vector2(rb.position.x, -1.8f);
                anim.Play("T-Bone-Jump");
            }
            else
            {
                anim.Play("T-Bone-Run");
            }
            running = true;
        }
        if (ready)
        {
            rb.MovePosition(rb.position + (new Vector2(horizSpeed, upSpeed) * Time.fixedDeltaTime));
        }
    }
    
}

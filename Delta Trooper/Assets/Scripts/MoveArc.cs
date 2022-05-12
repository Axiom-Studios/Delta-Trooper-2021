using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArc : MonoBehaviour
{
    private float upwardSpeed;
    private float leftSpeed;
    public float speed = 7;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float startTime;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        upwardSpeed = Random.Range(2.5f,4.5f);
        leftSpeed = Random.Range(-1.5f,-2.5f);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = new Vector2(leftSpeed, upwardSpeed) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity);
        upwardSpeed -= 0.02f;
        if (Vector2.Distance(player.transform.position, transform.position) > 40)
        {
            Destroy(gameObject);
        }
    }
}

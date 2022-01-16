using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageBehavior : MonoBehaviour
{
    public GameObject player;
    public bool chasing = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Go to player
        Vector2 direction = (Vector2)player.transform.position - rb.position;
        direction = direction.normalized;
        Vector2 velocity = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity);
    }
}

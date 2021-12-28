using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyBehavior : MonoBehaviour
{
    public bool chasing = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float speed = 7;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(-1, 0) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity);
        if (rb.position.x <= player.transform.position.x - 21)
        {
            Destroy(gameObject);
        }
    }
}

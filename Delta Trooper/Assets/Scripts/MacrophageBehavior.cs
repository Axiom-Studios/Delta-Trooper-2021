using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageBehavior : MonoBehaviour
{
    public GameObject player;
    public bool chasing = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float speed = 6;
    private Vector3 rot = new Vector3 (0, 0, 0);
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
        if (transform.position.x - player.transform.position.x > 0){
            sr.flipX = true;
        }
        else{
            sr.flipX = false;
        }
        //transform.position += new Vector3 (velocity.x, velocity.y, 0);
        //transform.LookAt(player.transform.position);
        //transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.x + 180);
    }
}

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
    public Vector2 direction = new Vector2(0, 0);
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
        Vector2 velocity = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-1*direction.y, -1*direction.x) * Mathf.Rad2Deg, Vector3.forward);
        if (Vector2.Distance(player.transform.position, transform.position) > 40)
        {
            Destroy(gameObject);
        }
    }
}

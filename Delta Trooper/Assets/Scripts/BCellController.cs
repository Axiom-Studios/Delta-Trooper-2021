using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCellController : MonoBehaviour
{
    private bool spawning;
    public GameObject player;
    public GameObject antibody;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnEnemy", 1, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 8)
            {
                spawning = true;
                sr.color = Color.gray;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 8)
            {
                spawning = false;
                sr.color = Color.white;
            }
        }
        
    }
    void FixedUpdate()
    {
        Vector3 velocity = new Vector3 (-1, 0, 0) * speed * Time.fixedDeltaTime;
        transform.position += velocity;
        if (Vector2.Distance(player.transform.position, transform.position) > 40)
        {
            Destroy(gameObject);
        }
    }
    public void SpawnEnemy()
    {
        if (spawning)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.transform.position, 1);
            Instantiate(antibody, newPos, transform.rotation).GetComponent<AntibodyBehavior>().direction = (player.transform.position - transform.position).normalized;
        }
    }
}

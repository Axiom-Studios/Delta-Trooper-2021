using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCellController : MonoBehaviour
{
    private bool spawning;
    public GameObject player;
    public GameObject antibody;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnEnemy", 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 5)
            {
                spawning = true;
                sr.color = Color.gray;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 5)
            {
                spawning = false;
                sr.color = Color.white;
            }
        }
        
    }
    public void SpawnEnemy()
    {
        if (spawning)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.transform.position, 1);
            Instantiate(antibody, newPos, transform.rotation);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCellController : MonoBehaviour
{
    private bool spawning;
    public GameObject player;
    public GameObject antibody;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning){
                if (Vector3.Distance(transform.position, player.transform.position) < 10){
                    spawning = true;
                }
        }
        else{
            if (Vector3.Distance(transform.position, player.transform.position) > 10){
                    spawning = false;
                }
        }
        
    }
    public void SpawnEnemy()
    {
        if (spawning)
        {
            var newPos = new Vector2 (Vector2.MoveTowards(transform.position, player.transform.position, 5));
            Instantiate(antibody, newPos);
        }
    }
}

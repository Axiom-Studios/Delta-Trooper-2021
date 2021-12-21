using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static int minY = -7;
    public static int maxY = 7;
    public static float spawnrate = 1;
    public GameObject player;
    public GameObject antibody;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnEnemy", spawnrate, spawnrate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = player.transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(antibody, spawnPos, transform.rotation);
    }
}

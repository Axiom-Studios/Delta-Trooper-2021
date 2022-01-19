using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static int minY = -7;
    public static int maxY = 7;
    public static float spawnrate = 0.5f;
    public GameObject player;
    public GameObject antibody;
    public GameObject macrophage;
    public GameObject bCell;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("SpawnMacrophage", 5f);
        InvokeRepeating("SpawnAntibody", 15f, spawnrate);
        InvokeRepeating("SpawnBCell", 30f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAntibody()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x - 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(antibody, spawnPos, transform.rotation).GetComponent<AntibodyBehavior>().direction = new Vector2 (-1,0);
    }

    void SpawnMacrophage()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(macrophage, spawnPos, transform.rotation);
    }
    void SpawnBCell()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(bCell, spawnPos, transform.rotation);
    }
}

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
    public List<(string, float, float, float)> demoSpawn = new List<(string, float, float, float)>
    {
        ("SpawnAntibody", 1f, 5f, 0.5f),
        ("SpawnMacrophage", 5f, -1f, -1f)
    };
    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        //Invoke("SpawnMacrophage", 5f);
        //InvokeRepeating("SpawnAntibody", 15f, spawnrate);
        InvokeRepeating("SpawnBCell", 30f, 20f);
        foreach(var i in demoSpawn){
            if (i.Item4 == -1)
            {
                Invoke(i.Item1, i.Item2);
            }
            else
            {
                InvokeRepeating(i.Item1, i.Item2, i.Item4);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var i in demoSpawn){
            if ((Time.time - startTime) - i.Item3 > 0 && (Time.time - startTime) - i.Item3 < Time.deltaTime){
                CancelInvoke(i.Item1);
            }
        }
    }

    void SpawnAntibody()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(antibody, spawnPos, transform.rotation).GetComponent<AntibodyBehavior>().direction = new Vector2 (-1,0);
    }

    void SpawnMacrophage()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x - 21;
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

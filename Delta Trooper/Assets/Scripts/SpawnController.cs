using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    public static int minY = -7;
    public static int maxY = 7;
    public static float spawnrate = 0.5f;
    public GameObject player;
    public GameObject antibody;
    public GameObject macrophage;
    public GameObject spawnedMacrophage;
    public GameObject bCell;
    public static int level = 0;
    public Slider levelProgressBar;
    //Function, start, end, rate
    public List<List<(string, float, float, float)>> spawnList = new List<List<(string, float, float, float)>>
    {
        new List<(string, float, float, float)>{
            ("SpawnAntibody", 1f, 5f, 0.5f),
            ("SpawnMacrophage", 5f, -1f, -1f),
            ("SpawnBCell", 30f, -1f, 20f)
        },
        new List<(string, float, float, float)>{
            ("SpawnBCell", 1f, -1f, 5f),
            ("SpawnAntibody", 6f, 10f, 0.5f),
            ("SpawnMacrophage", 11f, -1f, -1f)
        }
    };
    public List<int> levelLengths = new List<int>
    {
        10, 10, 10, 10
    };
    public List<(string, float, float, float)> spawning;
    public float startTime;
    public static float levelProgress = 0;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        LoadLevel();
    }

    void LoadLevel()
    {
        Debug.Log("Loading Level");
        foreach(var i in GameObject.FindGameObjectsWithTag("Antibody")){
            Destroy(i);
        }
        foreach(var i in GameObject.FindGameObjectsWithTag("Macrophage")){
            Destroy(i);
        }
        player.transform.position = new Vector2 (0, 0);
        startTime = Time.time;
        spawning = spawnList[level];
        
        foreach(var i in spawnList[level]){
            if (i.Item4 == -1)
            {
                Invoke(i.Item1, i.Item2);
            }
            else
            {
                InvokeRepeating(i.Item1, i.Item2, i.Item4);
            }
        }
        Invoke("ChangeLevel", levelLengths[level]);
    }

    // Update is called once per frame
    void Update()
    {
        levelProgress = Time.time - startTime / levelLengths[level];
        levelProgressBar.value = levelProgress;
        foreach(var i in spawnList[level]){
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

    public void SpawnMacrophage()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x - 21;
        spawnPos.y = Random.Range(minY, maxY);
        spawnedMacrophage = Instantiate(macrophage, spawnPos, transform.rotation);
    }
    void SpawnBCell()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(bCell, spawnPos, transform.rotation);
    }

    void ChangeLevel(){
        level += 1;
        LoadLevel();
    }
}

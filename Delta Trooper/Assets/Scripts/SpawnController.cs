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
    public GameObject antibodyPrefab;
    public GameObject macrophagePrefab;
    public GameObject spawnedMacrophage;
    public GameObject bCellPrefab;
    public GameObject wallPrefab;
    public GameObject BG;
    public GameObject winScreen;
    public GameObject endScreen;
    public GameObject transitionScreen;
    public Text winText;
    public static int level = 0;
    public Slider levelProgressBar;
    public int displayLevel = 0;
    float timeSinceTransition = 0;
    private string[] names = {
        "Alice", "Bob", "Charlie", "Dave", "Emily", "Frank", "Gannon", "Hank", "Ian", "Jakob", "John", 
        "Kristina", "Larry", "Maddie", "Nate", "Oliver", "Pete", "Quincy", "Raio", "Stacy", "Terry", 
        "Ulysses", "Vincent", "Waldo", "Xander", "Yale", "Zack"
    };

    //Function, start, end, rate
    public List<List<(string, float, float, float)>> spawnList = new List<List<(string, float, float, float)>>
    {
        new List<(string, float, float, float)>{
            ("SpawnAntibody", 5f, 15f, 1f),
            ("SpawnAntibody", 15f, 25f, 0.5f),
            ("SpawnMacrophage", 25f, 25f, -1f),
            ("SpawnAntibody", 40f, 60f, 0.5f),
            ("DespawnMacrophages", 60f, 60f, -1f),
            ("SpawnBCell", 65f, -1f, 10f),
            ("SpawnAntibody", 80f, -1f, 0.5f),
            ("SpawnMacrophage", 100f, 101f, 10f)
        },
        new List<(string, float, float, float)>{
            //B cells (IT GETS WORSE)
            ("SpawnBCell", 1f, 40f, 5f),
            ("SpawnBCell", 40f, 80f, 4f),
            ("SpawnBCell", 80f, 120f, 3f),
            //constant antibody spawning
            //("SpawnAntibody", 1f, -1f, 1f)
        },
        new List<(string, float, float, float)>{
            ("SpawnWall", 1f, -1f, 5f),
			("SpawnAntibody", 10f, -1f, 1f),
			("SpawnBCell", 15f, -1f, 3f)
        }
    };
    public List<int> levelLengths = new List<int>
    {
        120, 120, 120
    };
    public List<(string, float, float, float)> spawning;
    public float startTime;
    public static float levelProgress = 0;
    // Start is called before the first frame update
    void Start(){
        level = 0;
        winText = winScreen.GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        LoadLevel();
    }

    void LoadLevel()
    {
        Debug.Log("Loading Level: " + level);
        foreach(var i in GameObject.FindGameObjectsWithTag("Antibody")){
            Destroy(i);
        }
        foreach(var i in GameObject.FindGameObjectsWithTag("Macrophage")){
            Destroy(i);
        }
        foreach(var i in GameObject.FindGameObjectsWithTag("B-Cell")){
            Destroy(i);
        }
        CancelInvoke("SpawnMacrophage");
        CancelInvoke("SpawnAntibody");
        CancelInvoke("SpawnBCell");
        player.transform.position = new Vector2 (0, 0);
        startTime = Time.time;
        spawning = spawnList[level];
        Debug.Log("Got Spawning for level " + level);
        /*
        foreach(var i in spawnList[level]){
            if (i.Item4 == -1)
            {
                Invoke(i.Item1, i.Item2);
            }
            else
            {
                InvokeRepeating(i.Item1, i.Item2, i.Item4);
            }
        }*/
        Invoke("ChangeLevel", levelLengths[level]);
    }

    // Update is called once per frame
    void Update()
    {
        levelProgress = (Time.time - startTime) / levelLengths[level];
        levelProgressBar.value = levelProgress;
        foreach(var i in spawnList[level]){
            if ((Time.time - startTime) - i.Item2 > 0 && (Time.time - startTime) - i.Item2 < Time.deltaTime){
                if (i.Item4 == -1)
                {
                    Invoke(i.Item1, 0);
                }
                else
                {
                    InvokeRepeating(i.Item1, 0, i.Item4);
                }
            }
            else if ((Time.time - startTime) - i.Item3 > 0 && (Time.time - startTime) - i.Item3 < Time.deltaTime){
                CancelInvoke(i.Item1);
            }
        }

        if (transitionScreen.activeSelf && Time.unscaledTime - timeSinceTransition > 4)
        {
            transitionScreen.SetActive(false);
            timeSinceTransition = 0;
            DialogueSystem.menuPaused = false;
            Debug.Log("haaaaaaaaaaaa");

        }
    }

    void SpawnWall() {
        Instantiate(wallPrefab, transform.position + (Vector3.right * 18), transform.rotation);
        Debug.Log("beep boop I am a wall");
		if (!DialogueSystem.wallExplained) {
			Invoke("WallDialogue", 1);
		}
    }

	void WallDialogue() {
		DialogueSystem.sentencesQueue.Add("That wall to the right is a Cell Membrane.");
        DialogueSystem.sentencesQueue.Add("You have to dash to make it through.");
        DialogueSystem.sentencesQueue.Add("Watch out though. If it pushes you to the edge of the screen, you die!");
        DialogueSystem.wallExplained = true;
	}

    void SpawnAntibody()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(antibodyPrefab, spawnPos, transform.rotation).GetComponent<AntibodyBehavior>().direction = new Vector2 (-1,0);
    }

    public void SpawnMacrophage()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x - 21;
        spawnPos.y = Random.Range(minY, maxY);
        spawnedMacrophage = Instantiate(macrophagePrefab, spawnPos, transform.rotation);
    }
    void SpawnBCell()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x = transform.position.x + 21;
        spawnPos.y = Random.Range(minY, maxY);
        Instantiate(bCellPrefab, spawnPos, transform.rotation);
    }

    void ChangeLevel(){
        if (PlayerMovement.lives > 0)
        {
            if (level == 2)
            {
                Win();
            }
            else
            {
				level += 1;
            	displayLevel += 1;
                transitionScreen.SetActive(true);
                DialogueSystem.menuPaused = true;
                Debug.Log("Yaaaaaaaaaaaaaaaaaaaa");
                timeSinceTransition = Time.unscaledTime;
                PlayerMovement.lives = 5;
                BG.GetComponent<BackgroundController>().ChangeBG(level);
                LoadLevel();
            }
        }
    }
    public void DespawnMacrophages(){
        foreach(var i in GameObject.FindGameObjectsWithTag("Macrophage")){
            i.GetComponent<MacrophageBehavior>().chasing = false;
        }
    }
    public void Win(){
        Debug.Log("You won congrats ig");
        winScreen.SetActive(true);
        winText.text = "You Infected " + names[Mathf.RoundToInt(Random.Range(0,names.Length))] + "! \n\nCongratulations!";
        Time.timeScale = 0;
    }
}

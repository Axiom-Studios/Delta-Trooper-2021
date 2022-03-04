using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCellController : MonoBehaviour
{
    private bool spawning;
    public GameObject player;
    public GameObject antibody;
    private Rigidbody2D rb;
    private float speed = 3;

    private AudioSource audioSource;
    public AudioClip shootSound;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        InvokeRepeating("SpawnEnemy", 1, 0.2f);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 8)
            {
                spawning = true;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 8)
            {
                spawning = false;
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
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(player.transform.position.y - this.transform.position.y, player.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg - 113, Vector3.forward);
    }
    public void SpawnEnemy()
    {
        if (spawning)
        {
            anim.Play("HelperB");
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.transform.position, 1);
            audioSource.PlayOneShot(shootSound);
            Instantiate(antibody, newPos, transform.rotation).GetComponent<AntibodyBehavior>().direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            anim.Play("Default");
        }
    }
}

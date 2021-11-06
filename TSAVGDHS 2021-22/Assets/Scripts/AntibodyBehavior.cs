using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyBehavior : MonoBehaviour
{
    public bool chasing = false;
    private float speed = 8;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasing){
                if (Vector3.Distance(transform.position, player.transform.position) < 5){
                    chasing = true;
                }
        }
        else{
            if (Vector3.Distance(transform.position, player.transform.position) > 5){
                    chasing = false;
                }
            else{
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
    }
}

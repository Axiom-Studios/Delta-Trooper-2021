using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacrophageBehavior : MonoBehaviour
{
    public GameObject player;
    public bool chasing = false;
    private float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasing){
                if (Vector3.Distance(transform.position, player.transform.position) < 10){
                    chasing = true;
                }
        }
        else{
            if (Vector3.Distance(transform.position, player.transform.position) > 10){
                    chasing = false;
                }
            else{
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
    }
}

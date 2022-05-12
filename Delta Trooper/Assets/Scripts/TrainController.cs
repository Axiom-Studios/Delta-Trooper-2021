using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject coin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPhase(){
        Debug.Log("Advanced");
    }

    void fireCoins(){
        for(var i = 0; i < 20; i++){
            Instantiate(coin, transform.position, transform.rotation).GetComponent<AntibodyBehavior>.direction = new Vector2(-1, i*-0.1)
        }
    }
}

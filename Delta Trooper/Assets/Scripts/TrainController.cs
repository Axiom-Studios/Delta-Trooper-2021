using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject coin;
    public GameObject dynamite;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("fireCoins", 1f, 3f);
        InvokeRepeating("fireDynamite", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPhase(){
        Debug.Log("Advanced");
    }

    void fireCoins(){
        for(var i = -10; i < 10; i++){
            Instantiate(coin, new Vector2(1, -2), Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, -2), player.transform.position)+i*-4.5f-90));
        }
    }
    
    void fireDynamite(){
        Instantiate(dynamite, new Vector2(7, -2), transform.rotation);
    }
}

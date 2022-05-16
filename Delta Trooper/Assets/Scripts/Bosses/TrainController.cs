using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject coin;
    public GameObject dynamite;
    public GameObject player;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("fireCoins", 12f, 3f);
        InvokeRepeating("fireDynamite", 8f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.x > 4){
            rb.MovePosition(rb.position + (new Vector2(-8, 0) * Time.deltaTime));
        }
    }

    public void NextPhase(){
        Debug.Log("Advanced");
    }

    void fireCoins(){
        for(var i = -7; i <= 7; i++){
            Instantiate(coin, new Vector2(3, -2), Quaternion.Euler(0, 0, (Vector2.SignedAngle(new Vector2(1, 0), (Vector2)player.transform.position - new Vector2(3, -2))+i*6f)));
        }
    }
    
    void fireDynamite(){
        Instantiate(dynamite, new Vector2(7, -2), transform.rotation);
    }
}

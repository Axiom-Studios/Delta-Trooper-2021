using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetDrill : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    Vector3 moveDirection;

    void OnEnable()
    {
        transform.position = GameObject.FindGameObjectsWithTag("Drill Molar")[0].transform.position + (Vector3.left * 3);
        transform.rotation = Quaternion.Euler(0, 0, 45);
        moveDirection = Vector3.down + Vector3.left;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y) {
            transform.rotation = Quaternion.Euler(0, 0, -45);
            moveDirection = Vector3.up + Vector3.left;
        }
        if (transform.position.y > Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y) {
            transform.rotation = Quaternion.Euler(0, 0, 45);
            moveDirection = Vector3.down + Vector3.left;
        }
        
        if (transform.position.magnitude > maxDistance) {

        }
    }
}

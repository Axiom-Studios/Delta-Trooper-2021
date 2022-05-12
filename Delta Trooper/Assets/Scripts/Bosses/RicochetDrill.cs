using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetDrill : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    Vector2 moveDirection;
    Rigidbody2D rb;

    void OnEnable()
    {
        transform.position = GameObject.FindGameObjectsWithTag("Drill Molar")[0].transform.position + (Vector3.left * 3);
        moveDirection = Vector3.down + Vector3.left;
        transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(moveDirection, Vector3.left));
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = moveDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
        transform.rotation = Quaternion.Euler(0, 0, GetAngleByHeading(moveDirection));

        if (transform.position.y > Camera.main .ScreenToWorldPoint(Vector3.up * Screen.height).y) {
            moveDirection.y = -1;
        }
        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y) {
            moveDirection.y = 1;
        }
        if (transform.position.x > Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x) {
            moveDirection.x = -1;
        }
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x) {
            moveDirection.x = 1;
        }
    }

    float GetAngleByHeading(Vector3 heading) {
        float angle = Vector2.Angle(moveDirection, Vector3.left);
        return angle * -heading.y;
    }
}

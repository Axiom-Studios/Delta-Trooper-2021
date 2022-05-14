using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
	float velocity = 0;
	public float acceleration = 9.8f;
	Rigidbody2D rb;
	void OnEnable() {
		rb = gameObject.GetComponent<Rigidbody2D>();
	}
    void FixedUpdate()
    {
		rb.MovePosition(rb.position + (Vector2.down * velocity * Time.fixedDeltaTime));
		velocity += acceleration * Time.fixedDeltaTime;
        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y) {
			Destroy(gameObject);
		}
    }
}

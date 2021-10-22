using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
	public GameObject tracking;
	[Range(0, 100)]
	public float trackSpeed;

    void Update()
    {
		Vector2 target = tracking.transform.position;
		Vector2 current = transform.position;
		float distance = Vector2.Distance(target, current);
		float movementSpeed = (distance / (100-trackSpeed)) * 100;
		Vector2 newPosition = Vector2.MoveTowards(current, target, movementSpeed * Time.deltaTime);
		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}
}
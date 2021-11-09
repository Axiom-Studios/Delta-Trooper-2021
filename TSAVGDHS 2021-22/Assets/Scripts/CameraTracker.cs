using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
	public GameObject tracking;
	//clamp trackSpeed (this is an editor attribute)
	[Range(0, 100)]
	public float trackSpeed;

    void Update()
    {
		//Convert positions to Vector2 to preserve Z position
		Vector2 target = tracking.transform.position;
		Vector2 current = transform.position;
		//Set movement speed based on distance
		float distance = Vector2.Distance(target, current);
		float movementSpeed = (distance / (100-trackSpeed)) * 100;
		//Set new position while preserving Z position
		Vector2 newPosition = Vector2.MoveTowards(current, target, movementSpeed * Time.deltaTime);
		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
	}
}
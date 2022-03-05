using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavasTracker : MonoBehaviour
{
	public Transform tracking;
	public Camera cam;
	public Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2) cam.WorldToScreenPoint(tracking.position + (Vector3) offset);
    }

	void OnEnable() {
		Update();
		Debug.Log("canvas tracker enabled");
	}
}

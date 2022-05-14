using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < Camera.main.ScreenToWorldPoint(Vector3.zero).y - transform.lossyScale.y) {
			Destroy(gameObject);
		}
    }
}

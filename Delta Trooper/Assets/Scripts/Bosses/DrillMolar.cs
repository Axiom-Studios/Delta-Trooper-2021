using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMolar : MonoBehaviour
{
    public float bobScale = 2;
    public float xPosition;
    float phase = 0;
	float waggleTime;

    [Header("Ricochet")]
    public GameObject ricochetDrill;
    public float rate;
    float lastRicochet;

	[Header("Drill")]
	public GameObject rubble;
	public float rubbleAmount = 10;
	public float loopRate;
	public float drillTime;
	public float drillMoveSpeed;
	public float drillOscillationScale = 1;
	public float maxY = 5;
	float lastDrill;
	float drillPhase = 0;
	Rigidbody2D rb;

    void OnEnable() {
        Debug.Log("Spawned Drill Molar");
        transform.position = Vector3.right * 20;
		rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
		waggleTime += Time.fixedDeltaTime;
		if (phase == 0) {
			rb.MovePosition(rb.position + (Vector2.left * drillMoveSpeed * Time.fixedDeltaTime));
			if (rb.position.x <= xPosition) {
				rb.position = new Vector2(xPosition, rb.position.y);
				phase = 1;
			}
		}
        else if (phase == 1) {
			Waggle();
            Ricochet();
        }
		else if (phase == 2) {
			Drill();
		}
    }

    void Waggle() {
        float newY = Mathf.Sin(waggleTime * Mathf.PI) * bobScale;
        Vector2 newPosition = new Vector2(rb.position.x, newY);
		rb.MovePosition(newPosition);
    }

    void Ricochet() {
        if (Time.time - lastRicochet >= rate) {
            Instantiate(ricochetDrill);
            lastRicochet = Time.time;
        }
    }

	void Drill() {
		switch (drillPhase)
		{
			case 0:
				Waggle();
				if (Time.time - lastDrill >= loopRate) {
					drillPhase = 1;
				}
				break;
				
			case 1:
				rb.MovePosition(rb.position + (Vector2.up * drillMoveSpeed * Time.fixedDeltaTime));
				if (rb.position.y >= maxY) {
					drillPhase = 2;
					lastDrill = Time.time;
					waggleTime = 0;
				}
				break;

			case 2:
				float newX = Mathf.Sin(waggleTime * Mathf.PI) * drillOscillationScale;
	        	Vector2 newPosition = new Vector2(newX + xPosition, rb.position.y);
				rb.MovePosition(newPosition);
				if (Time.time - lastDrill >= drillTime) {
					drillPhase = 3;
					SpawnRubble();
				}
				break;

			case 3:
				rb.MovePosition(rb.position + (Vector2.down * drillMoveSpeed * Time.fixedDeltaTime));
				if (rb.position.y <= 0) {
					rb.position = new Vector2(rb.position.x, 0);
					drillPhase = 0;
					lastDrill = Time.time;
					waggleTime = 0;
				}
				break;
		}
	}

	void SpawnRubble() {
		for (int i = 0; i < rubbleAmount; i++) {
			float xPosition = Random.Range(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x);
			Vector3 position = new Vector3(xPosition, Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y, 0);
			Instantiate(rubble, position, transform.rotation);
		}
	}

    public void AdvancePhase() {
        phase += 1;
    }
}
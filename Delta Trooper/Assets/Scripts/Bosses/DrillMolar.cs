using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMolar : MonoBehaviour
{
    public float bobScale = 2;
    public float xPosition;
    float phase = 0;

    void OnEnable() {
        Debug.Log("Spawned Drill Molar");
        transform.position = Vector3.right * xPosition;
    }

    void Update() {
        Bob();
        if (phase == 1) {

        }
    }

    void Bob() {
        float newY = Mathf.Sin(Time.time * Mathf.PI) * bobScale;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    public void AdvancePhase() {
        phase += 1;
    }
}

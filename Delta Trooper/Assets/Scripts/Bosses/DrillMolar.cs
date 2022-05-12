using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillMolar : MonoBehaviour
{
    public float bobScale = 2;
    public float xPosition;
    float phase = 1;
    [Header("Ricochet")]
    public GameObject ricochetDrill;
    public float rate;
    float lastRicochet;

    void OnEnable() {
        Debug.Log("Spawned Drill Molar");
        transform.position = Vector3.right * xPosition;
    }

    void Update() {
        Waggle();
        if (phase == 1) {
            Ricochet();
        }
    }

    void Waggle() {
        float newY = Mathf.Sin(Time.time * Mathf.PI) * bobScale;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void Ricochet() {
        if (Time.time - lastRicochet >= 1/rate) {
            Instantiate(ricochetDrill);
            lastRicochet = Time.time;
        }
    }

    public void AdvancePhase() {
        phase += 1;
    }
}

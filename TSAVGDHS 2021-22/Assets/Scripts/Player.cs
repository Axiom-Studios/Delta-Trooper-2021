using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	InputMaster controls;
	public float speed;
    // Start is called before the first frame update
    void Start()
    {
        //input setup
		controls = new InputMaster();
		controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

	void movement()
	{
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * speed * Time.deltaTime;
		transform.Translate(movement);
	}
}

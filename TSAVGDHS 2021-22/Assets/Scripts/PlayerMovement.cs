using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
	public float maxSpeed;
	public float acceleration;
	public float deceleration;
	float currentSpeed;
	Vector2 lastDirection;
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		if (Mouse.current.leftButton.isPressed)
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
			Debug.Log("Speed: " + currentSpeed);
			transform.position = Vector2.MoveTowards(transform.position, mousePos, currentSpeed * Time.deltaTime);
			lastDirection = (mousePos - (Vector2)transform.position).normalized;
		}
		else
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
			transform.Translate(lastDirection * currentSpeed * Time.deltaTime);
		}
		/*
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * speed * Time.deltaTime;
		transform.Translate(movement);
		*/
	}
}

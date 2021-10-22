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

    void Start()
    {
        //input setup
		controls = new InputMaster();
		controls.Enable();
    }

    void Update()
    {
		//get mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		if (Mouse.current.leftButton.isPressed)
		{
			//Set the current speed based (accelerate)
			currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
			//Move towards the mouse at current speed
			transform.position = Vector2.MoveTowards(transform.position, mousePos, currentSpeed * Time.deltaTime);
			//set last direction (for deceleration)
			lastDirection = (mousePos - (Vector2)transform.position).normalized;
		}
		else
		{
			//Set current speed (deceleration)
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
			//Move in direction at current speed
			transform.Translate(lastDirection * currentSpeed * Time.deltaTime);
		}
		/* KEYBOARD INPUT (UNUSED)
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * speed * Time.deltaTime;
		transform.Translate(movement);
		*/
	}
}

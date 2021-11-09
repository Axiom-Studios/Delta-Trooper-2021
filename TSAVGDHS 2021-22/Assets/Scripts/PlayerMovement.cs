using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
    public float maxSpeed = 10f;
    public float acceleration = 8f;
    public float deceleration = 5f;
    float currentSpeed;
    Vector2 lastDirection;
	public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //input setup
		controls = new InputMaster();
		controls.Enable();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		if (Mouse.current.leftButton.isPressed)
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
			Debug.Log("Speed: " + currentSpeed);
			rb.MovePosition(Vector2.MoveTowards(transform.position, mousePos, currentSpeed * Time.fixedDeltaTime));
			lastDirection = (mousePos - (Vector2)transform.position).normalized;
		}
		else
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
			rb.MovePosition(rb.position + lastDirection * currentSpeed * Time.fixedDeltaTime);
		}
		/*
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * speed * Time.deltaTime;
		transform.Translate(movement);
		*/
	}
    /*
    void FixedUpdate()
    {
        movement();
    }

	void movement()
	{
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * acceleration * Time.fixedDeltaTime;
		rb.velocity += movement;
	}

    */

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Macrophage")
        {
            kill();
        }
        else if(other.gameObject.tag == "Antibody")
        {
            speed -= 1;
            Destroy(other.gameObject);
            if (speed <= 0)
            {
                kill();
            }
        }
    }

    public void kill()
    {
        transform.position = new Vector2(0, 0);
        speed = 10;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
    public float maxSpeed = 10f;
    public float acceleration = 12f;
    public float deceleration = 1f;
    float currentSpeed;
    Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //input setup
		controls = new InputMaster();
		controls.Enable();
    }

    void FixedUpdate()
    {
        movement();
    }

	void movement()
	{
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
        input = input.normalized;
        rb.velocity += input * Time.fixedDeltaTime * acceleration;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));

        if (rb.velocity != Vector2.zero)
        {
            rb.velocity -= (rb.velocity + Vector2.zero) * Time.fixedDeltaTime * deceleration;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Macrophage")
        {
            kill();
        }
        else if(other.gameObject.tag == "Antibody")
        {
            deceleration += 0.1f;
            maxSpeed -= 1f;
            Destroy(other.gameObject);
            if (maxSpeed <= 0)
            {
                kill();
            }
        }
    }

    public void kill()
    {
        transform.position = new Vector2(0, 0);
        maxSpeed = 10f;
        deceleration = 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float maxSpeed = 8f;
    public float acceleration = 50f;
    float currentSpeed;
    float cellTime = 0;
    public float infectTime = 5f;
    Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
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

        if (input != Vector2.zero && rb.velocity != Vector2.zero)
        {
            rb.velocity -= rb.velocity * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Macrophage")
        {
            Debug.Log("You got hit by a macrophage");
            kill();
        }
        else if (other.gameObject.tag == "Antibody")
        {
            Debug.Log("You got hit by an antibody");
            maxSpeed -= 1f;
            acceleration /= 1.3f;
            Destroy(other.gameObject);
            if (maxSpeed <= 1)
            {
                kill();
            }
        }
    }

    public void kill()
    {
        Debug.Log("You died");
        transform.position = new Vector2(0, 0);
        maxSpeed = 8f;
        acceleration = 50f;
        sr.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cell")
        {
            cellTime = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cell")
        {
            if (Keyboard.current.spaceKey.IsPressed())
            {
                sr.color = Color.black;
                cellTime += Time.deltaTime;
            }
            else
            {
                sr.color = Color.white;
                cellTime = 0;
            }

            if (cellTime > infectTime)
            {
                SpriteRenderer cell_sr = other.gameObject.GetComponent<SpriteRenderer>();
                cell_sr.color = new Color(0.5f, 0.7f, 0.5f, 1f);
                win();
            }
        }
    }

    public void win()
    {
        Debug.Log("You won");
        sr.color = Color.magenta;
        cellTime = 0;
    }
}

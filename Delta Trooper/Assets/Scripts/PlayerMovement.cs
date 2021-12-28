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

	//PLAYER CLAMPING
	public Camera clampCamera;
	float left;
	float right;
	float top;
	float bottom;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        //input setup
		controls = new InputMaster();
		controls.Enable();
		//Set camera corners
		Vector3 upperLeftScreen = new Vector3(0, Screen.height, 0);
		Vector3 upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
		Vector3 lowerLeftScreen = new Vector3(0, 0, 0);
		Vector3 lowerRightScreen = new Vector3(Screen.width, 0, 0);
		//Set world positions
		left = clampCamera.ScreenToWorldPoint(Vector3.zero).x;
		right = clampCamera.ScreenToWorldPoint(Vector3.right * Screen.width).x;
		top = clampCamera.ScreenToWorldPoint(Vector3.up * Screen.height).y;
		bottom = clampCamera.ScreenToWorldPoint(Vector3.zero).y;
    }

    void FixedUpdate()
    {
        Movement();
    }

	void Update() {
		Clamping();
	}

	void Movement()
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

	void Clamping() {
		float xPos = Mathf.Clamp(transform.position.x, left, right);
		float yPos = Mathf.Clamp(transform.position.y, bottom, top);

		if (xPos != transform.position.x) {
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
		if (yPos != transform.position.y) {
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}
		transform.position = new Vector3(xPos, yPos, transform.position.z);
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Macrophage")
        {
            Debug.Log("You got hit by a macrophage");
            Kill();
        }
        else if (other.gameObject.tag == "Antibody")
        {
            Debug.Log("You got hit by an antibody");
            maxSpeed -= 1f;
            acceleration /= 1.3f;
            Destroy(other.gameObject);
            if (maxSpeed <= 1)
            {
                Kill();
            }
        }
    }

    public void Kill()
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
                Win();
            }
        }
    }

    public void Win()
    {
        Debug.Log("You won");
        sr.color = Color.magenta;
        cellTime = 0;
    }
}

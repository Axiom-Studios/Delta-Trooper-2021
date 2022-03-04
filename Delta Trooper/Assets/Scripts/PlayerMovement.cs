using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GameObject endScreen;
    public GameObject dialogueSystem;
    public float maxSpeed = 8f;
    public float acceleration = 50f;
    public static int health = 100;
    public static int lives = 5;
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
    float t1; // <-- WHAT IS IT?????

    //DASHING
    bool dashing = false;
    float dashStart;
    float dashEnd;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    Vector2 dashDirection;
    CircleCollider2D playerCollider;

    // Audio
    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip killSound;
    public AudioClip deathSound;

	// Sliders
	public Slider playerHealthSlider;
	public Slider dashIndicator;

    // Death Immunity
    public float immunityTime;
    float immunityStart;
    public SpawnController spawnController;

    void Start()
    {
        lives = 5;
        
        t1 = Time.time;
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        playerCollider = gameObject.GetComponent<CircleCollider2D>();
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

        dashEnd = -dashCooldown;
        DialogueSystem.sentencesQueue.Add("WASD to move\n\n\n[SPACE] to skip dialogue");

        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Movement();
    }

	void Update() {
		Clamping();
        Dash();
		IndicatorUpdate();
        Immunity();
	}
    
    void Immunity() {
        if (Time.time - immunityStart >= immunityTime && !dashing) {
            playerCollider.enabled = true;
        }
        else {
            playerCollider.enabled = false;
        }
    }
	void IndicatorUpdate() {
        // Dash Indicator
		dashIndicator.value = (Time.time - dashEnd) / dashCooldown;
		if (dashIndicator.value >= 1) {
			dashIndicator.gameObject.SetActive(false);
		}
		else {
			dashIndicator.gameObject.SetActive(true);
		}

        // Health Bar
        playerHealthSlider.value = health;
	}

	void Movement()
	{
        if (!dashing) {
    		    Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
            input = input.normalized;
            rb.velocity += input * Time.fixedDeltaTime * acceleration;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));

            if (input != Vector2.zero && rb.velocity != Vector2.zero)
            {
                rb.velocity -= rb.velocity * Time.fixedDeltaTime;
            }
        }
    }
    
    public void Dash() {
        //Debug.Log(dashEnd);
        if (!dashing && controls.Player.Dash.triggered && Time.time - dashCooldown >= dashEnd) { // START DASH
            dashing = true;
            dashStart = Time.time;
            dashDirection = controls.Player.Movement.ReadValue<Vector2>();
            playerCollider.enabled = false;
        }
        if (dashing && Time.time - dashStart >= dashTime) { // STOP DASH
            dashing = false;
            playerCollider.enabled = true;
            dashEnd = Time.time;

        }
        if (dashing) { // DASH MOVEMENT
            transform.position += (Vector3) (dashDirection * dashSpeed * Time.deltaTime);
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
            if (!DialogueSystem.macrophagesExplained)
            {
                DialogueSystem.sentencesQueue.Add("Ouch! You were hit by a killer T cell");
                DialogueSystem.sentencesQueue.Add("Killer T cells will hunt you down");
                DialogueSystem.macrophagesExplained = true;
            }
            Kill();
        }
        else if (other.gameObject.tag == "Antibody")
        {
            if (lives >= 0)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Debug.Log("You got hit by an antibody");
            if (!DialogueSystem.antibodiesExplained)
            {
                DialogueSystem.sentencesQueue.Add("Avoid antibodies!");
                DialogueSystem.sentencesQueue.Add("Antibodies bind to your receptors making it harder to infect cells");
                DialogueSystem.sentencesQueue.Add("Too many will slow you down and eventually kill you!");
                DialogueSystem.antibodiesExplained = true;
            }
            maxSpeed -= 1f;
            health -= 100/8;
            acceleration /= 1.3f;
            Destroy(other.gameObject);
            if (maxSpeed < 1)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("You died");
        lives--;
        if (lives == 0)
        {
            audioSource.PlayOneShot(deathSound);
            Time.timeScale = 0;
            endScreen.SetActive(true);
            lives = -1;
        }
        else if (lives > 0)
        {
            audioSource.PlayOneShot(killSound);
            transform.position = new Vector2(0, 0);
            maxSpeed = 8f;
            acceleration = 50f;
            health = 100;
            sr.color = Color.white;
        }
        immunityStart = Time.time;
        //respawn macrophage
        foreach (var i in GameObject.FindGameObjectsWithTag("Macrophage")){
            Destroy(i);
            spawnController.SpawnMacrophage();
        }
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

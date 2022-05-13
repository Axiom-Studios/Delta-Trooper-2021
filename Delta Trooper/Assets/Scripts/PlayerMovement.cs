using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
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
    public AudioClip dashSound;

	// Sliders
	public Slider playerHealthSlider;
	public Slider dashIndicator;

    // Death Immunity
    public float immunityTime;
    float immunityStart;
    public SpawnController spawnController;
    float lastChange = 0;
    public float blinkSpeed;
    bool immune = false;
	public float wallKillError;

    void Start()
    {
        lives = 5;
        health = 100;
        immune = false;
        
        t1 = Time.time;
        rb = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
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
        CheckDialogue();
        Blink();
	}

    void CheckDialogue()
    {
        if (Time.time - t1 > 5 && !DialogueSystem.dashExplained)
        {
            DialogueSystem.sentencesQueue.Add("Press [LEFT SHIFT] to dash.");
            DialogueSystem.sentencesQueue.Add("Dash gives a speed boost.");
            DialogueSystem.sentencesQueue.Add("It also allows you to pass though dangerous stuff.");
            DialogueSystem.dashExplained = true;
        }

        if (Time.time - t1 > 10 && !DialogueSystem.antibodiesExplained)
        {
            DialogueSystem.sentencesQueue.Add("See those Y-shaped things coming from the right?");
            DialogueSystem.sentencesQueue.Add("Those are antibodies. They are part of the immune system.");
            DialogueSystem.sentencesQueue.Add("Antibodies bind to your receptors making it harder to infect cells.");
            DialogueSystem.sentencesQueue.Add("Too many will eventually kill you!");
            DialogueSystem.antibodiesExplained = true;
        }

        if (Time.time - t1 > 28 && !DialogueSystem.macrophagesExplained)
        {
            DialogueSystem.sentencesQueue.Add("Watch your back!");
            DialogueSystem.sentencesQueue.Add("The Killer T cell is hunting you down.");
            DialogueSystem.sentencesQueue.Add("They can kill you instantly!");
            DialogueSystem.sentencesQueue.Add("RUN!");
            DialogueSystem.macrophagesExplained = true;
        }

        if (Time.time - t1 > 68 && !DialogueSystem.helperbExplained)
        {
            DialogueSystem.sentencesQueue.Add("Those cannon things on the right are Helper B cells");
            DialogueSystem.sentencesQueue.Add("They produce antibodies.");
            DialogueSystem.sentencesQueue.Add("When in range, they shoot!");
            DialogueSystem.sentencesQueue.Add("Stay far away!");
            DialogueSystem.helperbExplained = true;
        }
	}

    void Blink() {
        if (immune && (Time.time - lastChange) >= blinkSpeed && !dashing) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            lastChange = Time.time;
        }
    }
    
    void Immunity() {
        if (immune && (Time.time - immunityStart) >= immunityTime && !dashing) {
            playerCollider.enabled = true;
            immune = false;
            spriteRenderer.enabled = true;
        }
        else if (immune) {
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
            audioSource.PlayOneShot(dashSound);
            dashing = true;
            dashStart = Time.time;
            dashDirection = controls.Player.Movement.ReadValue<Vector2>();
			if (dashDirection == Vector2.zero) {
				dashDirection = Vector2.right;
			}
            playerCollider.enabled = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .5f);
        }
        if (dashing && Time.time - dashStart >= dashTime) { // STOP DASH
            dashing = false;
            playerCollider.enabled = true;
            dashEnd = Time.time;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
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
            Kill();
        }
        else if (other.gameObject.tag == "Antibody")
        {
            if (lives >= 0)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Debug.Log("You got hit by an antibody");
            //maxSpeed -= 1f;
            health -= 100/8;
            //acceleration /= 1.3f;
            Destroy(other.gameObject);
            if (health <= 1)
            {
                Kill();
            }
        }
        else if (other.gameObject.tag == "Drill") {
            if (lives >= 0)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Debug.Log("You got hit by an antibody");
            //maxSpeed -= 1f;
            health -= 100/5;
            //acceleration /= 1.3f;
            Destroy(other.gameObject);
            if (health <= 1)
            {
                Kill();
            }
        }
    }

	void OnCollisionStay2D() {
		if (transform.position.x - clampCamera.ScreenToWorldPoint(Vector3.zero).x <= wallKillError) {
			Kill();
			Debug.Log("RIP");
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
            DialogueSystem.menuPaused = true;
            lives = -1;
        }
        else if (lives > 0)
        {
            audioSource.PlayOneShot(killSound);
            transform.position = new Vector2(0, 0);
            maxSpeed = 8f;
            acceleration = 50f;
            health = 100;
            spriteRenderer.color = Color.white;
            immune = true;
            immunityStart = Time.time;
        }
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
                spriteRenderer.color = Color.black;
                cellTime += Time.deltaTime;
            }
            else
            {
                spriteRenderer.color = Color.white;
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
        spriteRenderer.color = Color.magenta;
        cellTime = 0;
    }
}

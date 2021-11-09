using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	InputMaster controls;
    private Rigidbody2D rb;
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
        movement();
    }

	void movement()
	{
		Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
		Vector2 movement = input * speed * Time.fixedDeltaTime;
		rb.MovePosition(rb.position + movement);
	}

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Allows GameObjects to move around the scene using vector-based movement.
 * Caveats: None.
 * 
 */ 

public class Movement : MonoBehaviour
{
	public Vector3 position; // The position of the GameObject
	public Vector3 velocity = new Vector3(1.0f, 0.0f, 0.0f); // Velocity of the moving object
	private float speedIncrement = 20.0f; // Used with the speed variable to create acceleration

	public float speed = 0.0f;
	private float maxSpeed = 50.0f;
	private float slowDown = 0.97f;
	private float angularSpeed = 180.0f;
	public float angle = 0.0f;
	public bool useSlowdown = true;

	// Use this for initialization
	void Start ()
	{
		if (gameObject.tag != "Bullet") 
		{
			position = transform.position;
		}

		if(gameObject.tag == "Asteroid")
		{
			speed = Random.Range(10.0f, 25.0f);
			velocity = new Vector3(-1.0f, 0.0f, 0.0f);
			angle = Random.Range(0.0f, 360.0f);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Input based altering of the movement variables for the Player only
		if(gameObject.tag == "Player")
		{
			// Accelerate the Player 
			if(Input.GetKey(KeyCode.UpArrow))
			{
				speed += speedIncrement * Time.deltaTime;
			}
			else if(useSlowdown)
			{
				speed *= slowDown;
			}

			// Limit the speed of the Player
			if(speed > maxSpeed)
			{
				speed = maxSpeed;
			}
			else if(speed < 0.01f)
			{
				speed = 0.0f;
			}
			
			// Rotate the Player
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				angle += angularSpeed * Time.deltaTime;
			}
			else if(Input.GetKey(KeyCode.RightArrow))
			{
				angle -= angularSpeed * Time.deltaTime;
			}

			if(angle > 360.0f)
			{
				angle -= 360.0f;
			}
			else if (angle < 0.0f)
			{
				angle += 360.0f;
			}
		}

		// Update the position of all objects using movement
		if (gameObject.tag != "Bullet") 
		{
			transform.position = position;
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
		
			position += transform.rotation * velocity * speed * Time.deltaTime;
		
			ScreenWrap();
		} 
		else 
		{
			transform.position = position;
			position += velocity * speed * Time.deltaTime;
		}
	}

	// Gets the direction that the object is facing
	public Vector3 GetDirection()
	{
		return Vector3.Normalize(Quaternion.Euler(0.0f, 0.0f, angle) * velocity);
	}
	
	// Make the player wrap around when they reach an edge of the screen
	void ScreenWrap()
	{	
		Vector3 viewportPosition = Camera.main.WorldToViewportPoint (position);
		if (viewportPosition.x > 1 || viewportPosition.x < 0) 
		{
			position.x = -position.x;
		}

		if (viewportPosition.y > 1 || viewportPosition.y < 0) 
		{
			position.y = -position.y;
		}
	}


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Grants the player the ability to shoot bullets.
 * Caveats: None.
 * 
 */ 

public class Shoot : MonoBehaviour
{
	private BulletManager bulletMngr; 
	private Movement movement; 
	public GameObject player;
	private bool shooting = false;

	// Use this for initialization
	void Start ()
	{
		bulletMngr = gameObject.GetComponent<BulletManager>();
		if (bulletMngr == null)
		{
			Debug.Log("Bullet Manager not assigned in GameManager");
			Debug.Break();
		}

		if (player == null)
		{
			Debug.Log("Player not assigned in GameManager");
			Debug.Break();
		}
		movement = player.GetComponent<Movement>();
		if (movement == null)
		{
			Debug.Log("Movement not assigned in Player");
			Debug.Break();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Shoot only once per frame.
		if(shooting)
		{
			shooting = false;
			bulletMngr.Shoot(movement.position, movement.GetDirection() * 100.0f, Quaternion.Euler(0.0f, 0.0f, movement.angle));
		}
	}

	// Shoot only when the space bar is pressed
	void OnGUI()
	{
		if(Input.GetKeyDown("space"))
		{
				shooting = true;
		}
	}
}

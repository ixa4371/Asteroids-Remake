using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Detects collisions between the player and the asteroids on the scene. Also handles the player's lives.
 * Caveats: None.
 * 
 */ 

public class CollisionDetection : MonoBehaviour 
{
	public List<GameObject> objects; // List of the player and all of the asteroids on the scene 
	public GameObject player; // Player GameObject used to insert into the objects List

	private int firstObj; // The first object involved in a collision
	private int secondObj; // The second object involved in a collision
	private bool isColliding; 

	private Spawner spawnInfo; // Spawner component used to gain access to all of the asteroids that are spawned in

	private bool dead; // Bool denoting whether the player is dead or not
	private float deathTime; // The time at which the player died
	private float spawnBuffer = 3.0f; // The time (in seconds) that the player must wait to respawn after a death
	public int playerLives = 3; 

	// Use this for initialization
	void Start () 
	{
		spawnInfo = gameObject.GetComponent<Spawner> ();
		if (spawnInfo == null) 
		{
			Debug.Log ("Missing Spawner script on the Game Manager.");
			Debug.Break ();
		}

		// Fill the objects List with appropriate information
		objects = spawnInfo.activeAsteroids;
		objects.Add (player);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Check whether the player is colliding with any of the asteroids.
		isColliding = CheckCollision ();
		if (isColliding) 
		{
			isColliding = false;

			// When the player collides with an asteroid, the asteroid is destroyed and the player is temporarily relocated to give the illusion of a death
			if (objects [firstObj].tag == "Player") 
			{
				spawnInfo.SpawnChildren (objects [secondObj]);
				Destroy (objects [secondObj]);
				objects.RemoveAt(secondObj);

				player.GetComponent<Movement> ().enabled = false;
				player.transform.position = new Vector3 (1000.0f, 0.0f, 0.0f);
				gameObject.GetComponent<Shoot> ().enabled = false;
				spawnInfo.asteroidCount--;
			} 
			else 
			{
				spawnInfo.SpawnChildren (objects [firstObj]);
				Destroy (objects [firstObj]);
				objects.RemoveAt(firstObj);

				player.GetComponent<Movement> ().enabled = false;
				player.transform.position = new Vector3 (1000.0f, 0.0f, 0.0f);
				gameObject.GetComponent<Shoot> ().enabled = false;
				spawnInfo.asteroidCount--;
			}
				
			dead = true;
			playerLives--;
			deathTime = Time.time;
		} 

		// When the player "dies", there is a short period of time where they cannot do anything
		if (dead) 
		{
			if (Time.time > deathTime + spawnBuffer) 
			{
				player.GetComponent<Movement> ().position = new Vector3(0.0f, 0.0f, 0.0f);
				player.GetComponent<Movement> ().enabled = true;
				gameObject.GetComponent<Shoot> ().enabled = true;
				dead = false;
			}
		}

		// If the player runs out of lives, it is Game Over
		if (playerLives == 0) 
		{
			SceneManager.LoadScene("GameOver");
		}
			
	}

	/// <summary>
	/// Checks the collisions between the objects in the scene. 
	/// </summary>
	/// <returns><c>true</c>, if collision involves the player object, <c>false</c> otherwise.</returns>
	bool CheckCollision()
	{
		for (int i = 0; i < objects.Count; i++)
		{
			for (int j = 0; j < objects.Count; j++) 
			{
				if (j == i) {
					continue;
				} 

				if ((objects[i].transform.position - objects[j].transform.position).magnitude <= (objects[i].GetComponent<BoundingCircle>().radius + objects[j].GetComponent<BoundingCircle>().radius)) 
				{
					if (objects[i].tag == "Player") 
					{
						firstObj = i;
						secondObj = j;
						return true;
					}
					else if (objects[j].tag == "Player") 
					{
						firstObj = j;
						secondObj = i;
						return true;
					}
				}
			}
		}

		return false;
	}
}

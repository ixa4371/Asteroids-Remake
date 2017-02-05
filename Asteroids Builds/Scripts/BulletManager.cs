using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Manages bullets that are fired and their collisions with asteroids. Also displays necessary information to player on the screen.
 * Caveats: None.
 * 
 */ 

public class BulletManager : MonoBehaviour
{
	public GameObject bullet; // Bullet prefab that will be instantiated
	public List<GameObject> bulletPool; // List of bullets that are currently instantiated
	public int bulletsFired = 0; // Number of bullets fired

	public float lifeTime = 3.0f; // Amount of time (in seconds) that a bullet is allowed to stay active
	private List<float> bulletSpawnTimes; // List of Time.time float values that are used to determine when a bullet has lasted its intended life time

	// Components used to communicate information across the Game Manager's scripts
	private CollisionDetection collision;
	private Spawner spawner; 

	private int score; // Player's overall score
	public Texture livesIcon; // Image used to display the Player Lives on the upper left corner
	public Font font; // Custom font used to write the text on the upper left corner


	// Use this for initialization
	void Start ()
	{
		bulletPool = new List<GameObject>();
		bulletSpawnTimes = new List<float>();

		collision = gameObject.GetComponent<CollisionDetection> ();
		if (collision == null) 
		{
			Debug.Log ("Missing a CollisionDetection script on the Game Manager");
			Debug.Break ();
		}

		spawner = gameObject.GetComponent<Spawner> ();
		if (spawner == null) 
		{
			Debug.Log ("Missing a Spawner script on the Game Manager");
			Debug.Break ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i = 0; i < bulletsFired; i++)
		{
			for(int j = 0; j < collision.objects.Count; j++)
			{
				// If a bullet somehow hits the player, ignore it.
				if (collision.objects [j].tag == "Player") 
				{
					continue;
				}

				// Check to see if any of the active bullets are colliding with asteroids and destroy the bullets and asteroids that do collide
				if((bulletPool[i].transform.position - collision.objects[j].transform.position).magnitude <= (bulletPool[i].GetComponent<BoundingCircle>().radius + collision.objects[j].GetComponent<BoundingCircle>().radius))
				{
					Destroy(bulletPool[i]);
					if (collision.objects [j].transform.localScale.x > 1) 
					{
						spawner.SpawnChildren (collision.objects [j]);
						score += 20;
					} 
					else 
					{
						score += 50;
					}
					Destroy(collision.objects[j]);
					collision.objects.RemoveAt (j);
					bulletSpawnTimes.RemoveAt(i);
					bulletPool.RemoveAt(i);

					i--;
					bulletsFired--;
					spawner.asteroidCount--;
				}
			}
		}
			
		for(int i = 0; i < bulletsFired; i++)
		{
			// Compare the current time to the times stored in the bulletSpawnTimes list and despawn those that reach their life time limit
			if(Time.time > bulletSpawnTimes[i] + lifeTime)
			{
				Destroy(bulletPool[i]);
				bulletSpawnTimes.RemoveAt(i);
				bulletPool.RemoveAt(i);
			
				i--;
				bulletsFired--;
			}
		}
	}

	// Creates a bullet at the position of the player and launches it forward
	public void Shoot(Vector3 playerPosition, Vector3 direction, Quaternion playerRotation)
	{
		GameObject tempBullet = (GameObject) Instantiate(bullet, playerPosition, Quaternion.identity);
		Movement movement = tempBullet.GetComponent<Movement> ();

		// Alter the movement values to properly launch the bullet
		movement.position = playerPosition; 
		movement.velocity = direction;
		movement.speed = 1.0f;
		movement.useSlowdown = false;
		tempBullet.transform.rotation = playerRotation;

		bulletSpawnTimes.Add (Time.time); 
		bulletPool.Add(tempBullet); 
		bulletsFired++;
	}

	// Displays the current score and number of lives that the player has
	void OnGUI()
	{
		GUI.color = Color.white;
		GUI.skin.font = font;

		GUI.Label (new Rect(10.0f, 0.0f, 100.0f, 50.0f), "Score:" + score);
		GUI.Label (new Rect(10.0f, 20.0f, 200.0f, 50.0f), "Player Lives:" + collision.playerLives);

		float xOffset = 55.0f;
		for (int i = 0; i < collision.playerLives; i++) 
		{
			GUI.Box (new Rect(10.0f + (xOffset * i), 70.0f, 50.0f, 50.0f), livesIcon, GUIStyle.none);
		}
	}
}

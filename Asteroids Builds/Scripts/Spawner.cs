using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Spawns the Asteroids into the scene and repouplates once they are all destroyed.
 * Caveats: None.
 * 
 */ 

public class Spawner : MonoBehaviour 
{
	public GameObject[] asteroidTypes = new GameObject[4]; // The different kinds of asteroids available to spawn
	public GameObject[] childAsteroids = new GameObject[4]; // The different kinds of child asteroids available to spawn
	public List<GameObject> activeAsteroids; // List of currently active asteroids
	private GameObject asteroid; // Temporary varibale used to hold the randomly selected sprite that will be instantiated

	// World size information used to determine bounds when spawning
	private SpaceInfo space;
	private Vector3 worldSize;

	public int asteroidCount = 0;
	public int spawnLimit;

	// Use this for initialization
	void Start () 
	{
		space = gameObject.GetComponent<SpaceInfo> ();
		if (space == null) 
		{
			Debug.Log ("SpaceInfo component is not attached to the Game Manager");
		}
		worldSize = space.size;

		spawnLimit = 10;
		SpawnAsteroids();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Repopulates scene if all asteroids are destroyed
		if (asteroidCount == 0) 
		{
			SpawnAsteroids ();
		}
	}

	/// <summary>
	/// Spawns the asteroids at the beginning of the level or when all previous asteroids are destroyed.
	/// </summary>
	void SpawnAsteroids()
	{
		for (int i = 0; i < spawnLimit; i++) 
		{
			float worldWidth = Random.Range(-(worldSize.x / 2.0f), (worldSize.x / 2.0f));
			if (Mathf.Abs(worldWidth - 0.0f) < 50.0f) 
			{
				if (worldWidth < 0.0f) 
				{
					worldWidth -= 20.0f;
				} 
				else 
				{
					worldWidth += 20.0f;
				}
			}

			float worldHeight = Random.Range(-(worldSize.y / 2.0f), (worldSize.y / 2.0f));
			if (Mathf.Abs(worldHeight - 0.0f) < 10.0f) 
			{
				if (worldHeight < 0.0f) 
				{
					worldHeight -= 10.0f;
				} 
				else 
				{
					worldHeight += 10.0f;
				}
			}

			asteroid = asteroidTypes[Random.Range(0,asteroidTypes.Length)];
			GameObject temp = (GameObject) Instantiate(asteroid, new Vector3(worldWidth, worldHeight, 0.0f), Quaternion.identity);
			activeAsteroids.Add (temp);
			asteroidCount++;
		}
	}

	/// <summary>
	/// Spawns the children asteroids after a large asteroid is destroyed.
	/// </summary>
	/// <param name="asteroid">Large asteroid that needs to be broken down into children.</param>
	public void SpawnChildren(GameObject asteroid)
	{
		int childIndex = 0;
		for (int i = 0; i < asteroidTypes.Length; i++) 
		{
			if (asteroid.GetComponent<SpriteRenderer>().sprite == asteroidTypes[i].GetComponent<SpriteRenderer>().sprite) 
			{
				childIndex = i;
			}
		}

		int numChildren = Random.Range (2, 4);
		for (int i = 0; i < numChildren; i++) 
		{
			GameObject child = (GameObject)Instantiate (childAsteroids [childIndex], asteroid.transform.position, Quaternion.identity);
			Movement movement = child.GetComponent<Movement> ();

			movement.position = asteroid.transform.position; //Position of the bullet
			movement.velocity = asteroid.GetComponent<Movement>().GetDirection();//acceleration of the bullet
			movement.speed = asteroid.GetComponent<Movement>().speed;
			movement.useSlowdown = false;
			child.transform.rotation = Quaternion.Euler(0.0f, 0.0f, asteroid.GetComponent<Movement> ().angle);

			activeAsteroids.Add (child);
			asteroidCount++;
		}

	}
}

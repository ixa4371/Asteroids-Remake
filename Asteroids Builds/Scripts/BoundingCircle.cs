using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Israel Anthony
 * Purpose: Gives the GameObjects bounding circles that are used to detect collisions.
 * Caveats: None.
 * 
 */ 

public class BoundingCircle : MonoBehaviour
{
	public float radius = 1.0f;
	public Vector3 position;
	private Movement movement;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start ()
	{
		sprite = gameObject.GetComponent<SpriteRenderer>();
		if(null == sprite)
		{
			Debug.Log("Error: object" + gameObject.name + " needs to be a 2D sprite");
			Debug.Break();
		}

		movement = gameObject.GetComponent<Movement>();
		if(null == movement)
		{
			Debug.Log("Error: object" + gameObject.name + " also needs to have a Movement component");
			Debug.Break();
		}

		Vector3 size = sprite.bounds.size;
		radius = size.x / 2;
		if (radius < size.y) 
		{
			radius = size.y / 2;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		position = movement.position;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color (1.0f, 1.0f, 0.0f, 0.33f);
		Gizmos.DrawSphere (transform.position, radius);
	}
}

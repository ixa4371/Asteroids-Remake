using UnityEngine;
using System.Collections;

/*
 * Author: Israel Anthony
 * Purpose: Simple script to make the space background loop.
 * Caveats: Noticeable change at each iteration of the loop.
 * 
 */ 

public class BackgroundScroll : MonoBehaviour 
{
	private Vector3 position;

	// Use this for initialization
	void Start ()
	{
		position = gameObject.transform.position;
	}
	
	// Update is called once per frame
	// Make the background texture move down slowly to give the illusion that the objects are moving in space
	void Update () 
	{
		position.y -= 0.05f;
		gameObject.transform.position = position;

		if (position.y <= -127.0f) 
		{
			position.y = 0.0f;
		}
	}
}

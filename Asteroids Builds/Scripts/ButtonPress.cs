using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Author: Israel Anthony
 * Purpose: Allows buttons to load appropriate scenes.
 * Caveats: None.
 * 
 */ 

public class ButtonPress : MonoBehaviour 
{
	public Sprite pressedTexture;
	public string targetScene;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// When the button is clicked, a "pressed" texture is loaded and the scene is changed
	void OnMouseDown()
	{
		gameObject.GetComponent<SpriteRenderer> ().sprite = pressedTexture;
		if (targetScene != "Exit") 
		{
			SceneManager.LoadScene (targetScene);
		} 
		else 
		{
			Application.Quit();
		}
	}
}

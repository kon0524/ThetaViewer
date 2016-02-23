using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Label (new Rect (0, 0, 100, 30), "Hello");

		if (GUI.Button (new Rect (0, 30, 100, 50), "START")) {
			SceneManager.LoadScene ("main");
		}

		if (GUI.Button (new Rect (0, 80, 100, 50), "QUIT")) {
			Debug.Log ("Quit");
			Application.Quit ();
		}
	}
}

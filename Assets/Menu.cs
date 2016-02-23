using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class Menu : MonoBehaviour {

	public static string ImagePath = null;

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

		GUI.Box (new Rect (0, 130, 200, 100), "Drop Here!");

		string path = null;
		switch (Event.current.type) {
		case EventType.DragUpdated:
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			break;
		case EventType.DragPerform:
			DragAndDrop.AcceptDrag ();
			path = DragAndDrop.paths [0];
			break;
		default:
			break;
		}

		if (!string.IsNullOrEmpty (path)) {
			ImagePath = path;
			SceneManager.LoadScene ("main");
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	private const float ZOOM_SENSITIVITY = 0.6f;
	private const float ROTATE_SENSITIVITY = 0.2f;
	private const float ZOOM_MAX = 0.8f;
	private float prevZoom;
	private Vector3 prevMousePos;
	private Vector3 prevMoveVol;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// ズーム
		float zoom = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SENSITIVITY;
		if (zoom != 0.0f) {
			transform.Translate (0, 0, zoom, Space.Self);
			prevZoom = zoom;
		} else {
			prevZoom *= 0.9f;
			transform.Translate (0, 0, prevZoom, Space.Self);
		}

		// ドラッグしていなければ慣性で回す
		if (!Input.GetMouseButton (0)) {
			prevMoveVol *= 0.9f;
			transform.RotateAround (Vector3.zero, Vector3.up, -1 * prevMoveVol.x);
			transform.RotateAround (Vector3.zero, transform.right, prevMoveVol.y);
			return;
		}

		if (Input.GetMouseButtonDown (0))
			prevMousePos = Input.mousePosition;

		// 回転
		Vector3 curMousePos = Input.mousePosition;
		Vector3 moveVol = (curMousePos - prevMousePos) * ROTATE_SENSITIVITY;
		transform.RotateAround (Vector3.zero, Vector3.up, -1 * moveVol.x);
		transform.RotateAround (Vector3.zero, transform.right, moveVol.y);
		prevMousePos = curMousePos;
		prevMoveVol = moveVol;
	}
}

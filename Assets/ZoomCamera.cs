using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	private const float ZOOM_SENSITIVITY = 0.8f;
	private const float ZOOM_MAX = 0.8f;
	private Vector3 defaultPosition;

	// Use this for initialization
	void Start () {
		defaultPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// スペースキーで初期位置に戻る
		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.position = defaultPosition;
		}

		// カメラの現在位置とマウスホイールの値
		Vector3 curPosition = transform.position;
		float zoom = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SENSITIVITY;
		if (zoom == 0) return;
		Debug.Log ("pos : " + curPosition + " zoom : " + zoom);

		// ズームしすぎを判定する
		if (zoom > 0 && curPosition.z + zoom > ZOOM_MAX) return;

		// ズーム
		transform.Translate (new Vector3(0, 0, zoom));
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	private const float ZOOM_SENSITIVITY = 0.6f;
	private const float ZOOM_MAX = 0.8f;
	private Vector3 defaultPosition;
	private float prevZoom;

	// Use this for initialization
	void Start () {
		defaultPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// ESCキーでアプリ終了
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//Application.Quit ();
		}

		// スペースキーで初期位置に戻る
		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.position = defaultPosition;
			prevZoom = 0.0f;
		}

		// マウスが押下されたらズーム停止
		if (Input.GetMouseButtonDown (0)) {
			prevZoom = 0.0f;
		}

		// カメラの現在位置とマウスホイールの値
		Vector3 curPosition = transform.position;
		float zoom = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SENSITIVITY;

		// マウスホイールが止まっていれば慣性で回す
		if (zoom == 0) {
			prevZoom *= 0.9f;
			// ズームしすぎを判定する
			if (prevZoom > 0 && curPosition.z + prevZoom > ZOOM_MAX) return;
			// ズーム
			transform.Translate (new Vector3(0, 0, prevZoom));
			return;
		}

		// ズームしすぎを判定する
		if (zoom > 0 && curPosition.z + zoom > ZOOM_MAX) return;
		// ズーム
		transform.Translate (new Vector3(0, 0, zoom));

		prevZoom = zoom;
	}
}

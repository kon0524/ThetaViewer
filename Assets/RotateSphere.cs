﻿using UnityEngine;
using System.Collections;

public class RotateSphere : MonoBehaviour {

	private const float MOUSE_SENSITIVITY = 0.1f;
	private Quaternion defaultRotation;
	private Vector3 prevMousePos;
	private Vector3 prevScrollSpeed;

	// Use this for initialization
	void Start () {
		defaultRotation = transform.rotation;
		Debug.Log ("defaultRotation : " + defaultRotation);
	}
	
	// Update is called once per frame
	void Update () {
		// スペースキーで初期位置に戻る
		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.rotation = defaultRotation;
		}

		// マウスの左押下開始で初期位置を取得する
		if (Input.GetMouseButtonDown (0)) {
			prevMousePos = Input.mousePosition;
			prevScrollSpeed = Vector3.zero;
		}

		// カメラ位置によって感度を調整する
		float cameraZoom = Camera.main.transform.position.z;
		float sensitivity = MOUSE_SENSITIVITY + (-1 * cameraZoom * 0.1f);

		// マウスの左ボタン未押下では慣性で回す
		if (!Input.GetMouseButton (0)) {
			prevScrollSpeed *= 0.95f;
			//Debug.Log (prevScrollSpeed);
			transform.Rotate(0, prevScrollSpeed.x * sensitivity, 0, Space.Self);
			transform.Rotate (-1 * prevScrollSpeed.y * sensitivity, 0, 0, Space.World);
			return;
		}

		// 以下はドラッグ中.マウスの移動量を計算する
		Vector3 curMousePos = Input.mousePosition;
		Vector3 mouseMoveVol = curMousePos - prevMousePos;
		prevMousePos = curMousePos;

		// マウスが移動していなければ以下の処理は行わない
		if (mouseMoveVol != Vector3.zero) {
			// 回転する
			transform.Rotate (0, mouseMoveVol.x * sensitivity, 0, Space.Self);
			transform.Rotate (-1 * mouseMoveVol.y * sensitivity, 0, 0, Space.World);
		}

		prevScrollSpeed = mouseMoveVol;
	}
}

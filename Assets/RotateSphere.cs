using UnityEngine;
using System.Collections;

public class RotateSphere : MonoBehaviour {

	private const float MOUSE_SENSITIVITY = 0.2f;
	private Quaternion defaultRotation;
	private Vector3 prevMousePos;

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
		}

		// マウスの左ボタン未押下では以下の処理は行わない
		if (!Input.GetMouseButton(0)) return;

		// 以下はドラッグ中.マウスの移動量を計算する
		Vector3 curMousePos = Input.mousePosition;
		Vector3 mouseMoveVol = curMousePos - prevMousePos;
		prevMousePos = curMousePos;

		// マウスが移動していなければ以下の処理は行わない
		if (mouseMoveVol == Vector3.zero) return;

		// Y軸を回転する
		transform.Rotate(0, mouseMoveVol.x * MOUSE_SENSITIVITY, 0);
		Debug.Log("mouseMoveVol : " + mouseMoveVol);

	}
}

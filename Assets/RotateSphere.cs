using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEditor;
using System.IO;
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

		if (Menu.DownloadedImage != null) {
			loadImage (Menu.DownloadedImage);
			Menu.DownloadedImage = null;
		} else {
			loadImage (Menu.SelectedImage);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// ESCキーで戻る
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("menu");
		}

		// スペースキーで初期位置に戻る
		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.rotation = defaultRotation;
			prevScrollSpeed = Vector3.zero;
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

	void OnGUI () {
		dragAndDropEventHandler ();
	}

	/// <summary>
	/// D&Dの監視
	/// </summary>
	private void dragAndDropEventHandler() {
		Event cur = Event.current;
		switch (cur.type) {
		case EventType.DragUpdated:
			Debug.Log ("DragUpdated");
			//DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			break;
		case EventType.DragPerform:
			Debug.Log ("DragPerform");
			//DragAndDrop.AcceptDrag ();
			//loadImage (DragAndDrop.paths [0]);
			break;
		default:
			//DragAndDrop.visualMode = DragAndDropVisualMode.None;
			break;
		}
	}

	/// <summary>
	/// 画像をロードする
	/// </summary>
	/// <param name="path">Path.</param>
	private void loadImage(string path) {
		// 入力値およびファイルチェック
		if (string.IsNullOrEmpty (path) || !File.Exists (path))
			return;
		string ext = Path.GetExtension (path).ToUpper ();
		if (ext != ".JPG" && ext != ".JPEG")
			return;

		// 読込み
		byte[] imageBytes = null;
		using (FileStream fs = new FileStream (path, FileMode.Open, FileAccess.Read)) {
			using (BinaryReader br = new BinaryReader (fs)) {
				imageBytes = br.ReadBytes ((int)br.BaseStream.Length);
			}
		}

		// 表示
		if (imageBytes != null) {
			Texture2D tex = new Texture2D (1, 1);
			tex.LoadImage (imageBytes);
			Debug.Log (tex.width + " : " + tex.height);
			GetComponent<Renderer> ().material.mainTexture = tex;
		}
	}

	/// <summary>
	/// 画面をロードする
	/// </summary>
	/// <param name="data">Data.</param>
	private void loadImage(byte[] data) {
		if (data != null) {
			Texture2D tex = new Texture2D (1, 1);
			tex.LoadImage (data);
			Debug.Log (tex.width + " : " + tex.height);
			GetComponent<Renderer> ().material.mainTexture = tex;
		}
	}
}

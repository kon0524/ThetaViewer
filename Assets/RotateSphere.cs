using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class RotateSphere : MonoBehaviour {

	private const float MOUSE_SENSITIVITY = 0.1f;
	private Vector3 prevMousePos;
	private Vector3 prevScrollSpeed;
	private Zenith.PitchRollInfo info;

	// Use this for initialization
	void Start () {
		if (Menu.DownloadedImage != null) {
			loadImage (Menu.DownloadedImage);
			Menu.DownloadedImage = null;
		} else {
			loadImage (Menu.SelectedImage);
		}
		Debug.Log ("Pitch : " + info.Pitch + " ,Roll : " + info.Roll);
		//transform.Rotate (-1 * info.Roll, 0, info.Pitch);
		transform.Rotate(Vector3.left, info.Roll, Space.World);
		transform.Rotate (Vector3.forward, info.Pitch, Space.World);
	}
	
	// Update is called once per frame
	void Update () {
		// ESCキーで戻る
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("menu");
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
		loadImage(imageBytes);
	}

	/// <summary>
	/// 画面をロードする
	/// </summary>
	/// <param name="data">Data.</param>
	private void loadImage(byte[] data) {
		if (data != null) {
			info = Zenith.GetInfo (data);
			Texture2D tex = new Texture2D (1, 1);
			tex.LoadImage (data);
			Debug.Log (tex.width + " : " + tex.height);
			GetComponent<Renderer> ().material.mainTexture = tex;
		}
	}
}

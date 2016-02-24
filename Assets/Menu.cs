using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	/// <summary>
	/// 選択した画像
	/// </summary>
	public static string SelectedImage;

	/// <summary>
	/// MyPictureのパス
	/// </summary>
	private string myPicture;

	/// <summary>
	/// 現在のパス
	/// </summary>
	private string current;

	/// <summary>
	/// ファイル一覧
	/// </summary>
	private string[] files;

	/// <summary>
	/// ディレクトリ一覧
	/// </summary>
	private string[] directories;

	// Use this for initialization
	void Start () {
		myPicture = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures);
		current = myPicture;
		updateFiles (myPicture);

		// Buttonのプレハブを取得
		GameObject prefab = (GameObject)Resources.Load ("Button");
		// ContentPanelの取得
		GameObject content = GameObject.Find("ContentPanel");

		// ボタン生成
		float height = 0;
		foreach (string f in files) {
			GameObject button = Instantiate (prefab) as GameObject;
			Text btnText = button.transform.FindChild ("Text").GetComponent<Text> ();
			btnText.text = Path.GetFileName(f);
			Button b = button.GetComponent<Button> ();
			b.name = f;
			b.onClick.AddListener (() => {
				SelectedImage = b.name;
				Debug.Log(SelectedImage + " is Selected!");
			});
			button.transform.SetParent (content.transform);
			RectTransform btnRectTrans = button.GetComponent<RectTransform> ();
			btnRectTrans.localPosition = new Vector2 (0, height);
			height -= 30;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		#if false
		float y = 0;

		// 一つ上に戻る
		if (GUI.Button (new Rect (0, y, 300, 30), "modoru")) {
			Debug.Log ("modoru");
			current = Directory.GetParent (current).FullName;
			updateFiles (current);
		}
		y += 31.0f;

		// ディレクトリ一覧
		foreach (string d in directories) {
			if (GUI.Button (new Rect (0, y, 300, 30), Path.GetFileName(d))) {
				Debug.Log (d + " is Selected!");
			}
			y += 31.0f;
		}

		// JPEGファイル一覧
		foreach (string f in files) {
			if (GUI.Button (new Rect (0, y, 300, 30), Path.GetFileName (f))) {
				SelectedImage = f;
				Debug.Log (f + " is Selected!");
			}
			y += 31.0f;
		}
		#endif
	}

	/// <summary>
	/// 指定したパスのJPEGファイルでfilesを更新します
	/// </summary>
	/// <param name="path">Path.</param>
	private void updateFiles(string path) {
		List<string> list = new List<string> ();
		string[] temp = Directory.GetFiles (path);
		foreach (string f in temp) {
			string ext = Path.GetExtension (f).ToUpper ();
			if (ext == ".JPG" || ext == ".JPEG") {
				list.Add (f);
			}
		}
		files = list.ToArray ();

		directories = Directory.GetDirectories (path);
	}
}

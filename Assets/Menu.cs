using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	/// <summary>
	/// ボタン種別
	/// </summary>
	private enum ButtonType { Image, Directory, Up }

	// Use this for initialization
	void Start () {
		myPicture = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures);
		current = myPicture;
		updateFiles (myPicture);
		updateButtons ();
	}
	
	// Update is called once per frame
	void Update () {
		// ESCキーでアプリ終了
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
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

	/// <summary>
	/// ボタン群を生成します
	/// </summary>
	private void updateButtons() {
		float height = 0;

		removeAllButton ();

		createButtonObject (current, ButtonType.Up, height);
		height -= 30;

		foreach (string d in directories) {
			createButtonObject (d, ButtonType.Directory, height);
			height -= 30;
		}

		foreach (string f in files) {
			createButtonObject (f, ButtonType.Image, height);
			height -= 30;
		}

		GameObject content = GameObject.Find("ContentPanel");
		RectTransform rect = content.GetComponent<RectTransform> ();
		rect.sizeDelta = new Vector2 (rect.sizeDelta.x, -1 * height);
	}

	/// <summary>
	/// 一つのボタンを生成します
	/// </summary>
	/// <returns>The button object.</returns>
	/// <param name="path">Path.</param>
	/// <param name="type">Type.</param>
	/// <param name="offset">Offset.</param>
	private GameObject createButtonObject(string path, ButtonType type, float offset) {
		GameObject content = GameObject.Find("ContentPanel");
		GameObject prefab = (GameObject)Resources.Load ("Button");
		GameObject buttonObj = Instantiate (prefab) as GameObject;

		// テキスト
		Text text = buttonObj.GetComponentInChildren<Text>();
		text.text = (type != ButtonType.Up) ? Path.GetFileName (path) : "Up";
		text.alignment = TextAnchor.MiddleLeft;

		// ボタン
		Button button = buttonObj.GetComponent<Button> ();
		button.name = path;
		if (type == ButtonType.Image) {
			button.onClick.AddListener (() => {
				SelectedImage = button.name;
				Debug.Log (SelectedImage + " is Selected!");
				SceneManager.LoadScene("main");
			});
		} else if (type == ButtonType.Directory) {
			button.onClick.AddListener (() => {
				current = button.name;
				Debug.Log (current + " directory change.");
				updateFiles(current);
				updateButtons();
			});
		} else {
			button.onClick.AddListener (() => {
				Debug.Log("Up" + button.name);
				current = Directory.GetParent(button.name).FullName;
				Debug.Log (current + " directory change.");
				updateFiles(current);
				updateButtons();
			});
		}
		button.transform.SetParent (content.transform);
		RectTransform btnRectTrans = button.GetComponent<RectTransform> ();
		btnRectTrans.localPosition = new Vector2 (0, offset);

		return buttonObj;
	}

	/// <summary>
	/// すべてのボタンを消す
	/// </summary>
	private void removeAllButton() {
		GameObject content = GameObject.Find("ContentPanel");
		Button[] button = content.GetComponentsInChildren<Button> ();
		foreach (Button b in button) {
			b.gameObject.SetActive (false);
			GameObject.Destroy (b);
		}
	}
}

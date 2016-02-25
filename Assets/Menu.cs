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
	/// ダウンロードしたイメージ
	/// </summary>
	public static byte[] DownloadedImage;

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
	private enum ButtonType { Image, Directory}

	/// <summary>
	/// 論理ドライブ一覧
	/// </summary>
	private string[] drives;

    /// <summary>
    /// カレントパス
    /// </summary>
    private Text currentPath;

	// Use this for initialization
	void Start () {
		// 現在のパスを設定する
		currentPath = GameObject.Find("CurrentPath").GetComponent<Text> ();
		if (string.IsNullOrEmpty (ViewerInfo.CurrentPath)) {
			// デフォルトはMyPictureとする
			string myPicture = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures);
			currentPath.text = ViewerInfo.CurrentPath = myPicture;
		} else {
			// 前回のパスを設定する
			currentPath.text = ViewerInfo.CurrentPath;
		}

		// ドライブ選択を初期化する
		Dropdown driveSelect = GameObject.Find ("SelectDrive").GetComponent<Dropdown> ();
		driveSelect.options.Clear ();
		drives = Directory.GetLogicalDrives();
		for (int i = 0; i < drives.Length; i++) {
			driveSelect.options.Add (new Dropdown.OptionData (drives[i]));
			if (drives [i] == Path.GetPathRoot (currentPath.text)) {
				driveSelect.value = i;
				Text label = driveSelect.GetComponentInChildren<Text> ();
				label.text = Path.GetPathRoot (currentPath.text);
			}
		}
		driveSelect.onValueChanged.AddListener (new UnityEngine.Events.UnityAction<int> (driveChange));

		// パスのファイル・ディレクトリ一覧を取得してボタンを描画する
		updateFiles (currentPath.text);
		updateButtons ();

		// 各種ボタンにイベントを設定する
		GameObject.Find ("TakePicture").GetComponent<Button> ()
			.onClick.AddListener (new UnityEngine.Events.UnityAction (takePicture));
		GameObject.Find ("UpButton").GetComponent<Button> ()
			.onClick.AddListener (new UnityEngine.Events.UnityAction (upDirectory));
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
        text.text = Path.GetFileName(path);
		text.alignment = TextAnchor.MiddleLeft;

		// ボタン
		Button button = buttonObj.GetComponent<Button> ();
		if (type == ButtonType.Image) {
			button.onClick.AddListener (() => {
				SelectedImage = path;
				Debug.Log (SelectedImage + " is Selected!");
				SceneManager.LoadScene("main");
			});
		} else {
			button.onClick.AddListener (() => {
				currentPath.text = path;
                ViewerInfo.CurrentPath = path;
                updateFiles(path);
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

	/// <summary>
	/// TakePictureボタン押下時の処理
	/// </summary>
	private void takePicture() {
		Debug.Log ("take picture");

		string finger = ThetaAccess.GetFingerPrint ();
		ThetaAccess.TakePicture ();
		while (true) {
			System.Threading.Thread.Sleep (100);
			string newFinger = ThetaAccess.GetFingerPrint ();
			if (finger != newFinger)
				break;
		}

		DownloadedImage = ThetaAccess.GetLatestImage ();
		SceneManager.LoadScene ("main");
	}

	/// <summary>
	/// Upボタン押下時の処理
	/// </summary>
	private void upDirectory() {
		DirectoryInfo di = Directory.GetParent(currentPath.text);
		if (di != null)
		{
			currentPath.text = ViewerInfo.CurrentPath = di.FullName;
			updateFiles(currentPath.text);
			updateButtons();
		}
	}

	/// <summary>
	/// ドライブ変更時の処理
	/// </summary>
	/// <param name="value">Value.</param>
	private void driveChange(int value) {
		currentPath.text = ViewerInfo.CurrentPath = drives[value];
		updateFiles (currentPath.text);
		updateButtons ();
	}
}

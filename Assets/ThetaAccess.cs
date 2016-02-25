using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using MiniJSON;

public static class ThetaAccess {

	/// <summary>
	/// シリアル番号
	/// </summary>
	public static string SerialNumber = "";

	/// <summary>
	/// ファームウェアバージョン
	/// </summary>
	public static string FirmwareVersion = "";

	/// <summary>
	/// 最新の画像ファイル
	/// </summary>
	public static string LatestFileUri = "";

	/// <summary>
	/// ベースURL
	/// </summary>
	private static string URL_BASE = "http://192.168.1.1";

	/// <summary>
	/// InfoのURL
	/// </summary>
	private static string URL_INFO = "/osc/info";

	/// <summary>
	/// StateのURL
	/// </summary>
	private static string URL_STATE = "/osc/state";

	/// <summary>
	/// ExecuteのURL
	/// </summary>
	private static string URL_EXEC = "/osc/commands/execute";

	/// <summary>
	/// 情報を取得する
	/// </summary>
	public static void GetInfo() {
		Debug.Log ("GET INFO");
		WebClient client = new WebClient ();
		Stream response = client.OpenRead (URL_BASE + URL_INFO);
		StreamReader reader = new StreamReader (response);
		string res = reader.ReadToEnd ();
		Debug.Log (res);
		response.Close ();
		reader.Close ();
		client.Dispose ();

		// パース
		Dictionary<string, object> json = Json.Deserialize(res) as Dictionary<string, object>;
		SerialNumber = (string)json ["serialNumber"];
		FirmwareVersion = (string)json ["firmwareVersion"];
	}

	/// <summary>
	/// フィンガープリントを取得する
	/// </summary>
	/// <returns>The finger print.</returns>
	public static string GetFingerPrint() {
		Debug.Log ("GET FINGER PRINT");

		WebClient client = new WebClient ();
		string resStr = client.UploadString (URL_BASE + URL_STATE, "");
		Dictionary<string, object> resJson = Json.Deserialize(resStr) as Dictionary<string, object>;
		client.Dispose ();
		LatestFileUri = (string)((Dictionary<string, object>)resJson ["state"]) ["_latestFileUri"];
		Debug.Log (LatestFileUri);

		return (string)resJson ["fingerprint"];
	}

	/// <summary>
	/// 撮影する
	/// </summary>
	public static void TakePicture() {
		Debug.Log ("TAKE PICTURE");
		WebClient client = new WebClient ();

		string sid = startSession (client);
		setOptionsAuto (client, sid);
		takePicture (client, sid);
		closeSession (client, sid);

		client.Dispose ();
	}

	/// <summary>
	/// 最新の画像を取得する
	/// </summary>
	/// <returns>The latest image.</returns>
	public static byte[] GetLatestImage() {
		Debug.Log ("GET LATEST IMAGE");

		if (string.IsNullOrEmpty (LatestFileUri))
			return null;

		WebClient client = new WebClient ();

		string template = "{\"name\":\"camera.getImage\",\"parameters\":{\"fileUri\":\"URI\",\"maxSize\":10000,\"_type\":\"full\"}}";
		string reqJson = template.Replace ("URI", LatestFileUri);
		byte[] reqData = Encoding.ASCII.GetBytes (reqJson);
		byte[] image = client.UploadData(URL_BASE + URL_EXEC, reqData);

		client.Dispose ();
		return image;
	}

	private static string startSession(WebClient client) {
		string reqJson = "{\"name\":\"camera.startSession\",\"parameters\":{\"timeout\":60}}";
		string resStr = client.UploadString (URL_BASE + URL_EXEC, reqJson);
		Dictionary<string, object> resJson = Json.Deserialize(resStr) as Dictionary<string, object>;
		Dictionary<string, object> results = resJson["results"] as Dictionary<string, object>;
		return (string)results["sessionId"];
	}

	private static void closeSession(WebClient client, string sid) {
		string template = "{\"name\":\"camera.closeSession\",\"parameters\":{\"sessionId\":\"SID\"}}";
		string reqJson = template.Replace ("SID", sid);
		client.UploadString (URL_BASE + URL_EXEC, reqJson);
	}

	private static void setOptionsAuto(WebClient client, string sid) {
		string template = "{\"name\":\"camera.setOptions\",\"parameters\":{\"sessionId\":\"SID\",\"options\":{\"captureMode\":\"image\",\"exposureProgram\":2,\"exposureCompensation\":0,\"whiteBalance\":\"auto\",\"_filter\":\"off\"}}}";
		string reqJson = template.Replace ("SID", sid);
		client.UploadString (URL_BASE + URL_EXEC, reqJson);
	}

	private static void takePicture(WebClient client, string sid) {
		string template = "{\"name\":\"camera.takePicture\",\"parameters\":{\"sessionId\":\"SID\"}}";
		string reqJson = template.Replace ("SID", sid);
		client.UploadString (URL_BASE + URL_EXEC, reqJson);
	}
}

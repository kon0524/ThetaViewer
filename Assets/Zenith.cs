using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public static class Zenith {
	/// <summary>
	/// ピッチとロールの構造体
	/// </summary>
	public struct PitchRollInfo
	{
		public float Pitch;
		public float Roll;
	}

	/// <summary>
	/// ピッチとロールを取得する
	/// </summary>
	/// <returns>The info.</returns>
	/// <param name="image">Image.</param>
	public static PitchRollInfo GetInfo(byte[] image)
	{
		PitchRollInfo ret = new PitchRollInfo () {
			Pitch = 0,
			Roll = 0
		};

		if (image == null) {
			Debug.LogWarning ("image is null.");
			return ret;
		}
		
		string xml = getXml (image);
		if (xml == null) {
			Debug.LogWarning ("xml is null.");
			return ret;
		}

		using (StringReader sr = new StringReader (xml)) {
			XmlDocument xmlDoc = new XmlDocument ();

			try {
				xmlDoc.Load (sr);
				XmlNode desc = xmlDoc.FirstChild.FirstChild.FirstChild;
				Debug.Log ("name : " + desc.Name);

				XmlNodeList gpano = desc.ChildNodes;
				foreach (XmlNode n in gpano) {
					if (n.Name == "GPano:PosePitchDegrees") {
						ret.Pitch = float.Parse (n.InnerText);
						Debug.Log ("Pitch : " + ret.Pitch);
					}
					if (n.Name == "GPano:PoseRollDegrees") {
						ret.Roll = float.Parse (n.InnerText);
						Debug.Log ("Roll  : " + ret.Roll);
					}
				}
			} catch {
				Debug.Log ("Unknown XML");
			}
		}

		return ret;
	}

	/// <summary>
	/// XMLを取得する
	/// </summary>
	/// <returns>The xml.</returns>
	/// <param name="data">Data.</param>
	private static string getXml(byte[] data)
	{
		int index = 2;

		while (true) {
			int length = 0;
			// APP1
			if (data [index] == 0xFF && data [index + 1] == 0xE1) {
				Debug.Log ("found APP1");
				index += 2;
				length = ((int)(data [index]) << 8) + data [index + 1];
				index += 2;
				if (data [index] == 0x68) {
					// XML
					string temp = System.Text.ASCIIEncoding.ASCII.GetString(data, index, length);
					Debug.Log ("found data : " + temp.Substring (0, 100));
					string xml = temp.Substring (83);
					return xml;
				} else {
					// Exif
					index += length - 2;
				}
			// FFDB
			} else if (data[index] == 0xFF && data[index + 1] == 0xDB) {
				Debug.Log ("found FFDB. end.");
				break;
			// 他
			} else {
				Debug.Log ("found else. next.");
				index += 2;
				length = ((int)(data [index]) << 8) + data [index + 1];
				index += length;
			}
		}

		return null;
	}
}

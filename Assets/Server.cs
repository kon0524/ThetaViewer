using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Collections.Generic;

public class Server {

	private static Server instance = null;
	private TcpListener listener = null;
	private const int PORT_NO = 9999;
	private List<TcpClient> clients;
	private Thread recv = null;
	private bool isWorking = false;

	private Server() {
		clients = new List<TcpClient> ();
	}

	public static Server GetInstance() {
		if (instance == null) {
			instance = new Server(); 
		}
		return instance;
	}

	/// <summary>
	/// サーバーを開始します
	/// </summary>
	public bool Start() {
		if (listener != null)
			return false;
		
		listener = new TcpListener (PORT_NO);
		listener.Start ();
		IAsyncResult result = listener.BeginAcceptTcpClient (new AsyncCallback (callback), null);

		isWorking = true;
		recv = new Thread (new ThreadStart (serverThread));
		recv.Start ();

		Debug.Log ("Server Start !");
		return true;
	}

	/// <summary>
	/// サーバーを終了します
	/// </summary>
	public bool Stop() {
		if (listener == null)
			return false;
		listener.Stop ();
		listener = null;
		isWorking = false;
		return true;
	}

	/// <summary>
	/// 接続してきたクライアントを受け入れる
	/// </summary>
	/// <param name="ar">Ar.</param>
	private void callback(IAsyncResult ar) {
		Debug.Log ("callback");
		TcpClient client = listener.EndAcceptTcpClient (ar);
		clients.Add (client);
		listener.BeginAcceptTcpClient (new AsyncCallback (callback), null);
	}

	/// <summary>
	/// サーバー
	/// </summary>
	private void serverThread() {
		while (isWorking) {
			foreach (TcpClient c in clients) {
				if (c.Available > 0) {
					NetworkStream ns = c.GetStream ();

				}
			}
		}
	}
}

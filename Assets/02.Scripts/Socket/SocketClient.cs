using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class SocketClient : MonoBehaviour
{
	[SerializeField] private GameObject payPanel;
	[SerializeField] private Text payResult;

	private int port;
	private string address, result;
	private TcpClient socketConnection;
	private Thread clientReceiveThread;

	bool b_Fail = false;
	bool b_Success = false;

	// Use this for initialization
	void Start()
	{
		ConnectToTcpServer();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.S))
		{
			clientReceiveThread.Interrupt();
			socketConnection.Dispose();
			socketConnection.Close();

			b_Success = true;

			SoundManager.instance.PlayPayMoney();
		}

		if (b_Fail)
        {
			payPanel.SetActive(true);
			payResult.text = result;

			b_Fail = false;

			StartCoroutine(PayResultMethod("Fail"));
		}
		
		if(b_Success)
        {
			//payPanel.SetActive(true);
			//payResult.text = result;

			SoundManager.instance.PlayPayMoney();

			b_Success = false;

			//StartCoroutine(PayResultMethod("Success"));

			IntroManager.instance.GotoScene();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SendMessage();
        //}
    }

    IEnumerator PayResultMethod(string str)
    {
        yield return new WaitForSeconds(3.0f);

		if(str.Equals("Fail"))
        {
			payResult.text = string.Empty;
			payPanel.SetActive(false);

			IntroManager.instance.b_Start = false;
		}
        else if(str.Equals("Success"))
        {
			payPanel.SetActive(false);
		}
	}

    /// <summary>
    /// Setup socket connection.
    /// </summary>
    private void ConnectToTcpServer()
	{
		try
		{
			// ���� IPv4�ּ� ��������
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			foreach(var ip in host.AddressList)
            {
				if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
					address = ip.ToString();
                }
            }

			//Debug.Log(address);

			//address = "172.30.1.7";
			port = 50001;

			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient(address, port);
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message.

						Encoding euc = Encoding.GetEncoding("euc-kr");

						//int.TryParse(Encoding.ASCII.GetString(incommingData), out int data);
						//string[] data = Encoding.ASCII.GetString(incommingData).Split(',');

						//string[] data = string.Concat(Encoding.UTF8.GetString(incommingData).Where(x => !char.IsWhiteSpace(x))).Split(','); // �ѱ۱���
						string[] data = string.Concat(euc.GetString(incommingData).Where(x => !char.IsWhiteSpace(x))).Split(','); // ���������ؼ� ,�� ������


						ReceivePayData(data);
						//string serverMessage = Encoding.ASCII.GetString(incommingData);
						//Debug.Log("server message received as: " + serverMessage);
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendMessage()
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				//string clientMessage = "This is a message from one of your clients.";
				string clientMessage = "Charge";

				// Convert string message to byte array.                 
				//byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage);

				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log("Client sent his message - should be received by server");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	public void ReceivePayData(string[] data)
	{
		int.TryParse(data[0], out int ret);
		string num = data[1];
		result = data[2];

		if(ret.Equals(1))
        {
			//Debug.Log("���� ����");
			
			//Debug.Log("null : " + num);
			//Debug.Log("result : " + result);

			if (!num.Equals("null") && result.Equals("�������"))
			{
				socketConnection.Dispose();
				socketConnection.Close();

				b_Success = true;
			}
			else
            {
				b_Fail = true;
            }
		}
		else
        {
			switch (ret)
			{
				case -1:
					Debug.Log("NVCAT ���� ���� �ƴ�");
					break;

				case -2:
					Debug.Log("�ŷ��ݾ��� �������� ����");
					break;

				case -3:
					Debug.Log("ȯ������ �б� ����");
					break;

				case -4:
					Debug.Log("NVCAT ���� ���� ���� (���� ��� �ʿ�) VAN�翡 ���ǿ��");
					break;

				case -5:
					Debug.Log("��Ÿ ���䵥���� ���� (���� ��� �ʿ�) VAN�翡 ���ǿ��");
					break;

				case -6:
					Debug.Log("���� �ð� �ʰ�");
					break;

				case -7:
					Debug.Log("����� �� ������ ��û ���");
					break;

				case -8:
					Debug.Log("FALLBACK �ŷ� ��û �ʿ�");
					break;

				case -9:
					Debug.Log("��Ÿ ����");
					break;

				case -10:
					Debug.Log("IC �켱 �ŷ� ��û �ʿ� (ICī�� MS������)");
					break;

				case -11:
					Debug.Log("FALLBACK �ŷ� �ƴ�");
					break;

				case -12:
					Debug.Log("�ŷ� �Ұ� ī��");
					break;

				case -13:
					Debug.Log("���� ��û ����");
					break;

				case -14:
					Debug.Log("��û ���� ������ ���� ����");
					break;

				case -15:
					Debug.Log("ī�帮�� PORT OPEN ����");
					break;

				case -16:
					Debug.Log("�����ŷ� ������� �Ұ� (����������ȣ ����)");
					break;

				case -17:
					Debug.Log("�ߺ� ��û �Ұ�");
					break;

				case -18:
					Debug.Log("�������� �ʴ� ī��");
					break;

				case -19:
					Debug.Log("����ICī�� �������� ������");
					break;

				case -20:
					Debug.Log("TIT ī�帮���� ����");
					break;

				case -21:
					Debug.Log("NVCAT ���� ������� ���� (�ش� ī�� ī��� Ȯ�� ���)");
					break;

				case -22:
					Debug.Log("���� IC ī�� (���̼�)");
					break;
			}

			b_Fail = true;
		}
	}

	//private void OnApplicationQuit()
 //   {
	//	socketConnection.Close();
	//}
}

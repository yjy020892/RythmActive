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

			payPanel.SetActive(true);
			payResult.text = "결제가 정상적으로 처리됐습니다";

			SoundManager.instance.PlayPayMoney();

			b_Success = false;

			StartCoroutine(PayResultMethod("Success"));
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
			payResult.text = string.Empty;
			payPanel.SetActive(false);

			IntroManager.instance.GotoScene();
		}
	}

    /// <summary>
    /// Setup socket connection.
    /// </summary>
    private void ConnectToTcpServer()
	{
		try
		{
			// 로컬 IPv4주소 가져오기
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

						//string[] data = string.Concat(Encoding.UTF8.GetString(incommingData).Where(x => !char.IsWhiteSpace(x))).Split(','); // 한글깨짐
						string[] data = string.Concat(euc.GetString(incommingData).Where(x => !char.IsWhiteSpace(x))).Split(','); // 공백제거해서 ,로 나누기


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
			//Debug.Log("정상 결제");
			
			//Debug.Log("null : " + num);
			//Debug.Log("result : " + result);

			if (!num.Equals("null") && result.Equals("정상승인"))
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
					Debug.Log("NVCAT 실행 중이 아님");
					break;

				case -2:
					Debug.Log("거래금액이 존재하지 않음");
					break;

				case -3:
					Debug.Log("환경정보 읽기 실패");
					break;

				case -4:
					Debug.Log("NVCAT 연동 오류 실패 (망상 취소 필요) VAN사에 문의요망");
					break;

				case -5:
					Debug.Log("기타 응답데이터 오류 (망상 취소 필요) VAN사에 문의요망");
					break;

				case -6:
					Debug.Log("결제 시간 초과");
					break;

				case -7:
					Debug.Log("사용자 및 리더기 요청 취소");
					break;

				case -8:
					Debug.Log("FALLBACK 거래 요청 필요");
					break;

				case -9:
					Debug.Log("기타 오류");
					break;

				case -10:
					Debug.Log("IC 우선 거래 요청 필요 (IC카드 MS리딩시)");
					break;

				case -11:
					Debug.Log("FALLBACK 거래 아님");
					break;

				case -12:
					Debug.Log("거래 불가 카드");
					break;

				case -13:
					Debug.Log("서명 요청 오류");
					break;

				case -14:
					Debug.Log("요청 전문 데이터 포멧 오류");
					break;

				case -15:
					Debug.Log("카드리더 PORT OPEN 오류");
					break;

				case -16:
					Debug.Log("직전거래 망상취소 불가 (전문관리번호 없음)");
					break;

				case -17:
					Debug.Log("중복 요청 불가");
					break;

				case -18:
					Debug.Log("지원되지 않는 카드");
					break;

				case -19:
					Debug.Log("현금IC카드 복수계좌 미지원");
					break;

				case -20:
					Debug.Log("TIT 카드리더기 오류");
					break;

				case -21:
					Debug.Log("NVCAT 내부 망상취소 실패 (해당 카드 카드사 확인 요망)");
					break;

				case -22:
					Debug.Log("현금 IC 카드 (다이소)");
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

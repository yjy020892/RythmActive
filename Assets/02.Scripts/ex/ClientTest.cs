using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ClientTest : MonoBehaviour
{
    //local IP
    public string serverIp = "172.30.1.7";
    Socket clientSocket = null;

    // Start is called before the first frame update
    void Start()
    {
        //클라이언트에서 사용할 소켓 준비
        this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //클라이언트는 바인딩할 필요 없음

        //접속할 서버의 통신지점(목적지)
        IPAddress serverIPAdress = IPAddress.Parse(this.serverIp);
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, 50001);

        //서버로 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            this.clientSocket.Connect(serverEndPoint);
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (this.clientSocket != null)
        {
            this.clientSocket.Close();
            this.clientSocket = null;
        }
    }

    public void Send(SimplePacket packet)
    {
        if (clientSocket == null)
        {
            return;
        }
        byte[] sendData = SimplePacket.ToByteArray(packet);
        byte[] prefSize = new byte[1];
        prefSize[0] = (byte)sendData.Length;    //버퍼의 가장 앞부분에 이 버퍼의 길이에 대한 정보가 있는데 이것을 
        clientSocket.Send(prefSize);    //먼저 보낸다.
        clientSocket.Send(sendData);

        Debug.Log("Send Packet from Client :" + packet.mouseX.ToString() + "/" + packet.mouseX.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        //마우스 왼쪽 클리할 때마다 패킷 클래스를 이용해서 위치정보를 서버에 전송.
        if (Input.GetMouseButtonDown(0) == true)
        {
            SimplePacket newPacket = new SimplePacket();
            newPacket.mouseX = Input.mousePosition.x;
            newPacket.mouseY = Input.mousePosition.y;
            Send(newPacket);
        }
    }
}

[Serializable]  //하나로 직렬화 묶겠다. 뜻? 바이트화 하겠다?
public class SimplePacket      //모노비헤이비어는 싱글톤으로 만들거라서 여기서는 삭제
{

    public float mouseX = 0.0f;
    public float mouseY = 0.0f;

    //쏘는거
    public static byte[] ToByteArray(SimplePacket packet)
    {
        //스트림생성 한다.  물흘려보내기
        MemoryStream stream = new MemoryStream();

        //스트림으로 건너온 패킷을 포맷으로 바이너리 묶어준다.
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, packet.mouseX);       //스트림에 담는다. 시리얼라이즈는 담는다는 뜻임.
        formatter.Serialize(stream, packet.mouseY);

        return stream.ToArray();
    }

    //받는거
    public static SimplePacket FromByteArray(byte[] input)
    {
        //스트림 생성
        MemoryStream stream = new MemoryStream(input);
        //스트림으로 데이터 받을 때 바이너리 포매터 말고 다른거도 있는지 찾아보기
        //바이너리 포매터로 스트림에 떠내려온 데이터를 건져낸다.
        BinaryFormatter formatter = new BinaryFormatter();
        //패킷을 생성해서      //패킷 생성기에 대해 알아보기!
        SimplePacket packet = new SimplePacket();
        //생성한 패킷에 디이터를 디시리얼 라이즈해서 담는다.
        packet.mouseX = (float)formatter.Deserialize(stream);
        packet.mouseY = (float)formatter.Deserialize(stream);

        return packet;
    }

}
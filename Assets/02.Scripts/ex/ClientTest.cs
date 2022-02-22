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
        //Ŭ���̾�Ʈ���� ����� ���� �غ�
        this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //Ŭ���̾�Ʈ�� ���ε��� �ʿ� ����

        //������ ������ �������(������)
        IPAddress serverIPAdress = IPAddress.Parse(this.serverIp);
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, 50001);

        //������ ���� ��û
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
        prefSize[0] = (byte)sendData.Length;    //������ ���� �պκп� �� ������ ���̿� ���� ������ �ִµ� �̰��� 
        clientSocket.Send(prefSize);    //���� ������.
        clientSocket.Send(sendData);

        Debug.Log("Send Packet from Client :" + packet.mouseX.ToString() + "/" + packet.mouseX.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        //���콺 ���� Ŭ���� ������ ��Ŷ Ŭ������ �̿��ؼ� ��ġ������ ������ ����.
        if (Input.GetMouseButtonDown(0) == true)
        {
            SimplePacket newPacket = new SimplePacket();
            newPacket.mouseX = Input.mousePosition.x;
            newPacket.mouseY = Input.mousePosition.y;
            Send(newPacket);
        }
    }
}

[Serializable]  //�ϳ��� ����ȭ ���ڴ�. ��? ����Ʈȭ �ϰڴ�?
public class SimplePacket      //�������̺��� �̱������� ����Ŷ� ���⼭�� ����
{

    public float mouseX = 0.0f;
    public float mouseY = 0.0f;

    //��°�
    public static byte[] ToByteArray(SimplePacket packet)
    {
        //��Ʈ������ �Ѵ�.  �����������
        MemoryStream stream = new MemoryStream();

        //��Ʈ������ �ǳʿ� ��Ŷ�� �������� ���̳ʸ� �����ش�.
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, packet.mouseX);       //��Ʈ���� ��´�. �ø��������� ��´ٴ� ����.
        formatter.Serialize(stream, packet.mouseY);

        return stream.ToArray();
    }

    //�޴°�
    public static SimplePacket FromByteArray(byte[] input)
    {
        //��Ʈ�� ����
        MemoryStream stream = new MemoryStream(input);
        //��Ʈ������ ������ ���� �� ���̳ʸ� ������ ���� �ٸ��ŵ� �ִ��� ã�ƺ���
        //���̳ʸ� �����ͷ� ��Ʈ���� �������� �����͸� ��������.
        BinaryFormatter formatter = new BinaryFormatter();
        //��Ŷ�� �����ؼ�      //��Ŷ �����⿡ ���� �˾ƺ���!
        SimplePacket packet = new SimplePacket();
        //������ ��Ŷ�� �����͸� ��ø��� �������ؼ� ��´�.
        packet.mouseX = (float)formatter.Deserialize(stream);
        packet.mouseY = (float)formatter.Deserialize(stream);

        return packet;
    }

}
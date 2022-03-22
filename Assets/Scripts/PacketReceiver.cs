using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketReceiver : MonoBehaviour
{
    [Serializable]
    public class HandData
    {
        public GameObject hand;
        public int posX;
        public int posY;
    }

    public string ipAddress;
    public int port;

    UdpClient client;
    IPEndPoint ipep;
    Thread receiverThread;

    [Header("Data Detail")]
    public bool isTracking;
    public string dataReceived;
    public string[] dataSplitted;

    [Header("Hand and Cursor Attribute")]
    public float maxX = 0.0140625f;
    public float maxY = -0.01388888888f;
    public float moveSpeed;
    public GameObject cursor;
    public GameObject handsGroup;
    [SerializeField] public HandData[] handsPosition;

    void Start()
    {
        if (isTracking) {
            client = new UdpClient(8000);
            client.Connect(ipAddress, port);

            //UDPSender();
            receiverThread = new Thread(UDPReceiver);
            receiverThread.Start();
        }
    }

    void Update()
    {
        if (isTracking)
        {
            cursor.SetActive(false);
            if (dataReceived == "")
            {
                handsGroup.SetActive(false);
            }
            else
            {
                HandTracking();
                handsGroup.SetActive(true);
                for (int i = 0; i < handsPosition.Length; i++)
                {
                    if (i + 1 <= Convert.ToInt32(dataSplitted[0]))
                    {
                        handsPosition[i].hand.SetActive(true);
                    }
                    else
                        handsPosition[i].hand.SetActive(false);
                }
            }
        }
        else
        {
            cursor.SetActive(true);
            handsGroup.SetActive(false);
            MouseTracking();
        }
    }

    public void HandTracking()
    {

    }

    public void MouseTracking()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        cursor.transform.position = mousePosition;
    }

    void UDPSender()
    {
        string sendData = "Hello, from the unity";
        byte[] sendBytes = Encoding.ASCII.GetBytes(sendData);
        client.Send(sendBytes, sendBytes.Length);
        print("Message Initiator Sent");
    }

    void UDPReceiver()
    {
        print("[Receiver Thread Opened]");
        while (isTracking)
        {
            ipep = new IPEndPoint(IPAddress.Any, 8080);
            byte[] receiveBytes = client.Receive(ref ipep);
            dataReceived = Encoding.ASCII.GetString(receiveBytes);
            
            if (dataReceived != "")
                dataSplitted = dataReceived.Split(',');
        }
    }
}

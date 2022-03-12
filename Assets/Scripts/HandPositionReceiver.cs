using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using UnityEngine;
using System.Threading;

public class HandPositionReceiver : MonoBehaviour
{
    [Serializable]
    public class HandData
    {
        public GameObject hand;
        public int posX;
        public int posY;
    }

    [Header("Controller Attribute")]
    public bool isTracking;
    public int totalHandDetected;
    public float maxX = 0.0140625f;
    public float maxY = -0.01388888888f;
    [SerializeField] public HandData[] handPosition;

    [Header("Receiver Attribute")]
    public int port;
    public string dataString;
    public string[] dataSplitted;

    byte[] receivedData;

    IPEndPoint ipep;
    IPEndPoint sender;
    UdpClient newSock;
    Thread networkThread;

    void Awake()
    {
        networkThread = new Thread(ReceiveData);
        networkThread.Start();

        receivedData = new byte[0];

        ipep = new IPEndPoint(IPAddress.Any, port);
        newSock = new UdpClient(ipep);
        sender = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update()
    {
        ReceiveData();
        totalHandDetected = Convert.ToInt32(dataSplitted[0]);

        if (isTracking)
            HandTracking();
        else
            MouseTracking();
    }

    public void ReceiveData()
    {
        if (newSock.Available > 0)
        {
            receivedData = newSock.Receive(ref sender);
            dataString = Encoding.ASCII.GetString(receivedData);
            dataSplitted = dataString.Split(',');
        }
        else
        {
            //print("No data to receive");
        }
    }

    public void HandTracking()
    {
        handPosition[0].hand.SetActive(false);
        handPosition[1].hand.SetActive(false);
        handPosition[2].hand.SetActive(false);
        handPosition[3].hand.SetActive(false);

        for (int i = 0; i < totalHandDetected; i++)
        {
            handPosition[i].hand.SetActive(true);
        }

        if (totalHandDetected == 1)
        {
            handPosition[0].posX = Convert.ToInt32(dataSplitted[1]);
            handPosition[0].posY = Convert.ToInt32(dataSplitted[2]);
            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
        }
        else if (totalHandDetected == 2)
        {
            handPosition[0].posX = Convert.ToInt32(dataSplitted[1]);
            handPosition[0].posY = Convert.ToInt32(dataSplitted[2]);
            handPosition[1].posX = Convert.ToInt32(dataSplitted[3]);
            handPosition[1].posY = Convert.ToInt32(dataSplitted[4]);

            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
        }
        else if (totalHandDetected == 3)
        {
            handPosition[0].posX = Convert.ToInt32(dataSplitted[1]);
            handPosition[0].posY = Convert.ToInt32(dataSplitted[2]);
            handPosition[1].posX = Convert.ToInt32(dataSplitted[3]);
            handPosition[1].posY = Convert.ToInt32(dataSplitted[4]);
            handPosition[2].posX = Convert.ToInt32(dataSplitted[5]);
            handPosition[2].posY = Convert.ToInt32(dataSplitted[6]);

            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);
            handPosition[2].hand.transform.position = new Vector2(handPosition[2].posX * maxX, handPosition[2].posY * maxY);
            handPosition[2].hand.transform.position = new Vector2(handPosition[2].posX * maxX, handPosition[2].posY * maxY);
        }
        else if (totalHandDetected == 4)
        {
            handPosition[0].posX = Convert.ToInt32(dataSplitted[1]);
            handPosition[0].posY = Convert.ToInt32(dataSplitted[2]);
            handPosition[1].posX = Convert.ToInt32(dataSplitted[3]);
            handPosition[1].posY = Convert.ToInt32(dataSplitted[4]);
            handPosition[2].posX = Convert.ToInt32(dataSplitted[5]);
            handPosition[2].posY = Convert.ToInt32(dataSplitted[6]);
            handPosition[3].posX = Convert.ToInt32(dataSplitted[7]);
            handPosition[3].posY = Convert.ToInt32(dataSplitted[8]);

            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[0].hand.transform.position = new Vector2(handPosition[0].posX * maxX, handPosition[0].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);
            handPosition[1].hand.transform.position = new Vector2(handPosition[1].posX * maxX, handPosition[1].posY * maxY);
            handPosition[2].hand.transform.position = new Vector2(handPosition[2].posX * maxX, handPosition[2].posY * maxY);
            handPosition[2].hand.transform.position = new Vector2(handPosition[2].posX * maxX, handPosition[2].posY * maxY);
            handPosition[3].hand.transform.position = new Vector2(handPosition[3].posX * maxX, handPosition[3].posY * maxY);
            handPosition[3].hand.transform.position = new Vector2(handPosition[3].posX * maxX, handPosition[3].posY * maxY);
        }
    }

    public void MouseTracking()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        transform.position = mousePosition;
    }
}
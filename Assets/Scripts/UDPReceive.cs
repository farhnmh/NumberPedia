using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;

    public int port = 5053;
    public string packetReceived;
    public GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<GameManager>();

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    public void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                if (gameManager.isTracking)
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] dataByte = client.Receive(ref anyIP);
                    packetReceived = Encoding.UTF8.GetString(dataByte);

                    if (packetReceived != "")
                    {
                        gameManager.dataSplitted = packetReceived.Split(',');
                        gameManager.totalHand = Convert.ToInt32(gameManager.dataSplitted[0]);
                    }
                    else
                        gameManager.totalHand = 0;
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}

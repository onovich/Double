using System;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class ServerApp : MonoBehaviour
{
    private Socket listenerSocket;
    private List<Socket> clientSockets = new List<Socket>();

    private void Start()
    {
        // Socket
        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // Bind
        IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8888);
        listenerSocket.Bind(iPEndPoint);
        // Listen
        listenerSocket.Listen(10);

        // Start listening thread
        Thread thread = new Thread(ListenThread);
        thread.Start();
    }

    private void ListenThread()
    {
        while (true)
        {
            // Accept
            Socket clientSocket = listenerSocket.Accept();
            clientSockets.Add(clientSocket);

            // Start client thread
            Thread clientThread = new Thread(() => HandleClientThread(clientSocket));
            clientThread.Start();
        }
    }

    private void HandleClientThread(Socket clientSocket)
    {
        byte[] readBuff = new byte[1024];
        while (true)
        {
            // Recv
            int count = clientSocket.Receive(readBuff);
            if (count == 0) // client disconnected
            {
                clientSockets.Remove(clientSocket);
                clientSocket.Close();
                break;
            }
            string recvStr = System.Text.Encoding.UTF8.GetString(readBuff, 0, count);
            Debug.Log(recvStr);

            // Send
            string sendStr = "Success!";
            byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendStr);
            clientSocket.Send(sendBytes);
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class ClientApp : MonoBehaviour
{
    // Socket
    Socket socket;

    // UGUI

    // Click Connect
    public void Connection()
    {
        string IP = "127.0.0.1";
        int port = 8888;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Security.PrefetchSocketPolicy(IP, port);

        socket.Connect(IP, port);
        if (socket.Connected)
        {
            Debug.Log("Connected to server");
        }
    }

    // Click Send
    public void Send()
    {
        // Send
        string sendStr = "Hello World!";
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendStr);
        socket.Send(sendBytes);
        // Recv
        byte[] readBuff = new byte[1024];
        int count = socket.Receive(readBuff);
        string recvStr = System.Text.Encoding.UTF8.GetString(readBuff, 0, count);
        Debug.Log(recvStr);
        // Close
        socket.Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Connection();
            Send();
        }
    }
    
    private void OnApplicationQuit()
    {
        if (socket != null && socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
    
}

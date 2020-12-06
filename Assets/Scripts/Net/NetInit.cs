using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

// 小心Socket内存泄漏
public class NetInit : MonoBehaviour
{
    void Start()
    {
        startConnect();
        StartCoroutine(Ping(0));
    }

    void Update()
    {

    }

    Socket socket;
    private void startConnect()
    {
        try
        {
            int _port = 8999;
            string _ip = "39.97.171.148";

            //创建客户端Socket，获得远程ip和端口号
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(_ip);
            IPEndPoint point = new IPEndPoint(ip, _port);

            socket.Connect(point);
            Debug.Log("连接成功!");
            //开启新的线程，不停的接收服务器发来的消息
            Thread c_thread = new Thread(Received);
            c_thread.IsBackground = true;
            c_thread.Start();
        }
        catch (System.Exception)
        {
            Debug.Log("IP或者端口号错误...，服务器程序未启动");
            socket.Close();
            socket.Dispose();
            return;
        }
    }

    /// <summary>
    /// 接收服务端返回的消息
    /// </summary>
    byte[] receiveBuf = new byte[1024];
    private void Received()
    {
        while (true)
        {
            try
            {
                //实际接收到的有效字节数
                int len = socket.Receive(receiveBuf);
                if (len == 0) break;
                Message msg = PackUtil.Unpack(receiveBuf);
                Debug.Log(msg);
            }
            catch
            {
                Debug.Log("接收数据失败");
                socket.Close();
                socket.Dispose();
                return;
            }
        }
    }

    /// <summary>
    /// 向服务器发送消息
    /// </summary>
    private void Send(Message msg)
    {
        byte[] sendBuf = PackUtil.Pack(msg);
        socket.Send(sendBuf);
    }

    private IEnumerator Ping(uint id)
    {
        yield return new WaitForSeconds(1);
        try
        {
            Send(new Message(id, new byte[] { 1, 2, 3 }));
        }
        catch
        {
            Debug.Log("数据发送失败");
            socket.Close();
            socket.Dispose();
            yield break;
        }
        if (id == 0) StartCoroutine(Ping(1));
        if (id == 1) StartCoroutine(Ping(0));
    }
}

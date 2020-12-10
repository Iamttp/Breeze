using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using UnityEngine;


[System.Serializable]
public class baseJson
{
}

[System.Serializable]
class json2 : baseJson
{
    public float X;
    public float Y;
    public override string ToString()
    {
        return X + " " + Y;
    }
}

[System.Serializable]
class json1 : baseJson
{
    public int Id;
    public override string ToString()
    {
        return Id.ToString();
    }
}

[System.Serializable]
class json3 : baseJson
{
    public float X;
    public float Y;
    public int Id;
    public override string ToString()
    {
        return Id + " " + X + " " + Y;
    }
}


// 小心Socket内存泄漏
public class NetUtil
{
    private static NetUtil instan = null;

    public Queue<Message> msgQ = new Queue<Message>(); // 队列 + 锁

    private NetUtil() { }

    public static NetUtil instance
    {
        get
        {
            if (instan == null)
            {
                instan = new NetUtil();
            }
            return instan;
        }
    }

    Socket socket;
    public bool startConnect()
    {
        try
        {
            int _port = 8999;
            string _ip = "39.97.171.148";
            //string _ip = "127.0.0.1";

            //创建客户端Socket，获得远程ip和端口号
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(_ip);
            IPEndPoint point = new IPEndPoint(ip, _port);

            socket.Connect(point);
            //开启新的线程，不停的接收服务器发来的消息
            Thread c_thread = new Thread(Received);
            c_thread.IsBackground = true;
            c_thread.Start();
            return true;
        }
        catch (System.Exception)
        {
            socket.Close();
            socket.Dispose();
        }
        return false;
    }

    /// <summary>
    /// 接收服务端返回的消息
    /// 
    /// TODO 在Editor运行时，当点击上方停止运行按钮，线程并不会直接退出
    /// 在打包程序运行时，程序退出，线程退出
    /// 总之，Thread中不要使用任何需要回到unity中执行的函数。
    /// 
    byte[] receiveBuf = new byte[1024];
    private void Received()
    {
        while (true)
        {
            try
            {
                // 实际接收到的有效字节数
                int len = socket.Receive(receiveBuf);
                if (len == 0) break;
                Message msg = null;
                int startIndex = 0;
                // 粘包问题解决
                do
                {
                    msg = PackUtil.Unpack(receiveBuf, startIndex);
                    lock (msgQ)
                    {
                        msgQ.Enqueue(msg);
                    }
                    startIndex += (int)msg.len + 8;
                }
                while (startIndex < len);
            }
            catch
            {
                // Debug.Log("接收数据失败");   
                socket.Close();
                socket.Dispose();
                return;
            }
        }
    }

    /// <summary>
    /// 向服务器发送消息
    /// </summary>
    public bool Send(Message msg)
    {
        try
        {
            byte[] sendBuf = PackUtil.Pack(msg);
            socket.Send(sendBuf);
        }
        catch
        {
            socket.Close();
            socket.Dispose();
            return false;
        }
        return true;
    }
}

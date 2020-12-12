using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// 为保证传输速度 float 类型由 string 类型替代。（暂定保留两位小数）
/// </summary>
[System.Serializable]
public class baseJson
{
}

[System.Serializable]
class json2 : baseJson
{
    public string X;
    public string Y;
}

[System.Serializable]
class json1 : baseJson
{
    public int Id;
}

[System.Serializable]
class json3 : baseJson
{
    public string X;
    public string Y;
    public State State;
    public string MoveVecX;
    public string MoveVecY;
    public int Id;
}


// 小心Socket内存泄漏
public class NetUtil
{
    private static NetUtil instan = null;

    public Queue<Message> msgQ = new Queue<Message>(); // 队列 + 锁
    const int MAX_COUNT = 10; // 设定消息队列的最大缓存数，防止有消息未收到

    // ImportMsgQ 不错过重要消息
    public Queue<Message> importMsgQ = new Queue<Message>();

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
            string _ip = Begin._ip;

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
    const int MAX_RECEIVE_SIZE = 1024 * 4;
    byte[] receiveBuf = new byte[MAX_RECEIVE_SIZE];
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

                UnityEngine.Debug.Log(len);
                //////////////////////// 一旦超过容量，读取buf直到为空
                bool flag = false;
                while (len == MAX_RECEIVE_SIZE)
                {
                    len = socket.Receive(receiveBuf);
                    flag = true;
                }
                if (flag) continue;
                //////////////////////

                // 粘包问题解决
                do
                {
                    msg = PackUtil.Unpack(receiveBuf, startIndex);
                    if (msg.id == 3)
                    {
                        lock (msgQ)
                        {
                            if (msgQ.Count == MAX_COUNT) msgQ.Dequeue();
                            msgQ.Enqueue(msg);
                        }
                    }
                    else // TODO 攻击状态应该为importMsgQ
                    {
                        lock (importMsgQ)
                        {
                            importMsgQ.Enqueue(msg);
                        }
                    }
                    startIndex += (int)msg.len + 8;
                }
                while (startIndex < len);
            }
            catch
            {
                socket.Close();
                socket.Dispose();
                break;
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
            return false;
        }
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;

public class Message
{
    public uint len;
    public uint id;
    public byte[] data;

    public Message(uint id, byte[] data)
    {
        this.id = id;
        this.data = data;
        len = (uint)data.Length;
    }

    public override string ToString()
    {
        return len.ToString() + ", " + id.ToString() + ", " + BitConverter.ToString(data);
    }
}

// TODO 性能优化
public class PackUtil
{
    public static Message Unpack(byte[] dataPack, int startIndex)
    {
        byte[] arr = new byte[4];
        Array.Copy(dataPack, startIndex, arr, 0, 4);
        uint len = BitConverter.ToUInt32(arr, 0);
        Array.Copy(dataPack, startIndex + 4, arr, 0, 4);
        uint id = BitConverter.ToUInt32(arr, 0);
        byte[] data = new byte[len];
        // len id 4 + 4 = 8 字节
        Array.Copy(dataPack, startIndex + 8, data, 0, len);
        return new Message(id, data);
    }

    public static byte[] Pack(Message msg)
    {
        //if (BitConverter.IsLittleEndian) UnityEngine.Debug.Log("小端");
        //else UnityEngine.Debug.Log("大端");

        var d1 = BitConverter.GetBytes(msg.len);
        var d2 = BitConverter.GetBytes(msg.id);
        var d3 = msg.data;

        var res = new byte[d1.Length + d2.Length + d3.Length];
        d1.CopyTo(res, 0);
        d2.CopyTo(res, d1.Length);
        d3.CopyTo(res, d1.Length + d2.Length);
        return res;
    }
}

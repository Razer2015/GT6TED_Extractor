using System;
using System.IO;

public class EndianBinReader : BinaryReader
{
    public EndianBinReader(Stream param_35) : base(param_35)
    {
    }

    public override double ReadDouble()
    {
        byte[] array = base.ReadBytes(8);
        Array.Reverse(array);
        return BitConverter.ToDouble(array, 0);
    }

    public override short ReadInt16()
    {
        byte[] array = base.ReadBytes(2);
        Array.Reverse(array);
        return BitConverter.ToInt16(array, 0);
    }

    public override int ReadInt32()
    {
        byte[] array = base.ReadBytes(4);
        Array.Reverse(array);
        return BitConverter.ToInt32(array, 0);
    }

    public override long ReadInt64()
    {
        byte[] array = base.ReadBytes(8);
        Array.Reverse(array);
        return BitConverter.ToInt64(array, 0);
    }

    public override float ReadSingle()
    {
        byte[] array = base.ReadBytes(4);
        Array.Reverse(array);
        return BitConverter.ToSingle(array, 0);
    }

    public override ushort ReadUInt16()
    {
        byte[] array = base.ReadBytes(2);
        Array.Reverse(array);
        return BitConverter.ToUInt16(array, 0);
    }

    public override uint ReadUInt32()
    {
        byte[] array = base.ReadBytes(4);
        Array.Reverse(array);
        return BitConverter.ToUInt32(array, 0);
    }

    public override ulong ReadUInt64()
    {
        byte[] array = base.ReadBytes(8);
        Array.Reverse(array);
        return BitConverter.ToUInt64(array, 0);
    }
}


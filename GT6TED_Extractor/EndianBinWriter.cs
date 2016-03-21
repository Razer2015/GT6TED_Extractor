using System;
using System.IO;

public class EndianBinWriter : BinaryWriter
{
    public EndianBinWriter(Stream param_26) : base(param_26)
    {
    }

    public override void Write(double param_34)
    {
        byte[] bytes = BitConverter.GetBytes(param_34);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(short param_27)
    {
        byte[] bytes = BitConverter.GetBytes(param_27);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(int param_29)
    {
        byte[] bytes = BitConverter.GetBytes(param_29);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(long param_31)
    {
        byte[] bytes = BitConverter.GetBytes(param_31);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(float param_33)
    {
        byte[] bytes = BitConverter.GetBytes(param_33);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(ushort param_28)
    {
        byte[] bytes = BitConverter.GetBytes(param_28);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(uint param_30)
    {
        byte[] bytes = BitConverter.GetBytes(param_30);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(ulong param_32)
    {
        byte[] bytes = BitConverter.GetBytes(param_32);
        Array.Reverse(bytes);
        base.Write(bytes);
    }
}


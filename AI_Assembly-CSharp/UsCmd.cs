// Decompiled with JetBrains decompiler
// Type: UsCmd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

public class UsCmd
{
  public const int STRIP_NAME_MAX_LEN = 64;
  public const int BUFFER_SIZE = 16384;
  private int _writeOffset;
  private int _readOffset;
  private byte[] _buffer;

  public UsCmd()
  {
    this._buffer = new byte[16384];
  }

  public UsCmd(byte[] given)
  {
    this._buffer = given;
  }

  public byte[] Buffer
  {
    get
    {
      return this._buffer;
    }
  }

  public int WrittenLen
  {
    get
    {
      return this._writeOffset;
    }
  }

  public object ReadPrimitive<T>()
  {
    if (typeof (T) == typeof (string))
      throw new UsCmdIOError(UsCmdIOErrorCode.TypeMismatched);
    if (this._readOffset + Marshal.SizeOf(typeof (T)) > this._buffer.Length)
      throw new UsCmdIOError(UsCmdIOErrorCode.ReadOverflow);
    object obj = UsGeneric.Convert<T>(this._buffer, this._readOffset);
    this._readOffset += Marshal.SizeOf(typeof (T));
    return obj;
  }

  public string ReadString()
  {
    short num = this.ReadInt16();
    if (num == (short) 0)
      return string.Empty;
    if (this._readOffset + (int) num > this._buffer.Length)
      throw new UsCmdIOError(UsCmdIOErrorCode.ReadOverflow);
    string str = Encoding.Default.GetString(this._buffer, this._readOffset, (int) num);
    this._readOffset += (int) num;
    return str;
  }

  public void WritePrimitive<T>(T value)
  {
    if (typeof (T) == typeof (string))
      throw new UsCmdIOError(UsCmdIOErrorCode.TypeMismatched);
    if (this._writeOffset + Marshal.SizeOf(typeof (T)) > this._buffer.Length)
      throw new UsCmdIOError(UsCmdIOErrorCode.WriteOverflow);
    byte[] numArray = UsGeneric.Convert<T>(value);
    if (numArray == null)
      throw new UsCmdIOError(UsCmdIOErrorCode.TypeMismatched);
    numArray.CopyTo((Array) this._buffer, this._writeOffset);
    this._writeOffset += Marshal.SizeOf(typeof (T));
  }

  public void WriteStringStripped(string value, short stripLen)
  {
    if (string.IsNullOrEmpty(value))
    {
      this.WritePrimitive<short>((short) 0);
    }
    else
    {
      byte[] bytes = Encoding.Default.GetBytes(value.Length <= (int) stripLen ? value : value.Substring(0, (int) stripLen));
      this.WritePrimitive<short>((short) bytes.Length);
      bytes.CopyTo((Array) this._buffer, this._writeOffset);
      this._writeOffset += bytes.Length;
    }
  }

  public eNetCmd ReadNetCmd()
  {
    return (eNetCmd) this.ReadInt16();
  }

  public short ReadInt16()
  {
    return (short) this.ReadPrimitive<short>();
  }

  public int ReadInt32()
  {
    return (int) this.ReadPrimitive<int>();
  }

  public float ReadFloat()
  {
    return (float) this.ReadPrimitive<float>();
  }

  public void WriteNetCmd(eNetCmd cmd)
  {
    this.WritePrimitive<short>((short) cmd);
  }

  public void WriteInt16(short value)
  {
    this.WritePrimitive<short>(value);
  }

  public void WriteInt32(int value)
  {
    this.WritePrimitive<int>(value);
  }

  public void WriteFloat(float value)
  {
    this.WritePrimitive<float>(value);
  }

  public void WriteStringStripped(string value)
  {
    this.WriteStringStripped(value, (short) 64);
  }

  public void WriteString(string value)
  {
    this.WriteStringStripped(value, short.MaxValue);
  }
}

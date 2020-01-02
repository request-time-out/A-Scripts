// Decompiled with JetBrains decompiler
// Type: BytesProvider`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

public class BytesProvider<T> : IBytesProvider<T>
{
  private Func<T, byte[]> _conversion;

  internal BytesProvider(Func<T, byte[]> conversion)
  {
    this._conversion = conversion;
  }

  public static BytesProvider<T> Default
  {
    get
    {
      return DefaultBytesProviders.GetDefaultProvider<T>();
    }
  }

  public byte[] GetBytes(T value)
  {
    return this._conversion(value);
  }
}

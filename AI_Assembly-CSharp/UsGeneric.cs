// Decompiled with JetBrains decompiler
// Type: UsGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class UsGeneric
{
  [DebuggerHidden]
  public static IEnumerable<List<T>> Slice<T>(List<T> objList, int slice)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    UsGeneric.\u003CSlice\u003Ec__Iterator0<T> sliceCIterator0 = new UsGeneric.\u003CSlice\u003Ec__Iterator0<T>()
    {
      objList = objList,
      slice = slice
    };
    // ISSUE: reference to a compiler-generated field
    sliceCIterator0.\u0024PC = -2;
    return (IEnumerable<List<T>>) sliceCIterator0;
  }

  public static byte[] Convert<T>(T value)
  {
    return BytesProvider<T>.Default.GetBytes(value);
  }

  public static object Convert<T>(byte[] buffer, int startIndex)
  {
    if (typeof (T) == typeof (int))
      return (object) BitConverter.ToInt32(buffer, startIndex);
    if (typeof (T) == typeof (short))
      return (object) BitConverter.ToInt16(buffer, startIndex);
    return typeof (T) == typeof (float) ? (object) BitConverter.ToSingle(buffer, startIndex) : (object) null;
  }
}

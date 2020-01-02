// Decompiled with JetBrains decompiler
// Type: UsCmdIOError
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UsCmdIOError : Exception
{
  private static Dictionary<UsCmdIOErrorCode, string> InfoLut = new Dictionary<UsCmdIOErrorCode, string>()
  {
    {
      UsCmdIOErrorCode.ReadOverflow,
      "Not enough space for reading."
    },
    {
      UsCmdIOErrorCode.WriteOverflow,
      "Not enough space for writing."
    },
    {
      UsCmdIOErrorCode.TypeMismatched,
      "Reading/writing a string as a primitive."
    }
  };
  public UsCmdIOErrorCode ErrorCode;

  public UsCmdIOError(UsCmdIOErrorCode code)
    : base("[UsCmdIOError] " + UsCmdIOError.InfoLut[code])
  {
    this.ErrorCode = code;
  }
}

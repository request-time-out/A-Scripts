﻿// Decompiled with JetBrains decompiler
// Type: ClientConsoleCmdHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ClientConsoleCmdHandler : Attribute
{
  public string Command;

  public ClientConsoleCmdHandler(string cmd)
  {
    this.Command = cmd;
  }
}

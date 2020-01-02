// Decompiled with JetBrains decompiler
// Type: UsvConsole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UsvConsole
{
  private Dictionary<string, UsvConsoleCmdHandler> _handlers = new Dictionary<string, UsvConsoleCmdHandler>();
  public static UsvConsole Instance;

  public UsvConsole()
  {
    UsvConsoleCmds.Instance = new UsvConsoleCmds();
    foreach (MethodInfo method in typeof (UsvConsoleCmds).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      foreach (object customAttribute in method.GetCustomAttributes(typeof (ConsoleHandler), false))
      {
        if (customAttribute is ConsoleHandler consoleHandler)
        {
          try
          {
            Delegate @delegate = Delegate.CreateDelegate(typeof (UsvConsoleCmdHandler), (object) UsvConsoleCmds.Instance, method);
            if ((object) @delegate != null)
              this._handlers[consoleHandler.Command.ToLower()] = (UsvConsoleCmdHandler) @delegate;
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
          }
        }
      }
    }
  }

  public bool ExecuteCommand(string fullcmd)
  {
    string[] args = fullcmd.Split((char[]) Array.Empty<char>());
    if (args.Length == 0)
    {
      Log.Info((object) "empty command received, ignored.", (object[]) Array.Empty<object>());
      return false;
    }
    UsvConsoleCmdHandler consoleCmdHandler;
    if (!this._handlers.TryGetValue(args[0].ToLower(), out consoleCmdHandler))
    {
      Log.Info((object) "unknown command ('{0}') received, ignored.", (object) fullcmd);
      return false;
    }
    if (consoleCmdHandler(args))
      return true;
    Log.Info((object) "executing command ('{0}') failed.", (object) fullcmd);
    return false;
  }
}

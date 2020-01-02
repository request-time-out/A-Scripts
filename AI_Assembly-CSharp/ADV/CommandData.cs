// Decompiled with JetBrains decompiler
// Type: ADV.CommandData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Elements.Reference;
using System;
using System.Collections.Generic;

namespace ADV
{
  public class CommandData : Pointer<object>
  {
    public CommandData(
      CommandData.Command command,
      string key,
      Func<object> get,
      Action<object> set = null)
      : base(get, set)
    {
      this.command = command;
      this.key = key;
    }

    public bool ReceiveADV(TextScenario scenario)
    {
      ValData valData;
      if (!this.isVar || !scenario.Vars.TryGetValue(this.key, out valData))
        return false;
      object o = valData?.o;
      if (o == null)
        return false;
      this.value = o;
      return true;
    }

    public bool isVar
    {
      get
      {
        switch (this.command)
        {
          case CommandData.Command.None:
          case CommandData.Command.Replace:
            return false;
          default:
            return true;
        }
      }
    }

    public static void CreateCommand(
      List<Program.Transfer> transfers,
      IReadOnlyCollection<CommandData> collection)
    {
      foreach (CommandData commandData in (IEnumerable<CommandData>) collection)
      {
        if (commandData.value != null)
        {
          switch (commandData.command)
          {
            case CommandData.Command.Replace:
              transfers.Add(Program.Transfer.Create(true, ADV.Command.Replace, commandData.key, (string) commandData.value));
              continue;
            case CommandData.Command.Int:
              transfers.Add(Program.Transfer.Create(true, ADV.Command.VAR, "int", commandData.key, commandData.value?.ToString()));
              continue;
            case CommandData.Command.String:
              transfers.Add(Program.Transfer.Create(true, ADV.Command.VAR, "string", commandData.key, commandData.value?.ToString()));
              continue;
            case CommandData.Command.BOOL:
              transfers.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", commandData.key, commandData.value?.ToString()));
              continue;
            case CommandData.Command.FLOAT:
              transfers.Add(Program.Transfer.Create(true, ADV.Command.VAR, "float", commandData.key, commandData.value?.ToString()));
              continue;
            default:
              continue;
          }
        }
      }
    }

    public static CommandData.Command Cast(object o)
    {
      switch (o)
      {
        case string _:
          return CommandData.Command.String;
        case int _:
          return CommandData.Command.Int;
        case bool _:
          return CommandData.Command.BOOL;
        case float _:
          return CommandData.Command.FLOAT;
        default:
          return CommandData.Command.None;
      }
    }

    public static CommandData.Command Cast(Type type)
    {
      if (type == typeof (string))
        return CommandData.Command.String;
      if (type == typeof (int))
        return CommandData.Command.Int;
      if (type == typeof (bool))
        return CommandData.Command.BOOL;
      return type == typeof (float) ? CommandData.Command.FLOAT : CommandData.Command.None;
    }

    public CommandData.Command command { get; private set; }

    public string key { get; private set; }

    public enum Command
    {
      None,
      Replace,
      Int,
      String,
      BOOL,
      FLOAT,
    }
  }
}

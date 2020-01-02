// Decompiled with JetBrains decompiler
// Type: ADV.ScenarioData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public class ScenarioData : ScriptableObject
  {
    [SerializeField]
    public List<ScenarioData.Param> list;

    public ScenarioData()
    {
      base.\u002Ector();
    }

    private static bool MultiForce(Command command)
    {
      switch (command)
      {
        case Command.VAR:
        case Command.RandomVar:
        case Command.Calc:
        case Command.Format:
        case Command.Voice:
        case Command.Motion:
        case Command.Expression:
        case Command.ExpressionIcon:
        case Command.FormatVAR:
        case Command.CharaKaraokePlay:
          return true;
        default:
          return false;
      }
    }

    [Serializable]
    public class Param
    {
      [SerializeField]
      private int _hash;
      [SerializeField]
      private int _version;
      [SerializeField]
      private bool _multi;
      [SerializeField]
      private Command _command;
      [SerializeField]
      private string[] _args;

      public Param(bool multi, Command command, params string[] args)
      {
        this._multi = multi;
        this._command = command;
        this._args = args;
      }

      public Param(params string[] args)
      {
        this.Initialize(args);
      }

      public int Hash
      {
        get
        {
          return this._hash;
        }
      }

      public int Version
      {
        get
        {
          return this._version;
        }
      }

      public bool Multi
      {
        get
        {
          return this._multi;
        }
      }

      public Command Command
      {
        get
        {
          return this._command;
        }
      }

      public string[] Args
      {
        get
        {
          return this._args;
        }
      }

      public void SetHash(int hash)
      {
        this._hash = hash;
      }

      public IEnumerable<string> Output()
      {
        return ((IEnumerable<string>) new string[4]
        {
          this._hash.ToString(),
          this._version.ToString(),
          this._multi.ToString(),
          this._command.ToString()
        }).Concat<string>((IEnumerable<string>) this._args);
      }

      private void Initialize(params string[] args)
      {
        int num1 = 1;
        string[] strArray = args;
        int index1 = num1;
        int num2 = index1 + 1;
        bool flag = bool.TryParse(strArray[index1], out this._multi);
        string[] array = args;
        int index2 = num2;
        int count = index2 + 1;
        string self = array.SafeGet<string>(index2);
        try
        {
          this._command = (Command) Enum.ToObject(typeof (Command), self.Check(true, Enum.GetNames(typeof (Command))));
        }
        catch (Exception ex)
        {
          throw new Exception("CommandError:" + string.Join(",", ((IEnumerable<string>) args).Select<string, string>((Func<string, string>) (s => s.IsNullOrEmpty() ? "(null)" : s)).ToArray<string>()));
        }
        if (!flag)
          this._multi |= ScenarioData.MultiForce(this._command);
        this._args = ScenarioData.Param.ConvertAnalyze(this._command, ((IEnumerable<string>) args).Skip<string>(count).ToArray<string>().LastStringEmptySpaceRemove(), (string) null);
      }

      private static string[] ConvertAnalyze(Command command, string[] args, string fileName)
      {
        CommandBase commandBase = CommandList.CommandGet(command);
        if (commandBase != null)
          commandBase.Convert(fileName, ref args);
        else
          Debug.LogError((object) ("commandBase none : " + (object) command));
        return args.LastStringEmptySpaceRemove();
      }
    }
  }
}

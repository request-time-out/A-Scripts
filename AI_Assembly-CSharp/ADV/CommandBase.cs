// Decompiled with JetBrains decompiler
// Type: ADV.CommandBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public abstract class CommandBase : ICommand
  {
    public string[] args;
    public const int currentCharaDefaultIndex = 2147483647;

    public void Set(Command command)
    {
      this.command = command;
    }

    public static string[] RemoveArgsEmpty(string[] args)
    {
      return args == null ? (string[]) null : ((IEnumerable<string>) args).Where<string>((Func<string, bool>) (s => !s.IsNullOrEmpty())).ToArray<string>();
    }

    public string[] RemoveArgsEmpty()
    {
      return CommandBase.RemoveArgsEmpty(this.args);
    }

    public static string[] GetArgToSplit(int cnt, params string[] args)
    {
      string[] ret = (string[]) null;
      args.SafeProc(cnt, (Action<string>) (s => ret = s.Split(',')));
      return ret;
    }

    public string[] GetArgToSplit(int cnt)
    {
      return CommandBase.GetArgToSplit(cnt, this.args);
    }

    public static string[] GetArgToSplitLast(int cnt, params string[] args)
    {
      List<string> stringList = new List<string>();
      while (true)
      {
        string[] argToSplit = CommandBase.GetArgToSplit(cnt++, args);
        if (!((IList<string>) argToSplit).IsNullOrEmpty<string>())
          stringList.AddRange((IEnumerable<string>) argToSplit);
        else
          break;
      }
      return stringList.ToArray();
    }

    public string[] GetArgToSplitLast(int cnt)
    {
      return CommandBase.GetArgToSplitLast(cnt, this.args);
    }

    public static string[][] GetArgToSplitLastTable(int cnt, params string[] args)
    {
      List<string[]> strArrayList = new List<string[]>();
      while (true)
      {
        string[] argToSplit = CommandBase.GetArgToSplit(cnt++, args);
        if (!((IList<string>) argToSplit).IsNullOrEmpty<string>())
          strArrayList.Add(argToSplit);
        else
          break;
      }
      return strArrayList.ToArray();
    }

    public string[][] GetArgToSplitLastTable(int cnt)
    {
      return CommandBase.GetArgToSplitLastTable(cnt, this.args);
    }

    public int localLine { get; set; }

    public TextScenario scenario { get; private set; }

    public Command command { get; private set; }

    public void Initialize(TextScenario scenario, Command command, string[] args)
    {
      this.scenario = scenario;
      this.command = command;
      string[] argsDefault = this.ArgsDefault;
      if (argsDefault != null)
      {
        int length1 = argsDefault.Length;
        int length2 = args.Length;
        int num = Mathf.Min(length1, length2);
        for (int index = 0; index < num; ++index)
        {
          if (!args[index].IsNullOrEmpty())
            argsDefault[index] = args[index];
        }
        List<string> stringList = new List<string>((IEnumerable<string>) argsDefault);
        for (int count = stringList.Count; count < length2; ++count)
          stringList.Add(args[count]);
        this.args = stringList.ToArray();
      }
      else
        this.args = ((IEnumerable<string>) args).ToArray<string>();
    }

    public virtual void Convert(string fileName, ref string[] args)
    {
    }

    public virtual void ConvertBeforeArgsProc()
    {
    }

    public abstract string[] ArgsLabel { get; }

    public abstract string[] ArgsDefault { get; }

    protected static void CountAddV3(string[] args, ref int cnt, ref Vector3 v)
    {
      if (args == null)
        return;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        string[] array = args;
        int num;
        cnt = (num = cnt) + 1;
        int index2 = num;
        float result;
        if (float.TryParse(array.SafeGet<string>(index2), out result))
          ((Vector3) ref v).set_Item(index1, result);
      }
    }

    protected static void CountAddV3(ref int cnt)
    {
      cnt += 3;
    }

    protected static Vector3 LerpV3(Vector3 start, Vector3 end, float t)
    {
      return Vector3.Lerp(start, end, t);
    }

    protected static Vector3 LerpAngleV3(Vector3 start, Vector3 end, float t)
    {
      Vector3 zero = Vector3.get_zero();
      for (int index = 0; index < 3; ++index)
        ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(((Vector3) ref start).get_Item(index), ((Vector3) ref end).get_Item(index), t));
      return zero;
    }

    public virtual void Do()
    {
    }

    public virtual bool Process()
    {
      return true;
    }

    public virtual void Result(bool processEnd)
    {
    }

    [Conditional("ADV_DEBUG")]
    protected void ErrorCheckLog(bool isError, string message)
    {
      if (!isError)
        return;
      Debug.LogError((object) message);
    }

    [Conditional("ADV_DEBUG")]
    private void dbPrint(string procName, string[] command)
    {
    }

    [Conditional("__DEBUG_PROC__")]
    private void dbPrintDebug(string procName, string[] command)
    {
      Debug.LogFormat("{0}[{1}]{2}", new object[3]
      {
        (object) procName,
        (object) this.GetType(),
        (object) string.Join(", ", command)
      });
    }
  }
}

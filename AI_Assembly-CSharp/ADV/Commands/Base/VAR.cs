// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.VAR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class VAR : CommandBase
  {
    private System.Type type;
    private string name;
    private string value;
    private int refCnt;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Type", "Variable", "Value" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          "int",
          string.Empty,
          string.Empty
        };
      }
    }

    public static int RefCheck(ref string variable)
    {
      int num = 0;
      while (!variable.IsNullOrEmpty() && variable[0] == '*')
      {
        ++num;
        variable = variable.Remove(0, 1);
      }
      return num;
    }

    public static string RefGet(
      System.Type type,
      int refCount,
      string variable,
      Dictionary<string, ValData> Vars)
    {
      ValData valData = (ValData) null;
      if (refCount-- > 0)
        valData = new ValData(Vars[variable].o);
      while (refCount-- > 0)
        valData = Vars[valData.o.ToString()];
      return valData == null ? (string) null : ValData.Cast(valData.o, type).ToString();
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.type = System.Type.GetType(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.name = args2[index2];
      string[] args3 = this.args;
      int count = num3;
      int num4 = count + 1;
      this.value = ((IEnumerable<string>) args3).Skip<string>(count).Shuffle<string>().FirstOrDefault<string>();
      this.refCnt = VAR.RefCheck(ref this.value);
    }

    public override void Do()
    {
      base.Do();
      Dictionary<string, ValData> vars = this.scenario.Vars;
      VAR.RefGet(this.type, this.refCnt, this.value, vars).SafeProc<string>((Action<string>) (s => this.value = s));
      vars[this.name] = new ValData(ValData.Cast((object) this.value, this.type));
    }

    public override void Convert(string fileName, ref string[] args)
    {
      string str = args[0];
      if (str != null)
      {
        System.Type type;
        if (!(str == "int"))
        {
          if (!(str == "float"))
          {
            if (!(str == "string"))
            {
              if (str == "bool")
                type = typeof (bool);
              else
                goto label_9;
            }
            else
              type = typeof (string);
          }
          else
            type = typeof (float);
        }
        else
          type = typeof (int);
        args[0] = type.ToString();
        return;
      }
label_9:
      object obj = VAR.CheckLiterals((object) args[1]);
      args = new string[3]
      {
        obj.GetType().ToString(),
        args[0],
        obj.ToString()
      };
    }

    public static object CheckLiterals(object o)
    {
      string str1 = o.ToString();
      if (str1.Check(true, bool.TrueString, bool.FalseString) != -1)
        return (object) bool.Parse(str1);
      int num1;
      if (VAR.CheckFirst(str1, "0x", out num1) == 0)
        return (object) Convert.ToInt32(VAR.Convert(str1, num1 + 1), 16);
      if (VAR.CheckFirst(str1, "0b", out num1) == 0)
        return (object) Convert.ToInt32(VAR.Convert(str1, num1 + 1), 2);
      if (VAR.CheckFirst(str1, "0o", out num1) == 0)
        return (object) Convert.ToInt32(VAR.Convert(str1, num1 + 1), 8);
      if (VAR.CheckFirst(str1, ".", out num1) != -1)
      {
        int length = str1.Length;
        bool flag = false;
        int n;
        if (VAR.CheckLast(str1, "d", out n) != -1)
        {
          --length;
          flag = true;
        }
        else if (VAR.CheckLast(str1, "f", out n) != -1)
          --length;
        if (VAR.CheckLast(str1, "e", out num1) != -1)
        {
          string s = VAR.Convert(str1, num1);
          if (length != str1.Length)
            s = s.Substring(0, s.Length - 1);
          float num2 = Mathf.Pow(10f, (float) int.Parse(s));
          return flag ? (object) (double.Parse(str1.Substring(0, num1)) * (double) num2) : (object) (float) ((double) float.Parse(str1.Substring(0, num1)) * (double) num2);
        }
        return flag ? (object) double.Parse(length != str1.Length ? str1.Substring(0, n) : str1) : (object) float.Parse(length != str1.Length ? str1.Substring(0, n) : str1);
      }
      if (VAR.CheckLast(str1, "d", out num1) != -1)
      {
        int num2 = str1.Length - 1;
        string s1 = str1.Substring(0, num1);
        if (VAR.CheckLast(str1, "e", out num1) == -1)
          return (object) double.Parse(s1);
        string str2 = VAR.Convert(str1, num1);
        string s2 = str2.Substring(0, str2.Length - 1);
        return (object) (double.Parse(str1.Substring(0, num1)) * (double) Mathf.Pow(10f, (float) int.Parse(s2)));
      }
      if (VAR.CheckLast(str1, "f", out num1) != -1)
      {
        int num2 = str1.Length - 1;
        string s1 = str1.Substring(0, num1);
        if (VAR.CheckLast(str1, "e", out num1) == -1)
          return (object) float.Parse(s1);
        string str2 = VAR.Convert(str1, num1);
        string s2 = str2.Substring(0, str2.Length - 1);
        return (object) (float) ((double) float.Parse(str1.Substring(0, num1)) * (double) Mathf.Pow(10f, (float) int.Parse(s2)));
      }
      if (VAR.CheckLastLiterals(str1, "ul", out num1))
        return (object) ulong.Parse(str1.Substring(0, num1));
      if (VAR.CheckLastLiterals(str1, "l", out num1))
        return (object) long.Parse(str1.Substring(0, num1));
      if (VAR.CheckLastLiterals(str1, "u", out num1))
        return (object) uint.Parse(str1.Substring(0, num1));
      if (VAR.CheckLastLiterals(str1, "m", out num1))
        return (object) Decimal.Parse(str1.Substring(0, num1));
      if (VAR.CheckLast(str1, "e", out num1) != -1)
        return (object) (float) ((double) float.Parse(str1.Substring(0, num1)) * (double) Mathf.Pow(10f, (float) int.Parse(VAR.Convert(str1, num1))));
      if (str1[0] == '"' && str1[str1.Length - 1] == '"')
        return (object) str1.Remove(str1.Length - 1, 1).Remove(0, 1);
      return int.TryParse(str1, out num1) ? (object) num1 : o;
    }

    private static string Convert(string s, int n)
    {
      int startIndex = n + 1;
      return s.Substring(startIndex, s.Length - startIndex);
    }

    private static int CheckFirst(string s, string c, out int n)
    {
      return n = s.ToLower().IndexOf(c);
    }

    private static int CheckLast(string s, string c, out int n)
    {
      return n = s.ToLower().LastIndexOf(c);
    }

    private static bool CheckLastLiterals(string s, string c, out int n)
    {
      return VAR.CheckLast(s, c, out n) != -1 && s.Substring(n, s.Length - n).ToLower() == c;
    }
  }
}

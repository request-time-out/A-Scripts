// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Calc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Calc : CommandBase
  {
    private List<string> argsList = new List<string>();
    private int refCnt;
    private string answer;
    private string arg1;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Answer", "Formula", "Value" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          string.Empty,
          string.Empty,
          "0"
        };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.answer = args1[index1];
      this.refCnt = VAR.RefCheck(ref this.answer);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.arg1 = args2[index2];
      while (num3 < this.args.Length)
        this.argsList.Add(this.args[num3++]);
    }

    public static string RefGet(int refCount, string variable, Dictionary<string, ValData> Vars)
    {
      ValData valData = (ValData) null;
      if (refCount-- > 0)
        valData = new ValData(Vars[variable].o);
      while (refCount-- > 0)
        valData = Vars[valData.o.ToString()];
      return valData?.o.ToString();
    }

    public override void Do()
    {
      base.Do();
      Dictionary<string, ValData> Vars = this.scenario.Vars;
      Calc.RefGet(this.refCnt, this.answer, Vars).SafeProc<string>((Action<string>) (s => this.answer = s));
      ValData answerVal;
      if (!Vars.TryGetValue(this.answer, out answerVal))
      {
        string args = this.argsList[0];
        ValData valData;
        if (Vars.TryGetValue(args, out valData))
          args = valData.o.ToString();
        answerVal = !int.TryParse(args, out int _) ? (!float.TryParse(args, out float _) ? (!bool.TryParse(args, out bool _) ? new ValData((object) string.Empty) : new ValData((object) false)) : new ValData((object) 0.0f)) : new ValData((object) 0);
      }
      ValData valData1;
      Func<string, ValData> func1 = (Func<string, ValData>) (s => new ValData(ValData.Cast(!Vars.TryGetValue(s, out valData1) ? (object) s : valData1.o, answerVal.o.GetType())));
      int num1 = 0;
      Func<string, ValData> func2 = func1;
      List<string> argsList1 = this.argsList;
      int index1 = num1;
      int num2 = index1 + 1;
      string str1 = argsList1[index1];
      ValData b1 = func2(str1);
      while (num2 < this.argsList.Count)
      {
        ValData a = b1;
        List<string> argsList2 = this.argsList;
        int index2 = num2;
        int num3 = index2 + 1;
        int num4 = (int) Calc.Formula2to1((Calc.Formula2) int.Parse(argsList2[index2]));
        Func<string, ValData> func3 = func1;
        List<string> argsList3 = this.argsList;
        int index3 = num3;
        num2 = index3 + 1;
        string str2 = argsList3[index3];
        ValData b2 = func3(str2);
        b1 = Calc.Calculate(a, (Calc.Formula1) num4, b2);
      }
      answerVal = Calc.Calculate(answerVal, (Calc.Formula1) int.Parse(this.arg1), b1);
      Vars[this.answer] = answerVal;
    }

    private static Calc.Formula1 Formula2to1(Calc.Formula2 f2)
    {
      switch (f2)
      {
        case Calc.Formula2.Plus:
          return Calc.Formula1.PlusEqual;
        case Calc.Formula2.Minus:
          return Calc.Formula1.MinusEqual;
        case Calc.Formula2.Asta:
          return Calc.Formula1.AstaEqual;
        case Calc.Formula2.Slash:
          return Calc.Formula1.SlashEqual;
        default:
          return Calc.Formula1.Equal;
      }
    }

    private static ValData Calculate(ValData a, Calc.Formula1 f1, ValData b)
    {
      ValData valData = a;
      switch (f1)
      {
        case Calc.Formula1.Equal:
          valData = b;
          break;
        case Calc.Formula1.PlusEqual:
          valData += b;
          break;
        case Calc.Formula1.MinusEqual:
          valData -= b;
          break;
        case Calc.Formula1.AstaEqual:
          valData *= b;
          break;
        case Calc.Formula1.SlashEqual:
          valData /= b;
          break;
      }
      return valData;
    }

    [Conditional("ADV_DEBUG")]
    private void DBTEST(ValData answerVal)
    {
      Dictionary<string, ValData> Vars = this.scenario.Vars;
      ValData dbOutputVal = answerVal;
      List<string> stringList1 = new List<string>();
      ValData valData1;
      Func<string, ValData> func1 = (Func<string, ValData>) (s => Vars.TryGetValue(s, out valData1) ? new ValData(valData1.o) : new ValData(dbOutputVal.Convert((object) s)));
      int num1 = 0;
      Func<string, ValData> func2 = func1;
      List<string> argsList1 = this.argsList;
      int index1 = num1;
      int num2 = index1 + 1;
      string str1 = argsList1[index1];
      ValData valData2 = func2(str1);
      stringList1.Add(valData2.o.ToString());
      while (num2 < this.argsList.Count)
      {
        List<string> argsList2 = this.argsList;
        int index2 = num2;
        int num3 = index2 + 1;
        Calc.Formula2 formula2 = (Calc.Formula2) int.Parse(argsList2[index2]);
        Func<string, ValData> func3 = func1;
        List<string> argsList3 = this.argsList;
        int index3 = num3;
        num2 = index3 + 1;
        string str2 = argsList3[index3];
        ValData b = func3(str2);
        stringList1.Add(Calc.Cast(formula2));
        stringList1.Add(b.o.ToString());
        valData2 = Calc.Calculate(valData2, Calc.Formula2to1(formula2), b);
      }
      dbOutputVal = Calc.Calculate(dbOutputVal, (Calc.Formula1) int.Parse(this.arg1), valData2);
      Calc.Formula1 formula = (Calc.Formula1) int.Parse(this.arg1);
      int num4 = 0;
      if (formula != Calc.Formula1.Equal)
        stringList1.Insert(num4++, answerVal.o.ToString());
      List<string> stringList2 = stringList1;
      int index4 = num4;
      int num5 = index4 + 1;
      string str3 = Calc.Cast(formula);
      stringList2.Insert(index4, str3);
      Debug.LogFormat("answer:{0}\n{1}", new object[2]
      {
        dbOutputVal.o,
        (object) string.Join(" ", stringList1.ToArray())
      });
    }

    public static string Cast(Calc.Formula1 formula)
    {
      switch (formula)
      {
        case Calc.Formula1.Equal:
          return "=";
        case Calc.Formula1.PlusEqual:
          return "+=";
        case Calc.Formula1.MinusEqual:
          return "-=";
        case Calc.Formula1.AstaEqual:
          return "*=";
        case Calc.Formula1.SlashEqual:
          return "/=";
        default:
          return string.Empty;
      }
    }

    public static string Cast(Calc.Formula2 formula)
    {
      switch (formula)
      {
        case Calc.Formula2.Plus:
          return "+";
        case Calc.Formula2.Minus:
          return "-";
        case Calc.Formula2.Asta:
          return "*";
        case Calc.Formula2.Slash:
          return "/";
        default:
          return string.Empty;
      }
    }

    public override void Convert(string fileName, ref string[] args)
    {
      int cnt = 0;
      StringBuilder stringBuilder = new StringBuilder();
      string[] args1 = args;
      int num;
      cnt = (num = cnt) + 1;
      int index = num;
      if (args1.IsNullOrEmpty(index))
        stringBuilder.AppendLine("Answer none");
      if (!Calc.Formula1Cast(ref args[cnt]))
        stringBuilder.AppendLine("Formula1 Cast Error");
      Action action = (Action) (() => cnt += 2);
      action();
      while (!args.IsNullOrEmpty(cnt))
      {
        if (!Calc.Formula2Cast(ref args[cnt]))
          stringBuilder.AppendLine("Formula2 Cast Error");
        action();
        if (args.IsNullOrEmpty(cnt - 1))
          stringBuilder.AppendLine("Formula2 Value none");
      }
      if (stringBuilder.Length <= 0)
        return;
      Debug.LogErrorFormat("FileName:{0},CALC\n{1}", new object[2]
      {
        (object) fileName,
        (object) stringBuilder
      });
    }

    private static bool Formula1Cast(ref string arg)
    {
      string temp = arg;
      int num = Illusion.Utils.Value.Check(Illusion.Utils.Enum<Calc.Formula1>.Length, (Func<int, bool>) (index => temp == Calc.Cast((Calc.Formula1) index)));
      bool flag = true;
      if (num == -1)
      {
        num = 0;
        flag = false;
      }
      arg = num.ToString();
      return flag;
    }

    private static bool Formula2Cast(ref string arg)
    {
      string temp = arg;
      int num = Illusion.Utils.Value.Check(Illusion.Utils.Enum<Calc.Formula2>.Length, (Func<int, bool>) (index => temp == Calc.Cast((Calc.Formula2) index)));
      bool flag = true;
      if (num == -1)
      {
        num = 0;
        flag = false;
      }
      arg = num.ToString();
      return flag;
    }

    public enum Formula1
    {
      Equal,
      PlusEqual,
      MinusEqual,
      AstaEqual,
      SlashEqual,
    }

    public enum Formula2
    {
      Plus,
      Minus,
      Asta,
      Slash,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.IF
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class IF : CommandBase
  {
    private const string compStr = "check";
    private string left;
    private string center;
    private string right;
    private string jumpTrue;
    private string jumpFalse;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Left",
          "Center",
          "Right",
          "True",
          "False"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          "a",
          string.Empty,
          "b",
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Convert(string fileName, ref string[] args)
    {
      if (IF.Cast(ref args[1]))
        return;
      Debug.LogErrorFormat("FileName:{0},IF:Formula Cast Error:{1}\nArgs:{2}", new object[3]
      {
        (object) fileName,
        (object) args[1],
        (object) string.Join(",", args)
      });
    }

    private static bool Cast(ref string arg)
    {
      int num = Illusion.Utils.Comparer.STR.Check<string>(arg);
      bool flag = true;
      if (num == -1)
      {
        if (arg.Compare("check", true))
        {
          num = Illusion.Utils.Enum<Illusion.Utils.Comparer.Type>.Length;
        }
        else
        {
          num = 0;
          flag = false;
        }
      }
      arg = num.ToString();
      return flag;
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.left = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.center = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.right = args3[index3];
    }

    public override void Do()
    {
      base.Do();
      ValData valData1 = (ValData) null;
      ValData valData2 = (ValData) null;
      int num = int.Parse(this.center);
      if (num < Illusion.Utils.Enum<Illusion.Utils.Comparer.Type>.Length)
      {
        if (!this.scenario.Vars.TryGetValue(this.left, out valData1))
          valData1 = new ValData(VAR.CheckLiterals((object) this.left));
        if (!this.scenario.Vars.TryGetValue(this.right, out valData2))
          valData2 = new ValData(valData1.Convert((object) this.right));
      }
      bool flag;
      switch (num)
      {
        case 0:
          flag = valData1.o.Equals(valData2.o);
          break;
        case 1:
          flag = !valData1.o.Equals(valData2.o);
          break;
        case 2:
          flag = valData1 >= valData2;
          break;
        case 3:
          flag = valData1 <= valData2;
          break;
        case 4:
          flag = valData1 > valData2;
          break;
        case 5:
          flag = valData1 < valData2;
          break;
        default:
          flag = this.scenario.Vars.ContainsKey(this.left);
          break;
      }
      this.jumpTrue = this.args[3];
      this.jumpFalse = this.args[4];
      string str = !flag ? this.jumpFalse : this.jumpTrue;
      if (str.IsNullOrEmpty())
        Debug.LogWarning((object) string.Format("IF[tag:Empty]{0}:{1}", (object) "answer", (object) flag));
      else
        this.scenario.SearchTagJumpOrOpenFile(str, this.localLine);
    }
  }
}

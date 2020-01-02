// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.FontColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class FontColor : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Color", "Target" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "white", string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string self = args1[index1];
      string target = string.Empty;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      Action<string> act = (Action<string>) (s => target = s);
      args2.SafeProc(index2, act);
      Color? colorCheck = self.GetColorCheck();
      if (colorCheck.HasValue)
      {
        this.scenario.commandController.fontColor.Set(target, colorCheck.Value);
      }
      else
      {
        int configIndex;
        switch (self.ToLower())
        {
          case "color0":
            configIndex = 0;
            break;
          case "color1":
          case "color2":
            configIndex = 1;
            break;
          default:
            Debug.LogError((object) ("指定の色がない：" + self));
            return;
        }
        this.scenario.commandController.fontColor.Set(target, configIndex);
      }
    }
  }
}

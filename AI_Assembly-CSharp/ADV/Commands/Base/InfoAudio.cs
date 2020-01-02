// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.InfoAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Base
{
  public class InfoAudio : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "is2D", "isMoveMouth" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      ADV.Info.Audio audio = this.scenario.info.audio;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      Action<string> act1 = (Action<string>) (s => audio.is2D = bool.Parse(s));
      args1.SafeProc(index1, act1);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      Action<string> act2 = (Action<string>) (s => audio.isNotMoveMouth = bool.Parse(s));
      args2.SafeProc(index2, act2);
    }
  }
}

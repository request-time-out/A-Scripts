// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.InfoAnimePlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace ADV.Commands.Base
{
  public class InfoAnimePlay : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "crossFadeTime",
          "isCrossFade",
          "layerNo",
          "transitionDuration",
          "normalizedTime"
        };
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
      ADV.Info.Anime.Play play = this.scenario.info.anime.play;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      Action<string> act1 = (Action<string>) (s => play.crossFadeTime = float.Parse(s));
      args1.SafeProc(index1, act1);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      Action<string> act2 = (Action<string>) (s => play.isCrossFade = bool.Parse(s));
      args2.SafeProc(index2, act2);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      Action<string> act3 = (Action<string>) (s => play.layerNo = int.Parse(s));
      args3.SafeProc(index3, act3);
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      Action<string> act4 = (Action<string>) (s => play.transitionDuration = float.Parse(s));
      args4.SafeProc(index4, act4);
      string[] args5 = this.args;
      int index5 = num5;
      int num6 = index5 + 1;
      Action<string> act5 = (Action<string>) (s => play.normalizedTime = float.Parse(s));
      args5.SafeProc(index5, act5);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.Fade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Effect
{
  public class Fade : CommandBase
  {
    private Color color = Color.get_clear();
    private bool fadeIn = true;
    private bool isFront = true;
    private float time;
    private float timer;
    private bool isInitColorSet;
    private ADVFade advFade;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          nameof (Fade),
          "Time",
          "Color",
          "Type",
          "isInit"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          "in",
          "0",
          "clear",
          "front",
          bool.TrueString
        };
      }
    }

    public override void Do()
    {
      base.Do();
      this.timer = 0.0f;
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.fadeIn = args1[index1].Compare("in", true);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.time = float.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.color = args3[index3].GetColor();
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      this.isFront = args4[index4].Compare("front", true);
      if (this.fadeIn)
        this.scenario.FadeIn(this.time, true);
      else
        this.scenario.FadeOut(this.time, true);
    }

    public override bool Process()
    {
      base.Process();
      return !this.scenario.Fading;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      if (processEnd)
        return;
      if (this.fadeIn)
        this.scenario.FadeIn(0.0f, true);
      else
        this.scenario.FadeOut(0.0f, true);
    }
  }
}

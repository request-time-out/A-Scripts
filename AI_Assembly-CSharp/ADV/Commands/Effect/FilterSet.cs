// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.FilterSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ADV.Commands.Effect
{
  public class FilterSet : CommandBase
  {
    private Color initColor = Color.get_clear();
    private Color color = Color.get_clear();
    private bool isFront = true;
    private const string front = "front";
    private float time;
    private float timer;
    private Image filterImage;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]{ "From", "To", "Time", "Type" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          string.Empty,
          "clear",
          "0",
          "front"
        };
      }
    }

    public override void Do()
    {
      base.Do();
      this.timer = 0.0f;
      int num1 = 0;
      this.filterImage = this.scenario.FilterImage;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      Color? colorCheck = args1[index1].GetColorCheck();
      this.initColor = !colorCheck.HasValue ? ((Graphic) this.filterImage).get_color() : colorCheck.Value;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.color = args2[index2].GetColor();
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.time = float.Parse(args3[index3]);
      int num5 = num4 + 1;
    }

    public override bool Process()
    {
      base.Process();
      this.timer = Mathf.Min(this.timer + Time.get_deltaTime(), this.time);
      ((Graphic) this.filterImage).set_color(Color.Lerp(this.initColor, this.color, (double) this.time != 0.0 ? Mathf.InverseLerp(0.0f, this.time, this.timer) : 1f));
      return (double) this.timer >= (double) this.time;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      if (processEnd)
        return;
      ((Graphic) this.filterImage).set_color(this.color);
    }
  }
}

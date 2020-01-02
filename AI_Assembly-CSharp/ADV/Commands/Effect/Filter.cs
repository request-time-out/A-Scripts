// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.Filter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ADV.Commands.Effect
{
  public class Filter : CommandBase
  {
    private bool isFront = true;
    private Color color = Color.get_clear();
    private const string front = "front";
    private float time;
    private Image filterImage;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Type", "Color", "Time" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ "front", "clear", "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0 + 1;
      string[] args = this.args;
      int index = num1;
      int num2 = index + 1;
      this.color = args[index].GetColor();
      int num3 = num2 + 1;
      this.filterImage = this.scenario.FilterImage;
      ((Graphic) this.filterImage).set_color(this.color);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Regulate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using Illusion.Extensions;

namespace ADV.Commands.Base
{
  public class Regulate : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Type", nameof (Regulate) };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]
        {
          "Set",
          ADV.Regulate.Control.Next.ToString()
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = args1[index1].Check(true, Utils.Enum<Regulate.Type>.Names);
      string[] args2 = this.args;
      int index2 = num2;
      int num4 = index2 + 1;
      ADV.Regulate.Control regulate = (ADV.Regulate.Control) Utils.Enum<ADV.Regulate.Control>.Values.GetValue(args2[index2].Check(true, Utils.Enum<ADV.Regulate.Control>.Names));
      switch ((Regulate.Type) num3)
      {
        case Regulate.Type.Set:
          this.scenario.regulate.SetRegulate(regulate);
          break;
        case Regulate.Type.Add:
          this.scenario.regulate.AddRegulate(regulate);
          break;
        case Regulate.Type.Sub:
          this.scenario.regulate.SubRegulate(regulate);
          break;
      }
    }

    private enum Type
    {
      Set,
      Add,
      Sub,
    }
  }
}

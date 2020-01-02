// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Task
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Base
{
  public class Task : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "isTask" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ bool.FalseString };
      }
    }

    public override void Do()
    {
      base.Do();
      this.scenario.isBackGroundCommanding = bool.Parse(this.args[0]);
    }
  }
}

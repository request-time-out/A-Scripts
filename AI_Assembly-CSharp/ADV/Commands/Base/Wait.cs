// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Wait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Base
{
  public class Wait : CommandBase
  {
    private float time;
    private float timer;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Time" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      this.timer = 0.0f;
      float.TryParse(this.args[0], out this.time);
    }

    public override bool Process()
    {
      base.Process();
      this.timer += Time.get_deltaTime();
      return (double) this.timer >= (double) this.time;
    }
  }
}

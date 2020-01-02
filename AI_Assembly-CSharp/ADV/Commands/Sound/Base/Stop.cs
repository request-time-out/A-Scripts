// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Sound.Base.Stop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Sound.Base
{
  public abstract class Stop : CommandBase
  {
    private Manager.Sound.Type type;
    private float stopTime;
    private float timer;

    public Stop(Manager.Sound.Type type)
    {
      this.type = type;
    }

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
      float.TryParse(this.args.SafeGet<string>(0), out this.stopTime);
    }

    public override bool Process()
    {
      base.Process();
      if ((double) this.timer >= (double) this.stopTime)
        return true;
      this.timer += Time.get_deltaTime();
      Debug.Log((object) ("timer:" + (object) this.timer));
      return false;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      Singleton<Manager.Sound>.Instance.Stop(this.type);
    }
  }
}

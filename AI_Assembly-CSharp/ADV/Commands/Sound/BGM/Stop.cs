// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Sound.BGM.Stop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Sound.BGM
{
  public class Stop : CommandBase
  {
    private float stopTime;
    private float fadeTime;
    private float timer;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Time", "Fade" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "0", "0.8" };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      this.stopTime = float.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.fadeTime = float.Parse(args2[index2]);
    }

    public override bool Process()
    {
      base.Process();
      if ((double) this.timer >= (double) this.stopTime)
        return true;
      this.timer += Time.get_deltaTime();
      Debug.Log((object) ("timer" + (object) this.timer));
      return false;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      Singleton<Manager.Sound>.Instance.StopBGM(this.fadeTime);
    }
  }
}

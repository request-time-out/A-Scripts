// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.MotionWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class MotionWait : CommandBase
  {
    private int layerNo;
    private float time;
    private ChaControl chaCtrl;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "LayerNo", "Time" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          int.MaxValue.ToString(),
          "0",
          "1"
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
      int no = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      this.layerNo = int.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      this.time = float.Parse(args3[index3]);
      this.chaCtrl = this.scenario.commandController.GetChara(no).chaCtrl;
    }

    public override bool Process()
    {
      base.Process();
      AnimatorStateInfo animatorStateInfo = this.chaCtrl.getAnimatorStateInfo(this.layerNo);
      return (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() >= (double) this.time;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.MotionSetParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class MotionSetParam : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "Name", "Value" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          int.MaxValue.ToString(),
          string.Empty,
          string.Empty
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
      string name = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string s = args3[index3];
      Animator animBody = this.scenario.commandController.GetChara(no).chaCtrl.animBody;
      AnimatorControllerParameterType type = Utils.Animator.GetAnimeParam(name, animBody).get_type();
      if (type != 1)
      {
        if (type != 3)
        {
          if (type != 4)
            return;
          animBody.SetBool(name, bool.Parse(s));
        }
        else
          animBody.SetInteger(name, int.Parse(s));
      }
      else
        animBody.SetFloat(name, float.Parse(s));
    }
  }
}

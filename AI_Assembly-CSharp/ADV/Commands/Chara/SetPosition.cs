// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.SetPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Chara
{
  public class SetPosition : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[7]
        {
          "No",
          "X",
          "Y",
          "Z",
          "Pitch",
          "Yaw",
          "Roll"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ int.MaxValue.ToString() };
      }
    }

    public override void Do()
    {
      base.Do();
      int num = 0;
      string[] args = this.args;
      int index = num;
      int cnt = index + 1;
      Transform transform = this.scenario.commandController.GetChara(int.Parse(args[index])).transform;
      Vector3 pos1;
      if (!this.scenario.commandController.GetV3Dic(this.args.SafeGet<string>(cnt), out pos1))
      {
        pos1 = transform.get_position();
        CommandBase.CountAddV3(this.args, ref cnt, ref pos1);
      }
      else
        CommandBase.CountAddV3(ref cnt);
      transform.set_position(pos1);
      Vector3 pos2;
      if (!this.scenario.commandController.GetV3Dic(this.args.SafeGet<string>(cnt), out pos2))
      {
        pos2 = transform.get_eulerAngles();
        CommandBase.CountAddV3(this.args, ref cnt, ref pos2);
      }
      else
        CommandBase.CountAddV3(ref cnt);
      transform.set_eulerAngles(pos2);
    }
  }
}

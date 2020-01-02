// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.CameraLookAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Game
{
  public class CameraLookAt : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "X", "Y", "Z" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      Vector3 pos;
      if (!this.scenario.commandController.GetV3Dic(this.args[cnt], out pos))
        CommandBase.CountAddV3(this.args, ref cnt, ref pos);
      ((Component) this.scenario.AdvCamera).get_transform().LookAt(pos);
    }
  }
}

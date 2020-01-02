// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.ChangeADVFixedAngleCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.MapScene
{
  public class ChangeADVFixedAngleCamera : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "AttitudeID" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "0", "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      CharaData chara = this.GetChara(ref cnt);
      if (chara == null)
        return;
      string[] args = this.args;
      int index = cnt;
      int num = index + 1;
      int attitudeID = int.Parse(args[index]);
      Manager.ADV.ChangeADVFixedAngleCamera(chara.data.actor, attitudeID);
    }
  }
}

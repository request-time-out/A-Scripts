// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.CharaSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.MapScene
{
  public class CharaSetting : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "Active" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "0", bool.TrueString };
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
      if (bool.Parse(args[index]))
        chara.backup.Repair();
      else
        chara.backup.Set();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.PlaySystemSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;

namespace ADV.Commands.MapScene
{
  public class PlaySystemSE : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "Name" };
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
      string str = this.args[0];
      int result;
      if (!int.TryParse(str, out result))
        result = Illusion.Utils.Enum<SoundPack.SystemSE>.FindIndex(str, true);
      Singleton<Resources>.Instance.SoundPack.Play((SoundPack.SystemSE) result);
    }
  }
}

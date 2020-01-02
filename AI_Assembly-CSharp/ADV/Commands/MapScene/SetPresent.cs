// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.SetPresent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;

namespace ADV.Commands.MapScene
{
  public class SetPresent : PresentBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "No" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ "0" };
      }
    }

    protected override Resources.GameInfoTables.AdvPresentItemInfo GetAdvPresentItemInfo(
      ChaControl chaCtrl)
    {
      return Singleton<Resources>.Instance.GameInfo.GetAdvPresentInfo(chaCtrl);
    }

    public override void Do()
    {
      base.Do();
      int cnt = 0;
      this.SetPresentInfo(ref cnt);
    }
  }
}

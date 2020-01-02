// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.InventoryGiveItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;

namespace ADV.Commands.MapScene
{
  public class InventoryGiveItem : InventoryBase
  {
    private bool isGive;
    private CharaData myChara;
    private CharaData targetChara;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "MyNo", "TargetNo" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ "-1", string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      int cnt1 = 0;
      this.myChara = this.GetChara(ref cnt1);
      if (this.myChara == null)
        return;
      this.targetChara = this.GetChara(ref cnt1);
      int cnt2 = cnt1;
      int num = cnt2 + 1;
      this.OpenInventory(cnt2, this.myChara.data.characterInfo.ItemList);
      this.scenario.regulate.AddRegulate(Regulate.Control.Next);
    }

    public override bool Process()
    {
      base.Process();
      return this.isClose;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      this.scenario.regulate.SubRegulate(Regulate.Control.Next);
      this.isGive = this.scenario.AddItemVars(this.Item);
      this.scenario.Vars["isGive"] = new ValData((object) this.isGive);
      if (!this.isGive || this.targetChara == null)
        return;
      this.targetChara.data.characterInfo.ItemList.AddItem(this.Item);
    }
  }
}

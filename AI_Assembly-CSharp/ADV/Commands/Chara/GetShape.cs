// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.GetShape
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class GetShape : CommandBase
  {
    private int no;
    private string name;
    private int index;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "Name", "Index" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]{ "0", string.Empty, "0" };
      }
    }

    public override void ConvertBeforeArgsProc()
    {
      base.ConvertBeforeArgsProc();
      this.name = this.args[1];
    }

    public override void Do()
    {
      base.Do();
      this.no = int.Parse(this.args[0]);
      this.index = int.Parse(this.args[2]);
      this.scenario.Vars[this.name] = new ValData((object) this.scenario.commandController.GetChara(this.no).chaCtrl.GetShapeBodyValue(this.index));
    }
  }
}

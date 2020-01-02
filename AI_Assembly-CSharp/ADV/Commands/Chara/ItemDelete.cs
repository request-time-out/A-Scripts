// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.ItemDelete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ADV.Commands.Chara
{
  public class ItemDelete : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "No", "ItemNo" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ int.MaxValue.ToString(), "0" };
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
      int key = int.Parse(args2[index2]);
      CharaData chara = this.scenario.commandController.GetChara(no);
      chara.itemDic[key].Delete();
      chara.itemDic.Remove(key);
    }
  }
}

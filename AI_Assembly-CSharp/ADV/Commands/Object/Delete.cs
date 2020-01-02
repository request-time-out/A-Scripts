// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Delete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV.Commands.Object
{
  public class Delete : CommandBase
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
        return new string[1]{ string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      string key = this.args[0];
      UnityEngine.Object.Destroy((UnityEngine.Object) this.scenario.commandController.Objects[key]);
      this.scenario.commandController.Objects.Remove(key);
    }
  }
}

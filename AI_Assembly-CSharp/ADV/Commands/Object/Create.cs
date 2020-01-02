// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Create
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion;
using UnityEngine;

namespace ADV.Commands.Object
{
  public class Create : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Name", "Component" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]{ string.Empty, string.Empty };
      }
    }

    public override void Do()
    {
      base.Do();
      int num = 0;
      string[] args = this.args;
      int index = num;
      int cnt = index + 1;
      GameObject go = new GameObject(args[index]);
      foreach (string typeName in CommandBase.RemoveArgsEmpty(this.GetArgToSplitLast(cnt)))
        go.AddComponent(Utils.Type.Get(typeName));
      this.scenario.commandController.SetObject(go);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.PackData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.UI;
using System;
using System.Collections.Generic;

namespace AIProject
{
  internal class PackData : CharaPackData
  {
    public CommCommandList.CommandInfo[] restoreCommands { get; set; }

    public bool isSuccessH
    {
      get
      {
        ValData valData;
        return this.Vars != null && this.Vars.TryGetValue(nameof (isSuccessH), out valData) && (bool) valData.o;
      }
    }

    public bool isBirthday { get; set; }

    public override List<Program.Transfer> Create()
    {
      List<Program.Transfer> transferList = base.Create();
      transferList.Add(Program.Transfer.Create(true, Command.VAR, "bool", "isBirthday", this.isBirthday.ToString()));
      return transferList;
    }

    public override void Receive(TextScenario scenario)
    {
      base.Receive(scenario);
      if (this.restoreCommands == null)
        return;
      CommCommandList commandList = MapUIContainer.CommandList;
      commandList.Refresh(this.restoreCommands, commandList.CanvasGroup, (Action) null);
    }
  }
}

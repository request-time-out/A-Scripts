// Decompiled with JetBrains decompiler
// Type: ADV.IPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace ADV
{
  public interface IPack
  {
    IParams[] param { get; }

    List<Program.Transfer> Create();

    void Receive(TextScenario scenario);

    IReadOnlyCollection<CommandData> commandList { get; }

    void CommandListVisibleEnabledDefault();

    bool isCommandListVisibleEnabled { get; }
  }
}

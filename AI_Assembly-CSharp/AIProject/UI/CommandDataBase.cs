// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CommandDataBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;

namespace AIProject.UI
{
  [Serializable]
  public abstract class CommandDataBase : ICommandData
  {
    protected abstract bool IsInput(Input input);

    public bool IsActive { get; set; } = true;

    public void Invoke(Input input)
    {
      if (!this.IsActive)
        return;
      this.OnInvoke(input);
    }

    protected abstract void OnInvoke(Input input);
  }
}

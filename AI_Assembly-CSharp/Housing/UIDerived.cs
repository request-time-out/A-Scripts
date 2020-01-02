// Decompiled with JetBrains decompiler
// Type: Housing.UIDerived
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Housing
{
  public abstract class UIDerived
  {
    protected UICtrl UICtrl { get; private set; }

    public virtual void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      this.UICtrl = _uiCtrl;
    }

    public abstract void UpdateUI();
  }
}

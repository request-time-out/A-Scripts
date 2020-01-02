// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ListBoxItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class ListBoxItem : ItemContainer
  {
    private Toggle m_toggle;

    public override bool IsSelected
    {
      get
      {
        return base.IsSelected;
      }
      set
      {
        if (base.IsSelected == value)
          return;
        this.m_toggle.set_isOn(value);
        base.IsSelected = value;
      }
    }

    protected override void AwakeOverride()
    {
      this.m_toggle = (Toggle) ((Component) this).GetComponent<Toggle>();
      ((Selectable) this.m_toggle).set_interactable(false);
      this.m_toggle.set_isOn(this.IsSelected);
    }
  }
}

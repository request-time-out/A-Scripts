// Decompiled with JetBrains decompiler
// Type: ResetPositionOnDiactivated
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class ResetPositionOnDiactivated : MonoBehaviour
{
  public EffectSettings EffectSettings;

  public ResetPositionOnDiactivated()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.EffectSettings.EffectDeactivated += new EventHandler<EventArgs>(this.EffectSettings_EffectDeactivated);
  }

  private void EffectSettings_EffectDeactivated(object sender, EventArgs e)
  {
    ((Component) this).get_transform().set_localPosition(Vector3.get_zero());
  }
}

// Decompiled with JetBrains decompiler
// Type: CollisionActiveBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class CollisionActiveBehaviour : MonoBehaviour
{
  public bool IsReverse;
  public float TimeDelay;
  public bool IsLookAt;
  private EffectSettings effectSettings;

  public CollisionActiveBehaviour()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (this.IsReverse)
    {
      this.effectSettings.RegistreInactiveElement(((Component) this).get_gameObject(), this.TimeDelay);
      ((Component) this).get_gameObject().SetActive(false);
    }
    else
      this.effectSettings.RegistreActiveElement(((Component) this).get_gameObject(), this.TimeDelay);
    if (!this.IsLookAt)
      return;
    this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.effectSettings_CollisionEnter);
  }

  private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    ((Component) this).get_transform().LookAt(Vector3.op_Addition(((Component) this.effectSettings).get_transform().get_position(), ((RaycastHit) ref e.Hit).get_normal()));
  }

  private void GetEffectSettingsComponent(Transform tr)
  {
    Transform parent = tr.get_parent();
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    this.effectSettings = (EffectSettings) ((Component) parent).GetComponentInChildren<EffectSettings>();
    if (!Object.op_Equality((Object) this.effectSettings, (Object) null))
      return;
    this.GetEffectSettingsComponent(((Component) parent).get_transform());
  }
}

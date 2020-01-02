// Decompiled with JetBrains decompiler
// Type: AnimatorBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
{
  public Animator anim;
  private EffectSettings effectSettings;
  private bool isInitialized;
  private float oldSpeed;

  public AnimatorBehaviour()
  {
    base.\u002Ector();
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

  private void Start()
  {
    this.oldSpeed = this.anim.get_speed();
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null))
      this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
    this.isInitialized = true;
  }

  private void OnEnable()
  {
    if (!this.isInitialized)
      return;
    this.anim.set_speed(this.oldSpeed);
  }

  private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    this.anim.set_speed(0.0f);
  }

  private void Update()
  {
  }
}

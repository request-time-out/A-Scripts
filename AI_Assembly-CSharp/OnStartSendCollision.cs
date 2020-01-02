// Decompiled with JetBrains decompiler
// Type: OnStartSendCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class OnStartSendCollision : MonoBehaviour
{
  private EffectSettings effectSettings;
  private bool isInitialized;

  public OnStartSendCollision()
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
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    this.effectSettings.OnCollisionHandler(new CollisionInfo());
    this.isInitialized = true;
  }

  private void OnEnable()
  {
    if (!this.isInitialized)
      return;
    this.effectSettings.OnCollisionHandler(new CollisionInfo());
  }
}

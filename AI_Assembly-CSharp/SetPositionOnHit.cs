// Decompiled with JetBrains decompiler
// Type: SetPositionOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class SetPositionOnHit : MonoBehaviour
{
  public float OffsetPosition;
  private EffectSettings effectSettings;
  private Transform tRoot;
  private bool isInitialized;

  public SetPositionOnHit()
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
    if (Object.op_Equality((Object) this.effectSettings, (Object) null))
      Debug.Log((object) "Prefab root or children have not script \"PrefabSettings\"");
    this.tRoot = ((Component) this.effectSettings).get_transform();
  }

  private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    Vector3 vector3 = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(Vector3.Normalize(Vector3.op_Subtraction(((RaycastHit) ref e.Hit).get_point(), this.tRoot.get_position())), this.effectSettings.MoveDistance + 1f));
    Vector3 normalized = ((Vector3) ref vector3).get_normalized();
    ((Component) this).get_transform().set_position(Vector3.op_Subtraction(((RaycastHit) ref e.Hit).get_point(), Vector3.op_Multiply(normalized, this.OffsetPosition)));
  }

  private void Update()
  {
    if (this.isInitialized)
      return;
    this.isInitialized = true;
    this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.effectSettings_CollisionEnter);
  }

  private void OnDisable()
  {
    ((Component) this).get_transform().set_position(Vector3.get_zero());
  }
}

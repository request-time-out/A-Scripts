// Decompiled with JetBrains decompiler
// Type: DebuffOnEnemyFromCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class DebuffOnEnemyFromCollision : MonoBehaviour
{
  public EffectSettings EffectSettings;
  public GameObject Effect;

  public DebuffOnEnemyFromCollision()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.EffectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.EffectSettings_CollisionEnter);
  }

  private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    if (Object.op_Equality((Object) this.Effect, (Object) null))
      return;
    foreach (Collider collider in Physics.OverlapSphere(((Component) this).get_transform().get_position(), this.EffectSettings.EffectRadius, LayerMask.op_Implicit(this.EffectSettings.LayerMask)))
    {
      Renderer componentInChildren = (Renderer) ((Component) ((Component) collider).get_transform()).GetComponentInChildren<Renderer>();
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.Effect);
      gameObject.get_transform().set_parent(((Component) componentInChildren).get_transform());
      gameObject.get_transform().set_localPosition(Vector3.get_zero());
      ((AddMaterialOnHit) gameObject.GetComponent<AddMaterialOnHit>()).UpdateMaterial(((Component) collider).get_transform());
    }
  }

  private void Update()
  {
  }
}

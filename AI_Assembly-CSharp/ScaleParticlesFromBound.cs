// Decompiled with JetBrains decompiler
// Type: ScaleParticlesFromBound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ScaleParticlesFromBound : MonoBehaviour
{
  private Collider targetCollider;

  public ScaleParticlesFromBound()
  {
    base.\u002Ector();
  }

  private void GetMeshFilterParent(Transform t)
  {
    Collider component = (Collider) ((Component) t.get_parent()).GetComponent<Collider>();
    if (Object.op_Equality((Object) component, (Object) null))
      this.GetMeshFilterParent(t.get_parent());
    else
      this.targetCollider = component;
  }

  private void Start()
  {
    this.GetMeshFilterParent(((Component) this).get_transform());
    if (Object.op_Equality((Object) this.targetCollider, (Object) null))
      return;
    Bounds bounds = this.targetCollider.get_bounds();
    ((Component) this).get_transform().set_localScale(((Bounds) ref bounds).get_size());
  }

  private void Update()
  {
  }
}

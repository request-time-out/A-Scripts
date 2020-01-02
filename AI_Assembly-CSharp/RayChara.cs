// Decompiled with JetBrains decompiler
// Type: RayChara
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class RayChara : CollisionCamera
{
  public RayChara.Parts[] parts;

  private new void Start()
  {
    base.Start();
  }

  private void Update()
  {
    if (this.parts == null || this.objDels == null)
      return;
    foreach (GameObject objDel in this.objDels)
      ((Renderer) objDel.GetComponent<Renderer>()).set_enabled(true);
    foreach (RayChara.Parts part in this.parts)
      part.Update(((Component) this).get_transform().get_position(), this.tagName);
  }

  [Serializable]
  public class Parts
  {
    public Transform target;

    public void Update(Vector3 pos, string tag)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.target.get_position(), pos);
      foreach (RaycastHit raycastHit in Physics.RaycastAll(pos, ((Vector3) ref vector3).get_normalized(), ((Vector3) ref vector3).get_magnitude()))
      {
        if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_tag() == tag)
          ((Renderer) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().GetComponent<Renderer>()).set_enabled(false);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: CollisionCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class CollisionCamera : MonoBehaviour
{
  public string tagName;
  protected GameObject[] objDels;
  protected BaseCameraControl camCtrl;

  public CollisionCamera()
  {
    base.\u002Ector();
  }

  public void SetCollision()
  {
    this.objDels = GameObject.FindGameObjectsWithTag(this.tagName);
  }

  protected void Start()
  {
    this.camCtrl = (BaseCameraControl) ((Component) this).get_gameObject().GetComponent<BaseCameraControl>();
  }

  private void Update()
  {
    this.SetCollision();
    if (this.objDels == null)
      return;
    foreach (GameObject objDel in this.objDels)
    {
      if (Object.op_Implicit((Object) objDel.GetComponent<Renderer>()))
        ((Renderer) objDel.GetComponent<Renderer>()).set_enabled(true);
    }
    Vector3 position = ((Component) this).get_transform().get_position();
    Vector3 vector3 = Vector3.op_Subtraction(!Object.op_Implicit((Object) this.camCtrl.targetObj) ? this.camCtrl.TargetPos : ((Component) this.camCtrl.targetObj).get_transform().get_position(), position);
    foreach (RaycastHit raycastHit in Physics.RaycastAll(position, ((Vector3) ref vector3).get_normalized(), ((Vector3) ref vector3).get_magnitude()))
    {
      if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_tag() == this.tagName)
      {
        float magnitude = ((Vector3) ref vector3).get_magnitude();
        Bounds bounds = ((RaycastHit) ref raycastHit).get_collider().get_bounds();
        float num = Vector3.Distance(((Bounds) ref bounds).get_center(), position);
        if ((double) magnitude > (double) num)
          ((Renderer) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().GetComponent<Renderer>()).set_enabled(false);
      }
    }
  }
}

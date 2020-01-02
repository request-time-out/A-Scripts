// Decompiled with JetBrains decompiler
// Type: LookinCapsule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Text;
using UnityEngine;

public class LookinCapsule : CollisionCamera
{
  public float scaleRate = 5f;
  private GameObject lookCap;

  private new void Start()
  {
    base.Start();
    this.lookCap = GameObject.CreatePrimitive((PrimitiveType) 1);
    ((Collider) this.lookCap.GetComponent<CapsuleCollider>()).set_isTrigger(true);
    ((Renderer) this.lookCap.GetComponent<Renderer>()).set_enabled(false);
    this.lookCap.get_transform().set_position(Vector3.op_Multiply(Vector3.op_Addition(this.camCtrl.TargetPos, ((Component) this.camCtrl).get_transform().get_position()), 0.5f));
    this.lookCap.get_transform().set_parent(((Component) this.camCtrl).get_transform());
    Vector3 cameraAngle = this.camCtrl.CameraAngle;
    ref Vector3 local1 = ref cameraAngle;
    local1.x = (__Null) (local1.x + 90.0);
    this.lookCap.get_transform().set_rotation(Quaternion.Euler(cameraAngle));
    Vector3 localScale = this.lookCap.get_transform().get_localScale();
    ref Vector3 local2 = ref localScale;
    Vector3 vector3 = Vector3.op_Subtraction(this.camCtrl.TargetPos, ((Component) this.camCtrl).get_transform().get_position());
    double magnitude = (double) ((Vector3) ref vector3).get_magnitude();
    local2.y = (__Null) magnitude;
    this.lookCap.get_transform().set_localScale(localScale);
    this.lookCap.AddComponent<LookHit>();
    ((Rigidbody) this.lookCap.AddComponent<Rigidbody>()).set_useGravity(false);
  }

  private void Update()
  {
    this.lookCap.get_transform().set_position(Vector3.op_Multiply(Vector3.op_Addition(this.camCtrl.TargetPos, ((Component) this.camCtrl).get_transform().get_position()), 0.5f));
    Vector3 vector3;
    vector3.y = (__Null) ((double) Vector3.Distance(this.camCtrl.TargetPos, ((Component) this.camCtrl).get_transform().get_position()) * 0.5);
    vector3.x = (__Null) (double) (vector3.z = (__Null) this.scaleRate);
    this.lookCap.get_transform().set_localScale(vector3);
  }

  private void OnGUI()
  {
    StringBuilder stringBuilder = new StringBuilder();
    float num = 1000f;
    if (this.objDels != null)
    {
      foreach (GameObject objDel in this.objDels)
      {
        if (!((Renderer) objDel.GetComponent<Renderer>()).get_enabled())
        {
          stringBuilder.Append(((Object) objDel).get_name());
          stringBuilder.Append("\n");
        }
      }
    }
    GUI.Box(new Rect(5f, 5f, 300f, num), string.Empty);
    GUI.Label(new Rect(10f, 5f, 1000f, num), stringBuilder.ToString());
  }
}

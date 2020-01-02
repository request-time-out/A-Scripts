// Decompiled with JetBrains decompiler
// Type: FPControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class FPControl : MonoBehaviour
{
  public Camera myCamera;
  public CharacterController cc;
  public float walkSpeed;
  private float _theta;
  private float _phi;
  public float rotSpeed;
  private float _mx;
  private float _my;

  public FPControl()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Vector3 forward = ((Component) this.myCamera).get_transform().get_forward();
    this._phi = Mathf.Asin((float) forward.y);
    this._theta = Mathf.Atan2((float) forward.x, (float) forward.z);
    this._mx = (float) Input.get_mousePosition().x;
    this._my = (float) Input.get_mousePosition().y;
  }

  private void Update()
  {
    float axis1 = Input.GetAxis("Horizontal");
    float axis2 = Input.GetAxis("Vertical");
    float x = (float) Input.get_mousePosition().x;
    float y = (float) Input.get_mousePosition().y;
    float num1 = x - this._mx;
    float num2 = y - this._my;
    this._mx = x;
    this._my = y;
    Vector3 forward = ((Component) this.myCamera).get_transform().get_forward();
    forward.y = (__Null) 0.0;
    ((Vector3) ref forward).Normalize();
    Vector3 vector3_1 = Vector3.Cross(Vector3.get_up(), forward);
    this.cc.Move(Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_Multiply(this.walkSpeed, Vector3.op_Addition(Vector3.op_Multiply(forward, axis2), Vector3.op_Multiply(vector3_1, axis1))), Vector3.op_Multiply(Vector3.get_down(), 3f)), Time.get_deltaTime()));
    this._theta -= num1 * this.rotSpeed;
    if ((double) this._theta > 3.14159274101257)
      this._theta -= 6.283185f;
    else if ((double) this._theta <= -3.14159274101257)
      this._theta += 6.283185f;
    this._phi += num2 * this.rotSpeed;
    if ((double) this._phi > 1.0)
      this._phi = 1f;
    else if ((double) this._phi < -1.0)
      this._phi = -1f;
    Vector3 vector3_2;
    ((Vector3) ref vector3_2).\u002Ector(Mathf.Cos(this._phi) * Mathf.Cos(this._theta), Mathf.Sin(this._phi), Mathf.Cos(this._phi) * Mathf.Sin(this._theta));
    ((Component) this.myCamera).get_transform().LookAt(Vector3.op_Addition(((Component) this.myCamera).get_transform().get_position(), vector3_2), Vector3.get_up());
    if (!Input.GetKeyDown((KeyCode) 32))
      return;
    this.Shoot();
  }

  public void Shoot()
  {
    RaycastHit raycastHit;
    if (!Physics.Raycast(this.myCamera.ScreenPointToRay(new Vector3((float) this.myCamera.get_pixelWidth() * 0.5f, (float) this.myCamera.get_pixelHeight() * 0.5f, 0.0f)), ref raycastHit))
      return;
    DungeonControl componentInParents = Utils.FindComponentInParents<DungeonControl>(((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform());
    if (!Object.op_Inequality((Object) componentInParents, (Object) null))
      return;
    componentInParents.AddCell(((RaycastHit) ref raycastHit).get_point(), 2f);
  }
}

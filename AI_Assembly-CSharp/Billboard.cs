// Decompiled with JetBrains decompiler
// Type: Billboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class Billboard : MonoBehaviour
{
  public Camera Camera;
  public bool Active;
  public bool AutoInitCamera;
  private GameObject myContainer;
  private Transform t;
  private Transform camT;
  private Transform contT;

  public Billboard()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (this.AutoInitCamera)
    {
      this.Camera = Camera.get_main();
      this.Active = true;
    }
    this.t = ((Component) this).get_transform();
    this.camT = ((Component) this.Camera).get_transform();
    Transform parent = this.t.get_parent();
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("Billboard_" + ((Object) ((Component) this.t).get_gameObject()).get_name());
    this.myContainer = gameObject;
    this.contT = this.myContainer.get_transform();
    this.contT.set_position(this.t.get_position());
    this.t.set_parent(this.myContainer.get_transform());
    this.contT.set_parent(parent);
  }

  private void Update()
  {
    if (!this.Active)
      return;
    this.contT.LookAt(Vector3.op_Addition(this.contT.get_position(), Quaternion.op_Multiply(this.camT.get_rotation(), Vector3.get_back())), Quaternion.op_Multiply(this.camT.get_rotation(), Vector3.get_up()));
  }
}

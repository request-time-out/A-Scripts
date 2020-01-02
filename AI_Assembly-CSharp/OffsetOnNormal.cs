// Decompiled with JetBrains decompiler
// Type: OffsetOnNormal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class OffsetOnNormal : MonoBehaviour
{
  public float offset;
  public GameObject offsetGameObject;
  private Vector3 startPosition;

  public OffsetOnNormal()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.startPosition = ((Component) this).get_transform().get_position();
  }

  private void OnEnable()
  {
    RaycastHit raycastHit;
    Physics.Raycast(this.startPosition, Vector3.get_down(), ref raycastHit);
    if (Object.op_Inequality((Object) this.offsetGameObject, (Object) null))
      ((Component) this).get_transform().set_position(Vector3.op_Addition(this.offsetGameObject.get_transform().get_position(), Vector3.op_Multiply(((RaycastHit) ref raycastHit).get_normal(), this.offset)));
    else
      ((Component) this).get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(((RaycastHit) ref raycastHit).get_normal(), this.offset)));
  }

  private void Update()
  {
  }
}

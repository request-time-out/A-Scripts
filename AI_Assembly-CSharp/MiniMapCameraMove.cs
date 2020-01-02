// Decompiled with JetBrains decompiler
// Type: MiniMapCameraMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MiniMapCameraMove : MonoBehaviour
{
  public float MoveMinX;
  public float MoveMaxX;
  public float MoveMinZ;
  public float MoveMaxZ;

  public MiniMapCameraMove()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  public void Init()
  {
    Vector3 position = Singleton<Manager.Map>.Instance.Player.Position;
    position.y = ((Component) this).get_transform().get_position().y;
    ((Component) this).get_transform().set_position(position);
  }

  private void Update()
  {
    if (!Singleton<Manager.Map>.IsInstance())
      return;
    this.MoveCamera();
  }

  private void MoveCamera()
  {
    if (Object.op_Equality((Object) Singleton<Manager.Map>.Instance.Player, (Object) null))
      return;
    Vector3 position = Singleton<Manager.Map>.Instance.Player.Position;
    position.x = (__Null) (double) Mathf.Clamp((float) position.x, this.MoveMinX, this.MoveMaxX);
    position.y = ((Component) this).get_transform().get_position().y;
    position.z = (__Null) (double) Mathf.Clamp((float) position.z, this.MoveMinZ, this.MoveMaxZ);
    ((Component) this).get_transform().set_position(position);
  }
}

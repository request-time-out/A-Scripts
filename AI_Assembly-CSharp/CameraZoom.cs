// Decompiled with JetBrains decompiler
// Type: CameraZoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class CameraZoom : MonoBehaviour
{
  public float zoomSpeed;
  public float keyZoomSpeed;

  public CameraZoom()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), this.zoomSpeed), Input.GetAxis("Mouse ScrollWheel")));
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), this.keyZoomSpeed), Time.get_deltaTime()), Input.GetAxis("Vertical")));
  }
}

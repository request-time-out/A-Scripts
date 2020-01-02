// Decompiled with JetBrains decompiler
// Type: RotateViewpoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class RotateViewpoint : MonoBehaviour
{
  public float rotateSpeed;

  public RotateViewpoint()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    ((Component) this).get_transform().RotateAround(Vector3.get_zero(), Vector3.get_right(), this.rotateSpeed * Time.get_deltaTime());
  }
}

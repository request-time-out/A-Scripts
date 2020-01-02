// Decompiled with JetBrains decompiler
// Type: MayaCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MayaCamera : MonoBehaviour
{
  public Camera _camera;

  public MayaCamera()
  {
    base.\u002Ector();
  }

  private void LateUpdate()
  {
    if (!Object.op_Inequality((Object) this._camera, (Object) null))
      return;
    this._camera.set_fieldOfView((float) ((Component) this).get_transform().get_localScale().z);
  }
}

// Decompiled with JetBrains decompiler
// Type: AQUAS_Camera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("AQUAS/AQUAS Camera")]
[RequireComponent(typeof (Camera))]
public class AQUAS_Camera : MonoBehaviour
{
  public AQUAS_Camera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.Set();
  }

  private void Set()
  {
    if (((Camera) ((Component) this).GetComponent<Camera>()).get_depthTextureMode() != null)
      return;
    ((Camera) ((Component) this).GetComponent<Camera>()).set_depthTextureMode((DepthTextureMode) 1);
  }
}

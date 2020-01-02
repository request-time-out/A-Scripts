// Decompiled with JetBrains decompiler
// Type: WireFrameRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class WireFrameRender : MonoBehaviour
{
  public bool wireFrameDraw;

  public WireFrameRender()
  {
    base.\u002Ector();
  }

  private void OnPreRender()
  {
    if (!this.wireFrameDraw)
      return;
    GL.set_wireframe(true);
  }

  private void OnPostRender()
  {
    if (!this.wireFrameDraw)
      return;
    GL.set_wireframe(false);
  }
}

// Decompiled with JetBrains decompiler
// Type: CrossFadeCreater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class CrossFadeCreater
{
  private static CrossFadeObject Create()
  {
    return (CrossFadeObject) new GameObject("CrossFade").AddComponent<CrossFadeObject>();
  }

  public static RenderTexture Capture(Camera renderCamera)
  {
    RenderTexture renderTexture = new RenderTexture(Screen.get_width(), Screen.get_height(), 24);
    renderTexture.set_enableRandomWrite(false);
    renderCamera.set_targetTexture(renderTexture);
    renderCamera.Render();
    renderCamera.set_targetTexture((RenderTexture) null);
    return renderTexture;
  }
}

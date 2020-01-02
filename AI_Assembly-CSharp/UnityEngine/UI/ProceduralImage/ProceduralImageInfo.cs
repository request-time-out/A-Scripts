// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ProceduralImage.ProceduralImageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace UnityEngine.UI.ProceduralImage
{
  public struct ProceduralImageInfo
  {
    public float width;
    public float height;
    public float fallOffDistance;
    public Vector4 radius;
    public float borderWidth;
    public float pixelSize;

    public ProceduralImageInfo(
      float width,
      float height,
      float fallOffDistance,
      float pixelSize,
      Vector4 radius,
      float borderWidth)
    {
      this.width = Mathf.Abs(width);
      this.height = Mathf.Abs(height);
      this.fallOffDistance = Mathf.Max(0.0f, fallOffDistance);
      this.radius = radius;
      this.borderWidth = Mathf.Max(borderWidth, 0.0f);
      this.pixelSize = Mathf.Max(0.0f, pixelSize);
    }
  }
}

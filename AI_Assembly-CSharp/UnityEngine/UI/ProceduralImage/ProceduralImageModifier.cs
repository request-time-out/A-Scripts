// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ProceduralImage.ProceduralImageModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace UnityEngine.UI.ProceduralImage
{
  [DisallowMultipleComponent]
  public abstract class ProceduralImageModifier : MonoBehaviour
  {
    protected ProceduralImageModifier()
    {
      base.\u002Ector();
    }

    public abstract Vector4 CalculateRadius(Rect imageRect);
  }
}

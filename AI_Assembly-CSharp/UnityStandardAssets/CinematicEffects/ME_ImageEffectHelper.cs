// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.CinematicEffects.ME_ImageEffectHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
  public static class ME_ImageEffectHelper
  {
    public static bool IsSupported(Shader s, bool needDepth, bool needHdr, MonoBehaviour effect)
    {
      if (Object.op_Equality((Object) s, (Object) null) || !s.get_isSupported())
      {
        Debug.LogWarningFormat("Missing shader for image effect {0}", new object[1]
        {
          (object) effect
        });
        return false;
      }
      if (!SystemInfo.get_supportsImageEffects())
      {
        Debug.LogWarningFormat("Image effects aren't supported on this device ({0})", new object[1]
        {
          (object) effect
        });
        return false;
      }
      if (needDepth && !SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 1))
      {
        Debug.LogWarningFormat("Depth textures aren't supported on this device ({0})", new object[1]
        {
          (object) effect
        });
        return false;
      }
      if (!needHdr || SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 2))
        return true;
      Debug.LogWarningFormat("Floating point textures aren't supported on this device ({0})", new object[1]
      {
        (object) effect
      });
      return false;
    }

    public static Material CheckShaderAndCreateMaterial(Shader s)
    {
      if (Object.op_Equality((Object) s, (Object) null) || !s.get_isSupported())
        return (Material) null;
      Material material = new Material(s);
      ((Object) material).set_hideFlags((HideFlags) 52);
      return material;
    }

    public static bool supportsDX11
    {
      get
      {
        return SystemInfo.get_graphicsShaderLevel() >= 50 && SystemInfo.get_supportsComputeShaders();
      }
    }
  }
}

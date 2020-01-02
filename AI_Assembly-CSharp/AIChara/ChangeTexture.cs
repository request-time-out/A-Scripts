// Decompiled with JetBrains decompiler
// Type: AIChara.ChangeTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public class ChangeTexture
  {
    public static void SetTexture(
      string propertyName,
      Texture tex,
      int idx,
      params Renderer[] arrRend)
    {
      ChangeTexture.SetTexture(Shader.PropertyToID(propertyName), tex, idx, arrRend);
    }

    public static void SetTexture(int propertyID, Texture tex, int idx, params Renderer[] arrRend)
    {
      if (arrRend == null || arrRend.Length == 0)
        return;
      foreach (Renderer renderer in arrRend)
      {
        if (idx < renderer.get_materials().Length)
        {
          Material material = renderer.get_materials()[idx];
          if (Object.op_Inequality((Object) null, (Object) material))
            material.SetTexture(propertyID, tex);
        }
      }
    }

    public static void SetTexture(string propertyName, Texture tex, params Material[] mat)
    {
      ChangeTexture.SetTexture(Shader.PropertyToID(propertyName), tex, mat);
    }

    public static void SetTexture(int propertyID, Texture tex, params Material[] mat)
    {
      if (mat == null || mat.Length == 0)
        return;
      foreach (Material material in mat)
        material.SetTexture(propertyID, tex);
    }
  }
}

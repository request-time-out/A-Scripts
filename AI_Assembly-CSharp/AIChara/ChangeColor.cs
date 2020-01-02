// Decompiled with JetBrains decompiler
// Type: AIChara.ChangeColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public class ChangeColor
  {
    public static void SetColor(
      string propertyName,
      Color color,
      int idx,
      params Renderer[] arrRend)
    {
      ChangeColor.SetColor(Shader.PropertyToID(propertyName), color, idx, arrRend);
    }

    public static void SetColor(int propertyID, Color color, int idx, params Renderer[] arrRend)
    {
      if (arrRend == null || arrRend.Length == 0)
        return;
      foreach (Renderer renderer in arrRend)
      {
        if (idx < renderer.get_materials().Length)
        {
          Material material = renderer.get_materials()[idx];
          if (Object.op_Inequality((Object) null, (Object) material))
            material.SetColor(propertyID, color);
        }
      }
    }

    public static void SetColor(string propertyName, Color color, params Material[] mat)
    {
      ChangeColor.SetColor(Shader.PropertyToID(propertyName), color, mat);
    }

    public static void SetColor(int propertyID, Color color, params Material[] mat)
    {
      if (mat == null || mat.Length == 0)
        return;
      foreach (Material material in mat)
        material.SetColor(propertyID, color);
    }
  }
}

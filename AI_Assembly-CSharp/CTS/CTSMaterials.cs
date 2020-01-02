// Decompiled with JetBrains decompiler
// Type: CTS.CTSMaterials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CTS
{
  public static class CTSMaterials
  {
    private static Dictionary<string, Material> m_materialLookup = new Dictionary<string, Material>();

    public static Material GetMaterial(string shaderType, CTSProfile profile)
    {
      Material material1;
      if (profile.m_useMaterialControlBlock && CTSMaterials.m_materialLookup.TryGetValue(shaderType + ":" + ((Object) profile).get_name(), out material1))
        return material1;
      Shader shader = CTSShaders.GetShader(shaderType);
      if (Object.op_Equality((Object) shader, (Object) null))
      {
        Debug.LogErrorFormat("Could not create CTS material for shader : {0}. Make sure you add your CTS shader is pre-loaded!", new object[1]
        {
          (object) shaderType
        });
        return (Material) null;
      }
      Stopwatch stopwatch = Stopwatch.StartNew();
      Material material2 = new Material(shader);
      ((Object) material2).set_name(shaderType + ":" + ((Object) profile).get_name());
      if (profile.m_useMaterialControlBlock)
      {
        ((Object) material2).set_hideFlags((HideFlags) 52);
        CTSMaterials.m_materialLookup.Add(((Object) material2).get_name(), material2);
      }
      if (stopwatch.ElapsedMilliseconds <= 5L)
        ;
      return material2;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: CTS.CTSShaders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CTS
{
  public static class CTSShaders
  {
    private static Dictionary<string, Shader> m_shaderLookup = new Dictionary<string, Shader>();

    static CTSShaders()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      Shader shader1 = Shader.Find("CTS/CTS Terrain Shader Lite");
      if (Object.op_Inequality((Object) shader1, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Lite", shader1);
      Shader shader2 = Shader.Find("CTS/CTS Terrain Shader Basic");
      if (Object.op_Inequality((Object) shader2, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Basic", shader2);
      Shader shader3 = Shader.Find("CTS/CTS Terrain Shader Basic CutOut");
      if (Object.op_Inequality((Object) shader3, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Basic CutOut", shader3);
      Shader shader4 = Shader.Find("CTS/CTS Terrain Shader Advanced");
      if (Object.op_Inequality((Object) shader4, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Advanced", shader4);
      Shader shader5 = Shader.Find("CTS/CTS Terrain Shader Advanced CutOut");
      if (Object.op_Inequality((Object) shader5, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Advanced CutOut", shader5);
      Shader shader6 = Shader.Find("CTS/CTS Terrain Shader Advanced Tess");
      if (Object.op_Inequality((Object) shader6, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Advanced Tess", shader6);
      Shader shader7 = Shader.Find("CTS/CTS Terrain Shader Advanced Tess CutOut");
      if (Object.op_Inequality((Object) shader7, (Object) null))
        CTSShaders.m_shaderLookup.Add("CTS/CTS Terrain Shader Advanced Tess CutOut", shader7);
      if (stopwatch.ElapsedMilliseconds <= 0L)
        ;
    }

    public static Shader GetShader(string shaderType)
    {
      Shader shader;
      if (CTSShaders.m_shaderLookup.TryGetValue(shaderType, out shader))
        return shader;
      Debug.LogErrorFormat("Could not load CTS shader : {0}. Make sure you add your CTS shader to pre-loaded assets!", new object[1]
      {
        (object) shaderType
      });
      return (Shader) null;
    }
  }
}

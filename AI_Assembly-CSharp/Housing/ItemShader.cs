// Decompiled with JetBrains decompiler
// Type: Housing.ItemShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Housing
{
  [DefaultExecutionOrder(-5)]
  public static class ItemShader
  {
    static ItemShader()
    {
      ItemShader._Color = Shader.PropertyToID(nameof (_Color));
      ItemShader._Color2 = Shader.PropertyToID(nameof (_Color2));
      ItemShader._Color3 = Shader.PropertyToID(nameof (_Color3));
      ItemShader._Color4 = Shader.PropertyToID(nameof (_Color4));
      ItemShader._EmissionColor = Shader.PropertyToID(nameof (_EmissionColor));
      ItemShader._EmissionPower = Shader.PropertyToID(nameof (_EmissionPower));
    }

    public static int _Color { get; private set; }

    public static int _Color2 { get; private set; }

    public static int _Color3 { get; private set; }

    public static int _Color4 { get; private set; }

    public static int _EmissionColor { get; private set; }

    public static int _EmissionPower { get; private set; }
  }
}

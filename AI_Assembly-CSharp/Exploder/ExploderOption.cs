// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class ExploderOption : MonoBehaviour
  {
    public bool Plane2D;
    public Color CrossSectionVertexColor;
    public Vector4 CrossSectionUV;
    public bool SplitMeshIslands;
    public bool UseLocalForce;
    public float Force;
    public Material FragmentMaterial;

    public ExploderOption()
    {
      base.\u002Ector();
    }

    public void DuplicateSettings(ExploderOption options)
    {
      options.Plane2D = this.Plane2D;
      options.CrossSectionVertexColor = this.CrossSectionVertexColor;
      options.CrossSectionUV = this.CrossSectionUV;
      options.SplitMeshIslands = this.SplitMeshIslands;
      options.UseLocalForce = this.UseLocalForce;
      options.Force = this.Force;
      options.FragmentMaterial = this.FragmentMaterial;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: SplineProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu(fileName = "SplineProfile", menuName = "SplineProfile", order = 1)]
public class SplineProfile : ScriptableObject
{
  public Material splineMaterial;
  public AnimationCurve meshCurve;
  public float minVal;
  public float maxVal;
  public int vertsInShape;
  public float traingleDensity;
  public float uvScale;
  public bool uvRotation;
  public AnimationCurve flowFlat;
  public AnimationCurve flowWaterfall;

  public SplineProfile()
  {
    base.\u002Ector();
  }
}

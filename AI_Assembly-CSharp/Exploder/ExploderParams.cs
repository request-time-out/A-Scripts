// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderParams
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class ExploderParams
  {
    public Vector3 Position;
    public Vector3 ForceVector;
    public Vector3 CubeRadius;
    public Vector3 HitPosition;
    public Vector3 ShotDir;
    public float Force;
    public float FrameBudget;
    public float Radius;
    public float BulletSize;
    public int id;
    public int TargetFragments;
    public int FragmentPoolSize;
    public ExploderObject.ThreadOptions ThreadOptions;
    public ExploderObject.OnExplosion Callback;
    public FragmentOption FragmentOptions;
    public FragmentDeactivation FragmentDeactivation;
    public FragmentSFX FragmentSFX;
    public ExploderObject.CuttingStyleOption CuttingStyle;
    public GameObject[] Targets;
    public GameObject ExploderGameObject;
    public bool UseCubeRadius;
    public bool DontUseTag;
    public bool UseForceVector;
    public bool ExplodeSelf;
    public bool HideSelf;
    public bool DestroyOriginalObject;
    public bool SplitMeshIslands;
    public bool Use2DCollision;
    public bool DisableRadiusScan;
    public bool UniformFragmentDistribution;
    public bool DisableTriangulation;
    public bool Crack;
    public bool processing;

    public ExploderParams(ExploderObject exploder)
    {
      this.Position = ExploderUtils.GetCentroid(((Component) exploder).get_gameObject());
      this.DontUseTag = exploder.DontUseTag;
      this.Radius = exploder.Radius;
      this.UseCubeRadius = exploder.UseCubeRadius;
      this.CubeRadius = exploder.CubeRadius;
      this.ForceVector = exploder.ForceVector;
      this.UseForceVector = exploder.UseForceVector;
      this.Force = exploder.Force;
      this.FrameBudget = exploder.FrameBudget;
      this.TargetFragments = exploder.TargetFragments;
      this.ExplodeSelf = exploder.ExplodeSelf;
      this.HideSelf = exploder.HideSelf;
      this.ThreadOptions = exploder.ThreadOption;
      this.DestroyOriginalObject = exploder.DestroyOriginalObject;
      this.SplitMeshIslands = exploder.SplitMeshIslands;
      this.FragmentOptions = exploder.FragmentOptions.Clone();
      this.FragmentDeactivation = exploder.FragmentDeactivation.Clone();
      this.FragmentSFX = exploder.FragmentSFX.Clone();
      this.Use2DCollision = exploder.Use2DCollision;
      this.FragmentPoolSize = exploder.FragmentPoolSize;
      this.DisableRadiusScan = exploder.DisableRadiusScan;
      this.UniformFragmentDistribution = exploder.UniformFragmentDistribution;
      this.DisableTriangulation = exploder.DisableTriangulation;
      this.ExploderGameObject = ((Component) exploder).get_gameObject();
      this.CuttingStyle = exploder.CuttingStyle;
    }
  }
}

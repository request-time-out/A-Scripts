// Decompiled with JetBrains decompiler
// Type: Exploder.FragmentOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Exploder
{
  [Serializable]
  public class FragmentOption
  {
    public bool ExplodeFragments = true;
    public GameObject FragmentPrefab;
    public bool FreezePositionX;
    public bool FreezePositionY;
    public bool FreezePositionZ;
    public bool FreezeRotationX;
    public bool FreezeRotationY;
    public bool FreezeRotationZ;
    public string Layer;
    public float MaxVelocity;
    public bool InheritParentPhysicsProperty;
    public float Mass;
    public bool UseGravity;
    public bool DisableColliders;
    public bool MeshColliders;
    public float AngularVelocity;
    public float MaxAngularVelocity;
    public Vector3 AngularVelocityVector;
    public bool RandomAngularVelocityVector;
    public Material FragmentMaterial;

    public FragmentOption Clone()
    {
      return new FragmentOption()
      {
        ExplodeFragments = this.ExplodeFragments,
        FreezePositionX = this.FreezePositionX,
        FreezePositionY = this.FreezePositionY,
        FreezePositionZ = this.FreezePositionZ,
        FreezeRotationX = this.FreezeRotationX,
        FreezeRotationY = this.FreezeRotationY,
        FreezeRotationZ = this.FreezeRotationZ,
        Layer = this.Layer,
        Mass = this.Mass,
        DisableColliders = this.DisableColliders,
        MeshColliders = this.MeshColliders,
        UseGravity = this.UseGravity,
        MaxVelocity = this.MaxVelocity,
        MaxAngularVelocity = this.MaxAngularVelocity,
        InheritParentPhysicsProperty = this.InheritParentPhysicsProperty,
        AngularVelocity = this.AngularVelocity,
        AngularVelocityVector = this.AngularVelocityVector,
        RandomAngularVelocityVector = this.RandomAngularVelocityVector,
        FragmentMaterial = this.FragmentMaterial
      };
    }
  }
}

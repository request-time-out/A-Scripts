// Decompiled with JetBrains decompiler
// Type: NavMeshSurfaceModifierVolumeTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

[TestFixture]
public class NavMeshSurfaceModifierVolumeTests
{
  private NavMeshSurface surface;
  private NavMeshModifierVolume modifier;

  [SetUp]
  public void CreatePlaneAndModifierVolume()
  {
    this.surface = (NavMeshSurface) GameObject.CreatePrimitive((PrimitiveType) 4).AddComponent<NavMeshSurface>();
    this.modifier = (NavMeshModifierVolume) new GameObject().AddComponent<NavMeshModifierVolume>();
  }

  [TearDown]
  public void DestroyPlaneAndModifierVolume()
  {
    Object.DestroyImmediate((Object) ((Component) this.surface).get_gameObject());
    Object.DestroyImmediate((Object) ((Component) this.modifier).get_gameObject());
  }

  [Test]
  public void AreaAffectsNavMeshOverlapping()
  {
    this.modifier.center = Vector3.get_zero();
    this.modifier.size = Vector3.get_one();
    this.modifier.area = 4;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(16, 0));
  }

  [Test]
  public void AreaDoesNotAffectsNavMeshWhenNotOverlapping()
  {
    this.modifier.center = Vector3.op_Multiply(1.1f, Vector3.get_right());
    this.modifier.size = Vector3.get_one();
    this.modifier.area = 4;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(1, 0));
  }

  [Test]
  public void BuildUsesOnlyIncludedModifierVolume()
  {
    this.modifier.center = Vector3.get_zero();
    this.modifier.size = Vector3.get_one();
    this.modifier.area = 4;
    ((Component) this.modifier).get_gameObject().set_layer(7);
    this.surface.layerMask = LayerMask.op_Implicit(-129);
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(1, 0));
  }
}

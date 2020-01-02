// Decompiled with JetBrains decompiler
// Type: NavMeshSurfaceTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using NUnit.Framework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

[TestFixture]
public class NavMeshSurfaceTests
{
  private GameObject plane;
  private NavMeshSurface surface;

  [SetUp]
  public void CreatePlaneWithSurface()
  {
    this.plane = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.surface = (NavMeshSurface) new GameObject().AddComponent<NavMeshSurface>();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [TearDown]
  public void DestroyPlaneWithSurface()
  {
    Object.DestroyImmediate((Object) this.plane);
    Object.DestroyImmediate((Object) ((Component) this.surface).get_gameObject());
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void NavMeshIsAvailableAfterBuild()
  {
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void NavMeshCanBeRemovedAndAdded()
  {
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    this.surface.RemoveData();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    this.surface.AddData();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void NavMeshIsNotAvailableWhenDisabled()
  {
    this.surface.BuildNavMesh();
    ((Behaviour) this.surface).set_enabled(false);
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    ((Behaviour) this.surface).set_enabled(true);
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void CanBuildWithCustomArea()
  {
    this.surface.defaultArea = 4;
    int areaMask = 16;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(areaMask, 0));
  }

  [Test]
  public void CanBuildWithCustomAgentTypeID()
  {
    this.surface.agentTypeID = 1234;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 1234));
  }

  [Test]
  public void CanBuildCollidersAndIgnoreRenderMeshes()
  {
    ((Renderer) this.plane.GetComponent<MeshRenderer>()).set_enabled(false);
    this.surface.useGeometry = (NavMeshCollectGeometry) 1;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    this.surface.useGeometry = (NavMeshCollectGeometry) 0;
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void CanBuildRenderMeshesAndIgnoreColliders()
  {
    ((Collider) this.plane.GetComponent<Collider>()).set_enabled(false);
    this.surface.useGeometry = (NavMeshCollectGeometry) 1;
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    this.surface.useGeometry = (NavMeshCollectGeometry) 0;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void BuildIgnoresGeometryOutsideBounds()
  {
    this.surface.collectObjects = CollectObjects.Volume;
    this.surface.center = new Vector3(20f, 0.0f, 0.0f);
    this.surface.size = new Vector3(10f, 10f, 10f);
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void BuildIgnoresGeometrySiblings()
  {
    this.surface.collectObjects = CollectObjects.Children;
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void BuildUsesOnlyIncludedLayers()
  {
    this.plane.set_layer(4);
    this.surface.layerMask = LayerMask.op_Implicit(-17);
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void DefaultSettingsMatchBuiltinSettings()
  {
    Assert.AreEqual((object) NavMesh.GetSettingsByIndex(0), (object) this.surface.GetBuildSettings());
  }

  [Test]
  public void ActiveSurfacesContainsOnlyActiveAndEnabledSurface()
  {
    Assert.IsTrue(NavMeshSurface.activeSurfaces.Contains(this.surface));
    Assert.AreEqual((object) 1, (object) NavMeshSurface.activeSurfaces.Count);
    ((Behaviour) this.surface).set_enabled(false);
    Assert.IsFalse(NavMeshSurface.activeSurfaces.Contains(this.surface));
    Assert.AreEqual((object) 0, (object) NavMeshSurface.activeSurfaces.Count);
    ((Behaviour) this.surface).set_enabled(true);
    ((Component) this.surface).get_gameObject().SetActive(false);
    Assert.IsFalse(NavMeshSurface.activeSurfaces.Contains(this.surface));
    Assert.AreEqual((object) 0, (object) NavMeshSurface.activeSurfaces.Count);
  }

  [DebuggerHidden]
  [UnityTest]
  public IEnumerator NavMeshMovesToSurfacePositionNextFrame()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new NavMeshSurfaceTests.\u003CNavMeshMovesToSurfacePositionNextFrame\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  [UnityTest]
  public IEnumerator UpdatingAndAddingNavMesh()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new NavMeshSurfaceTests.\u003CUpdatingAndAddingNavMesh\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  public static bool HasNavMeshAtOrigin(int areaMask = -1, int agentTypeID = 0)
  {
    NavMeshHit navMeshHit = (NavMeshHit) null;
    NavMeshQueryFilter navMeshQueryFilter = (NavMeshQueryFilter) null;
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_areaMask(areaMask);
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_agentTypeID(agentTypeID);
    return NavMesh.SamplePosition(Vector3.get_zero(), ref navMeshHit, 0.1f, navMeshQueryFilter);
  }
}

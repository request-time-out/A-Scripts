// Decompiled with JetBrains decompiler
// Type: NavMeshSurfaceModifierTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

[TestFixture]
public class NavMeshSurfaceModifierTests
{
  private NavMeshSurface surface;
  private NavMeshModifier modifier;

  [SetUp]
  public void CreatePlaneWithModifier()
  {
    GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.surface = (NavMeshSurface) primitive.AddComponent<NavMeshSurface>();
    this.modifier = (NavMeshModifier) primitive.AddComponent<NavMeshModifier>();
  }

  [TearDown]
  public void DestroyPlaneWithModifier()
  {
    Object.DestroyImmediate((Object) ((Component) this.modifier).get_gameObject());
  }

  [Test]
  public void ModifierIgnoreAffectsSelf()
  {
    this.modifier.ignoreFromBuild = true;
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
  }

  [Test]
  public void ModifierIgnoreAffectsChild()
  {
    this.modifier.ignoreFromBuild = true;
    ((Renderer) ((Component) this.modifier).GetComponent<MeshRenderer>()).set_enabled(false);
    GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 4);
    primitive.get_transform().SetParent(((Component) this.modifier).get_transform());
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    Object.DestroyImmediate((Object) primitive);
  }

  [Test]
  public void ModifierIgnoreDoesNotAffectSibling()
  {
    this.modifier.ignoreFromBuild = true;
    ((Renderer) ((Component) this.modifier).GetComponent<MeshRenderer>()).set_enabled(false);
    GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(-1, 0));
    Object.DestroyImmediate((Object) primitive);
  }

  [Test]
  public void ModifierOverrideAreaAffectsSelf()
  {
    this.modifier.area = 4;
    this.modifier.overrideArea = true;
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(16, 0));
  }

  [Test]
  public void ModifierOverrideAreaAffectsChild()
  {
    this.modifier.area = 4;
    this.modifier.overrideArea = true;
    ((Renderer) ((Component) this.modifier).GetComponent<MeshRenderer>()).set_enabled(false);
    GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 4);
    primitive.get_transform().SetParent(((Component) this.modifier).get_transform());
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(16, 0));
    Object.DestroyImmediate((Object) primitive);
  }

  [Test]
  public void ModifierOverrideAreaDoesNotAffectSibling()
  {
    this.modifier.area = 4;
    this.modifier.overrideArea = true;
    ((Renderer) ((Component) this.modifier).GetComponent<MeshRenderer>()).set_enabled(false);
    GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.surface.BuildNavMesh();
    Assert.IsTrue(NavMeshSurfaceTests.HasNavMeshAtOrigin(1, 0));
    Object.DestroyImmediate((Object) primitive);
  }
}

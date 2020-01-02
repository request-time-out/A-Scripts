// Decompiled with JetBrains decompiler
// Type: NavMeshSurfaceLinkTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using NUnit.Framework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

[TestFixture]
public class NavMeshSurfaceLinkTests
{
  public GameObject plane1;
  public GameObject plane2;
  public NavMeshLink link;
  public NavMeshSurface surface;

  [SetUp]
  public void CreatesPlanesAndLink()
  {
    this.plane1 = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.plane2 = GameObject.CreatePrimitive((PrimitiveType) 4);
    this.plane1.get_transform().set_position(Vector3.op_Multiply(11f, Vector3.get_right()));
    this.surface = (NavMeshSurface) new GameObject().AddComponent<NavMeshSurface>();
    this.surface.BuildNavMesh();
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, -1, 0));
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane2, this.plane1, -1, 0));
    this.link = (NavMeshLink) new GameObject().AddComponent<NavMeshLink>();
    this.link.startPoint = this.plane1.get_transform().get_position();
    this.link.endPoint = this.plane2.get_transform().get_position();
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, -1, 0));
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane2, this.plane1, -1, 0));
  }

  [TearDown]
  public void DestroyPlanesAndLink()
  {
    Object.DestroyImmediate((Object) ((Component) this.surface).get_gameObject());
    Object.DestroyImmediate((Object) ((Component) this.link).get_gameObject());
    Object.DestroyImmediate((Object) this.plane1);
    Object.DestroyImmediate((Object) this.plane2);
  }

  [Test]
  public void NavMeshLinkCanConnectTwoSurfaces()
  {
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, -1, 0));
  }

  [Test]
  public void DisablingBidirectionalMakesTheLinkOneWay()
  {
    this.link.bidirectional = false;
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, -1, 0));
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane2, this.plane1, -1, 0));
  }

  [Test]
  public void ChangingAreaTypeCanBlockPath()
  {
    int areaMask = -17;
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, areaMask, 0));
    this.link.area = 4;
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnecting(this.plane1, this.plane2, areaMask, 0));
  }

  [Test]
  public void EndpointsMoveRelativeToLinkOnUpdate()
  {
    Transform transform = ((Component) this.link).get_transform();
    transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.get_forward()));
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, Vector3.op_Addition(this.plane1.get_transform().get_position(), Vector3.get_forward()), -1, 0));
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, Vector3.op_Addition(this.plane2.get_transform().get_position(), Vector3.get_forward()), -1, 0));
    this.link.UpdateLink();
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, Vector3.op_Addition(this.plane1.get_transform().get_position(), Vector3.get_forward()), -1, 0));
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, Vector3.op_Addition(this.plane2.get_transform().get_position(), Vector3.get_forward()), -1, 0));
  }

  [DebuggerHidden]
  [UnityTest]
  public IEnumerator EndpointsMoveRelativeToLinkNextFrameWhenAutoUpdating()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new NavMeshSurfaceLinkTests.\u003CEndpointsMoveRelativeToLinkNextFrameWhenAutoUpdating\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [Test]
  public void ChangingCostModifierAffectsRoute()
  {
    NavMeshLink link = this.link;
    link.startPoint = this.plane1.get_transform().get_position();
    link.endPoint = Vector3.op_Addition(this.plane2.get_transform().get_position(), Vector3.get_forward());
    NavMeshLink navMeshLink = (NavMeshLink) ((Component) this.link).get_gameObject().AddComponent<NavMeshLink>();
    navMeshLink.startPoint = this.plane1.get_transform().get_position();
    navMeshLink.endPoint = Vector3.op_Subtraction(this.plane2.get_transform().get_position(), Vector3.get_forward());
    link.costModifier = -1;
    navMeshLink.costModifier = 100;
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, link.endPoint, -1, 0));
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, navMeshLink.endPoint, -1, 0));
    link.costModifier = 100;
    navMeshLink.costModifier = -1;
    Assert.IsFalse(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, link.endPoint, -1, 0));
    Assert.IsTrue(NavMeshSurfaceLinkTests.HasPathConnectingViaPoint(this.plane1, this.plane2, navMeshLink.endPoint, -1, 0));
  }

  public static bool HasPathConnecting(GameObject a, GameObject b, int areaMask = -1, int agentTypeID = 0)
  {
    NavMeshPath navMeshPath = new NavMeshPath();
    NavMeshQueryFilter navMeshQueryFilter = (NavMeshQueryFilter) null;
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_areaMask(areaMask);
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_agentTypeID(agentTypeID);
    NavMesh.CalculatePath(a.get_transform().get_position(), b.get_transform().get_position(), navMeshQueryFilter, navMeshPath);
    return navMeshPath.get_status() == 0;
  }

  public static bool HasPathConnectingViaPoint(
    GameObject a,
    GameObject b,
    Vector3 point,
    int areaMask = -1,
    int agentTypeID = 0)
  {
    NavMeshPath navMeshPath = new NavMeshPath();
    NavMeshQueryFilter navMeshQueryFilter = (NavMeshQueryFilter) null;
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_areaMask(areaMask);
    ((NavMeshQueryFilter) ref navMeshQueryFilter).set_agentTypeID(agentTypeID);
    NavMesh.CalculatePath(a.get_transform().get_position(), b.get_transform().get_position(), navMeshQueryFilter, navMeshPath);
    if (navMeshPath.get_status() != null)
      return false;
    for (int index = 0; index < navMeshPath.get_corners().Length; ++index)
    {
      if ((double) Vector3.Distance(navMeshPath.get_corners()[index], point) < 0.100000001490116)
        return true;
    }
    return false;
  }
}

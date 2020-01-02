// Decompiled with JetBrains decompiler
// Type: AgentLinkMover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
  public OffMeshLinkMoveMethod m_Method;
  public AnimationCurve m_Curve;

  public AgentLinkMover()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AgentLinkMover.\u003CStart\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator NormalSpeed(NavMeshAgent agent)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AgentLinkMover.\u003CNormalSpeed\u003Ec__Iterator1()
    {
      agent = agent
    };
  }

  [DebuggerHidden]
  private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AgentLinkMover.\u003CParabola\u003Ec__Iterator2()
    {
      agent = agent,
      height = height,
      duration = duration
    };
  }

  [DebuggerHidden]
  private IEnumerator Curve(NavMeshAgent agent, float duration)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AgentLinkMover.\u003CCurve\u003Ec__Iterator3()
    {
      agent = agent,
      duration = duration,
      \u0024this = this
    };
  }
}

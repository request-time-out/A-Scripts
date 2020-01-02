// Decompiled with JetBrains decompiler
// Type: RandomWalk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class RandomWalk : MonoBehaviour
{
  public float m_Range;
  private NavMeshAgent m_agent;

  public RandomWalk()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.m_agent = (NavMeshAgent) ((Component) this).GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    if (this.m_agent.get_pathPending() || (double) this.m_agent.get_remainingDistance() > 0.100000001490116)
      return;
    this.m_agent.set_destination(Vector2.op_Implicit(Vector2.op_Multiply(this.m_Range, Random.get_insideUnitCircle())));
  }
}

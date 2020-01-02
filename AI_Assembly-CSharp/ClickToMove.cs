// Decompiled with JetBrains decompiler
// Type: ClickToMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
  private NavMeshAgent m_Agent;
  private RaycastHit m_HitInfo;

  public ClickToMove()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.m_Agent = (NavMeshAgent) ((Component) this).GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    if (!Input.GetMouseButtonDown(0) || Input.GetKey((KeyCode) 304))
      return;
    Ray ray = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
    if (!Physics.Raycast(((Ray) ref ray).get_origin(), ((Ray) ref ray).get_direction(), ref this.m_HitInfo))
      return;
    this.m_Agent.set_destination(((RaycastHit) ref this.m_HitInfo).get_point());
  }
}

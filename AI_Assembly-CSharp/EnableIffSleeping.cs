// Decompiled with JetBrains decompiler
// Type: EnableIffSleeping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnableIffSleeping : MonoBehaviour
{
  public Behaviour m_Behaviour;
  private Rigidbody m_Rigidbody;

  public EnableIffSleeping()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.m_Rigidbody = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.m_Rigidbody, (Object) null) || Object.op_Equality((Object) this.m_Behaviour, (Object) null))
      return;
    if (this.m_Rigidbody.IsSleeping() && !this.m_Behaviour.get_enabled())
      this.m_Behaviour.set_enabled(true);
    if (this.m_Rigidbody.IsSleeping() || !this.m_Behaviour.get_enabled())
      return;
    this.m_Behaviour.set_enabled(false);
  }
}

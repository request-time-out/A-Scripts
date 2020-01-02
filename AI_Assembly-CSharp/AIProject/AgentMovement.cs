// Decompiled with JetBrains decompiler
// Type: AIProject.AgentMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public abstract class AgentMovement : AgentAction
  {
    protected Vector3 _moveDirection = Vector3.get_zero();
    protected Vector3 _moveDirectionVelocity = Vector3.get_zero();

    protected abstract bool SetDestination(Vector3 destination);

    protected abstract void UpdateRotation(bool update);

    protected abstract bool HasPath();

    protected abstract Vector3 Velocity();

    protected abstract bool HasArrived();

    protected abstract void Stop();
  }
}

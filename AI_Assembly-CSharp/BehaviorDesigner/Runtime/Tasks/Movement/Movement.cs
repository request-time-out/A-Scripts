// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Movement.Movement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
  public abstract class Movement : Action
  {
    protected Movement()
    {
      base.\u002Ector();
    }

    protected abstract bool SetDestination(Vector3 destination);

    protected abstract void UpdateRotation(bool update);

    protected abstract bool HasPath();

    protected abstract Vector3 Velocity();

    protected abstract bool HasArrived();

    protected abstract void Stop();
  }
}

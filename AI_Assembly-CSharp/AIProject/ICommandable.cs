// Decompiled with JetBrains decompiler
// Type: AIProject.ICommandable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public interface ICommandable
  {
    int InstanceID { get; }

    bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward);

    bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB);

    bool IsImpossible { get; }

    bool SetImpossible(bool value, Actor actor);

    bool IsNeutralCommand { get; }

    Vector3 Position { get; }

    Vector3 CommandCenter { get; }

    CommandLabel.CommandInfo[] Labels { get; }

    CommandLabel.CommandInfo[] DateLabels { get; }

    ObjectLayer Layer { get; }

    CommandType CommandType { get; }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.IState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

namespace AIProject
{
  public interface IState
  {
    void Awake(Actor actor);

    void Release(Actor actor, EventType type);

    void AfterUpdate(Actor actor, Actor.InputInfo info);

    void Update(Actor actor, ref Actor.InputInfo info);

    void FixedUpdate(Actor actor, Actor.InputInfo info);

    void OnAnimatorStateEnter(ActorController control, AnimatorStateInfo stateInfo);

    void OnAnimatorStateExit(ActorController control, AnimatorStateInfo stateInfo);

    IEnumerator OnEnumerate(Actor actor);

    IEnumerator End(Actor actor);
  }
}

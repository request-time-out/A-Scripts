// Decompiled with JetBrains decompiler
// Type: AIProject.StateBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject
{
  public abstract class StateBase : IState
  {
    public const string NormalAnimationStateName = "Locomotion";

    public abstract void Awake(Actor actor);

    public abstract void Release(Actor actor, EventType type);

    public abstract void AfterUpdate(Actor actor, Actor.InputInfo info);

    public abstract void Update(Actor actor, ref Actor.InputInfo info);

    public abstract void FixedUpdate(Actor actor, Actor.InputInfo info);

    public abstract void OnAnimatorStateEnter(ActorController control, AnimatorStateInfo stateInfo);

    public abstract void OnAnimatorStateExit(ActorController control, AnimatorStateInfo stateInfo);

    [DebuggerHidden]
    public virtual IEnumerator OnEnumerate(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StateBase.\u003COnEnumerate\u003Ec__Iterator0 enumerateCIterator0 = new StateBase.\u003COnEnumerate\u003Ec__Iterator0();
      return (IEnumerator) enumerateCIterator0;
    }

    public abstract IEnumerator End(Actor actor);
  }
}

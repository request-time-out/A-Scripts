// Decompiled with JetBrains decompiler
// Type: AIProject.TutorialBehaviorTreeResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class TutorialBehaviorTreeResources : SerializedMonoBehaviour
  {
    private AgentBehaviorTree _current;
    private Dictionary<Tutorial.ActionType, AgentBehaviorTree> _behaviorTreeTable;
    private IDisposable _nextDisposable;

    public TutorialBehaviorTreeResources()
    {
      base.\u002Ector();
    }

    public AgentBehaviorTree Current
    {
      get
      {
        return this._current;
      }
    }

    public Tutorial.ActionType Mode { get; private set; }

    public AgentActor SourceAgent { get; set; }

    public AgentBehaviorTree GetBehaviorTree(Tutorial.ActionType mode)
    {
      return (AgentBehaviorTree) null;
    }

    public void Initialize(AgentActor actor)
    {
      this.SourceAgent = actor;
      this._behaviorTreeTable.Clear();
      foreach (Tutorial.ActionType actionType in Enum.GetValues(typeof (Tutorial.ActionType)))
      {
        AgentBehaviorTree tutorialBehavior = Singleton<Resources>.Instance.BehaviorTree.GetTutorialBehavior(actionType);
        if (!Object.op_Equality((Object) tutorialBehavior, (Object) null))
        {
          AgentBehaviorTree agentBehaviorTree = (AgentBehaviorTree) Object.Instantiate<AgentBehaviorTree>((M0) tutorialBehavior);
          ((Component) agentBehaviorTree).get_transform().SetParent(((Component) this).get_transform(), false);
          ((Component) agentBehaviorTree).get_transform().SetPositionAndRotation(Vector3.get_zero(), Quaternion.get_identity());
          this._behaviorTreeTable[actionType] = agentBehaviorTree;
          agentBehaviorTree.SourceAgent = actor;
          agentBehaviorTree.set_StartWhenEnabled(false);
        }
      }
    }

    public void ChangeMode(Tutorial.ActionType mode)
    {
      AgentBehaviorTree agentBehaviorTree;
      if (!this._behaviorTreeTable.TryGetValue(mode, out agentBehaviorTree) || Object.op_Equality((Object) agentBehaviorTree, (Object) null) || Object.op_Equality((Object) this._current, (Object) agentBehaviorTree))
        return;
      this.Mode = mode;
      if (this._current != null)
        this._current.DisableBehavior(false);
      this._current = agentBehaviorTree;
      if (this._nextDisposable != null)
        this._nextDisposable.Dispose();
      this._nextDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ => this.Current.EnableBehavior()));
    }

    private void OnEnable()
    {
      if (this._current == null)
        return;
      this._current.EnableBehavior();
    }

    private void OnDisable()
    {
      if (this._current == null)
        return;
      this._current.DisableBehavior(true);
    }
  }
}

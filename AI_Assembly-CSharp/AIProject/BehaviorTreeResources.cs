// Decompiled with JetBrains decompiler
// Type: AIProject.BehaviorTreeResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class BehaviorTreeResources : SerializedMonoBehaviour
  {
    [SerializeField]
    [HideInInspector]
    private AgentBehaviorTree _current;
    [SerializeField]
    private AgentActor _sourceAgent;
    [SerializeField]
    private Dictionary<Desire.ActionType, AgentBehaviorTree> _behaviorTreeTable;

    public BehaviorTreeResources()
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

    public Desire.ActionType Mode { get; private set; }

    public AgentActor SourceAgent
    {
      get
      {
        return this._sourceAgent;
      }
    }

    public AgentBehaviorTree GetBehaviorTree(Desire.ActionType mode)
    {
      AgentBehaviorTree agentBehaviorTree;
      return this._behaviorTreeTable.TryGetValue(mode, out agentBehaviorTree) ? agentBehaviorTree : (AgentBehaviorTree) null;
    }

    public void Initialize()
    {
      this._behaviorTreeTable.Clear();
      foreach (Desire.ActionType actionType in Enum.GetValues(typeof (Desire.ActionType)))
      {
        AgentBehaviorTree behavior = Singleton<Resources>.Instance.BehaviorTree.GetBehavior(actionType);
        if (Object.op_Inequality((Object) behavior, (Object) null))
        {
          AgentBehaviorTree agentBehaviorTree = (AgentBehaviorTree) Object.Instantiate<AgentBehaviorTree>((M0) behavior);
          ((Component) agentBehaviorTree).get_transform().SetParent(((Component) this).get_transform(), false);
          ((Component) agentBehaviorTree).get_transform().SetPositionAndRotation(Vector3.get_zero(), Quaternion.get_identity());
          this._behaviorTreeTable[actionType] = agentBehaviorTree;
          agentBehaviorTree.SourceAgent = this._sourceAgent;
        }
      }
      foreach (Behavior behavior in this._behaviorTreeTable.Values)
        behavior.set_StartWhenEnabled(false);
    }

    public void ChangeMode(Desire.ActionType mode)
    {
      AgentBehaviorTree agentBehaviorTree;
      if (!this._behaviorTreeTable.TryGetValue(mode, out agentBehaviorTree) || Object.op_Equality((Object) this.Current, (Object) agentBehaviorTree))
        return;
      Debug.Log((object) string.Format("[{0}] モード変更 {1} → {2}", (object) ((Object) ((Component) this.SourceAgent).get_gameObject()).get_name(), (object) this.Mode, (object) mode));
      this.Mode = mode;
      this.Current?.DisableBehavior(false);
      this._current = agentBehaviorTree;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ => this.Current.EnableBehavior()));
    }

    private void OnEnable()
    {
      this.Current?.EnableBehavior();
    }

    private void OnDisable()
    {
      this.Current?.DisableBehavior(true);
    }

    public void DisableAllBehaviors()
    {
      foreach (KeyValuePair<Desire.ActionType, AgentBehaviorTree> keyValuePair in this._behaviorTreeTable)
      {
        if (!Object.op_Equality((Object) keyValuePair.Value, (Object) null))
          keyValuePair.Value.DisableBehavior();
      }
    }
  }
}

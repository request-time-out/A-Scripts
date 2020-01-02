// Decompiled with JetBrains decompiler
// Type: AIProject.AgentBehaviorTree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AIProject
{
  public class AgentBehaviorTree : Behavior
  {
    [SerializeField]
    private AgentActor _sourceAgent;
    private bool _initialized;
    private bool _isPause;
    private ExternalBehavior _externalBehavior;

    public AgentBehaviorTree()
    {
      base.\u002Ector();
    }

    public AgentActor SourceAgent
    {
      get
      {
        return this._sourceAgent;
      }
      set
      {
        this._sourceAgent = value;
      }
    }

    public ExternalBehavior ExternalBehavior
    {
      get
      {
        return this._externalBehavior;
      }
      set
      {
        if (Object.op_Equality((Object) this._externalBehavior, (Object) value))
          return;
        if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
          ((BehaviorManager) BehaviorManager.instance).DisableBehavior((Behavior) this);
        if (Object.op_Inequality((Object) value, (Object) null) && value.get_Initialized())
        {
          List<SharedVariable> allVariables = this.GetBehaviorSource().GetAllVariables();
          BehaviorSource behaviorSource = value.get_BehaviorSource();
          behaviorSource.set_HasSerialized(true);
          if (allVariables != null)
          {
            for (int index = 0; index < allVariables.Count; ++index)
            {
              if (allVariables[index] != null)
                behaviorSource.SetVariable(allVariables[index].get_Name(), allVariables[index]);
            }
          }
          this.SetBehaviorSource(behaviorSource);
        }
        else
        {
          this.GetBehaviorSource().set_HasSerialized(false);
          this.set_HasInheritedVariables(false);
        }
        this._initialized = false;
        this._externalBehavior = value;
        if (!this.get_StartWhenEnabled())
          return;
        this.EnableBehavior();
      }
    }

    private void Start()
    {
      if (this.get_StartWhenEnabled())
        this.set_StartWhenEnabled(false);
      base.Start();
    }

    public void OnEnable()
    {
      if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null) && this._isPause)
      {
        ((BehaviorManager) BehaviorManager.instance).EnableBehavior((Behavior) this);
        this._isPause = false;
      }
      else
      {
        if (!this.get_StartWhenEnabled() || !this._initialized)
          return;
        this.EnableBehavior();
      }
    }

    public static AIBehaviorManager CreateAIBehaviorManager()
    {
      if (!Object.op_Equality((Object) BehaviorManager.instance, (Object) null) || !Application.get_isPlaying())
        return (AIBehaviorManager) null;
      GameObject gameObject = new GameObject();
      ((Object) gameObject).set_name("Behavior Manager");
      return (AIBehaviorManager) gameObject.AddComponent<AIBehaviorManager>();
    }

    public void EnableBehavior()
    {
      AgentBehaviorTree.CreateAIBehaviorManager();
      if (Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
        ((BehaviorManager) BehaviorManager.instance).EnableBehavior((Behavior) this);
      BehaviorSource behaviorSource = this.GetBehaviorSource();
      if (this._initialized)
        return;
      for (int index = 0; index < 12; ++index)
        this.get_HasEvent()[index] = this.TaskContainsMethod(((Behavior.EventTypes) index).ToString(), behaviorSource.get_RootTask());
    }

    public void DisableBehavior()
    {
      if (!Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
        return;
      ((BehaviorManager) BehaviorManager.instance).DisableBehavior((Behavior) this, this.get_PauseWhenDisabled());
      this._isPause = this.get_PauseWhenDisabled();
    }

    public void DisableBehavior(bool pause)
    {
      if (!Object.op_Inequality((Object) BehaviorManager.instance, (Object) null))
        return;
      ((BehaviorManager) BehaviorManager.instance).DisableBehavior((Behavior) this, pause);
      this._isPause = pause;
    }

    private bool TaskContainsMethod(string methodName, Task task)
    {
      if (task == null)
        return false;
      MethodInfo method = ((object) task).GetType().GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != (MethodInfo) null && method.DeclaringType.IsAssignableFrom(((object) task).GetType()))
        return true;
      if (task is ParentTask)
      {
        ParentTask parentTask = task as ParentTask;
        if (parentTask.get_Children() != null)
        {
          for (int index = 0; index < parentTask.get_Children().Count; ++index)
          {
            if (this.TaskContainsMethod(methodName, parentTask.get_Children()[index]))
              return true;
          }
        }
      }
      return false;
    }
  }
}

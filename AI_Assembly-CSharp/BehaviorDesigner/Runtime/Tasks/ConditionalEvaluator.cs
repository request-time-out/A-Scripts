// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.ConditionalEvaluator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Evaluates the specified conditional task. If the conditional task returns success then the child task is run and the child status is returned. If the conditional task does not return success then the child task is not run and a failure status is immediately returned.")]
  [TaskIcon("{SkinColor}ConditionalEvaluatorIcon.png")]
  public class ConditionalEvaluator : Decorator
  {
    [Tooltip("Should the conditional task be reevaluated every tick?")]
    public SharedBool reevaluate;
    [InspectTask]
    [Tooltip("The conditional task to evaluate")]
    public Conditional conditionalTask;
    private TaskStatus executionStatus;
    private bool checkConditionalTask;
    private bool conditionalTaskFailed;

    public ConditionalEvaluator()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      if (this.conditionalTask == null)
        return;
      ((Task) this.conditionalTask).set_Owner(((Task) this).get_Owner());
      ((Task) this.conditionalTask).set_GameObject((GameObject) ((Task) this).gameObject);
      ((Task) this.conditionalTask).set_Transform((Transform) ((Task) this).transform);
      ((Task) this.conditionalTask).OnAwake();
    }

    public virtual void OnStart()
    {
      if (this.conditionalTask == null)
        return;
      ((Task) this.conditionalTask).OnStart();
    }

    public virtual bool CanExecute()
    {
      if (this.checkConditionalTask)
      {
        this.checkConditionalTask = false;
        ((Task) this).OnUpdate();
      }
      if (this.conditionalTaskFailed)
        return false;
      return this.executionStatus == null || this.executionStatus == 3;
    }

    public virtual bool CanReevaluate()
    {
      return this.reevaluate.get_Value();
    }

    public virtual TaskStatus OnUpdate()
    {
      TaskStatus taskStatus = ((Task) this.conditionalTask).OnUpdate();
      this.conditionalTaskFailed = this.conditionalTask == null || taskStatus == 1;
      return taskStatus;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      this.executionStatus = childStatus;
    }

    public virtual TaskStatus OverrideStatus()
    {
      return (TaskStatus) 1;
    }

    public virtual TaskStatus OverrideStatus(TaskStatus status)
    {
      return this.conditionalTaskFailed ? (TaskStatus) 1 : status;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
      this.checkConditionalTask = true;
      this.conditionalTaskFailed = false;
      if (this.conditionalTask == null)
        return;
      ((Task) this.conditionalTask).OnEnd();
    }

    public virtual void OnReset()
    {
      this.conditionalTask = (Conditional) null;
    }
  }
}

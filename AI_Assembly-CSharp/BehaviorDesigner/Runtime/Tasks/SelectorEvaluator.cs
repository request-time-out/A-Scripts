// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.SelectorEvaluator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The selector evaluator is a selector task which reevaluates its children every tick. It will run the lowest priority child which returns a task status of running. This is done each tick. If a higher priority child is running and the next frame a lower priority child wants to run it will interrupt the higher priority child. The selector evaluator will return success as soon as the first child returns success otherwise it will keep trying higher priority children. This task mimics the conditional abort functionality except the child tasks don't always have to be conditional tasks.")]
  [TaskIcon("{SkinColor}SelectorEvaluatorIcon.png")]
  public class SelectorEvaluator : Composite
  {
    private int currentChildIndex;
    private TaskStatus executionStatus;
    private int storedCurrentChildIndex;
    private TaskStatus storedExecutionStatus;

    public SelectorEvaluator()
    {
      base.\u002Ector();
    }

    public virtual int CurrentChildIndex()
    {
      return this.currentChildIndex;
    }

    public virtual void OnChildStarted(int childIndex)
    {
      ++this.currentChildIndex;
      this.executionStatus = (TaskStatus) 3;
    }

    public virtual bool CanExecute()
    {
      if (this.executionStatus == 2 || this.executionStatus == 3)
        return false;
      return this.storedCurrentChildIndex != -1 ? this.currentChildIndex < this.storedCurrentChildIndex - 1 : this.currentChildIndex < ((List<Task>) ((ParentTask) this).children).Count;
    }

    public virtual void OnChildExecuted(int childIndex, TaskStatus childStatus)
    {
      if (childStatus == null && ((List<Task>) ((ParentTask) this).children)[childIndex].get_Disabled())
        this.executionStatus = (TaskStatus) 1;
      if (childStatus == null || childStatus == 3)
        return;
      this.executionStatus = childStatus;
    }

    public virtual void OnConditionalAbort(int childIndex)
    {
      this.currentChildIndex = childIndex;
      this.executionStatus = (TaskStatus) 0;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
      this.currentChildIndex = 0;
    }

    public virtual TaskStatus OverrideStatus(TaskStatus status)
    {
      return this.executionStatus;
    }

    public virtual bool CanRunParallelChildren()
    {
      return true;
    }

    public virtual bool CanReevaluate()
    {
      return true;
    }

    public virtual bool OnReevaluationStarted()
    {
      if (this.executionStatus == null)
        return false;
      this.storedCurrentChildIndex = this.currentChildIndex;
      this.storedExecutionStatus = this.executionStatus;
      this.currentChildIndex = 0;
      this.executionStatus = (TaskStatus) 0;
      return true;
    }

    public virtual void OnReevaluationEnded(TaskStatus status)
    {
      if (this.executionStatus != 1 && this.executionStatus != null)
      {
        ((BehaviorManager) BehaviorManager.instance).Interrupt(((Task) this).get_Owner(), ((List<Task>) ((ParentTask) this).children)[this.storedCurrentChildIndex - 1], (Task) this);
      }
      else
      {
        this.currentChildIndex = this.storedCurrentChildIndex;
        this.executionStatus = this.storedExecutionStatus;
      }
      this.storedCurrentChildIndex = -1;
      this.storedExecutionStatus = (TaskStatus) 0;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.ParallelComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Similar to the parallel selector task, except the parallel complete task will return the child status as soon as the child returns success or failure.The child tasks are executed simultaneously.")]
  [TaskIcon("{SkinColor}ParallelCompleteIcon.png")]
  public class ParallelComplete : Composite
  {
    private int currentChildIndex;
    private TaskStatus[] executionStatus;

    public ParallelComplete()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      this.executionStatus = new TaskStatus[((List<Task>) ((ParentTask) this).children).Count];
    }

    public virtual void OnChildStarted(int childIndex)
    {
      ++this.currentChildIndex;
      this.executionStatus[childIndex] = (TaskStatus) 3;
    }

    public virtual bool CanRunParallelChildren()
    {
      return true;
    }

    public virtual int CurrentChildIndex()
    {
      return this.currentChildIndex;
    }

    public virtual bool CanExecute()
    {
      return this.currentChildIndex < ((List<Task>) ((ParentTask) this).children).Count;
    }

    public virtual void OnChildExecuted(int childIndex, TaskStatus childStatus)
    {
      this.executionStatus[childIndex] = childStatus;
    }

    public virtual void OnConditionalAbort(int childIndex)
    {
      this.currentChildIndex = 0;
      for (int index = 0; index < this.executionStatus.Length; ++index)
        this.executionStatus[index] = (TaskStatus) 0;
    }

    public virtual TaskStatus OverrideStatus(TaskStatus status)
    {
      if (this.currentChildIndex == 0)
        return (TaskStatus) 2;
      for (int index = 0; index < this.currentChildIndex; ++index)
      {
        if ((int) this.executionStatus[index] == 2 || (int) this.executionStatus[index] == 1)
          return (TaskStatus) (int) this.executionStatus[index];
      }
      return (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      for (int index = 0; index < this.executionStatus.Length; ++index)
        this.executionStatus[index] = (TaskStatus) 0;
      this.currentChildIndex = 0;
    }
  }
}

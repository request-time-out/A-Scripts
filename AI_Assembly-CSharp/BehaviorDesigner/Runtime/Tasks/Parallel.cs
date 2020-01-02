// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Parallel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Similar to the sequence task, the parallel task will run each child task until a child task returns failure. The difference is that the parallel task will run all of its children tasks simultaneously versus running each task one at a time. Like the sequence class, the parallel task will return success once all of its children tasks have return success. If one tasks returns failure the parallel task will end all of the child tasks and return failure.")]
  [TaskIcon("{SkinColor}ParallelIcon.png")]
  public class Parallel : Composite
  {
    private int currentChildIndex;
    private TaskStatus[] executionStatus;

    public Parallel()
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

    public virtual TaskStatus OverrideStatus(TaskStatus status)
    {
      bool flag = true;
      for (int index = 0; index < this.executionStatus.Length; ++index)
      {
        if ((int) this.executionStatus[index] == 3)
          flag = false;
        else if ((int) this.executionStatus[index] == 1)
          return (TaskStatus) 1;
      }
      return flag ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnConditionalAbort(int childIndex)
    {
      this.currentChildIndex = 0;
      for (int index = 0; index < this.executionStatus.Length; ++index)
        this.executionStatus[index] = (TaskStatus) 0;
    }

    public virtual void OnEnd()
    {
      for (int index = 0; index < this.executionStatus.Length; ++index)
        this.executionStatus[index] = (TaskStatus) 0;
      this.currentChildIndex = 0;
    }
  }
}

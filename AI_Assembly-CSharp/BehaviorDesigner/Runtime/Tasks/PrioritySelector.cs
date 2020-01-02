// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.PrioritySelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Similar to the selector task, the priority selector task will return success as soon as a child task returns success. Instead of running the tasks sequentially from left to right within the tree, the priority selector will ask the task what its priority is to determine the order. The higher priority tasks have a higher chance at being run first.")]
  [TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
  public class PrioritySelector : Composite
  {
    private int currentChildIndex;
    private TaskStatus executionStatus;
    private List<int> childrenExecutionOrder;

    public PrioritySelector()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      this.childrenExecutionOrder.Clear();
      for (int index1 = 0; index1 < ((List<Task>) ((ParentTask) this).children).Count; ++index1)
      {
        float priority = ((List<Task>) ((ParentTask) this).children)[index1].GetPriority();
        int index2 = this.childrenExecutionOrder.Count;
        for (int index3 = 0; index3 < this.childrenExecutionOrder.Count; ++index3)
        {
          if ((double) ((List<Task>) ((ParentTask) this).children)[this.childrenExecutionOrder[index3]].GetPriority() < (double) priority)
          {
            index2 = index3;
            break;
          }
        }
        this.childrenExecutionOrder.Insert(index2, index1);
      }
    }

    public virtual int CurrentChildIndex()
    {
      return this.childrenExecutionOrder[this.currentChildIndex];
    }

    public virtual bool CanExecute()
    {
      return this.currentChildIndex < ((List<Task>) ((ParentTask) this).children).Count && this.executionStatus != 2;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      ++this.currentChildIndex;
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
  }
}

// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.UtilitySelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The utility selector task evaluates the child tasks using Utility Theory AI. The child task can override the GetUtility method and return the utility value at that particular time. The task with the highest utility value will be selected and the existing running task will be aborted. The utility selector task reevaluates its children every tick.")]
  [TaskIcon("{SkinColor}UtilitySelectorIcon.png")]
  public class UtilitySelector : Composite
  {
    private int currentChildIndex;
    private float highestUtility;
    private TaskStatus executionStatus;
    private bool reevaluating;
    private List<int> availableChildren;

    public UtilitySelector()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      this.highestUtility = float.MinValue;
      this.availableChildren.Clear();
      for (int index = 0; index < ((List<Task>) ((ParentTask) this).children).Count; ++index)
      {
        float utility = ((List<Task>) ((ParentTask) this).children)[index].GetUtility();
        if ((double) utility > (double) this.highestUtility)
        {
          this.highestUtility = utility;
          this.currentChildIndex = index;
        }
        this.availableChildren.Add(index);
      }
    }

    public virtual int CurrentChildIndex()
    {
      return this.currentChildIndex;
    }

    public virtual void OnChildStarted(int childIndex)
    {
      this.executionStatus = (TaskStatus) 3;
    }

    public virtual bool CanExecute()
    {
      return this.executionStatus != 2 && this.executionStatus != 3 && !this.reevaluating && this.availableChildren.Count > 0;
    }

    public virtual void OnChildExecuted(int childIndex, TaskStatus childStatus)
    {
      if (childStatus == null || childStatus == 3)
        return;
      this.executionStatus = childStatus;
      if (this.executionStatus != 1)
        return;
      this.availableChildren.Remove(childIndex);
      this.highestUtility = float.MinValue;
      for (int index = 0; index < this.availableChildren.Count; ++index)
      {
        float utility = ((List<Task>) ((ParentTask) this).children)[this.availableChildren[index]].GetUtility();
        if ((double) utility > (double) this.highestUtility)
        {
          this.highestUtility = utility;
          this.currentChildIndex = this.availableChildren[index];
        }
      }
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
      this.reevaluating = true;
      return true;
    }

    public virtual void OnReevaluationEnded(TaskStatus status)
    {
      this.reevaluating = false;
      int currentChildIndex = this.currentChildIndex;
      this.highestUtility = float.MinValue;
      for (int index = 0; index < this.availableChildren.Count; ++index)
      {
        float utility = ((List<Task>) ((ParentTask) this).children)[this.availableChildren[index]].GetUtility();
        if ((double) utility > (double) this.highestUtility)
        {
          this.highestUtility = utility;
          this.currentChildIndex = this.availableChildren[index];
        }
      }
      if (currentChildIndex == this.currentChildIndex)
        return;
      ((BehaviorManager) BehaviorManager.instance).Interrupt(((Task) this).get_Owner(), ((List<Task>) ((ParentTask) this).children)[currentChildIndex], (Task) this);
      this.executionStatus = (TaskStatus) 0;
    }
  }
}

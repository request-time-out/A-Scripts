// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.RandomSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Similar to the selector task, the random selector task will return success as soon as a child task returns success.  The difference is that the random selector class will run its children in a random order. The selector task is deterministic in that it will always run the tasks from left to right within the tree. The random selector task shuffles the child tasks up and then begins execution in a random order. Other than that the random selector class is the same as the selector class. It will continue running tasks until a task completes successfully. If no child tasks return success then it will return failure.")]
  [TaskIcon("{SkinColor}RandomSelectorIcon.png")]
  public class RandomSelector : Composite
  {
    [Tooltip("Seed the random number generator to make things easier to debug")]
    public int seed;
    [Tooltip("Do we want to use the seed?")]
    public bool useSeed;
    private List<int> childIndexList;
    private Stack<int> childrenExecutionOrder;
    private TaskStatus executionStatus;

    public RandomSelector()
    {
      base.\u002Ector();
    }

    public virtual void OnAwake()
    {
      if (this.useSeed)
        Random.InitState(this.seed);
      this.childIndexList.Clear();
      for (int index = 0; index < ((List<Task>) ((ParentTask) this).children).Count; ++index)
        this.childIndexList.Add(index);
    }

    public virtual void OnStart()
    {
      this.ShuffleChilden();
    }

    public virtual int CurrentChildIndex()
    {
      return this.childrenExecutionOrder.Peek();
    }

    public virtual bool CanExecute()
    {
      return this.childrenExecutionOrder.Count > 0 && this.executionStatus != 2;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      if (this.childrenExecutionOrder.Count > 0)
        this.childrenExecutionOrder.Pop();
      this.executionStatus = childStatus;
    }

    public virtual void OnConditionalAbort(int childIndex)
    {
      this.childrenExecutionOrder.Clear();
      this.executionStatus = (TaskStatus) 0;
      this.ShuffleChilden();
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
      this.childrenExecutionOrder.Clear();
    }

    public virtual void OnReset()
    {
      this.seed = 0;
      this.useSeed = false;
    }

    private void ShuffleChilden()
    {
      for (int count = this.childIndexList.Count; count > 0; --count)
      {
        int index = Random.Range(0, count);
        int childIndex = this.childIndexList[index];
        this.childrenExecutionOrder.Push(childIndex);
        this.childIndexList[index] = this.childIndexList[count - 1];
        this.childIndexList[count - 1] = childIndex;
      }
    }
  }
}

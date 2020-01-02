// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.TaskGuard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The task guard task is similar to a semaphore in multithreaded programming. The task guard task is there to ensure a limited resource is not being overused. \n\nFor example, you may place a task guard above a task that plays an animation. Elsewhere within your behavior tree you may also have another task that plays a different animation but uses the same bones for that animation. Because of this you don't want that animation to play twice at the same time. Placing a task guard will let you specify how many times a particular task can be accessed at the same time.\n\nIn the previous animation task example you would specify an access count of 1. With this setup the animation task can be only controlled by one task at a time. If the first task is playing the animation and a second task wants to control the animation as well, it will either have to wait or skip over the task completely.")]
  [TaskIcon("{SkinColor}TaskGuardIcon.png")]
  public class TaskGuard : Decorator
  {
    [Tooltip("The number of times the child tasks can be accessed by parallel tasks at once")]
    public SharedInt maxTaskAccessCount;
    [Tooltip("The linked tasks that also guard a task. If the task guard is not linked against any other tasks it doesn't have much purpose. Marked as LinkedTask to ensure all tasks linked are linked to the same set of tasks")]
    [LinkedTask]
    public TaskGuard[] linkedTaskGuards;
    [Tooltip("If true the task will wait until the child task is available. If false then any unavailable child tasks will be skipped over")]
    public SharedBool waitUntilTaskAvailable;
    private int executingTasks;
    private bool executing;

    public TaskGuard()
    {
      base.\u002Ector();
    }

    public virtual bool CanExecute()
    {
      return this.executingTasks < this.maxTaskAccessCount.get_Value() && !this.executing;
    }

    public virtual void OnChildStarted()
    {
      ++this.executingTasks;
      this.executing = true;
      for (int index = 0; index < this.linkedTaskGuards.Length; ++index)
        this.linkedTaskGuards[index].taskExecuting(true);
    }

    public virtual TaskStatus OverrideStatus(TaskStatus status)
    {
      return !this.executing && this.waitUntilTaskAvailable.get_Value() ? (TaskStatus) 3 : status;
    }

    public void taskExecuting(bool increase)
    {
      this.executingTasks += !increase ? -1 : 1;
    }

    public virtual void OnEnd()
    {
      if (!this.executing)
        return;
      --this.executingTasks;
      for (int index = 0; index < this.linkedTaskGuards.Length; ++index)
        this.linkedTaskGuards[index].taskExecuting(false);
      this.executing = false;
    }

    public virtual void OnReset()
    {
      this.maxTaskAccessCount = (SharedInt) null;
      this.linkedTaskGuards = (TaskGuard[]) null;
      this.waitUntilTaskAvailable = (SharedBool) true;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Repeater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The repeater task will repeat execution of its child task until the child task has been run a specified number of times. It has the option of continuing to execute the child task even if the child task returns a failure.")]
  [TaskIcon("{SkinColor}RepeaterIcon.png")]
  public class Repeater : Decorator
  {
    [Tooltip("The number of times to repeat the execution of its child task")]
    public SharedInt count;
    [Tooltip("Allows the repeater to repeat forever")]
    public SharedBool repeatForever;
    [Tooltip("Should the task return if the child task returns a failure")]
    public SharedBool endOnFailure;
    private int executionCount;
    private TaskStatus executionStatus;

    public Repeater()
    {
      base.\u002Ector();
    }

    public virtual bool CanExecute()
    {
      if (!this.repeatForever.get_Value() && this.executionCount >= this.count.get_Value())
        return false;
      if (!this.endOnFailure.get_Value())
        return true;
      return this.endOnFailure.get_Value() && this.executionStatus != 1;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      ++this.executionCount;
      this.executionStatus = childStatus;
    }

    public virtual void OnEnd()
    {
      this.executionCount = 0;
      this.executionStatus = (TaskStatus) 0;
    }

    public virtual void OnReset()
    {
      this.count = (SharedInt) 0;
      this.endOnFailure = (SharedBool) true;
    }
  }
}

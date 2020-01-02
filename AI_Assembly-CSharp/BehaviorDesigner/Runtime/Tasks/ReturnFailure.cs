// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.ReturnFailure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The return failure task will always return failure except when the child task is running.")]
  [TaskIcon("{SkinColor}ReturnFailureIcon.png")]
  public class ReturnFailure : Decorator
  {
    private TaskStatus executionStatus;

    public ReturnFailure()
    {
      base.\u002Ector();
    }

    public virtual bool CanExecute()
    {
      return this.executionStatus == null || this.executionStatus == 3;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      this.executionStatus = childStatus;
    }

    public virtual TaskStatus Decorate(TaskStatus status)
    {
      return status == 2 ? (TaskStatus) 1 : status;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
    }
  }
}

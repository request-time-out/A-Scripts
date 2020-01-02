// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.UntilFailure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The until failure task will keep executing its child task until the child task returns failure.")]
  [TaskIcon("{SkinColor}UntilFailureIcon.png")]
  public class UntilFailure : Decorator
  {
    private TaskStatus executionStatus;

    public UntilFailure()
    {
      base.\u002Ector();
    }

    public virtual bool CanExecute()
    {
      return this.executionStatus == 2 || this.executionStatus == 0;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      this.executionStatus = childStatus;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
    }
  }
}

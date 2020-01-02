// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Inverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The inverter task will invert the return value of the child task after it has finished executing. If the child returns success, the inverter task will return failure. If the child returns failure, the inverter task will return success.")]
  [TaskIcon("{SkinColor}InverterIcon.png")]
  public class Inverter : Decorator
  {
    private TaskStatus executionStatus;

    public Inverter()
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
      if (status == 2)
        return (TaskStatus) 1;
      return status == 1 ? (TaskStatus) 2 : status;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
    }
  }
}

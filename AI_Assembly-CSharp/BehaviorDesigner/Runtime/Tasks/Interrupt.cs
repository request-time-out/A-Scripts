// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Interrupt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The interrupt task will stop all child tasks from running if it is interrupted. The interruption can be triggered by the perform interruption task. The interrupt task will keep running its child until this interruption is called. If no interruption happens and the child task completed its execution the interrupt task will return the value assigned by the child task.")]
  [TaskIcon("{SkinColor}InterruptIcon.png")]
  public class Interrupt : Decorator
  {
    private TaskStatus interruptStatus;
    private TaskStatus executionStatus;

    public Interrupt()
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

    public void DoInterrupt(TaskStatus status)
    {
      this.interruptStatus = status;
      ((BehaviorManager) BehaviorManager.instance).Interrupt(((Task) this).get_Owner(), (Task) this);
    }

    public virtual TaskStatus OverrideStatus()
    {
      return this.interruptStatus;
    }

    public virtual void OnEnd()
    {
      this.interruptStatus = (TaskStatus) 1;
      this.executionStatus = (TaskStatus) 0;
    }
  }
}

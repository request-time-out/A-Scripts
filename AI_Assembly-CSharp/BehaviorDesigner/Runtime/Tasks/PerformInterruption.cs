// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.PerformInterruption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Perform the actual interruption. This will immediately stop the specified tasks from running and will return success or failure depending on the value of interrupt success.")]
  [TaskIcon("{SkinColor}PerformInterruptionIcon.png")]
  public class PerformInterruption : Action
  {
    [Tooltip("The list of tasks to interrupt. Can be any number of tasks")]
    public Interrupt[] interruptTasks;
    [Tooltip("When we interrupt the task should we return a task status of success?")]
    public SharedBool interruptSuccess;

    public PerformInterruption()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      for (int index = 0; index < this.interruptTasks.Length; ++index)
        this.interruptTasks[index].DoInterrupt(!this.interruptSuccess.get_Value() ? (TaskStatus) 1 : (TaskStatus) 2);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.interruptTasks = (Interrupt[]) null;
      this.interruptSuccess = (SharedBool) false;
    }
  }
}

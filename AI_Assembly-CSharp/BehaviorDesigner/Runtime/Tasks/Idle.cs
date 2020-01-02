// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Idle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
  [TaskIcon("{SkinColor}IdleIcon.png")]
  public class Idle : Action
  {
    public Idle()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (TaskStatus) 3;
    }
  }
}

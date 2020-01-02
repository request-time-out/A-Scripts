// Decompiled with JetBrains decompiler
// Type: AIProject.RemovePointBooking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class RemovePointBooking : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      ActionPoint bookingActionPoint = agent.BookingActionPoint;
      if (Object.op_Equality((Object) bookingActionPoint, (Object) null))
        return (TaskStatus) 1;
      bookingActionPoint.RemoveBooking((Actor) agent);
      agent.BookingActionPoint = (ActionPoint) null;
      return (TaskStatus) 2;
    }
  }
}

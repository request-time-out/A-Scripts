// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.RemovePointBooking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class RemovePointBooking : MerchantAction
  {
    public virtual TaskStatus OnUpdate()
    {
      MerchantActor merchant = this.Merchant;
      ActionPoint bookingActionPoint = merchant.BookingActionPoint;
      if (Object.op_Equality((Object) bookingActionPoint, (Object) null))
        return (TaskStatus) 1;
      bookingActionPoint.RemoveBooking((Actor) merchant);
      merchant.BookingActionPoint = (ActionPoint) null;
      return (TaskStatus) 2;
    }
  }
}

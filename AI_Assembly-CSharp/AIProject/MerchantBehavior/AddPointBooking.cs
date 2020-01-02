// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.AddPointBooking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class AddPointBooking : MerchantAction
  {
    private ActionPoint _bookingPoint;
    private MerchantActor _merchant;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._merchant = this.Merchant;
    }

    public virtual TaskStatus OnUpdate()
    {
      NavMeshAgent navMeshAgent = this._merchant.NavMeshAgent;
      M0 m0;
      if (navMeshAgent == null)
      {
        m0 = (M0) null;
      }
      else
      {
        OffMeshLinkData currentOffMeshLinkData = navMeshAgent.get_currentOffMeshLinkData();
        m0 = ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink())?.GetComponent<ActionPoint>();
      }
      this._bookingPoint = (ActionPoint) m0;
      if (Object.op_Equality((Object) this._bookingPoint, (Object) null))
        return (TaskStatus) 1;
      this._bookingPoint.AddBooking((Actor) this._merchant);
      this._merchant.BookingActionPoint = this._bookingPoint;
      return (TaskStatus) 2;
    }

    public virtual void OnBehaviorComplete()
    {
      if (Object.op_Equality((Object) this._bookingPoint, (Object) null))
        return;
      this._bookingPoint.RemoveBooking((Actor) this._merchant);
      if (Object.op_Equality((Object) this._merchant.BookingActionPoint, (Object) this._bookingPoint))
        this._merchant.BookingActionPoint = (ActionPoint) null;
      this._bookingPoint = (ActionPoint) null;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.MerchantAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject.MerchantBehavior
{
  public abstract class MerchantAction : Action
  {
    private MerchantActor _merchant;

    protected MerchantAction()
    {
      base.\u002Ector();
    }

    protected MerchantActor Merchant
    {
      get
      {
        return this._merchant ?? (this._merchant = (((Task) this).get_Owner() as MerchantBehaviorTree).SourceMerchant);
      }
    }

    protected MerchantPoint CurrentMerchantPoint
    {
      get
      {
        return this.Merchant.CurrentMerchantPoint;
      }
      set
      {
        this.Merchant.CurrentMerchantPoint = value;
      }
    }

    protected MerchantPoint TargetInSightMerchantPoint
    {
      get
      {
        return this.Merchant.TargetInSightMerchantPoint;
      }
      set
      {
        this.Merchant.TargetInSightMerchantPoint = value;
      }
    }

    protected MerchantPoint MainTargetInSightMerchantPoint
    {
      get
      {
        return this.Merchant.MainTargetInSightMerchantPoint;
      }
      set
      {
        this.Merchant.MainTargetInSightMerchantPoint = value;
      }
    }

    protected MerchantPoint PrevMerchantPoint
    {
      get
      {
        return this.Merchant.PrevMerchantPoint;
      }
      set
      {
        this.Merchant.PrevMerchantPoint = value;
      }
    }

    protected MerchantActor.MerchantSchedule CurrentSchedule
    {
      get
      {
        return this.Merchant.CurrentSchedule;
      }
    }

    protected MerchantActor.MerchantSchedule PrevSchedule
    {
      get
      {
        return this.Merchant.PrevSchedule;
      }
    }
  }
}

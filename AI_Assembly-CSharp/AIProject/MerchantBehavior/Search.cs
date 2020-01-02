// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.Search
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UniRx;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class Search : MerchantStateAction
  {
    private float counter;
    private float searchSecondTime;

    public override void OnStart()
    {
      this.counter = 0.0f;
      base.OnStart();
      this.searchSecondTime = (float) Random.Range(this.animInfo.loopMinTime, this.animInfo.loopMaxTime);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Merchant.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (this.Merchant.ElapsedDay)
      {
        if (this.onEndAction != null)
          this.onEndAction.OnNext(Unit.get_Default());
        if (this.Merchant.Animation.PlayingOutAnimation)
          return (TaskStatus) 3;
        this.Merchant.ElapsedDay = false;
        this.Complete();
        this.Merchant.ChangeNextSchedule();
        this.Merchant.SetCurrentSchedule();
        return (TaskStatus) 2;
      }
      if (this.animInfo.isLoop)
      {
        if ((double) this.searchSecondTime < (double) this.counter)
        {
          if (this.onEndAction != null)
            this.onEndAction.OnNext(Unit.get_Default());
          if (!this.Merchant.Animation.PlayingOutAnimation)
          {
            this.Complete();
            return (TaskStatus) 2;
          }
        }
        else
          this.counter += Time.get_deltaTime();
      }
      else
      {
        if (this.onEndAction != null)
          this.onEndAction.OnNext(Unit.get_Default());
        if (!this.Merchant.Animation.PlayingOutAnimation)
        {
          this.Complete();
          return (TaskStatus) 2;
        }
      }
      return (TaskStatus) 3;
    }
  }
}

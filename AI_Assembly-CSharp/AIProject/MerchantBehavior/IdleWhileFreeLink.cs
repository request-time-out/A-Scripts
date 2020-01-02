// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.IdleWhileFreeLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class IdleWhileFreeLink : MerchantAction
  {
    private bool _prevActiveEncount;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._prevActiveEncount = this.Merchant.ActiveEncount;
      if (this._prevActiveEncount)
        return;
      this.Merchant.ActiveEncount = true;
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Merchant.IsInvalidMoveDestination() ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      if (!this._prevActiveEncount)
        this.Merchant.ActiveEncount = false;
      ((Task) this).OnEnd();
    }
  }
}

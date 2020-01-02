// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.ChangePrevMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class ChangePrevMode : MerchantAction
  {
    public virtual TaskStatus OnUpdate()
    {
      this.Merchant.ChangeBehavior(this.Merchant.PrevMode);
      return (TaskStatus) 2;
    }
  }
}

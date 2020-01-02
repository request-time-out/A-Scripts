// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.ApplyMainTargetMerchantPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class ApplyMainTargetMerchantPoint : MerchantAction
  {
    public virtual TaskStatus OnUpdate()
    {
      if (!Object.op_Inequality((Object) this.MainTargetInSightMerchantPoint, (Object) null))
        return (TaskStatus) 1;
      this.TargetInSightMerchantPoint = this.MainTargetInSightMerchantPoint;
      this.MainTargetInSightMerchantPoint = (MerchantPoint) null;
      MerchantData merchantData = this.Merchant.MerchantData;
      merchantData.PointPosition = ((Component) this.TargetInSightMerchantPoint).get_transform().get_position();
      merchantData.MainPointPosition = new Vector3(-999f, -999f, -999f);
      return (TaskStatus) 2;
    }
  }
}

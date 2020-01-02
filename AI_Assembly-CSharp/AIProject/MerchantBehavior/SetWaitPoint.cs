// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.SetWaitPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class SetWaitPoint : MerchantSetPoint
  {
    private bool IsMatchPoint(MerchantPoint _point, bool _isMiddlePoint)
    {
      if (Object.op_Equality((Object) _point, (Object) null))
        return false;
      Merchant.EventType eventType = Merchant.EventType.Wait;
      MerchantActor merchant = this.Merchant;
      MerchantData merchantData = merchant.MerchantData;
      int openAreaId = merchant.OpenAreaID;
      int pointAreaId = merchantData.PointAreaID;
      int pointGroupId = merchantData.PointGroupID;
      MerchantPoint prevMerchantPoint = this.PrevMerchantPoint;
      if (openAreaId < _point.AreaID || (_point.EventType & eventType) != eventType)
        return false;
      if (_isMiddlePoint || _point.AreaID != pointAreaId)
        return _point.GroupID == 0;
      return _point.GroupID != pointGroupId || Object.op_Inequality((Object) _point, (Object) prevMerchantPoint);
    }

    private bool IsSameArea(MerchantPoint _point0, MerchantPoint _point1)
    {
      return !Object.op_Equality((Object) _point0, (Object) null) && !Object.op_Equality((Object) _point1, (Object) null) && _point0.AreaID == _point1.AreaID;
    }

    [DebuggerHidden]
    protected override IEnumerator NextPointSettingCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SetWaitPoint.\u003CNextPointSettingCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}

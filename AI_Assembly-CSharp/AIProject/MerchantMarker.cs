// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class MerchantMarker : MonoBehaviour
  {
    private int InstanceID;
    private MerchantActor merchant;

    public MerchantMarker()
    {
      base.\u002Ector();
    }

    public static Dictionary<int, MerchantActor> MerchantMarkerTable { get; } = new Dictionary<int, MerchantActor>();

    public static List<int> Keys { get; } = new List<int>();

    private void Awake()
    {
      if (Object.op_Equality((Object) this.merchant, (Object) null))
        this.merchant = (MerchantActor) ((Component) this).GetComponent<MerchantActor>();
      if (Object.op_Equality((Object) this.merchant, (Object) null))
        Object.Destroy((Object) this);
      else
        this.InstanceID = this.merchant.InstanceID;
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this.merchant, (Object) null))
        return;
      MerchantMarker.MerchantMarkerTable[this.InstanceID] = this.merchant;
      if (MerchantMarker.Keys.Contains(this.InstanceID))
        return;
      MerchantMarker.Keys.Add(this.InstanceID);
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this.merchant, (Object) null))
        return;
      MerchantMarker.MerchantMarkerTable.Remove(this.InstanceID);
      MerchantMarker.Keys.RemoveAll((Predicate<int>) (x => x == this.InstanceID));
    }
  }
}

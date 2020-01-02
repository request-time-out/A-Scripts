// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsSelectAccessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsSelectAccessory : CvsSelectWindow
  {
    public override void Start()
    {
      base.Start();
      Singleton<CustomBase>.Instance.ChangeAcsSlotColor(0);
      if (!((IEnumerable<CvsSelectWindow.ItemInfo>) this.items).Any<CvsSelectWindow.ItemInfo>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CvsSelectWindow.ItemInfo>) this.items).Select<CvsSelectWindow.ItemInfo, \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>((Func<CvsSelectWindow.ItemInfo, int, \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.btnItem, (Object) null))).ToList<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) item.val.btnItem), (Action<M0>) (_ =>
      {
        int idx = item.idx;
        if (idx < 0 || 19 < idx)
          ;
        Singleton<CustomBase>.Instance.ChangeAcsSlotColor(item.idx);
      }))));
    }
  }
}

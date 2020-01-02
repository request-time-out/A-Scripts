// Decompiled with JetBrains decompiler
// Type: UI_ToggleGroupCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToggleGroupCtrl : MonoBehaviour
{
  public UI_ToggleGroupCtrl.ItemInfo[] items;

  public UI_ToggleGroupCtrl()
  {
    base.\u002Ector();
  }

  public virtual void Start()
  {
    if (!((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Any<UI_ToggleGroupCtrl.ItemInfo>())
      return;
    // ISSUE: object of a compiler-generated type is created
    ((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Select<UI_ToggleGroupCtrl.ItemInfo, \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<UI_ToggleGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.tglItem, (Object) null))).ToList<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val.tglItem), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ =>
    {
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int> anonType5 in ((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Select<UI_ToggleGroupCtrl.ItemInfo, \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<UI_ToggleGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>>) ((v, i) => new \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>(v, i))))
      {
        if (anonType5.i != item.idx && anonType5.v != null)
        {
          CanvasGroup cgItem = anonType5.v.cgItem;
          if (Object.op_Inequality((Object) null, (Object) cgItem))
            cgItem.Enable(false, false);
        }
      }
      if (!Object.op_Inequality((Object) null, (Object) item.val.cgItem))
        return;
      item.val.cgItem.Enable(true, false);
    }))));
  }

  public int GetSelectIndex()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int> anonType5 = ((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Select<UI_ToggleGroupCtrl.ItemInfo, \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<UI_ToggleGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>>) ((v, i) => new \u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>(v, i))).FirstOrDefault<\u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<\u003C\u003E__AnonType5<UI_ToggleGroupCtrl.ItemInfo, int>, bool>) (x => x.v.tglItem.get_isOn()));
    return anonType5 != null ? anonType5.i : -1;
  }

  [Serializable]
  public class ItemInfo
  {
    public Toggle tglItem;
    public CanvasGroup cgItem;
  }
}

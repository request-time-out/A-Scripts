// Decompiled with JetBrains decompiler
// Type: UI_ButtonGroupCtrl
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

public class UI_ButtonGroupCtrl : MonoBehaviour
{
  public UI_ButtonGroupCtrl.ItemInfo[] items;

  public UI_ButtonGroupCtrl()
  {
    base.\u002Ector();
  }

  public virtual void Start()
  {
    if (!((IEnumerable<UI_ButtonGroupCtrl.ItemInfo>) this.items).Any<UI_ButtonGroupCtrl.ItemInfo>())
      return;
    // ISSUE: object of a compiler-generated type is created
    ((IEnumerable<UI_ButtonGroupCtrl.ItemInfo>) this.items).Select<UI_ButtonGroupCtrl.ItemInfo, \u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>>((Func<UI_ButtonGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.btnItem, (Object) null))).ToList<\u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<UI_ButtonGroupCtrl.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val.btnItem), (Action<M0>) (_ =>
    {
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType5<UI_ButtonGroupCtrl.ItemInfo, int> anonType5 in ((IEnumerable<UI_ButtonGroupCtrl.ItemInfo>) this.items).Select<UI_ButtonGroupCtrl.ItemInfo, \u003C\u003E__AnonType5<UI_ButtonGroupCtrl.ItemInfo, int>>((Func<UI_ButtonGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType5<UI_ButtonGroupCtrl.ItemInfo, int>>) ((v, i) => new \u003C\u003E__AnonType5<UI_ButtonGroupCtrl.ItemInfo, int>(v, i))))
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

  [Serializable]
  public class ItemInfo
  {
    public Button btnItem;
    public CanvasGroup cgItem;
  }
}

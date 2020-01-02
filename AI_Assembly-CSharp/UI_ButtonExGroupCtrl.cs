// Decompiled with JetBrains decompiler
// Type: UI_ButtonExGroupCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonExGroupCtrl : MonoBehaviour
{
  public UI_ButtonExGroupCtrl.ItemInfo[] items;

  public UI_ButtonExGroupCtrl()
  {
    base.\u002Ector();
  }

  public virtual void Start()
  {
    if (!((IEnumerable<UI_ButtonExGroupCtrl.ItemInfo>) this.items).Any<UI_ButtonExGroupCtrl.ItemInfo>())
      return;
    // ISSUE: object of a compiler-generated type is created
    ((IEnumerable<UI_ButtonExGroupCtrl.ItemInfo>) this.items).Select<UI_ButtonExGroupCtrl.ItemInfo, \u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>>((Func<UI_ButtonExGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.btnItem, (Object) null))).ToList<\u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<UI_ButtonExGroupCtrl.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) item.val.btnItem), (Action<M0>) (_ =>
    {
      foreach (UI_ButtonExGroupCtrl.ItemInfo itemInfo in this.items)
      {
        if (itemInfo != null && Object.op_Inequality((Object) itemInfo.btnItem, (Object) item.val.btnItem))
        {
          foreach (CanvasGroup canvasGroup in itemInfo.cgItem)
          {
            if (Object.op_Inequality((Object) null, (Object) canvasGroup))
              canvasGroup.Enable(false, false);
          }
        }
      }
      foreach (CanvasGroup canvasGroup in item.val.cgItem)
      {
        if (Object.op_Inequality((Object) null, (Object) canvasGroup))
          canvasGroup.Enable(true, false);
      }
    }))));
  }

  [Serializable]
  public class ItemInfo
  {
    public UI_ButtonEx btnItem;
    public CanvasGroup[] cgItem;
  }
}

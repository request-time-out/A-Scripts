// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsSelectWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsSelectWindow : MonoBehaviour
  {
    public CanvasGroup cgBaseWindow;
    public UI_ButtonEx btnNewFirst;
    public UI_ButtonEx btnEditFirst;
    public CvsSelectWindow.ItemInfo[] items;
    private int backSelect;
    private Text titleText;

    public CvsSelectWindow()
    {
      base.\u002Ector();
    }

    public virtual void Start()
    {
      if (((IEnumerable<CvsSelectWindow.ItemInfo>) this.items).Any<CvsSelectWindow.ItemInfo>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<CvsSelectWindow.ItemInfo>) this.items).Select<CvsSelectWindow.ItemInfo, \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>((Func<CvsSelectWindow.ItemInfo, int, \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.btnItem, (Object) null))).ToList<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<CvsSelectWindow.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) item.val.btnItem), (Action<M0>) (_ =>
        {
          foreach (CvsSelectWindow.ItemInfo itemInfo in this.items)
          {
            if (itemInfo != null && Object.op_Inequality((Object) itemInfo.btnItem, (Object) item.val.btnItem))
            {
              foreach (CanvasGroup canvasGroup in itemInfo.cgItem)
              {
                if (Object.op_Implicit((Object) canvasGroup))
                  canvasGroup.Enable(false, false);
              }
            }
          }
          foreach (CanvasGroup canvasGroup in item.val.cgItem)
          {
            if (Object.op_Implicit((Object) canvasGroup))
              canvasGroup.Enable(true, false);
          }
          if (Object.op_Implicit((Object) this.cgBaseWindow))
            this.cgBaseWindow.Enable(true, false);
          if (this.backSelect == item.idx)
            return;
          if (Object.op_Implicit((Object) item.val.cvsBase))
          {
            this.titleText = (Text) ((Component) item.val.btnItem).GetComponentInChildren<Text>();
            if (Object.op_Implicit((Object) this.titleText) && Object.op_Implicit((Object) item.val.cvsBase.titleText))
              item.val.cvsBase.titleText.set_text(this.titleText.get_text());
            item.val.cvsBase.SNo = item.val.No;
            item.val.cvsBase.UpdateCustomUI();
            item.val.cvsBase.ChangeMenuFunc();
          }
          CustomColorCtrl customColorCtrl = Singleton<CustomBase>.Instance.customColorCtrl;
          if (Object.op_Implicit((Object) customColorCtrl))
            customColorCtrl.Close();
          this.backSelect = item.idx;
        }))));
      }
      if (Singleton<CustomBase>.Instance.modeNew)
      {
        if (this.btnNewFirst == null)
          return;
        ((UnityEvent) this.btnNewFirst.get_onClick()).Invoke();
      }
      else
      {
        if (this.btnEditFirst == null)
          return;
        ((UnityEvent) this.btnEditFirst.get_onClick()).Invoke();
      }
    }

    [Serializable]
    public class ItemInfo
    {
      public UI_ButtonEx btnItem;
      public CanvasGroup[] cgItem;
      public CvsBase cvsBase;
      public int No;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_Status
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
  public class CvsO_Status : CvsBase
  {
    [SerializeField]
    private UI_ToggleEx[] tglWish;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      foreach (int index in this.parameter.hsWish)
      {
        this.tglWish[index].SetIsOnWithoutCallback(true);
        this.tglWish[index].SetTextColor(1);
      }
      for (int index = 0; index < this.tglWish.Length; ++index)
      {
        bool flag = false;
        foreach (int num in this.parameter.hsWish)
        {
          if (index == num)
            flag = true;
        }
        if (!flag)
          this.tglWish[index].SetIsOnWithoutCallback(false);
      }
      this.ChangeRestrictWishSelect();
    }

    public void ChangeRestrictWishSelect()
    {
      if (this.parameter.hsWish.Count >= 3)
      {
        foreach (UI_ToggleEx uiToggleEx in this.tglWish)
          ((Selectable) uiToggleEx).set_interactable(uiToggleEx.get_isOn());
      }
      else
      {
        foreach (Selectable selectable in this.tglWish)
          selectable.set_interactable(true);
      }
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsStatus += new Action(((CvsBase) this).UpdateCustomUI);
      if (!((IEnumerable<UI_ToggleEx>) this.tglWish).Any<UI_ToggleEx>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<UI_ToggleEx>) this.tglWish).Select<UI_ToggleEx, \u003C\u003E__AnonType15<UI_ToggleEx, int>>((Func<UI_ToggleEx, int, \u003C\u003E__AnonType15<UI_ToggleEx, int>>) ((val, idx) => new \u003C\u003E__AnonType15<UI_ToggleEx, int>(val, idx))).Where<\u003C\u003E__AnonType15<UI_ToggleEx, int>>((Func<\u003C\u003E__AnonType15<UI_ToggleEx, int>, bool>) (tgl => Object.op_Inequality((Object) tgl.val, (Object) null))).ToList<\u003C\u003E__AnonType15<UI_ToggleEx, int>>().ForEach((Action<\u003C\u003E__AnonType15<UI_ToggleEx, int>>) (tgl => ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) tgl.val.onValueChanged), (Action<M0>) (isOn =>
      {
        if (isOn)
          this.parameter.hsWish.Add(tgl.idx);
        else
          this.parameter.hsWish.Remove(tgl.idx);
        this.ChangeRestrictWishSelect();
      }))));
    }
  }
}

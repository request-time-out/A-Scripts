// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomClothesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomClothesWindow : MonoBehaviour
  {
    [SerializeField]
    private CustomClothesScrollController cscClothes;
    [SerializeField]
    private CustomClothesWindow.SortWindow winSort;
    [SerializeField]
    private Button btnShowWinSort;
    [SerializeField]
    private Toggle tglSortOrder;
    [SerializeField]
    private Toggle[] tglLoadOption;
    [SerializeField]
    private Button[] button;
    private IntReactiveProperty _sortType;
    private IntReactiveProperty _sortOrder;
    private List<CustomClothesFileInfo> lstClothes;
    public Action<CustomClothesFileInfo> onClick01;
    public Action<CustomClothesFileInfo> onClick02;
    public Action<CustomClothesFileInfo> onClick03;
    public bool btnDisableNotSelect01;
    public bool btnDisableNotSelect02;
    public bool btnDisableNotSelect03;

    public CustomClothesWindow()
    {
      base.\u002Ector();
    }

    public int sortType
    {
      get
      {
        return ((ReactiveProperty<int>) this._sortType).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._sortType).set_Value(value);
      }
    }

    public int sortOrder
    {
      get
      {
        return ((ReactiveProperty<int>) this._sortOrder).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._sortOrder).set_Value(value);
      }
    }

    public void UpdateWindow(bool modeNew, int sex, bool save)
    {
      if (this.tglLoadOption != null && this.tglLoadOption.Length > 4 && Object.op_Implicit((Object) this.tglLoadOption[4]))
        ((Component) this.tglLoadOption[4]).get_gameObject().SetActiveIfDifferent(modeNew);
      this.lstClothes = CustomClothesFileInfoAssist.CreateClothesFileInfoList(0 == sex, 1 == sex, true, !save);
      this.Sort();
    }

    public void Sort()
    {
      if (this.lstClothes == null)
        return;
      if (this.lstClothes.Count == 0)
      {
        this.cscClothes.CreateList(this.lstClothes);
      }
      else
      {
        using (new GameSystem.CultureScope())
        {
          this.lstClothes = this.sortType != 0 ? (this.sortOrder != 0 ? this.lstClothes.OrderByDescending<CustomClothesFileInfo, string>((Func<CustomClothesFileInfo, string>) (n => n.name)).ThenByDescending<CustomClothesFileInfo, DateTime>((Func<CustomClothesFileInfo, DateTime>) (n => n.time)).ToList<CustomClothesFileInfo>() : this.lstClothes.OrderBy<CustomClothesFileInfo, string>((Func<CustomClothesFileInfo, string>) (n => n.name)).ThenBy<CustomClothesFileInfo, DateTime>((Func<CustomClothesFileInfo, DateTime>) (n => n.time)).ToList<CustomClothesFileInfo>()) : (this.sortOrder != 0 ? this.lstClothes.OrderByDescending<CustomClothesFileInfo, DateTime>((Func<CustomClothesFileInfo, DateTime>) (n => n.time)).ThenByDescending<CustomClothesFileInfo, string>((Func<CustomClothesFileInfo, string>) (n => n.name)).ToList<CustomClothesFileInfo>() : this.lstClothes.OrderBy<CustomClothesFileInfo, DateTime>((Func<CustomClothesFileInfo, DateTime>) (n => n.time)).ThenBy<CustomClothesFileInfo, string>((Func<CustomClothesFileInfo, string>) (n => n.name)).ToList<CustomClothesFileInfo>());
          this.cscClothes.CreateList(this.lstClothes);
        }
      }
    }

    public void SelectInfoClear()
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cscClothes))
        return;
      this.cscClothes.SelectInfoClear();
    }

    public void Start()
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnShowWinSort), (Action<M0>) (_ => this.winSort.objWinSort.SetActiveIfDifferent(!this.winSort.objWinSort.get_activeSelf())));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.winSort.btnCloseWinSort), (Action<M0>) (_ => this.winSort.objWinSort.SetActiveIfDifferent(false)));
      if (((IEnumerable<Toggle>) this.winSort.tglSort).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.winSort.tglSort).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (tgl => Object.op_Inequality((Object) tgl.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (tgl => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(tgl.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn => this.sortType = tgl.idx))));
      }
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglSortOrder), (Action<M0>) (isOn => this.sortOrder = !isOn ? 1 : 0));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._sortType, (Action<M0>) (_ => this.Sort()));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._sortOrder, (Action<M0>) (_ => this.Sort()));
      if (this.button == null || this.button.Length != 3)
        return;
      if (Object.op_Implicit((Object) this.button[0]))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[0]), (Action<M0>) (_ =>
        {
          if (this.onClick01 == null)
            return;
          this.onClick01(this.cscClothes.selectInfo?.info);
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[0]), (Action<M0>) (_ => ((Selectable) this.button[0]).set_interactable((!this.btnDisableNotSelect01 ? 0 : (null == this.cscClothes.selectInfo ? 1 : 0)) == 0)));
      }
      if (Object.op_Implicit((Object) this.button[1]))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[1]), (Action<M0>) (_ =>
        {
          if (this.onClick02 == null)
            return;
          this.onClick02(this.cscClothes.selectInfo?.info);
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[1]), (Action<M0>) (_ => ((Selectable) this.button[1]).set_interactable((!this.btnDisableNotSelect02 ? 0 : (null == this.cscClothes.selectInfo ? 1 : 0)) == 0)));
      }
      if (!Object.op_Implicit((Object) this.button[2]))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[2]), (Action<M0>) (_ =>
      {
        if (this.onClick03 == null)
          return;
        this.onClick03(this.cscClothes.selectInfo?.info);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[2]), (Action<M0>) (_ => ((Selectable) this.button[2]).set_interactable((!this.btnDisableNotSelect03 ? 0 : (null == this.cscClothes.selectInfo ? 1 : 0)) == 0)));
    }

    [Serializable]
    public class SortWindow
    {
      public GameObject objWinSort;
      public Button btnCloseWinSort;
      public Toggle[] tglSort;
    }
  }
}

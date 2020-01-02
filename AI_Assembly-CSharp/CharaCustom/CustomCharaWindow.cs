// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCharaWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

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
  public class CustomCharaWindow : MonoBehaviour
  {
    public CustomCharaScrollController cscChara;
    [SerializeField]
    private CustomCharaWindow.SortWindow winSort;
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
    private List<CustomCharaFileInfo> lstChara;
    public Action<CustomCharaFileInfo> onClick01;
    public Action<CustomCharaFileInfo> onClick02;
    public Action<CustomCharaFileInfo, int> onClick03;
    public bool btnDisableNotSelect01;
    public bool btnDisableNotSelect02;
    public bool btnDisableNotSelect03;

    public CustomCharaWindow()
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

    public void UpdateWindow(bool modeNew, int sex, bool save, List<CustomCharaFileInfo> _lst = null)
    {
      if (this.tglLoadOption != null && this.tglLoadOption.Length > 4 && Object.op_Inequality((Object) null, (Object) this.tglLoadOption[4]))
        ((Component) this.tglLoadOption[4]).get_gameObject().SetActiveIfDifferent(modeNew);
      this.lstChara = _lst != null ? _lst : CustomCharaFileInfoAssist.CreateCharaFileInfoList(0 == sex, 1 == sex, true, true, false, save);
      this.Sort();
    }

    public void UpdateWindowInUploader(
      bool modeNew,
      int sex,
      bool save,
      List<CustomCharaFileInfo> _lst = null)
    {
      if (this.tglLoadOption != null && this.tglLoadOption.Length > 4 && Object.op_Inequality((Object) null, (Object) this.tglLoadOption[4]))
        ((Component) this.tglLoadOption[4]).get_gameObject().SetActiveIfDifferent(modeNew);
      this.lstChara = _lst != null ? _lst : CustomCharaFileInfoAssist.CreateCharaFileInfoList(0 == sex, 1 == sex, true, false, false, save);
      this.Sort();
    }

    public void Sort()
    {
      if (this.lstChara == null)
        return;
      if (this.lstChara.Count == 0)
      {
        this.cscChara.CreateList(this.lstChara);
      }
      else
      {
        using (new GameSystem.CultureScope())
        {
          this.lstChara = this.sortType != 0 ? (this.sortOrder != 0 ? this.lstChara.OrderByDescending<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.name)).ThenByDescending<CustomCharaFileInfo, DateTime>((Func<CustomCharaFileInfo, DateTime>) (n => n.time)).ThenByDescending<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.personality)).ToList<CustomCharaFileInfo>() : this.lstChara.OrderBy<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.name)).ThenBy<CustomCharaFileInfo, DateTime>((Func<CustomCharaFileInfo, DateTime>) (n => n.time)).ThenBy<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.personality)).ToList<CustomCharaFileInfo>()) : (this.sortOrder != 0 ? this.lstChara.OrderByDescending<CustomCharaFileInfo, DateTime>((Func<CustomCharaFileInfo, DateTime>) (n => n.time)).ThenByDescending<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.name)).ThenByDescending<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.personality)).ToList<CustomCharaFileInfo>() : this.lstChara.OrderBy<CustomCharaFileInfo, DateTime>((Func<CustomCharaFileInfo, DateTime>) (n => n.time)).ThenBy<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.name)).ThenBy<CustomCharaFileInfo, string>((Func<CustomCharaFileInfo, string>) (n => n.personality)).ToList<CustomCharaFileInfo>());
          this.cscChara.CreateList(this.lstChara);
        }
      }
    }

    public void SelectInfoClear()
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cscChara))
        return;
      this.cscChara.SelectInfoClear();
    }

    public CustomCharaScrollController.ScrollData GetSelectInfo()
    {
      return Object.op_Inequality((Object) null, (Object) this.cscChara) ? this.cscChara.selectInfo : (CustomCharaScrollController.ScrollData) null;
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
      if (Object.op_Inequality((Object) null, (Object) this.button[0]))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[0]), (Action<M0>) (_ =>
        {
          if (this.onClick01 == null)
            return;
          this.onClick01(this.cscChara.selectInfo?.info);
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[0]), (Action<M0>) (_ => ((Selectable) this.button[0]).set_interactable((!this.btnDisableNotSelect01 ? 0 : (null == this.cscChara.selectInfo ? 1 : 0)) == 0)));
      }
      if (Object.op_Inequality((Object) null, (Object) this.button[1]))
      {
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[1]), (Action<M0>) (_ =>
        {
          if (this.onClick02 == null)
            return;
          this.onClick02(this.cscChara.selectInfo?.info);
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[1]), (Action<M0>) (_ => ((Selectable) this.button[1]).set_interactable((!this.btnDisableNotSelect02 ? 0 : (null == this.cscChara.selectInfo ? 1 : 0)) == 0)));
      }
      if (!Object.op_Inequality((Object) null, (Object) this.button[2]))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.button[2]), (Action<M0>) (_ =>
      {
        int num = 0;
        if (this.tglLoadOption[0].get_isOn())
          num |= 1;
        if (this.tglLoadOption[1].get_isOn())
          num |= 2;
        if (this.tglLoadOption[2].get_isOn())
          num |= 4;
        if (this.tglLoadOption[3].get_isOn())
          num |= 8;
        if (this.tglLoadOption[4].get_isOn())
          num |= 16;
        if (this.onClick03 == null)
          return;
        this.onClick03(this.cscChara.selectInfo?.info, num);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.button[2]), (Action<M0>) (_ => ((Selectable) this.button[2]).set_interactable((!this.btnDisableNotSelect03 ? 0 : (null == this.cscChara.selectInfo ? 1 : 0)) == 0)));
    }

    public enum LoadOption
    {
      Face = 1,
      Body = 2,
      Hair = 4,
      Coorde = 8,
      Status = 16, // 0x00000010
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

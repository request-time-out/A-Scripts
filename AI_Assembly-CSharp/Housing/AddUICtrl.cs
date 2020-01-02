// Decompiled with JetBrains decompiler
// Type: Housing.AddUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.SaveData;
using AIProject.Scene;
using Housing.Add;
using Housing.Command;
using Illusion.Extensions;
using Manager;
using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class AddUICtrl : UIDerived
  {
    [SerializeField]
    private int countPerRow = 6;
    private BoolReactiveProperty activeReactive = new BoolReactiveProperty(false);
    private IntReactiveProperty categoryReactive = new IntReactiveProperty(-1);
    private IntReactiveProperty selectReactive = new IntReactiveProperty(-1);
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    [Header("一覧関係")]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    [SerializeField]
    [Header("フィルター関係")]
    private Transform togglesRoot;
    [SerializeField]
    private GameObject objCategory;
    [SerializeField]
    private RawImage imageCategory;
    [SerializeField]
    private Text textCategory;
    [SerializeField]
    [Header("動作関係")]
    private Button buttonAdd;
    [SerializeField]
    private ButtonEx buttonClose;
    [SerializeField]
    private Text textItemLimit;
    [SerializeField]
    [Header("情報関係")]
    private CanvasGroup cgInfo;
    [SerializeField]
    private Text textName;
    [SerializeField]
    private Text textText;
    [SerializeField]
    private Image[] imagesInfo;
    [SerializeField]
    [Header("ロック関係")]
    private CanvasGroup cgCraft;
    [SerializeField]
    private MaterialUI materialUI;
    [SerializeField]
    private Button buttonActivate;
    private AddUICtrl.FileInfo[] fileInfos;
    private bool isForceUpdate;

    public bool Active
    {
      get
      {
        return ((ReactiveProperty<bool>) this.activeReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.activeReactive).set_Value(value);
      }
    }

    private int Category
    {
      get
      {
        return ((ReactiveProperty<int>) this.categoryReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this.categoryReactive).set_Value(value);
      }
    }

    private int Select
    {
      get
      {
        return ((ReactiveProperty<int>) this.selectReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this.selectReactive).set_Value(value);
      }
    }

    private bool ButtonAddEnable
    {
      set
      {
        ((Component) this.buttonAdd).get_gameObject().SetActiveIfDifferent(value);
      }
    }

    private bool Visible
    {
      set
      {
        this.canvasGroup.Enable(value, false);
      }
    }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      foreach (KeyValuePair<int, Manager.Housing.CategoryInfo> keyValuePair in Singleton<Manager.Housing>.Instance.dicCategoryInfo)
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objCategory, this.togglesRoot);
        gameObject.SetActive(true);
        TabUI tab = (TabUI) gameObject.GetComponent<TabUI>();
        tab.rawImage.set_texture((Texture) keyValuePair.Value.Thumbnail);
        Toggle toggleSelect = tab.toggleSelect;
        toggleSelect.set_isOn(false);
        int c = keyValuePair.Key;
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(toggleSelect), (Func<M0, bool>) (_b => _b)), (Action<M0>) (_ => this.Category = c));
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) toggleSelect), (Component) tab.imageCursor), (Action<M0>) (_ => ((Behaviour) tab.imageCursor).set_enabled(true))), gameObject);
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) toggleSelect), (Component) tab.imageCursor), (Action<M0>) (_ => ((Behaviour) tab.imageCursor).set_enabled(false))), gameObject);
      }
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.buttonClose), (Action<M0>) (_ => this.Active = false));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonAdd), (Action<M0>) (_ =>
      {
        AddUICtrl.FileInfo fileInfo = this.fileInfos.SafeGet<AddUICtrl.FileInfo>(this.Select);
        if (fileInfo != null)
        {
          Singleton<UndoRedoManager>.Instance.Do((ICommand) new AddItemCommand(fileInfo.no));
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        }
        this.Active = false;
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonActivate), (Action<M0>) (_ =>
      {
        ConfirmScene.Sentence = "作成しますか";
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          AddUICtrl.FileInfo fileInfo = this.fileInfos.SafeGet<AddUICtrl.FileInfo>(this.Select);
          if (fileInfo == null)
            return;
          fileInfo.SetUnlock();
          this.Visible = true;
          ((ReactiveProperty<int>) this.selectReactive).SetValueAndForceNotify(this.Select);
          this.view.RefreshAllShownItem();
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Craft);
        });
        ConfirmScene.OnClickedNo = (Action) (() => this.Visible = true);
        Singleton<Game>.Instance.LoadDialog();
        this.Visible = false;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.activeReactive, (Action<M0>) (_b =>
      {
        this.Visible = _b;
        if (!_b)
          return;
        this.isForceUpdate = true;
        ((ReactiveProperty<int>) this.selectReactive).SetValueAndForceNotify(-1);
        this.buttonClose.ClearState();
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.categoryReactive, (Action<M0>) (_i =>
      {
        this.CreateList(_i);
        Manager.Housing.CategoryInfo categoryInfo = (Manager.Housing.CategoryInfo) null;
        if (Singleton<Manager.Housing>.Instance.dicCategoryInfo.TryGetValue(_i, out categoryInfo))
        {
          this.textCategory.set_text(categoryInfo.name);
          this.imageCategory.set_texture((Texture) categoryInfo.Thumbnail);
        }
        else
          Debug.LogErrorFormat("存在しないカテゴリー[{0}]", new object[1]
          {
            (object) _i
          });
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this.selectReactive, (Action<M0>) (_i =>
      {
        AddUICtrl.FileInfo fileInfo = this.fileInfos.SafeGet<AddUICtrl.FileInfo>(_i);
        if (fileInfo != null)
        {
          if (fileInfo.Unlock)
          {
            this.cgCraft.Enable(false, false);
            bool flag = !Singleton<Manager.Housing>.Instance.CheckLimitNum(fileInfo.no);
            this.ButtonAddEnable = !flag;
            ((Behaviour) this.textItemLimit).set_enabled(flag);
          }
          else
          {
            this.cgCraft.Enable(true, false);
            ((Selectable) this.buttonActivate).set_interactable(this.materialUI.UpdateUI(fileInfo.loadInfo));
            this.ButtonAddEnable = false;
            ((Behaviour) this.textItemLimit).set_enabled(false);
          }
          this.textName.set_text(fileInfo.loadInfo.name);
          this.textText.set_text(fileInfo.loadInfo.text);
          ((Behaviour) this.imagesInfo[0]).set_enabled(fileInfo.loadInfo.isAccess);
          ((Behaviour) this.imagesInfo[1]).set_enabled(fileInfo.loadInfo.isAction);
          ((Behaviour) this.imagesInfo[2]).set_enabled(fileInfo.loadInfo.isHPoint);
          this.cgInfo.Enable(true, false);
        }
        else
        {
          this.cgInfo.Enable(false, false);
          this.cgCraft.Enable(false, false);
          this.ButtonAddEnable = false;
          ((Behaviour) this.textItemLimit).set_enabled(false);
          if (!this.isForceUpdate)
            return;
          this.view.RefreshAllShownItem();
          this.isForceUpdate = false;
        }
      }));
      ((ReactiveProperty<int>) this.categoryReactive).SetValueAndForceNotify(-1);
    }

    public override void UpdateUI()
    {
    }

    public void Reselect()
    {
      ((ReactiveProperty<int>) this.selectReactive).SetValueAndForceNotify(((ReactiveProperty<int>) this.selectReactive).get_Value());
    }

    private void CreateList(int _category)
    {
      int filter = _category < 0 ? -1 : 1 << _category;
      this.fileInfos = Singleton<Manager.Housing>.Instance.dicLoadInfo.Where<KeyValuePair<int, Manager.Housing.LoadInfo>>((Func<KeyValuePair<int, Manager.Housing.LoadInfo>, bool>) (v => (v.Value.Category & filter) > 0)).Where<KeyValuePair<int, Manager.Housing.LoadInfo>>((Func<KeyValuePair<int, Manager.Housing.LoadInfo>, bool>) (v => Singleton<Manager.Housing>.Instance.CheckSize(v.Value.size))).Select<KeyValuePair<int, Manager.Housing.LoadInfo>, AddUICtrl.FileInfo>((Func<KeyValuePair<int, Manager.Housing.LoadInfo>, AddUICtrl.FileInfo>) (v => new AddUICtrl.FileInfo()
      {
        no = v.Key,
        loadInfo = v.Value
      })).ToArray<AddUICtrl.FileInfo>();
      ((ReactiveProperty<int>) this.selectReactive).SetValueAndForceNotify(-1);
      int num = !((IList<AddUICtrl.FileInfo>) this.fileInfos).IsNullOrEmpty<AddUICtrl.FileInfo>() ? this.fileInfos.Length / this.countPerRow : 0;
      if (!((IList<AddUICtrl.FileInfo>) this.fileInfos).IsNullOrEmpty<AddUICtrl.FileInfo>() && this.fileInfos.Length % this.countPerRow > 0)
        ++num;
      if (!this.view.IsInit)
      {
        this.view.InitListView(num, (Func<LoopListView2, int, LoopListViewItem2>) ((_view, _index) => this.OnUpdate(_view, _index)), (LoopListViewInitParam) null);
      }
      else
      {
        if (this.view.SetListItemCount(num, true))
          return;
        this.view.RefreshAllShownItem();
      }
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      ItemRow itemRow = loopListViewItem2 as ItemRow;
      if (Object.op_Implicit((Object) itemRow))
      {
        for (int _index1 = 0; _index1 < this.countPerRow; ++_index1)
        {
          int index = _index * this.countPerRow + _index1;
          itemRow.SetData(_index1, this.fileInfos.SafeGet<AddUICtrl.FileInfo>(index), (Action) (() => this.Select = index), this.Select == index);
        }
      }
      return loopListViewItem2;
    }

    public class FileInfo
    {
      public int no;
      public Manager.Housing.LoadInfo loadInfo;

      public bool Unlock
      {
        get
        {
          Dictionary<int, Dictionary<int, bool>> unlock = Singleton<Game>.Instance.WorldData.HousingData.Unlock;
          if (!unlock.ContainsKey(this.loadInfo.category))
            return true;
          bool flag = false;
          return !unlock[this.loadInfo.category].TryGetValue(this.no, out flag) || flag;
        }
      }

      public void SetUnlock()
      {
        foreach (Manager.Housing.RequiredMaterial requiredMaterial in this.loadInfo.requiredMaterials)
          StuffItem.RemoveStorages(new StuffItem(requiredMaterial.category, requiredMaterial.no, requiredMaterial.num), (IReadOnlyCollection<List<StuffItem>>) new List<StuffItem>[2]
          {
            Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList,
            Singleton<Game>.Instance.Environment.ItemListInStorage
          });
        Singleton<Game>.Instance.WorldData.HousingData.Unlock[this.loadInfo.category][this.no] = true;
      }
    }
  }
}

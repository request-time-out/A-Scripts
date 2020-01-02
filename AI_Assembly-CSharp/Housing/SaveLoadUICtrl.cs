// Decompiled with JetBrains decompiler
// Type: Housing.SaveLoadUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Housing.SaveLoad;
using Illusion.Extensions;
using Manager;
using SuperScrollView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class SaveLoadUICtrl : UIDerived
  {
    [SerializeField]
    [Header("読込情報関係")]
    private SaveLoadUICtrl.Info infoLoad = new SaveLoadUICtrl.Info();
    private BoolReactiveProperty visibleReactive = new BoolReactiveProperty(false);
    private int thumbnailNum = -1;
    private int thumbnailLimit = 16;
    private int pageNum = -1;
    private int tab = -1;
    private int select = -1;
    private Dictionary<int, SaveLoadUICtrl.TabInfo> dicTabInfo = new Dictionary<int, SaveLoadUICtrl.TabInfo>();
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private ButtonEx buttonClose;
    [SerializeField]
    private Transform tabRoot;
    [SerializeField]
    private GameObject tabOriginal;
    [SerializeField]
    private RawImage[] rawsThumbnail;
    [SerializeField]
    private Button[] buttonsThumbnail;
    [SerializeField]
    private Texture textureNoData;
    [SerializeField]
    [Header("一覧関係")]
    private LoopListView2 view;
    [SerializeField]
    private GameObject original;
    private List<string> listPath;
    private int page;

    public bool Visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this.visibleReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.visibleReactive).set_Value(value);
      }
    }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.buttonClose), (Action<M0>) (_ => this.Close()));
      foreach (KeyValuePair<int, Manager.Housing.AreaSizeInfo> keyValuePair in Singleton<Manager.Housing>.Instance.dicAreaSizeInfo)
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.tabOriginal, this.tabRoot);
        gameObject.SetActive(true);
        SizeTab component = (SizeTab) gameObject.GetComponent<SizeTab>();
        int type = keyValuePair.Value.no;
        component.Text = string.Format("{0}X{1}X{2}", (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_x(), (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_y(), (object) ((Vector3Int) ref keyValuePair.Value.limitSize).get_z());
        ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(component.toggle), (Func<M0, bool>) (_b => _b)), (Action<M0>) (_ => this.SelectTab(type, false)));
        this.dicTabInfo[type] = new SaveLoadUICtrl.TabInfo()
        {
          gameObject = gameObject,
          sizeTab = component
        };
      }
      for (int index = 0; index < this.buttonsThumbnail.Length; ++index)
      {
        int idx = index;
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonsThumbnail[index]), (Action<M0>) (_ =>
        {
          this.infoLoad.Setup(this.rawsThumbnail[idx].get_texture());
          this.select = this.thumbnailLimit * this.page + idx;
        }));
      }
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.infoLoad.buttonClose), (Action<M0>) (_ => this.infoLoad.Visible = false));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.infoLoad.buttonDelete), (Action<M0>) (_ =>
      {
        ConfirmScene.Sentence = "データを消去しますか？";
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          File.Delete(this.listPath[this.select]);
          this.InitInfo(this.tab);
          this.SetPage(this.page, true);
          this.infoLoad.Visible = false;
        });
        ConfirmScene.OnClickedNo = (Action) (() => {});
        Singleton<Game>.Instance.LoadDialog();
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.infoLoad.buttonLoad), (Action<M0>) (_ =>
      {
        ConfirmScene.Sentence = "データを読込みますか？\n" + "セットされたアイテムは削除されます。".Coloring("#DE4529FF").Size(24);
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          Singleton<Selection>.Instance.SetSelectObjects((ObjectCtrl[]) null);
          this.UICtrl.ListUICtrl.ClearList();
          Singleton<UndoRedoManager>.Instance.Clear();
          this.Visible = false;
          Singleton<CraftScene>.Instance.WorkingUICtrl.Visible = true;
          Singleton<Manager.Housing>.Instance.LoadAsync(this.listPath[this.select], (Action<bool>) (_b =>
          {
            this.UICtrl.ListUICtrl.UpdateUI();
            Singleton<CraftScene>.Instance.WorkingUICtrl.Visible = false;
            this.Close();
          }));
        });
        ConfirmScene.OnClickedNo = (Action) (() => {});
        Singleton<Game>.Instance.LoadDialog();
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.visibleReactive, (Action<M0>) (_b =>
      {
        this.canvasGroup.Enable(_b, false);
        if (!_b)
          return;
        this.buttonClose.ClearState();
        this.infoLoad.Visible = false;
      }));
      int housingId = Singleton<CraftScene>.Instance.HousingID;
      Manager.Housing.AreaInfo areaInfo = (Manager.Housing.AreaInfo) null;
      if (Singleton<Manager.Housing>.Instance.dicAreaInfo.TryGetValue(housingId, out areaInfo))
      {
        Manager.Housing.AreaSizeInfo areaSizeInfo = (Manager.Housing.AreaSizeInfo) null;
        if (Singleton<Manager.Housing>.Instance.dicAreaSizeInfo.TryGetValue(areaInfo.size, out areaSizeInfo))
        {
          foreach (KeyValuePair<int, SaveLoadUICtrl.TabInfo> keyValuePair in this.dicTabInfo)
            keyValuePair.Value.Interactable = areaSizeInfo.compatibility.Contains(keyValuePair.Key);
        }
      }
      this.Visible = false;
    }

    public override void UpdateUI()
    {
    }

    public void Open()
    {
      this.Visible = true;
      this.UICtrl.ListUICtrl.Visible = false;
      this.UICtrl.AddUICtrl.Active = false;
      this.SelectTab(Singleton<CraftScene>.Instance.HousingID, true);
      Manager.Housing.AreaInfo areaInfo = (Manager.Housing.AreaInfo) null;
      if (!Singleton<Manager.Housing>.Instance.dicAreaInfo.TryGetValue(Singleton<CraftScene>.Instance.HousingID, out areaInfo))
        return;
      SaveLoadUICtrl.TabInfo tabInfo = (SaveLoadUICtrl.TabInfo) null;
      if (this.dicTabInfo.TryGetValue(areaInfo.size, out tabInfo))
        tabInfo.IsOn = true;
      this.SelectTab(areaInfo.size, false);
    }

    public void Close()
    {
      this.Visible = false;
      this.UICtrl.ListUICtrl.Visible = true;
    }

    private void InitInfo(int _idx)
    {
      List<KeyValuePair<DateTime, string>> list = System.IO.Directory.EnumerateFiles(UserData.Create(string.Format("{0}{1:00}/", (object) "housing/", (object) (_idx + 1))), "*.png").Select<string, SaveLoadUICtrl.FileInfo>((Func<string, SaveLoadUICtrl.FileInfo>) (s => new SaveLoadUICtrl.FileInfo(s))).Where<SaveLoadUICtrl.FileInfo>((Func<SaveLoadUICtrl.FileInfo, bool>) (fi => fi.CraftInfo != null)).Where<SaveLoadUICtrl.FileInfo>((Func<SaveLoadUICtrl.FileInfo, bool>) (fi => fi.Check(_idx))).Select<SaveLoadUICtrl.FileInfo, KeyValuePair<DateTime, string>>((Func<SaveLoadUICtrl.FileInfo, KeyValuePair<DateTime, string>>) (fi => new KeyValuePair<DateTime, string>(File.GetLastWriteTime(fi.Path), fi.Path))).ToList<KeyValuePair<DateTime, string>>();
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
      list.Sort((Comparison<KeyValuePair<DateTime, string>>) ((a, b) => b.Key.CompareTo(a.Key)));
      Thread.CurrentThread.CurrentCulture = currentCulture;
      this.listPath = list.Select<KeyValuePair<DateTime, string>, string>((Func<KeyValuePair<DateTime, string>, string>) (v => v.Value)).ToList<string>();
      this.thumbnailNum = !this.listPath.IsNullOrEmpty<string>() ? this.listPath.Count : 0;
      this.pageNum = this.thumbnailNum / this.thumbnailLimit + (this.thumbnailNum % this.thumbnailLimit == 0 ? 0 : 1);
      this.pageNum = Mathf.Max(this.pageNum, 1);
      if (!this.view.IsInit)
        this.view.InitListView(this.pageNum, (Func<LoopListView2, int, LoopListViewItem2>) ((_view, _index) => this.OnUpdate(_view, _index)), (LoopListViewInitParam) null);
      else if (!this.view.SetListItemCount(this.pageNum, true))
        this.view.RefreshAllShownItem();
      foreach (PageButton pageButton in this.view.ItemList.Select<LoopListViewItem2, PageButton>((Func<LoopListViewItem2, PageButton>) (_v => _v as PageButton)))
        pageButton?.Deselect();
    }

    private void SelectTab(int _idx, bool _force = false)
    {
      if (!_force && this.tab == _idx)
        return;
      this.InitInfo(_idx);
      this.tab = _idx;
      this.SetPage(0, true);
    }

    private void SetPage(int _page, bool _force = false)
    {
      if (!_force && this.page == _page)
        return;
      _page = Mathf.Clamp(_page, 0, this.pageNum - 1);
      int num = this.thumbnailLimit * _page;
      for (int index = 0; index < this.thumbnailLimit; ++index)
      {
        int n = num + index;
        if (!MathfEx.RangeEqualOn<int>(0, n, this.thumbnailNum - 1))
        {
          if (_page == 0 && index == 0)
          {
            this.rawsThumbnail[index].set_texture(this.textureNoData);
            ((Behaviour) this.rawsThumbnail[index]).set_enabled(true);
          }
          else
            ((Behaviour) this.rawsThumbnail[index]).set_enabled(false);
          ((Behaviour) this.buttonsThumbnail[index]).set_enabled(false);
        }
        else
        {
          this.rawsThumbnail[index].set_texture((Texture) PngAssist.LoadTexture(this.listPath[n]));
          ((Behaviour) this.rawsThumbnail[index]).set_enabled(true);
          ((Behaviour) this.buttonsThumbnail[index]).set_enabled(true);
          ((Selectable) this.buttonsThumbnail[index]).set_interactable(true);
        }
      }
      this.page = _page;
      if (_force && this.view.ItemList.FirstOrDefault<LoopListViewItem2>((Func<LoopListViewItem2, bool>) (_v => _v.ItemIndex == _page)) is PageButton pageButton)
        pageButton.Select();
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    private LoopListViewItem2 OnUpdate(LoopListView2 _view, int _index)
    {
      if (_index < 0)
        return (LoopListViewItem2) null;
      LoopListViewItem2 loopListViewItem2 = _view.NewListViewItem(((Object) this.original).get_name());
      if (loopListViewItem2 is PageButton pageButton)
        pageButton.SetData(_index, this.page == _index, (Action<int>) (_i => this.SetPage(_i, false)));
      return loopListViewItem2;
    }

    [Serializable]
    private class Info
    {
      public CanvasGroup canvasGroup;
      public ButtonEx buttonClose;
      public RawImage rawThumbnail;
      public ButtonEx buttonDelete;
      public ButtonEx buttonLoad;

      public bool Visible
      {
        get
        {
          return (double) this.canvasGroup.get_alpha() != 0.0;
        }
        set
        {
          this.canvasGroup.Enable(value, false);
        }
      }

      public void Setup(Texture _texture)
      {
        this.rawThumbnail.set_texture(_texture);
        this.Visible = true;
        this.buttonClose.ClearState();
        this.buttonDelete.ClearState();
        this.buttonLoad.ClearState();
      }
    }

    private class TabInfo
    {
      public GameObject gameObject;
      public SizeTab sizeTab;

      public bool Interactable
      {
        set
        {
          ((Selectable) this.sizeTab.toggle).set_interactable(value);
        }
      }

      public bool IsOn
      {
        set
        {
          this.sizeTab.toggle.set_isOn(value);
        }
      }
    }

    private class FileInfo
    {
      public FileInfo(string _path)
      {
        this.Path = _path;
        this.CraftInfo = CraftInfo.LoadStatic(_path);
      }

      public string Path { get; private set; }

      public CraftInfo CraftInfo { get; private set; }

      public bool Check(int _areaType)
      {
        return this.CraftInfo != null && _areaType == Singleton<Manager.Housing>.Instance.GetSizeType(this.CraftInfo.AreaNo);
      }
    }
  }
}

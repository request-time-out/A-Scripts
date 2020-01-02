// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CraftItemInfoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.ColorDefine;
using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class CraftItemInfoUI : ItemInfoUI
  {
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private RectTransform _warningViewerLayout;
    [SerializeField]
    private WarningViewer _warningViewer;
    private IDisposable _storageDisposable;
    private StuffItem _item;

    [DebuggerHidden]
    public static IEnumerator Load(
      CraftUI craftUI,
      Transform viewerParent,
      Action<CraftItemInfoUI> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftItemInfoUI.\u003CLoad\u003Ec__Iterator0()
      {
        viewerParent = viewerParent,
        craftUI = craftUI,
        onComplete = onComplete
      };
    }

    public int createSum
    {
      set
      {
        ((ReactiveProperty<int>) this._createSum).SetValueAndForceNotify(value);
      }
    }

    private string itemName { get; set; } = string.Empty;

    private IntReactiveProperty _createSum { get; } = new IntReactiveProperty(0);

    private BoolReactiveProperty _active { get; } = new BoolReactiveProperty(false);

    public IReadOnlyCollection<StuffItem> targetStorage { get; private set; }

    private IReadOnlyCollection<IReadOnlyCollection<StuffItem>> checkStorages
    {
      get
      {
        return this._craftUI.checkStorages;
      }
    }

    public override void Refresh(int count)
    {
      base.Refresh(count);
      ((ReactiveProperty<bool>) this._active).set_Value(count > 0);
    }

    public override void Refresh(StuffItem item)
    {
      this._item = item;
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID);
      this.itemName = stuffItemInfo.Name;
      ((ReactiveProperty<int>) this._createSum).SetValueAndForceNotify(0);
      this._flavorText.set_text(stuffItemInfo.Explanation);
      this.Refresh(item.Count);
    }

    public override void Close()
    {
      this._item = (StuffItem) null;
      this.Refresh(0);
      base.Close();
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Inequality((Object) this._infoLayout, (Object) null))
        this._infoLayout.SetActive(true);
      ((ReactiveProperty<bool>) this._active).set_Value(this.isOpen);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._active, (Action<M0>) (active => this.isCountViewerVisible = active));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._active, (Action<M0>) (active => ((Component) this._submitButton).get_gameObject().SetActive(active)));
      UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._createSum, this._itemName, (Func<M0, string>) (i =>
      {
        string str = string.Empty;
        if (i > 0)
          str = string.Format(" x {0}", (object) i);
        string itemName = this.itemName;
        if (itemName.IsNullOrEmpty())
          str = string.Empty;
        return string.Format("{0}{1}", (object) itemName, (object) str);
      }));
      this._storageDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this.LoadStorageType()), false));
      Text text = (Text) ((Component) this._submitButton).GetComponentInChildren<Text>();
      Color baseTextColor = ((Graphic) text).get_color();
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._submitButton), (Action<M0>) (_ => ((Graphic) text).set_color(Define.Get(Colors.Orange))));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._submitButton), (Action<M0>) (_ => ((Graphic) text).set_color(baseTextColor)));
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      if (this._storageDisposable != null)
        this._storageDisposable.Dispose();
      this._storageDisposable = (IDisposable) null;
      ((ReactiveProperty<bool>) this._active).Dispose();
    }

    [DebuggerHidden]
    private IEnumerator LoadStorageType()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftItemInfoUI.\u003CLoadStorageType\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadWarningViewer()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftItemInfoUI.\u003CLoadWarningViewer\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void EmptySlotCheck(int count)
    {
      count *= ((ReactiveProperty<int>) this._createSum).get_Value();
      this.targetStorage = (IReadOnlyCollection<StuffItem>) null;
      bool flag = false;
      if (count <= 0)
      {
        flag = true;
      }
      else
      {
        using (IEnumerator<IReadOnlyCollection<StuffItem>> enumerator = ((IEnumerable<IReadOnlyCollection<StuffItem>>) this.checkStorages).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            IReadOnlyCollection<StuffItem> current = enumerator.Current;
            int capacity;
            if (current == Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList)
              capacity = Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax;
            else if (current == Singleton<Game>.Instance.Environment.ItemListInStorage)
            {
              capacity = Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.StorageCapacity;
            }
            else
            {
              if (current != Singleton<Game>.Instance.Environment.ItemListInPantry)
              {
                Debug.LogError((object) "StorageType not found");
                continue;
              }
              continue;
            }
            if (this.IsStrageIn(capacity, count, current))
            {
              this.targetStorage = current;
              flag = true;
              break;
            }
          }
        }
      }
      this._warningViewer.visible = !flag;
      ((Selectable) this._submitButton).set_interactable(flag);
    }

    private bool IsStrageIn(int capacity, int count, IReadOnlyCollection<StuffItem> storage)
    {
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      foreach (StuffItem stuffItem in ((IEnumerable<StuffItem>) storage).FindItems(this._item))
      {
        int num = itemSlotMax - stuffItem.Count;
        count = Mathf.Max(count - num, 0);
        if (count <= 0)
          break;
      }
      int num1 = count / itemSlotMax + (count % itemSlotMax <= 0 ? 0 : 1);
      return capacity - storage.get_Count() - num1 >= 0;
    }

    private new void SetFocusLevel(int level)
    {
      Singleton<Input>.Instance.FocusLevel = level;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.ItemNodeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Illusion.Extensions;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class ItemNodeUI : SerializedMonoBehaviour
  {
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private Text _iconText;
    [SerializeField]
    private Text _nameLabel;
    [SerializeField]
    private RectTransform _itemNameLabelViewport;
    [SerializeField]
    private Image _rarelityImage;
    [SerializeField]
    private Text _rarelityText;
    [SerializeField]
    private Text _rateText;
    [SerializeField]
    private Text _stackCountText;
    [SerializeField]
    private Button _button;
    [SerializeField]
    [Header("Animation")]
    private Transform _animationRoot;
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    private float _easingDuration;
    [SerializeField]
    private MotionType _motionType;
    [SerializeField]
    private Transform _from;
    [SerializeField]
    private Transform _to;
    [SerializeField]
    private MotionType _alphaMotionType;
    [SerializeField]
    private float _fromAlpha;
    [SerializeField]
    private float _toAlpha;
    [SerializeField]
    private Image _line;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private StringReactiveProperty _name;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    protected IntReactiveProperty _stackCount;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _rate;
    [SerializeField]
    private Sprite[] _rarelities;
    private ReactiveProperty<Sprite> _rarelitySprite;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private ReactiveProperty<Rarelity> _rarelity;
    private GameObject _cachedgameObject;
    private CompositeDisposable disposables;

    public ItemNodeUI()
    {
      base.\u002Ector();
    }

    public static StuffItemInfo GetItemInfo(StuffItem item)
    {
      if (item == null)
      {
        Debug.LogError((object) "Item none");
        return (StuffItemInfo) null;
      }
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID) ?? Singleton<Resources>.Instance.GameInfo.GetItem_System(item.CategoryID, item.ID);
      if (stuffItemInfo == null)
        Debug.LogError((object) string.Format("{0}:{1:00},{2}:{3:00}", (object) "CategoryID", (object) item.CategoryID, (object) "ID", (object) item.ID));
      return stuffItemInfo;
    }

    public static void Sort(int sortID, bool ascending, IDictionary<int, ItemNodeUI> table)
    {
      List<KeyValuePair<int, ItemNodeUI>> keyValuePairList = new List<KeyValuePair<int, ItemNodeUI>>((IEnumerable<KeyValuePair<int, ItemNodeUI>>) table);
      keyValuePairList.Sort((IComparer<KeyValuePair<int, ItemNodeUI>>) new ItemNodeUI.DictionaryComparer((ItemNodeUI.DictionaryComparer.SortID) sortID, ascending));
      foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in keyValuePairList)
        ((Component) keyValuePair.Value).get_transform().SetAsLastSibling();
    }

    public Button.ButtonClickedEvent OnClick
    {
      get
      {
        return this._button.get_onClick();
      }
    }

    public bool IsInteractable
    {
      get
      {
        return ((Selectable) this._button).IsInteractable();
      }
    }

    public void Enabled()
    {
      ((Behaviour) this._button).set_enabled(true);
    }

    public void Disabled()
    {
      ((Behaviour) this._button).set_enabled(false);
    }

    public IObservable<PointerEventData> onEnter
    {
      get
      {
        return (IObservable<PointerEventData>) Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._button), (Func<M0, bool>) (_ => ((Behaviour) this._button).get_enabled()));
      }
    }

    public int CategoryID
    {
      get
      {
        return this._item.CategoryID;
      }
    }

    public string Name
    {
      get
      {
        return ((ReactiveProperty<string>) this._name).get_Value();
      }
    }

    public int Rate
    {
      get
      {
        return ((ReactiveProperty<int>) this._rate).get_Value();
      }
    }

    public DateTime LatestDateTime
    {
      get
      {
        return this._item.LatestDateTime;
      }
    }

    public int IconID
    {
      get
      {
        return this._info.IconID;
      }
    }

    public StuffItem Item
    {
      get
      {
        return this._item;
      }
    }

    private StuffItem _item { get; set; }

    public bool Visible
    {
      get
      {
        return this.cachedgameObject.get_activeSelf();
      }
      set
      {
        this.cachedgameObject.SetActiveIfDifferent(value);
      }
    }

    private GameObject cachedgameObject
    {
      get
      {
        return ((object) this).GetCacheObject<GameObject>(ref this._cachedgameObject, new Func<GameObject>(((Component) this).get_gameObject));
      }
    }

    private StuffItemInfo _info { get; set; }

    public bool isTrash { get; private set; }

    public bool isNone { get; private set; }

    public ItemNodeUI.ExtraData extraData { get; set; }

    public void Bind(StuffItem item, StuffItemInfo info = null)
    {
      this._info = info ?? ItemNodeUI.GetItemInfo(item);
      this.isTrash = this._info.isTrash;
      this.isNone = this._info.isNone;
      if (this.isNone)
      {
        this.isTrash = false;
        if (Object.op_Inequality((Object) this._stackCountText, (Object) null))
          ((Behaviour) this._stackCountText).set_enabled(false);
      }
      ((ReactiveProperty<string>) this._name).set_Value(this._info.Name);
      if (Object.op_Inequality((Object) this._iconImage, (Object) null))
      {
        Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, this._info.IconID, this._iconImage, true);
        if (Object.op_Equality((Object) this._iconImage.get_sprite(), (Object) null))
          ((Behaviour) this._iconImage).set_enabled(false);
      }
      this._rarelity.set_Value(this._info.Rarelity);
      IntReactiveProperty rate = this._rate;
      int? nullable = item is MerchantData.VendorItem vendorItem ? new int?(vendorItem.Rate) : new int?();
      int num = !nullable.HasValue ? this._info.Rate : nullable.Value;
      ((ReactiveProperty<int>) rate).set_Value(num);
      this._rarelitySprite.set_Value(this._rarelities.GetElement<Sprite>((int) this._info.Grade));
      this._item = item;
      ((ReactiveProperty<int>) this._stackCount).set_Value(this._item.Count);
    }

    public void Refresh()
    {
      ((ReactiveProperty<int>) this._stackCount).set_Value(this._item.Count);
    }

    protected virtual void Start()
    {
      if (Object.op_Inequality((Object) this._nameLabel, (Object) null))
        UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._name, this._nameLabel);
      if (Object.op_Inequality((Object) this._stackCountText, (Object) null))
        UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._stackCount, this._stackCountText);
      if (Object.op_Inequality((Object) this._rateText, (Object) null))
        UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._rate, this._rateText, (Func<M0, string>) (x => x >= 0 ? x.ToString() : string.Empty));
      if (Object.op_Inequality((Object) this._iconText, (Object) null))
        ((Behaviour) this._iconText).set_enabled(false);
      if (Object.op_Inequality((Object) this._rarelityText, (Object) null))
      {
        UnityUIComponentExtensions.SubscribeToText<Rarelity>((IObservable<M0>) this._rarelity, this._rarelityText, (Func<M0, string>) (x => x.GetType().IsEnumDefined((object) x) ? x.ToString() : string.Empty));
        ObservableExtensions.Subscribe<Sprite>((IObservable<M0>) this._rarelitySprite, (System.Action<M0>) (sprite => ((Behaviour) this._rarelityText).set_enabled(Object.op_Equality((Object) sprite, (Object) null))));
      }
      if (Object.op_Inequality((Object) this._rarelityImage, (Object) null))
        ObservableExtensions.Subscribe<Sprite>((IObservable<M0>) this._rarelitySprite, (System.Action<M0>) (sprite =>
        {
          ((Behaviour) this._rarelityImage).set_enabled(!Object.op_Equality((Object) sprite, (Object) null));
          this._rarelityImage.set_sprite(sprite);
        }));
      EasingFunction easing;
      if (!Tween.MotionFunctionTable.TryGetValue(this._motionType, ref easing))
        ;
      if (!Tween.MotionFunctionTable.TryGetValue(this._alphaMotionType, ref easing))
        return;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Create(easing, this._easingDuration, true), true), (System.Action<M0>) (x => this._canvasGroup.set_alpha(Mathf.Lerp(this._fromAlpha, this._toAlpha, ((TimeInterval<float>) ref x).get_Value())))), (ICollection<IDisposable>) this.disposables);
    }

    private void OnDestroy()
    {
      this.disposables.Clear();
    }

    public interface ExtraData
    {
    }

    private class DictionaryComparer : IComparer<KeyValuePair<int, ItemNodeUI>>
    {
      public DictionaryComparer(ItemNodeUI.DictionaryComparer.SortID sortID, bool ascending)
      {
        this.sortID = sortID;
        this.ascending = ascending;
      }

      private ItemNodeUI.DictionaryComparer.SortID sortID { get; }

      private bool ascending { get; }

      public int Compare(KeyValuePair<int, ItemNodeUI> x, KeyValuePair<int, ItemNodeUI> y)
      {
        ItemNodeUI itemNodeUi1 = x.Value;
        ItemNodeUI itemNodeUi2 = y.Value;
        if (itemNodeUi1.isNone && itemNodeUi2.isNone)
        {
          int num = this.SortCompare<int>(itemNodeUi1._item.CategoryID, itemNodeUi2._item.CategoryID);
          return num != 0 ? num : this.SortCompare<int>(itemNodeUi1._item.ID, itemNodeUi2._item.ID);
        }
        if (itemNodeUi1.isNone)
          return -1;
        if (itemNodeUi2.isNone)
          return 1;
        switch (this.sortID)
        {
          case ItemNodeUI.DictionaryComparer.SortID.Time:
            int num1 = !this.ascending ? this.SortCompare<DateTime>(itemNodeUi2.LatestDateTime, itemNodeUi1.LatestDateTime) : this.SortCompare<DateTime>(itemNodeUi1.LatestDateTime, itemNodeUi2.LatestDateTime);
            return num1 != 0 ? num1 : this.SortCompare<string>(itemNodeUi1.Name, itemNodeUi2.Name);
          case ItemNodeUI.DictionaryComparer.SortID.Name:
            return !this.ascending ? this.SortCompare<string>(itemNodeUi2.Name, itemNodeUi1.Name) : this.SortCompare<string>(itemNodeUi1.Name, itemNodeUi2.Name);
          case ItemNodeUI.DictionaryComparer.SortID.Gread:
            int num2 = !this.ascending ? this.SortCompare<Grade>(itemNodeUi2._info.Grade, itemNodeUi1._info.Grade) : this.SortCompare<Grade>(itemNodeUi1._info.Grade, itemNodeUi2._info.Grade);
            return num2 != 0 ? num2 : this.SortCompare<string>(itemNodeUi1.Name, itemNodeUi2.Name);
          case ItemNodeUI.DictionaryComparer.SortID.Category:
            int num3 = !this.ascending ? this.SortCompare<int>(itemNodeUi2._item.CategoryID, itemNodeUi1._item.CategoryID) : this.SortCompare<int>(itemNodeUi1._item.CategoryID, itemNodeUi2._item.CategoryID);
            if (num3 != 0)
              return num3;
            int num4 = this.SortCompare<int>(itemNodeUi1.IconID, itemNodeUi2.IconID);
            return num4 != 0 ? num4 : this.SortCompare<string>(itemNodeUi1.Name, itemNodeUi2.Name);
          default:
            Debug.LogError((object) ("DicComparer:" + (object) this.sortID));
            return 0;
        }
      }

      private int SortCompare<T>(T a, T b) where T : IComparable
      {
        return a.CompareTo((object) b);
      }

      public enum SortID
      {
        Time,
        Name,
        Gread,
        Category,
      }
    }
  }
}

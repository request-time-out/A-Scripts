// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CraftItemNodeUI
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class CraftItemNodeUI : SerializedMonoBehaviour
  {
    private UITrigger.TriggerEvent _onEnter;
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private Text _nameLabel;
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
    private CraftItemNodeUI.StuffItemInfoPack _pack;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private StringReactiveProperty _name;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    protected IntReactiveProperty _stackCount;
    private GameObject _cachedgameObject;
    private CompositeDisposable disposables;
    private CraftUI _craftUI;

    public CraftItemNodeUI()
    {
      base.\u002Ector();
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

    public UITrigger.TriggerEvent onEnter
    {
      get
      {
        return ((object) this).GetCache<UITrigger.TriggerEvent>(ref this._onEnter, (Func<UITrigger.TriggerEvent>) (() =>
        {
          UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
          ((UITrigger) ((Component) this).GetOrAddComponent<PointerEnterTrigger>()).get_Triggers().Add(triggerEvent);
          return triggerEvent;
        }));
      }
    }

    public RecipeDataInfo[] data { get; private set; }

    public bool isUnknown
    {
      get
      {
        return this._pack.isUnknown;
      }
    }

    public int CategoryID
    {
      get
      {
        return this._pack.info.CategoryID;
      }
    }

    public int ID
    {
      get
      {
        return this._pack.info.ID;
      }
    }

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

    private int ItemCount
    {
      get
      {
        return this._pack.isUnknown ? -1 : ((IEnumerable<IReadOnlyCollection<StuffItem>>) this._craftUI.checkStorages).SelectMany<IReadOnlyCollection<StuffItem>, StuffItem>((Func<IReadOnlyCollection<StuffItem>, IEnumerable<StuffItem>>) (x => (IEnumerable<StuffItem>) x)).FindItems(new StuffItem(this._pack.info.CategoryID, this._pack.info.ID, 0)).Sum<StuffItem>((Func<StuffItem, int>) (p => p.Count));
      }
    }

    public void Bind(
      CraftUI craftUI,
      CraftItemNodeUI.StuffItemInfoPack pack,
      RecipeDataInfo[] data)
    {
      this._craftUI = craftUI;
      this._pack = pack;
      this.data = data;
      this.Refresh();
    }

    public void Refresh()
    {
      bool flag1 = this._pack.possible == null;
      bool isUnknown = this._pack.isUnknown;
      bool isSuccess = this._pack.isSuccess;
      this._pack.Refresh();
      ((ReactiveProperty<int>) this._stackCount).set_Value(this.ItemCount);
      bool flag2 = isUnknown != this._pack.isUnknown;
      if (flag1 || flag2)
      {
        if (Object.op_Inequality((Object) this._iconImage, (Object) null))
          Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Item, this._pack.IconID, this._iconImage, true);
        ((ReactiveProperty<string>) this._name).set_Value(this._pack.Label);
      }
      bool flag3 = flag2 | isSuccess != this._pack.isSuccess;
      if (!flag1 && !flag3)
        return;
      float num = this._pack.isUnknown || !this._pack.isSuccess ? 0.5f : 1f;
      using (IEnumerator<Text> enumerator = ((IEnumerable<Text>) new Text[2]
      {
        this._nameLabel,
        this._stackCountText
      }).Where<Text>((Func<Text, bool>) (p => Object.op_Inequality((Object) p, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Graphic current = (Graphic) enumerator.Current;
          Color color = current.get_color();
          color.a = (__Null) (double) num;
          current.set_color(color);
        }
      }
    }

    protected virtual void Start()
    {
      if (Object.op_Inequality((Object) this._nameLabel, (Object) null))
        UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._name, this._nameLabel);
      if (Object.op_Inequality((Object) this._stackCountText, (Object) null))
      {
        int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
        UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._stackCount, this._stackCountText, (Func<M0, string>) (i =>
        {
          if (i < 0)
            return string.Empty;
          return i <= itemSlotMax ? string.Format("{0}", (object) i) : string.Format("{0}+", (object) itemSlotMax);
        }));
      }
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

    public class Possible
    {
      public Possible(bool unknown, bool success)
      {
        this.unknown = unknown;
        this.success = success;
      }

      public bool unknown { get; }

      public bool success { get; }
    }

    public class StuffItemInfoPack
    {
      public StuffItemInfoPack(StuffItemInfo info, Func<CraftItemNodeUI.Possible> PossibleFunc)
      {
        this._PossibleFunc = PossibleFunc;
        this.info = info;
      }

      public void Refresh()
      {
        this.possible = this._PossibleFunc();
      }

      public CraftItemNodeUI.Possible possible { get; private set; }

      private Func<CraftItemNodeUI.Possible> _PossibleFunc { get; }

      public StuffItemInfo info { get; }

      public bool isUnknown
      {
        get
        {
          bool? unknown = this.possible?.unknown;
          return !unknown.HasValue || unknown.Value;
        }
      }

      public bool isSuccess
      {
        get
        {
          bool? success = this.possible?.success;
          return success.HasValue && success.Value;
        }
      }

      public string Label
      {
        get
        {
          return this.isUnknown ? this.unknownInfo.Label : this.info.Name;
        }
      }

      public int IconID
      {
        get
        {
          return this.isUnknown ? this.unknownInfo.IconID : this.info.IconID;
        }
      }

      private CraftItemNodeUI.StuffItemInfoPack.UnknownInfo unknownInfo { get; } = new CraftItemNodeUI.StuffItemInfoPack.UnknownInfo();

      private class UnknownInfo
      {
        public int IconID
        {
          get
          {
            return 122;
          }
        }

        public string Label
        {
          get
          {
            return "？？？？";
          }
        }
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.UI.RecipeItemTitleNodeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.ColorDefine;
using AIProject.Definitions;
using AIProject.Scene;
using Illusion.Extensions;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class RecipeItemTitleNodeUI : SerializedMonoBehaviour
  {
    private UITrigger.TriggerEvent _onEnter;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _nameLabel;
    [SerializeField]
    private Text _successLabel;
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
    private BoolReactiveProperty _isSuccess;
    private GameObject _cachedgameObject;
    private CompositeDisposable disposables;
    private static GameObject _nodeBase;
    private RecipeDataInfo _data;
    [SerializeField]
    private RectTransform _content;

    public RecipeItemTitleNodeUI()
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

    public int MaxCount { get; private set; }

    public RecipeDataInfo data
    {
      get
      {
        return this._data;
      }
    }

    public IReadOnlyList<RecipeItemNodeUI> recipeItemNodeUIs { get; private set; }

    public string Name
    {
      get
      {
        return ((ReactiveProperty<string>) this._name).get_Value();
      }
    }

    public bool isSuccess
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isSuccess).get_Value();
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

    private static GameObject nodeBase
    {
      get
      {
        if (Object.op_Equality((Object) RecipeItemTitleNodeUI._nodeBase, (Object) null))
        {
          string bundle = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
          RecipeItemTitleNodeUI._nodeBase = CommonLib.LoadAsset<GameObject>(bundle, "RecipeItemNodeOption", false, string.Empty);
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundle)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundle, string.Empty));
        }
        return RecipeItemTitleNodeUI._nodeBase;
      }
    }

    public void Bind(CraftUI craftUI, int count, RecipeDataInfo data)
    {
      this._data = data;
      ((ReactiveProperty<string>) this._name).set_Value("レシピ" + (object) (count + 1));
      List<RecipeItemNodeUI> recipeItemNodeUiList = new List<RecipeItemNodeUI>();
      foreach (RecipeDataInfo.NeedData need in data.NeedList)
      {
        RecipeItemNodeUI recipeItemNodeUi = (RecipeItemNodeUI) Object.Instantiate<RecipeItemNodeUI>(RecipeItemTitleNodeUI.nodeBase.GetComponent<RecipeItemNodeUI>(), (Transform) this._content, false);
        recipeItemNodeUi.Bind(craftUI, need);
        recipeItemNodeUiList.Add(recipeItemNodeUi);
      }
      this.recipeItemNodeUIs = (IReadOnlyList<RecipeItemNodeUI>) recipeItemNodeUiList;
      this.Refresh();
    }

    public void Refresh()
    {
      foreach (RecipeItemNodeUI recipeItemNodeUi in (IEnumerable<RecipeItemNodeUI>) this.recipeItemNodeUIs)
        recipeItemNodeUi.Refresh();
      this.MaxCount = ((IEnumerable<RecipeItemNodeUI>) this.recipeItemNodeUIs).Select<RecipeItemNodeUI, int>((Func<RecipeItemNodeUI, int>) (x => x.MaxCount)).OrderBy<int, int>((Func<int, int>) (x => x)).FirstOrDefault<int>();
      ((ReactiveProperty<bool>) this._isSuccess).set_Value(this.MaxCount > 0);
      float num = !((ReactiveProperty<bool>) this._isSuccess).get_Value() ? 0.5f : 1f;
      foreach (RecipeItemNodeUI recipeItemNodeUi in (IEnumerable<RecipeItemNodeUI>) this.recipeItemNodeUIs)
      {
        foreach (Graphic graphic in recipeItemNodeUi.graphics)
        {
          Color color = graphic.get_color();
          color.a = (__Null) (double) num;
          graphic.set_color(color);
        }
      }
    }

    protected virtual void Start()
    {
      if (Object.op_Inequality((Object) this._nameLabel, (Object) null))
        UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._name, this._nameLabel);
      if (Object.op_Inequality((Object) this._successLabel, (Object) null))
      {
        UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) this._isSuccess, (Selectable) this._button);
        UnityUIComponentExtensions.SubscribeToText<bool>((IObservable<M0>) this._isSuccess, this._successLabel, (Func<M0, string>) (success => success ? "OK!" : string.Empty));
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isSuccess, (System.Action<M0>) (success => ((Graphic) this._successLabel).set_color(Define.Get(!success ? Colors.White : Colors.Blue))));
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
  }
}

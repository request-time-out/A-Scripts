// Decompiled with JetBrains decompiler
// Type: AIProject.UI.RecipeItemNodeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.ColorDefine;
using AIProject.SaveData;
using AIProject.UI.Viewer;
using Illusion.Extensions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class RecipeItemNodeUI : SerializedMonoBehaviour
  {
    [SerializeField]
    private Text _nameLabel;
    [SerializeField]
    private ConditionalTextXtoYViewer _slotCounter;
    [SerializeField]
    [Header("Animation")]
    private Transform _animationRoot;
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _line;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private StringReactiveProperty _name;
    private GameObject _cachedgameObject;
    private CompositeDisposable disposables;
    private RecipeDataInfo.NeedData _data;
    private Graphic[] _graphics;
    private CraftUI _craftUI;

    public RecipeItemNodeUI()
    {
      base.\u002Ector();
    }

    public string Name
    {
      get
      {
        return ((ReactiveProperty<string>) this._name).get_Value();
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

    public Graphic[] graphics
    {
      get
      {
        return ((object) this).GetCache<Graphic[]>(ref this._graphics, (Func<Graphic[]>) (() =>
        {
          List<Graphic> graphicList = new List<Graphic>();
          if (Object.op_Inequality((Object) this._nameLabel, (Object) null))
            graphicList.Add((Graphic) this._nameLabel);
          if (Object.op_Inequality((Object) this._slotCounter.layout, (Object) null))
            graphicList.AddRange((IEnumerable<Graphic>) this._slotCounter.layout.GetComponentsInChildren<Graphic>(true));
          return graphicList.ToArray();
        }));
      }
    }

    public int MaxCount { get; private set; }

    private int ItemCount
    {
      get
      {
        return ((IEnumerable<IReadOnlyCollection<StuffItem>>) this._craftUI.checkStorages).SelectMany<IReadOnlyCollection<StuffItem>, StuffItem>((Func<IReadOnlyCollection<StuffItem>, IEnumerable<StuffItem>>) (x => (IEnumerable<StuffItem>) x)).FindItems(new StuffItem(this._data.CategoryID, this._data.ID, 0)).Sum<StuffItem>((Func<StuffItem, int>) (p => p.Count));
      }
    }

    public void Bind(CraftUI craftUI, RecipeDataInfo.NeedData data)
    {
      this._craftUI = craftUI;
      this._data = data;
      ((ReactiveProperty<string>) this._name).set_Value(data.Name);
      this._slotCounter.y = data.Sum;
      this.Refresh();
    }

    public void Refresh()
    {
      this._slotCounter.x = this.ItemCount;
      this.MaxCount = this._slotCounter.x / this._slotCounter.y;
    }

    protected virtual void Start()
    {
      if (Object.op_Inequality((Object) this._nameLabel, (Object) null))
        UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._name, this._nameLabel);
      this._slotCounter.Initialize();
      if (!Object.op_Inequality((Object) this._slotCounter.xText, (Object) null))
        return;
      IObservable<M1> observable = Observable.Select<int, Colors>((IObservable<M0>) this._slotCounter.X, (Func<M0, M1>) (i => i < this._slotCounter.y ? Colors.Red : Colors.White));
      // ISSUE: reference to a compiler-generated field
      if (RecipeItemNodeUI.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RecipeItemNodeUI.\u003C\u003Ef__mg\u0024cache0 = new Func<Colors, Color>(Define.Get);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Colors, Color> fMgCache0 = RecipeItemNodeUI.\u003C\u003Ef__mg\u0024cache0;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Color>((IObservable<M0>) Observable.Select<Colors, Color>((IObservable<M0>) observable, (Func<M0, M1>) fMgCache0), (Action<M0>) (color => ((Graphic) this._slotCounter.xText).set_color(color))), (Component) this._slotCounter.xText);
    }

    private void OnDestroy()
    {
      this.disposables.Clear();
    }
  }
}

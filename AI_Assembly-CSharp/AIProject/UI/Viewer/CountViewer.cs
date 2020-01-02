// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.CountViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class CountViewer : MonoBehaviour
  {
    [SerializeField]
    private Text _countText;
    [SerializeField]
    private InputField _inputCounter;
    [SerializeField]
    [Header("Addition")]
    private Button[] _addButtons;
    [SerializeField]
    [Header("Subtract")]
    private Button[] _subButtons;
    private IntReactiveProperty _count;

    public CountViewer()
    {
      base.\u002Ector();
    }

    public IObservable<int> Counter
    {
      get
      {
        return (IObservable<int>) this._count;
      }
    }

    public int Count
    {
      get
      {
        return ((ReactiveProperty<int>) this._count).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._count).set_Value(value);
      }
    }

    public int MaxCount { get; set; }

    public int ForceCount
    {
      set
      {
        ((ReactiveProperty<int>) this._count).SetValueAndForceNotify(value);
      }
    }

    [DebuggerHidden]
    public static IEnumerator Load(
      Transform viewerParent,
      Action<CountViewer> onComplete)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CountViewer.\u003CLoad\u003Ec__Iterator0()
      {
        onComplete = onComplete,
        viewerParent = viewerParent
      };
    }

    private void Start()
    {
      ReadOnlyReactiveProperty<bool> reactiveProperty1 = (ReadOnlyReactiveProperty<bool>) ReactivePropertyExtensions.ToReadOnlyReactiveProperty<bool>((IObservable<M0>) Observable.Select<int, bool>((IObservable<M0>) this._count, (Func<M0, M1>) (i => i > 1)));
      ReadOnlyReactiveProperty<bool> reactiveProperty2 = (ReadOnlyReactiveProperty<bool>) ReactivePropertyExtensions.ToReadOnlyReactiveProperty<bool>((IObservable<M0>) Observable.Select<int, bool>((IObservable<M0>) this._count, (Func<M0, M1>) (i => i < this.MaxCount)));
      List<IObservable<int>> source = new List<IObservable<int>>();
      // ISSUE: object of a compiler-generated type is created
      using (IEnumerator<\u003C\u003E__AnonType27<Button, int>> enumerator = ((IEnumerable<Button>) this._addButtons).Select<Button, \u003C\u003E__AnonType27<Button, int>>((Func<Button, int, \u003C\u003E__AnonType27<Button, int>>) ((bt, index) => new \u003C\u003E__AnonType27<Button, int>(bt, index != 0 ? index * 10 : 1))).Where<\u003C\u003E__AnonType27<Button, int>>((Func<\u003C\u003E__AnonType27<Button, int>, bool>) (bt => bt != null)).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType27<Button, int> item = enumerator.Current;
          UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) reactiveProperty2, (Selectable) item.bt);
          source.Add((IObservable<int>) Observable.Select<Unit, int>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.bt), (Func<M0, M1>) (_ => item.add)));
        }
      }
      // ISSUE: object of a compiler-generated type is created
      using (IEnumerator<\u003C\u003E__AnonType27<Button, int>> enumerator = ((IEnumerable<Button>) this._subButtons).Select<Button, \u003C\u003E__AnonType27<Button, int>>((Func<Button, int, \u003C\u003E__AnonType27<Button, int>>) ((bt, index) => new \u003C\u003E__AnonType27<Button, int>(bt, index != 0 ? index * 10 : 1))).Where<\u003C\u003E__AnonType27<Button, int>>((Func<\u003C\u003E__AnonType27<Button, int>, bool>) (bt => bt != null)).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType27<Button, int> item = enumerator.Current;
          UnityUIComponentExtensions.SubscribeToInteractable((IObservable<bool>) reactiveProperty1, (Selectable) item.bt);
          source.Add((IObservable<int>) Observable.Select<Unit, int>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.bt), (Func<M0, M1>) (_ => -item.add)));
        }
      }
      if (source.Any<IObservable<int>>())
        ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<int, int>(Observable.Merge<int>((IEnumerable<IObservable<M0>>) source), (Func<M0, M1>) (value => Mathf.Clamp(((ReactiveProperty<int>) this._count).get_Value() + value, 1, this.MaxCount))), (Action<M0>) (value =>
        {
          ((ReactiveProperty<int>) this._count).set_Value(value);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        }));
      if (!Object.op_Inequality((Object) this._inputCounter, (Object) null))
        return;
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._count, (Action<M0>) (i => this._inputCounter.set_text(i.ToString())));
      ObservableExtensions.Subscribe<BaseEventData>(Observable.Take<BaseEventData>((IObservable<M0>) ObservableTriggerExtensions.OnSelectAsObservable((UIBehaviour) this._inputCounter), 1), (Action<M0>) (_ =>
      {
        Transform transform = ((Component) this._inputCounter).get_transform().Find(((Object) this._inputCounter).get_name() + " Input Caret");
        if (!Object.op_Inequality((Object) transform, (Object) null))
          return;
        RectTransform component = (RectTransform) ((Component) transform).GetComponent<RectTransform>();
        RectTransform rectTransform = ((Graphic) this._countText).get_rectTransform();
        component.set_anchoredPosition(rectTransform.get_anchoredPosition());
        component.set_sizeDelta(rectTransform.get_sizeDelta());
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<string, int>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this._inputCounter), (Func<M0, M1>) (text =>
      {
        int result;
        int.TryParse(text, out result);
        return Mathf.Clamp(result, 1, this.MaxCount);
      })), (Action<M0>) (n =>
      {
        ((ReactiveProperty<int>) this._count).SetValueAndForceNotify(n);
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
    }
  }
}

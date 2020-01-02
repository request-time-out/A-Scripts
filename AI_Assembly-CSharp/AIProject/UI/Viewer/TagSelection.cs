// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.TagSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public static class TagSelection
  {
    public static IDisposable BindToGroup(
      this IEnumerable<Toggle> toggles,
      Action<int> action)
    {
      return TagSelection.BindToGroup(action, toggles.ToArray<Toggle>());
    }

    public static IDisposable BindToGroup(Action<int> action, params Toggle[] toggles)
    {
      Toggle[] toggleArray = toggles;
      // ISSUE: reference to a compiler-generated field
      if (TagSelection.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TagSelection.\u003C\u003Ef__mg\u0024cache0 = new Func<Toggle, IObservable<bool>>(UnityUIComponentExtensions.OnValueChangedAsObservable);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Toggle, IObservable<bool>> fMgCache0 = TagSelection.\u003C\u003Ef__mg\u0024cache0;
      return ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) Observable.Select<IList<bool>, int>((IObservable<M0>) Observable.CombineLatest<bool>((IEnumerable<IObservable<M0>>) ((IEnumerable<Toggle>) toggleArray).Select<Toggle, IObservable<bool>>(fMgCache0)), (Func<M0, M1>) (list => list.IndexOf(true))), (Func<M0, bool>) (sel => sel >= 0)), (Action<M0>) (sel =>
      {
        Action<int> action1 = action;
        if (action1 == null)
          return;
        action1(sel);
      }));
    }

    public static CompositeDisposable BindToEnter(
      this IEnumerable<Selectable> selectables,
      bool isExit,
      Image cursor)
    {
      return TagSelection.BindToEnter(isExit, cursor, selectables.ToArray<Selectable>());
    }

    public static CompositeDisposable BindToEnter(
      bool isExit,
      Image cursor,
      params Selectable[] selectables)
    {
      CompositeDisposable compositeDisposable = new CompositeDisposable();
      foreach (Selectable selectable in selectables)
      {
        Selectable item = selectable;
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) item), (Component) cursor), (Func<M0, bool>) (_ => item.IsInteractable())), (Action<M0>) (_ =>
        {
          ((Behaviour) cursor).set_enabled(true);
          CursorFrame.Set(((Graphic) cursor).get_rectTransform(), (RectTransform) ((Component) item).GetComponent<RectTransform>(), (RectTransform) null);
        })), (ICollection<IDisposable>) compositeDisposable);
        if (isExit)
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) item), (Component) cursor), (Action<M0>) (_ => ((Behaviour) cursor).set_enabled(false))), (ICollection<IDisposable>) compositeDisposable);
      }
      return compositeDisposable;
    }

    public static CompositeDisposable BindToEnter(
      this IEnumerable<TagSelection.ICursorTagElement> elements,
      bool isExit)
    {
      return TagSelection.BindToEnter(isExit, elements.ToArray<TagSelection.ICursorTagElement>());
    }

    public static CompositeDisposable BindToEnter(
      bool isExit,
      params TagSelection.ICursorTagElement[] elements)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TagSelection.\u003CBindToEnter\u003Ec__AnonStorey4 enterCAnonStorey4_1 = new TagSelection.\u003CBindToEnter\u003Ec__AnonStorey4();
      // ISSUE: reference to a compiler-generated field
      enterCAnonStorey4_1.isExit = isExit;
      // ISSUE: reference to a compiler-generated field
      enterCAnonStorey4_1.elements = elements;
      CompositeDisposable compositeDisposable = new CompositeDisposable();
      // ISSUE: reference to a compiler-generated field
      foreach (TagSelection.ICursorTagElement element in enterCAnonStorey4_1.elements)
      {
        TagSelection.ICursorTagElement item = element;
        CursorFrame.Set(((Graphic) item.cursor).get_rectTransform(), (RectTransform) ((Component) item.selectable).GetComponent<RectTransform>(), (RectTransform) null);
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Image>((IObservable<M0>) Observable.Select<PointerEventData, Image>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) item.selectable), (Component) item.cursor), (Func<M0, M1>) (_ => item.cursor)), (Action<M0>) (cursor =>
        {
          // ISSUE: variable of a compiler-generated type
          TagSelection.\u003CBindToEnter\u003Ec__AnonStorey4 enterCAnonStorey4 = enterCAnonStorey4_1;
          Image cursor1 = cursor;
          ((Behaviour) cursor1).set_enabled(true);
          if (isExit)
            return;
          foreach (TagSelection.ICursorTagElement cursorTagElement in ((IEnumerable<TagSelection.ICursorTagElement>) elements).Where<TagSelection.ICursorTagElement>((Func<TagSelection.ICursorTagElement, bool>) (x => Object.op_Inequality((Object) x.cursor, (Object) cursor1))))
            ((Behaviour) cursorTagElement.cursor).set_enabled(false);
        })), (ICollection<IDisposable>) compositeDisposable);
        // ISSUE: reference to a compiler-generated field
        if (enterCAnonStorey4_1.isExit)
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) item.selectable), (Component) item.cursor), (Action<M0>) (_ => ((Behaviour) item.cursor).set_enabled(false))), (ICollection<IDisposable>) compositeDisposable);
      }
      return compositeDisposable;
    }

    public interface ICursorTagElement
    {
      Image cursor { get; }

      Selectable selectable { get; }
    }
  }
}

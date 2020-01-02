// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Extensions.SelectableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Illusion.Game.Extensions
{
  public static class SelectableExtensions
  {
    public static IObservable<IList<double>> DoubleInterval<T>(
      this IObservable<T> source,
      float interval,
      bool isHot = true)
    {
      return !isHot ? SelectableExtensions.CreateDoubleIntervalStream<T>(source, interval) : (IObservable<IList<double>>) Observable.Share<IList<double>>((IObservable<M0>) SelectableExtensions.CreateDoubleIntervalStream<T>(source, interval));
    }

    public static IConnectableObservable<IList<double>> DoubleIntervalPublish<T>(
      this IObservable<T> source,
      float interval)
    {
      return (IConnectableObservable<IList<double>>) Observable.Publish<IList<double>>((IObservable<M0>) SelectableExtensions.CreateDoubleIntervalStream<T>(source, interval));
    }

    private static IObservable<IList<double>> CreateDoubleIntervalStream<T>(
      IObservable<T> source,
      float interval)
    {
      return (IObservable<IList<double>>) Observable.Where<IList<double>>(Observable.Where<IList<double>>((IObservable<M0>) Observable.Buffer<double>((IObservable<M0>) Observable.Select<TimeInterval<T>, double>((IObservable<M0>) Observable.TimeInterval<T>(source), (Func<M0, M1>) (t => ((TimeInterval<T>) ref t).get_Interval().TotalMilliseconds)), 2, 1), (Func<M0, bool>) (list => list[0] > (double) interval)), (Func<M0, bool>) (list => list[1] <= (double) interval));
    }

    public static IDisposable SubscribeToText(
      this IObservable<string> source,
      TextMeshProUGUI text)
    {
      return ObservableExtensions.SubscribeWithState<string, TextMeshProUGUI>((IObservable<M0>) source, (M1) text, (Action<M0, M1>) ((x, t) => ((TMP_Text) t).set_text(x)));
    }

    public static List<Tuple<Selectable, SelectUI>> SelectSEAdd<T>(
      this T _,
      params Selectable[] selectables)
    {
      Selectable[] selectableArray = selectables;
      // ISSUE: reference to a compiler-generated field
      if (SelectableExtensions.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SelectableExtensions.\u003C\u003Ef__mg\u0024cache0 = new Func<Selectable, Tuple<Selectable, SelectUI>>(SelectableExtensions.SelectSE);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Selectable, Tuple<Selectable, SelectUI>> fMgCache0 = SelectableExtensions.\u003C\u003Ef__mg\u0024cache0;
      return ((IEnumerable<Selectable>) selectableArray).Select<Selectable, Tuple<Selectable, SelectUI>>(fMgCache0).ToList<Tuple<Selectable, SelectUI>>();
    }

    public static Tuple<Selectable, SelectUI> SelectSE(
      this Selectable selectable)
    {
      SelectUI selectUI = ((Component) selectable).GetOrAddComponent<SelectUI>();
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<Selectable, bool>((M0) selectable, (Func<M0, M1>) (_ => selectUI.IsSelect && selectable.IsInteractable()), (FrameCountType) 0, false), (Component) selectable), (Func<M0, bool>) (sel => sel)), (Action<M0>) (_ => Illusion.Game.Utils.Sound.Play(SystemSE.sel)));
      return Tuple.Create<Selectable, SelectUI>(selectable, selectUI);
    }

    public static void FocusAdd(
      this Component component,
      bool isTabKey,
      Func<bool> isFocus,
      Func<int> next,
      Func<Tuple<bool, int>> direct,
      int firstIndex,
      params Selectable[] sels)
    {
      if (((IList<Selectable>) sels).IsNullOrEmpty<Selectable>())
      {
        Debug.LogError((object) "FocusRegister Selection None");
      }
      else
      {
        Selectable lastCurrent = sels.SafeGet<Selectable>(firstIndex);
        ObservableExtensions.Subscribe<Selectable>((IObservable<M0>) Observable.Select<GameObject, Selectable>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<Component, GameObject>((M0) component, (Func<M0, M1>) (_ => EventSystem.get_current().get_currentSelectedGameObject()), (FrameCountType) 0, false), (Func<M0, M1>) (go => Object.op_Inequality((Object) go, (Object) null) ? (Selectable) go.GetComponent<Selectable>() : (Selectable) null)), (Action<M0>) (sel =>
        {
          if (((IEnumerable<Selectable>) sels).Contains<Selectable>(sel))
          {
            lastCurrent = sel;
          }
          else
          {
            if (!isFocus() || !Object.op_Inequality((Object) lastCurrent, (Object) null))
              return;
            lastCurrent.Select();
          }
        }));
        Action<int, bool> focus = (Action<int, bool>) ((index, isDirect) =>
        {
          bool flag = index >= 0;
          if (!isDirect)
            index += sels.Check<Selectable>((Func<Selectable, bool>) (v => Object.op_Equality((Object) v, (Object) lastCurrent)));
          MathfEx.LoopValue(ref index, 0, sels.Length - 1);
          if (!sels[index].IsInteractable() && ((IEnumerable<Selectable>) sels).Any<Selectable>((Func<Selectable, bool>) (p => p.IsInteractable())))
          {
            if (!flag)
              index = Mathf.Min(sels.Length, index + 1);
            IEnumerable<int> ints1 = Enumerable.Range(index, sels.Length - index);
            IEnumerable<int> ints2 = Enumerable.Range(0, index);
            index = (!flag ? ints2.Reverse<int>().Concat<int>(ints1.Reverse<int>()) : ints1.Concat<int>(ints2)).FirstOrDefault<int>((Func<int, bool>) (i => sels[i].IsInteractable()));
          }
          sels[index].Select();
        });
        if (isTabKey)
          ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable(component), (Func<M0, bool>) (_ => isFocus())), (Func<M0, bool>) (_ => Input.GetKeyDown((KeyCode) 9))), (Action<M0>) (_ => focus(Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303) ? -1 : 1, false)));
        if (!next.IsNullOrEmpty())
          ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable(component), (Func<M0, bool>) (_ => isFocus())), (Action<M0>) (_ =>
          {
            int num = next();
            if (num == 0)
              return;
            focus(num, false);
          }));
        if (direct.IsNullOrEmpty())
          return;
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable(component), (Func<M0, bool>) (_ => isFocus())), (Action<M0>) (_ =>
        {
          Tuple<bool, int> tuple = direct();
          if (!tuple.Item1)
            return;
          focus(tuple.Item2, true);
        }));
      }
    }
  }
}

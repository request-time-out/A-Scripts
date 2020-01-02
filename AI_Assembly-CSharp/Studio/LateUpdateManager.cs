// Decompiled with JetBrains decompiler
// Type: Studio.LateUpdateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Studio
{
  public class LateUpdateManager : Singleton<LateUpdateManager>
  {
    private ILateUpdatable[] arrayUpdatable = new ILateUpdatable[16];
    private const int InitializeSize = 16;
    private int tail;
    [SerializeField]
    private bool m_ReduceArraySizeWhenNeed;

    public static bool reduceArraySizeWhenNeed
    {
      get
      {
        return Singleton<LateUpdateManager>.IsInstance() && Singleton<LateUpdateManager>.Instance.m_ReduceArraySizeWhenNeed;
      }
      set
      {
        if (!Singleton<LateUpdateManager>.IsInstance())
          return;
        Singleton<LateUpdateManager>.Instance.m_ReduceArraySizeWhenNeed = value;
      }
    }

    public static void AddUpdatableST(ILateUpdatable _updatable)
    {
      if (_updatable == null || !Singleton<LateUpdateManager>.IsInstance())
        return;
      Singleton<LateUpdateManager>.Instance.AddUpdatable(_updatable);
    }

    private void AddUpdatable(ILateUpdatable _updatable)
    {
      if (this.arrayUpdatable.Length == this.tail)
        Array.Resize<ILateUpdatable>(ref this.arrayUpdatable, checked (this.tail * 2));
      this.arrayUpdatable[this.tail++] = _updatable;
    }

    public static void RemoveUpdatableST(ILateUpdatable _updatable)
    {
      if (_updatable == null || !Singleton<LateUpdateManager>.IsInstance())
        return;
      Singleton<LateUpdateManager>.Instance.RemoveUpdatable(_updatable);
    }

    private void RemoveUpdatable(ILateUpdatable _updatable)
    {
      for (int index = 0; index < this.arrayUpdatable.Length; ++index)
      {
        if (this.arrayUpdatable[index] == _updatable)
        {
          this.arrayUpdatable[index] = (ILateUpdatable) null;
          break;
        }
      }
    }

    public static void RefreshArrayUpdatableST()
    {
      if (!Singleton<LateUpdateManager>.IsInstance())
        return;
      Singleton<LateUpdateManager>.Instance.RefreshArrayUpdatable();
    }

    private void RefreshArrayUpdatable()
    {
      int index1 = this.tail - 1;
label_9:
      for (int index2 = 0; index2 < this.arrayUpdatable.Length; ++index2)
      {
        if (this.arrayUpdatable[index2] == null)
        {
          for (; index2 < index1; --index1)
          {
            ILateUpdatable lateUpdatable = this.arrayUpdatable[index1];
            if (lateUpdatable != null)
            {
              this.arrayUpdatable[index2] = lateUpdatable;
              this.arrayUpdatable[index1] = (ILateUpdatable) null;
              --index1;
              goto label_9;
            }
          }
          this.tail = index2;
          break;
        }
      }
      if (!this.m_ReduceArraySizeWhenNeed || this.tail >= this.arrayUpdatable.Length / 2)
        return;
      Array.Resize<ILateUpdatable>(ref this.arrayUpdatable, this.arrayUpdatable.Length / 2);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), (Action<M0>) (_ =>
      {
        for (int index = 0; index < this.tail; ++index)
        {
          if (this.arrayUpdatable[index] != null)
            this.arrayUpdatable[index].LateUpdateFunc();
        }
      })), (Component) this);
    }
  }
}

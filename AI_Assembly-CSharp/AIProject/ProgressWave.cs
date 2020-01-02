// Decompiled with JetBrains decompiler
// Type: AIProject.ProgressWave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class ProgressWave : MonoBehaviour
  {
    [SerializeField]
    private Image[] _images;
    private IDisposable _disposable;
    [SerializeField]
    private float _minSize;
    [SerializeField]
    private float _maxSize;
    private Vector2 _inEndSize;
    private Vector2 _outEndSize;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _delayDuration;
    [SerializeField]
    private float _waitDuration;

    public ProgressWave()
    {
      base.\u002Ector();
    }

    public void PlayAnim(bool isLoop)
    {
      this._disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.WaveAnimCoroutine(isLoop)), false));
    }

    public void Stop()
    {
      if (this._disposable == null)
        return;
      this._disposable.Dispose();
    }

    [DebuggerHidden]
    private IEnumerator WaveAnimCoroutine(bool isLoop)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ProgressWave.\u003CWaveAnimCoroutine\u003Ec__Iterator0()
      {
        isLoop = isLoop,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator WaveCoroutine(Image image)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ProgressWave.\u003CWaveCoroutine\u003Ec__Iterator1()
      {
        image = image,
        \u0024this = this
      };
    }
  }
}

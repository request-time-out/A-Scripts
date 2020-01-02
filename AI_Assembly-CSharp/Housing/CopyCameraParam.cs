// Decompiled with JetBrains decompiler
// Type: Housing.CopyCameraParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace Housing
{
  public class CopyCameraParam : MonoBehaviour
  {
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera targetCamera;

    public CopyCameraParam()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<Camera, float>((M0) this.mainCamera, (Func<M0, M1>) (_c => _c.get_fieldOfView()), (FrameCountType) 0, false), (Action<M0>) (_f => this.targetCamera.set_fieldOfView(_f))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<Camera, float>((M0) this.mainCamera, (Func<M0, M1>) (_c => _c.get_nearClipPlane()), (FrameCountType) 0, false), (Action<M0>) (_f => this.targetCamera.set_nearClipPlane(_f))), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<float>((IObservable<M0>) ObserveExtensions.ObserveEveryValueChanged<Camera, float>((M0) this.mainCamera, (Func<M0, M1>) (_c => _c.get_farClipPlane()), (FrameCountType) 0, false), (Action<M0>) (_f => this.targetCamera.set_farClipPlane(_f))), (Component) this);
    }
  }
}

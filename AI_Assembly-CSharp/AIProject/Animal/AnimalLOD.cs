// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalLOD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalLOD : MonoBehaviour
  {
    [SerializeField]
    private AnimalBase _animal;
    [SerializeField]
    [Min(0.0f)]
    private float _visibleDistance;
    private Camera _camera;
    private bool _prevVisible;
    private IDisposable _lateUpdateDisposable;

    public AnimalLOD()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this._animal, (Object) null))
        this._animal = (AnimalBase) ((Component) this).GetComponent<AnimalBase>();
      if (Object.op_Equality((Object) this._animal, (Object) null))
        this._animal = (AnimalBase) ((Component) this).GetComponentInChildren<AnimalBase>(true);
      if (Object.op_Equality((Object) this._animal, (Object) null))
        this._animal = (AnimalBase) ((Component) this).GetComponentInParent<AnimalBase>();
      if (Object.op_Equality((Object) this._animal, (Object) null))
        Object.Destroy((Object) this);
      else
        this.StartLateUpdate();
    }

    private void OnLateUpdate()
    {
      bool _enabled = (double) Vector3.Distance(((Component) this._camera).get_transform().get_position(), this._animal.Position) <= (double) this._visibleDistance;
      if (this._prevVisible != _enabled)
        this._animal.SetForcedBodyEnabled(_enabled);
      this._prevVisible = _enabled;
    }

    private void StartLateUpdate()
    {
      this.StopLateUpdate();
      this._camera = Manager.Map.GetCameraComponent();
      if (Object.op_Equality((Object) this._camera, (Object) null))
        this._camera = Camera.get_main();
      if (Object.op_Equality((Object) this._camera, (Object) null))
        Object.Destroy((Object) this);
      else
        this._lateUpdateDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Component) this), (Component) this._camera), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
    }

    private void StopLateUpdate()
    {
      if (this._lateUpdateDisposable == null)
        return;
      this._lateUpdateDisposable.Dispose();
      this._lateUpdateDisposable = (IDisposable) null;
    }

    private void OnDestroy()
    {
      this.StopLateUpdate();
    }
  }
}

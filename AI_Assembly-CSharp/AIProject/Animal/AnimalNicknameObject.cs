// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalNicknameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalNicknameObject : MonoBehaviour
  {
    [SerializeField]
    private AnimalBase _animal;
    private AnimalNicknameOutput _outputter;

    public AnimalNicknameObject()
    {
      base.\u002Ector();
    }

    public INicknameObject Obj { get; private set; }

    private void Awake()
    {
      if (this.Obj == null)
        this.Obj = (this._animal ?? (AnimalBase) ((Component) this).GetComponent<AnimalBase>()) as INicknameObject;
      if (this.Obj == null)
        return;
      this._outputter = !Singleton<MapUIContainer>.IsInstance() ? (AnimalNicknameOutput) null : MapUIContainer.NicknameUI;
      if (Object.op_Equality((Object) this._outputter, (Object) null))
      {
        Object.DestroyImmediate((Object) this);
      }
      else
      {
        this.Obj.NicknameEnabled = true;
        this._outputter.AddElement(this.Obj);
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnEnableAsObservable((Component) this), (Func<M0, bool>) (_ => this.Obj != null)), (Action<M0>) (_ => this.Obj.NicknameEnabled = true));
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDisableAsObservable((Component) this), (Func<M0, bool>) (_ => this.Obj != null)), (Action<M0>) (_ => this.Obj.NicknameEnabled = false));
        ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) this), (Func<M0, bool>) (_ => this.Obj != null)), (Action<M0>) (_ =>
        {
          if (!Object.op_Inequality((Object) this._outputter, (Object) null))
            return;
          this._outputter.RemoveElement(this.Obj);
        }));
      }
    }

    private void LateUpdate()
    {
    }
  }
}

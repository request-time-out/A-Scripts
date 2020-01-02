// Decompiled with JetBrains decompiler
// Type: Correct.Process.BaseProcess
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UniRx;
using UnityEngine;

namespace Correct.Process
{
  [RequireComponent(typeof (BaseData))]
  public abstract class BaseProcess : MonoBehaviour
  {
    protected BaseData _data;
    public BaseProcess.Type type;
    public bool noRestore;
    private ChaControl _chaCtrl;
    private Vector3 pos;
    private Quaternion rot;

    protected BaseProcess()
    {
      base.\u002Ector();
    }

    public BaseData data
    {
      get
      {
        if (Object.op_Equality((Object) this._data, (Object) null))
          this._data = (BaseData) ((Component) this).GetComponent<BaseData>();
        return this._data;
      }
    }

    public ChaControl chaCtrl
    {
      get
      {
        if (Object.op_Equality((Object) this._chaCtrl, (Object) null))
          this._chaCtrl = (ChaControl) ((Component) this).GetComponentInParent<ChaControl>();
        return this._chaCtrl;
      }
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryEndOfFrame(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this, (Object) null) && ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.Restore()));
    }

    protected virtual void LateUpdate()
    {
      if (Object.op_Equality((Object) this._data, (Object) null))
        return;
      Transform bone = this._data.bone;
      if (Object.op_Equality((Object) bone, (Object) null))
        return;
      switch (this.type)
      {
        case BaseProcess.Type.Target:
          this.pos = bone.get_localPosition();
          this.rot = bone.get_localRotation();
          bone.set_localPosition(Vector3.op_Addition(this.pos, this._data.pos));
          bone.set_localRotation(Quaternion.op_Multiply(this.rot, this._data.rot));
          break;
        case BaseProcess.Type.Sync:
          if (!Object.op_Inequality((Object) this.chaCtrl, (Object) null))
            break;
          ((Component) this).get_transform().set_position(Vector3.op_Addition(bone.get_position(), this.chaCtrl.objBodyBone.get_transform().TransformDirection(this._data.pos)));
          ((Component) this).get_transform().set_rotation(Quaternion.op_Multiply(bone.get_rotation(), this._data.rot));
          break;
      }
    }

    private void Restore()
    {
      if (this.type != BaseProcess.Type.Target || this.noRestore || Object.op_Equality((Object) this._data, (Object) null))
        return;
      Transform bone = this._data.bone;
      if (Object.op_Equality((Object) bone, (Object) null))
        return;
      bone.set_localPosition(this.pos);
      bone.set_localRotation(this.rot);
    }

    public enum Type
    {
      Target,
      Sync,
    }
  }
}

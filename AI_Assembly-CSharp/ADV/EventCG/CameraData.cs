// Decompiled with JetBrains decompiler
// Type: ADV.EventCG.CameraData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;

namespace ADV.EventCG
{
  public class CameraData : MonoBehaviour
  {
    [Header("カメラデータ")]
    [SerializeField]
    private float _fieldOfView;
    private float? baseFieldOfView;
    private Transform lookAt;
    [Header("身長補正座標")]
    [SerializeField]
    private Vector3 _minPos;
    [SerializeField]
    private Vector3 _maxPos;
    [Header("身長補正角度")]
    [SerializeField]
    private Vector3 _minAng;
    [SerializeField]
    private Vector3 _maxAng;
    private ReactiveCollection<ChaControl> _chaCtrlList;
    private Vector3 basePos;
    private Vector3 baseAng;

    public CameraData()
    {
      base.\u002Ector();
    }

    public bool initialized { get; private set; }

    public float fieldOfView
    {
      get
      {
        return this._fieldOfView;
      }
      set
      {
        this._fieldOfView = value;
      }
    }

    public void SetCameraData(Component component)
    {
      if (this.SetCameraData((Camera) component.GetComponent<Camera>()) || !this.SetCameraData((CinemachineVirtualCamera) component.GetComponent<CinemachineVirtualCamera>()))
        ;
    }

    public void RepairCameraData(Component component)
    {
      if (!this.baseFieldOfView.HasValue || this.RepairCameraData((Camera) component.GetComponent<Camera>()) || !this.RepairCameraData((CinemachineVirtualCamera) component.GetComponent<CinemachineVirtualCamera>()))
        ;
    }

    private bool SetCameraData(Camera cam)
    {
      if (Object.op_Equality((Object) cam, (Object) null))
        return false;
      this.baseFieldOfView = new float?(cam.get_fieldOfView());
      return true;
    }

    private bool SetCameraData(CinemachineVirtualCamera cam)
    {
      if (Object.op_Equality((Object) cam, (Object) null))
        return false;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      this.baseFieldOfView = new float?((float) (^(LensSettings&) ref cam.m_Lens).FieldOfView);
      this.lookAt = ((CinemachineVirtualCameraBase) cam).get_LookAt();
      return true;
    }

    private bool RepairCameraData(Camera cam)
    {
      if (Object.op_Equality((Object) cam, (Object) null) || !this.baseFieldOfView.HasValue)
        return false;
      cam.set_fieldOfView(this.baseFieldOfView.Value);
      return true;
    }

    private bool RepairCameraData(CinemachineVirtualCamera cam)
    {
      if (Object.op_Equality((Object) cam, (Object) null) || !this.baseFieldOfView.HasValue)
        return false;
      ((Behaviour) cam).set_enabled(true);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(LensSettings&) ref cam.m_Lens).FieldOfView = (__Null) (double) this.baseFieldOfView.Value;
      ((CinemachineVirtualCameraBase) cam).set_LookAt(this.lookAt);
      return true;
    }

    public ReactiveCollection<ChaControl> chaCtrlList
    {
      get
      {
        return this._chaCtrlList;
      }
    }

    private void Calculate()
    {
      if (!((IEnumerable<ChaControl>) this._chaCtrlList).Any<ChaControl>())
        return;
      float shape = ((IEnumerable<ChaControl>) this._chaCtrlList).Average<ChaControl>((Func<ChaControl, float>) (p => p.GetShapeBodyValue(0)));
      ((Component) this).get_transform().SetPositionAndRotation(Vector3.op_Addition(this.basePos, ((Component) this).get_transform().get_parent().TransformDirection(MathfEx.GetShapeLerpPositionValue(shape, this._minPos, this._maxPos))), Quaternion.Euler(Vector3.op_Addition(this.baseAng, MathfEx.GetShapeLerpAngleValue(shape, this._minAng, this._maxAng))));
    }

    private void OnEnable()
    {
      this.basePos = ((Component) this).get_transform().get_position();
      this.baseAng = ((Component) this).get_transform().get_eulerAngles();
    }

    private void OnDisable()
    {
      ((Component) this).get_transform().set_position(this.basePos);
      ((Component) this).get_transform().set_eulerAngles(this.baseAng);
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CameraData.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}

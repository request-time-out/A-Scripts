// Decompiled with JetBrains decompiler
// Type: BoneSwayCtrl.CSwayParamDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BoneSwayCtrl
{
  [Serializable]
  public class CSwayParamDetail
  {
    public Vector3 vLimitMaxT = Vector3.get_one();
    public Vector3 vLimitMinT = Vector3.get_one();
    public Vector3 vLimitMaxR = Vector3.get_one();
    public Vector3 vLimitMinR = Vector3.get_one();
    public Vector3 vAddR = Vector3.get_zero();
    public Vector3 vAddT = Vector3.get_zero();
    public Vector3 vAddS = Vector3.get_one();
    public float fForceScale = 1f;
    public float fForceLimit = 1f;
    public CSwayParamCalc Calc = new CSwayParamCalc();
    public float fInertiaScale;
    public float fGravity;
    public float fDrag;
    public float fTension;
    public float fShear;
    public float fAttenuation;
    public float fMass;
    public float fCrushZMax;
    public float fCrushZMin;
    public float fCrushXYMax;
    public float fCrushXYMin;
    public bool bAutoRotProc;
    public float fAutoRot;
    public bool bAutoRotUp;
  }
}

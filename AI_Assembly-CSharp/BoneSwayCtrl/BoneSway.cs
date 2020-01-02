// Decompiled with JetBrains decompiler
// Type: BoneSwayCtrl.BoneSway
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using DeepCopy;
using IllusionUtility.SetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoneSwayCtrl
{
  public class BoneSway
  {
    private List<CBoneData> m_listBone = new List<CBoneData>();
    private List<Vector3> m_listPos = new List<Vector3>();
    private List<Vector3> m_listRot = new List<Vector3>();
    private List<Vector3> m_listOldWorldPos = new List<Vector3>();
    private CSwayParam m_Param = new CSwayParam();
    private List<SpringCtrl> m_listSpringCtrl = new List<SpringCtrl>();
    private List<GameObject> m_listObjCalc = new List<GameObject>();
    private readonly int m_nObjCalcNum = 5;
    private bool m_bLR;
    private int m_nNumBone;

    public BoneSway()
    {
      for (int index = 0; index < this.m_nObjCalcNum; ++index)
      {
        this.m_listObjCalc.Add(new GameObject());
        ((Object) this.m_listObjCalc[index]).set_name("HSecneCalc" + index.ToString());
        ((Object) this.m_listObjCalc[index]).set_hideFlags((HideFlags) 1);
      }
    }

    ~BoneSway()
    {
      using (List<GameObject>.Enumerator enumerator = this.m_listObjCalc.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.Destroy((Object) enumerator.Current);
      }
      this.m_listObjCalc.Clear();
      this.Release();
    }

    public bool Init(List<List<Transform>> _llTrans, CSwayParam _Param, bool _bLR)
    {
      bool flag = true;
      this.m_bLR = _bLR;
      this.m_nNumBone = _llTrans.Count;
      for (int index1 = 0; index1 < this.m_nNumBone; ++index1)
      {
        this.m_listBone.Add(new CBoneData());
        this.m_listSpringCtrl.Add(new SpringCtrl());
        this.m_listPos.Add((Vector3) null);
        this.m_listRot.Add((Vector3) null);
        this.m_listOldWorldPos.Add((Vector3) null);
        if (Object.op_Inequality((Object) _llTrans[index1][0], (Object) null))
          flag &= this.setFrameInfo(this.m_listBone[index1].Bone, _llTrans[index1][0]);
        if (Object.op_Inequality((Object) _llTrans[index1][1], (Object) null))
          flag &= this.setFrameInfo(this.m_listBone[index1].Reference, _llTrans[index1][1]);
        this.m_listBone[index1].nNumLocater = _llTrans[index1].Count - 2;
        for (int index2 = 2; index2 < this.m_listBone[index1].nNumLocater + 2; ++index2)
        {
          this.m_listBone[index1].listLocater.Add(new CFrameInfo());
          if (!Object.op_Equality((Object) _llTrans[index1][index2], (Object) null))
            flag &= this.setFrameInfo(this.m_listBone[index1].listLocater[index2], _llTrans[index1][index2]);
        }
        _Param.listDetail.Add(new CSwayParamDetail());
        flag &= this.m_listSpringCtrl[index1].Init(_Param.listDetail[index1].fMass, _Param.listDetail[index1].fTension, _Param.listDetail[index1].fShear, _Param.listDetail[index1].fAttenuation, _Param.listDetail[index1].fGravity, _Param.listDetail[index1].fDrag, 2U, 1U);
        if (flag)
        {
          this.m_listPos[index1] = this.m_listSpringCtrl[index1].listPoint[0].vPos;
          this.m_listSpringCtrl[index1].listPoint[0].bLock = true;
          this.setParamAll(_Param);
        }
        else
          flag = false;
      }
      return flag;
    }

    public bool Release()
    {
      this.MemberInit();
      return true;
    }

    public bool CatchProc(Transform _transRef, float _fmx, float _fmy, int _nBoneIdx)
    {
      bool flag1 = true;
      FakeTransform _ftransBlend = new FakeTransform();
      bool flag2 = flag1 & this.CalcBlendMatrixT(_ftransBlend, this.m_listBone[_nBoneIdx], true, this.m_Param.listDetail[_nBoneIdx]);
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(_fmx, _fmy, 0.0f);
      Vector3 vector3_2 = _transRef.TransformPoint(vector3_1);
      this.m_listObjCalc[0].get_transform().Identity();
      this.m_listObjCalc[0].get_transform().set_position(_ftransBlend.Pos);
      this.m_listObjCalc[0].get_transform().set_rotation(_ftransBlend.Rot);
      this.m_listObjCalc[0].get_transform().set_localScale(_ftransBlend.Scale);
      Vector3 vector3_3 = this.m_listObjCalc[0].get_transform().InverseTransformVector(_transRef.get_position());
      Vector3 vector3_4 = Vector3.op_Subtraction(this.m_listObjCalc[0].get_transform().InverseTransformVector(vector3_2), vector3_3);
      SpringCtrl.CSpringPoint cspringPoint = this.m_listSpringCtrl[_nBoneIdx].listPoint[0];
      cspringPoint.vPos = Vector3.op_Addition(cspringPoint.vPos, vector3_4);
      return flag2 & this.MoveLimit(_nBoneIdx);
    }

    public bool AutoProc(float _ftime, bool _bCatch, CBoneBlend _Blend, int _nBoneIdx)
    {
      bool flag1 = true;
      FakeTransform _ftransBlend = new FakeTransform();
      bool flag2 = flag1 & this.CalcBlendMatrixT(_ftransBlend, this.m_listBone[_nBoneIdx], true, this.m_Param.listDetail[_nBoneIdx]);
      this.m_listObjCalc[0].get_transform().set_position(_ftransBlend.Pos);
      this.m_listObjCalc[0].get_transform().set_rotation(_ftransBlend.Rot);
      bool flag3 = flag2 & this.CalcForce(this.m_listObjCalc[0].get_transform(), _nBoneIdx);
      if (_bCatch && (int) this.getBoneCatch() == _nBoneIdx)
        this.m_listSpringCtrl[_nBoneIdx].ResetAllForce();
      else
        flag3 = flag3 & this.m_listSpringCtrl[_nBoneIdx].UpdateEulerMethod(this.m_listObjCalc[0].get_transform(), _ftime) & this.MoveLimit(_nBoneIdx);
      bool flag4 = flag3 & this.CalcRot(_nBoneIdx);
      BoneSway.ResultMatrixFunc[] resultMatrixFuncArray = new BoneSway.ResultMatrixFunc[3]
      {
        new BoneSway.ResultMatrixFunc(this.ResultMatrixProgram),
        new BoneSway.ResultMatrixFunc(this.ResultMatrixLocater),
        new BoneSway.ResultMatrixFunc(this.ResultMatrixBone)
      };
      if (_Blend.bBlend)
      {
        byte nCalcKind1 = _Blend.pNowParam.listDetail[_nBoneIdx].Calc.nCalcKind;
        byte nCalcKind2 = _Blend.pNextParam.listDetail[_nBoneIdx].Calc.nCalcKind;
        FakeTransform _ftransResult1 = new FakeTransform();
        FakeTransform _ftransResult2 = new FakeTransform();
        int num1 = resultMatrixFuncArray[(int) nCalcKind1](_ftransResult1, this.m_listBone[_nBoneIdx], _nBoneIdx) ? 1 : 0;
        int num2 = resultMatrixFuncArray[(int) nCalcKind2](_ftransResult2, this.m_listBone[_nBoneIdx], _nBoneIdx) ? 1 : 0;
        this.m_listBone[_nBoneIdx].transResult.Pos = Vector3.Lerp(_ftransResult1.Pos, _ftransResult2.Pos, _Blend.fLerp);
        this.m_listBone[_nBoneIdx].transResult.Rot = Quaternion.Lerp(_ftransResult1.Rot, _ftransResult2.Rot, _Blend.fLerp);
        this.m_listBone[_nBoneIdx].transResult.Scale = Vector3.Lerp(_ftransResult1.Scale, _ftransResult2.Scale, _Blend.fLerp);
      }
      else
      {
        FakeTransform fakeTransform = new FakeTransform();
        int num = resultMatrixFuncArray[(int) this.m_listBone[_nBoneIdx].nCalcKind](fakeTransform, this.m_listBone[_nBoneIdx], _nBoneIdx) ? 1 : 0;
        this.m_listBone[_nBoneIdx].transResult = (FakeTransform) fakeTransform.DeepCopy();
      }
      this.m_listSpringCtrl[_nBoneIdx].setForce = Vector3.get_zero();
      return true;
    }

    private bool CalcBlendMatrixT(
      FakeTransform _ftransBlend,
      CBoneData _Bone,
      bool _bWorld,
      CSwayParamDetail _Detail)
    {
      CFrameInfo cframeInfo1 = _Bone.listLocater[(int) _Bone.anLocaterTIdx[0]];
      CFrameInfo cframeInfo2 = _Bone.listLocater[(int) _Bone.anLocaterTIdx[1]];
      FakeTransform target = new FakeTransform();
      FakeTransform fakeTransform1 = new FakeTransform();
      FakeTransform fakeTransform2 = new FakeTransform();
      target.Pos = cframeInfo1.transFrame.get_localPosition();
      target.Rot = cframeInfo1.transFrame.get_localRotation();
      target.Scale = cframeInfo1.transFrame.get_localScale();
      fakeTransform1.Pos = cframeInfo2.transFrame.get_localPosition();
      fakeTransform1.Rot = cframeInfo2.transFrame.get_localRotation();
      fakeTransform1.Scale = cframeInfo2.transFrame.get_localScale();
      if (this.m_bLR)
        ((Vector3) ref fakeTransform2.Pos).Set((float) -_Detail.vAddT.x, (float) _Detail.vAddT.y, (float) _Detail.vAddT.z);
      else
        ((Vector3) ref fakeTransform2.Pos).Set((float) _Detail.vAddT.x, (float) _Detail.vAddT.y, (float) _Detail.vAddT.z);
      FakeTransform fakeTransform3 = target;
      fakeTransform3.Pos = Vector3.op_Addition(fakeTransform3.Pos, fakeTransform2.Pos);
      FakeTransform fakeTransform4 = fakeTransform1;
      fakeTransform4.Pos = Vector3.op_Addition(fakeTransform4.Pos, fakeTransform2.Pos);
      if (_bWorld)
      {
        target.Pos = cframeInfo1.transParent.TransformPoint(target.Pos);
        target.Rot = cframeInfo1.transFrame.get_rotation();
        fakeTransform1.Pos = cframeInfo2.transParent.TransformPoint(fakeTransform1.Pos);
        fakeTransform1.Rot = cframeInfo2.transFrame.get_rotation();
      }
      if ((int) _Bone.anLocaterTIdx[0] == (int) _Bone.anLocaterTIdx[1])
      {
        _ftransBlend = (FakeTransform) target.DeepCopy();
        return true;
      }
      _ftransBlend.Pos = Vector3.Lerp(target.Pos, fakeTransform1.Pos, _Bone.fLerp);
      _ftransBlend.Rot = Quaternion.Slerp(target.Rot, fakeTransform1.Rot, _Bone.fLerp);
      _ftransBlend.Scale = Vector3.Lerp(target.Scale, fakeTransform1.Scale, _Bone.fLerp);
      return true;
    }

    private bool CalcBlendMatrixR(
      FakeTransform _ftransBlend,
      CBoneData _Bone,
      bool _bWorld,
      CSwayParamDetail _Detail,
      bool _bAddRot,
      bool _bRot)
    {
      CFrameInfo cframeInfo1 = _Bone.listLocater[(int) _Bone.anLocaterRIdx[0]];
      CFrameInfo cframeInfo2 = _Bone.listLocater[(int) _Bone.anLocaterRIdx[1]];
      CFrameInfo reference = _Bone.Reference;
      FakeTransform fakeTransform1 = new FakeTransform();
      FakeTransform fakeTransform2 = new FakeTransform();
      FakeTransform fakeTransform3 = new FakeTransform();
      FakeTransform fakeTransform4 = new FakeTransform();
      fakeTransform1.Pos = cframeInfo1.transFrame.get_localPosition();
      fakeTransform1.Rot = cframeInfo1.transFrame.get_localRotation();
      fakeTransform1.Scale = cframeInfo1.transFrame.get_localScale();
      fakeTransform2.Pos = cframeInfo2.transFrame.get_localPosition();
      fakeTransform2.Rot = cframeInfo2.transFrame.get_localRotation();
      fakeTransform2.Scale = cframeInfo2.transFrame.get_localScale();
      if (this.m_bLR)
        ((Vector3) ref fakeTransform3.Pos).Set((float) -_Detail.vAddT.x, (float) _Detail.vAddT.y, (float) _Detail.vAddT.z);
      else
        ((Vector3) ref fakeTransform3.Pos).Set((float) _Detail.vAddT.x, (float) _Detail.vAddT.y, (float) _Detail.vAddT.z);
      FakeTransform fakeTransform5 = fakeTransform1;
      fakeTransform5.Pos = Vector3.op_Addition(fakeTransform5.Pos, fakeTransform3.Pos);
      FakeTransform fakeTransform6 = fakeTransform2;
      fakeTransform6.Pos = Vector3.op_Addition(fakeTransform6.Pos, fakeTransform3.Pos);
      if (_bWorld)
      {
        fakeTransform1.Pos = cframeInfo1.transParent.TransformPoint(fakeTransform1.Pos);
        fakeTransform1.Rot = cframeInfo1.transFrame.get_rotation();
        fakeTransform2.Pos = cframeInfo2.transParent.TransformPoint(fakeTransform2.Pos);
        fakeTransform2.Rot = cframeInfo2.transFrame.get_rotation();
      }
      if ((int) _Bone.anLocaterRIdx[0] == (int) _Bone.anLocaterRIdx[1])
      {
        fakeTransform4.Rot = _bAddRot ? (!this.m_bLR ? Quaternion.Euler((float) _Detail.vAddR.x, (float) _Detail.vAddR.y, (float) _Detail.vAddR.z) : Quaternion.Euler((float) _Detail.vAddR.x, (float) -_Detail.vAddR.y, (float) _Detail.vAddR.z)) : Quaternion.get_identity();
        FakeTransform fakeTransform7 = fakeTransform4;
        fakeTransform7.Rot = Quaternion.op_Multiply(fakeTransform7.Rot, fakeTransform1.Rot);
        fakeTransform4.Pos = fakeTransform1.Pos;
        fakeTransform4.Scale = fakeTransform1.Scale;
        if (!_bRot || Object.op_Equality((Object) reference.transFrame, (Object) null))
          _ftransBlend = (FakeTransform) fakeTransform4.DeepCopy();
        else
          this.CalcAutoRotation(_ftransBlend, fakeTransform4, reference.transFrame, _Detail);
        return true;
      }
      _ftransBlend.Pos = Vector3.Lerp(fakeTransform1.Pos, fakeTransform2.Pos, _Bone.fLerp);
      _ftransBlend.Rot = Quaternion.Slerp(fakeTransform1.Rot, fakeTransform2.Rot, _Bone.fLerp);
      _ftransBlend.Scale = Vector3.Lerp(fakeTransform1.Scale, fakeTransform2.Scale, _Bone.fLerp);
      fakeTransform4.Rot = _bAddRot ? (!this.m_bLR ? Quaternion.Euler((float) _Detail.vAddR.x, (float) _Detail.vAddR.y, (float) _Detail.vAddR.z) : Quaternion.Euler((float) _Detail.vAddR.x, (float) -_Detail.vAddR.y, (float) _Detail.vAddR.z)) : Quaternion.get_identity();
      FakeTransform fakeTransform8 = _ftransBlend;
      fakeTransform8.Rot = Quaternion.op_Multiply(fakeTransform8.Rot, fakeTransform4.Rot);
      if (_bRot && Object.op_Inequality((Object) reference.transFrame, (Object) null))
        this.CalcAutoRotation(_ftransBlend, _ftransBlend, reference.transFrame, _Detail);
      return true;
    }

    private bool CalcAutoRotation(
      FakeTransform _ftransBlend,
      FakeTransform _ftransBase,
      Transform _transRef,
      CSwayParamDetail _Detail)
    {
      Quaternion.get_identity();
      float num1 = 1f;
      float num2 = 1f;
      double num3;
      float num4 = (float) (num3 = 1.0);
      float num5 = 0.0f;
      num4 = 0.0f;
      float num6 = Vector3.Dot(Vector3.get_up(), _transRef.get_up());
      float num7 = Vector3.Dot(Vector3.get_up(), _transRef.get_forward());
      float num8 = Vector3.Dot(Vector3.get_up(), _transRef.get_right());
      float num9 = Mathf.Abs(num6);
      if ((double) num7 > 0.0)
        num1 = -1f;
      float num10 = Mathf.Abs(num7);
      if ((double) num8 > 0.0)
        num2 = -1f;
      float num11 = Mathf.Abs(num8);
      float num12 = Mathf.InverseLerp(1f, 0.0f, num9) * Mathf.InverseLerp(0.0f, 1f, num11);
      float fAutoRot1 = _Detail.fAutoRot;
      if ((double) num2 < 0.0)
        fAutoRot1 *= -1f;
      float num13 = Mathf.Lerp(0.0f, fAutoRot1, num12);
      if (!_Detail.bAutoRotUp || (double) num1 > 0.0)
      {
        float num14 = Mathf.InverseLerp(1f, 0.0f, num9) * Mathf.InverseLerp(0.0f, 1f, num10);
        float fAutoRot2 = _Detail.fAutoRot;
        if (!((double) num1 <= 0.0 ? !this.m_bLR : this.m_bLR))
          fAutoRot2 *= -1f;
        num5 = Mathf.Lerp(0.0f, fAutoRot2, num14);
      }
      Quaternion quaternion = Quaternion.AngleAxis(MathfEx.ToDegree(num5 + num13), Vector3.get_up());
      _ftransBlend.Rot = Quaternion.op_Multiply(_ftransBase.Rot, quaternion);
      _ftransBlend.Pos = _ftransBase.Pos;
      _ftransBlend.Scale = _ftransBase.Scale;
      return true;
    }

    private bool CalcForce(Transform _transFrame, int _nIdx)
    {
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      Vector3 vector3_1 = Vector3.op_Subtraction(this.m_listOldWorldPos[_nIdx], _transFrame.get_position());
      this.m_listOldWorldPos[_nIdx] = _transFrame.get_position();
      if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() > (double) Mathf.Pow(cswayParamDetail.fForceLimit, 2f))
        vector3_1 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), cswayParamDetail.fForceLimit);
      Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, cswayParamDetail.fForceScale);
      this.m_listSpringCtrl[_nIdx].setAutoForce = _transFrame.InverseTransformDirection(vector3_2);
      return true;
    }

    private bool ResultMatrixProgram(FakeTransform _ftransResult, CBoneData _Bone, int _nIdx)
    {
      BoneSway.ResultTransformFunc[] resultTransformFuncArray = new BoneSway.ResultTransformFunc[3]
      {
        new BoneSway.ResultTransformFunc(this.TransformRotateAndTrans),
        new BoneSway.ResultTransformFunc(this.TransformRotate),
        new BoneSway.ResultTransformFunc(this.TransformTrans)
      };
      bool flag = true;
      bool bAutoRotProc = this.m_Param.listDetail[_nIdx].bAutoRotProc;
      FakeTransform fakeTransform1 = new FakeTransform();
      FakeTransform fakeTransform2 = new FakeTransform();
      return flag & this.CalcBlendMatrixR(fakeTransform1, _Bone, false, this.m_Param.listDetail[_nIdx], true, bAutoRotProc) & this.CalcBlendMatrixT(fakeTransform2, _Bone, false, this.m_Param.listDetail[_nIdx]) & resultTransformFuncArray[(int) _Bone.nTransformKind](_ftransResult, _Bone, _nIdx, fakeTransform2, fakeTransform1);
    }

    private bool ResultMatrixLocater(FakeTransform _ftransResult, CBoneData _Bone, int _nIdx)
    {
      bool flag1 = true;
      bool bAutoRotProc = this.m_Param.listDetail[_nIdx].bAutoRotProc;
      FakeTransform _ftransBlend1 = new FakeTransform();
      FakeTransform _ftransBlend2 = new FakeTransform();
      bool flag2 = flag1 & this.CalcBlendMatrixR(_ftransBlend1, _Bone, false, this.m_Param.listDetail[_nIdx], true, bAutoRotProc) & this.CalcBlendMatrixT(_ftransBlend2, _Bone, false, this.m_Param.listDetail[_nIdx]);
      _ftransResult.Pos = _ftransBlend2.Pos;
      _ftransResult.Rot = _ftransBlend1.Rot;
      _ftransResult.Scale = _ftransBlend1.Scale;
      return true;
    }

    private bool ResultMatrixBone(FakeTransform _ftransResult, CBoneData _Bone, int _nIdx)
    {
      _ftransResult.Pos = _Bone.Bone.transFrame.get_localPosition();
      _ftransResult.Rot = _Bone.Bone.transFrame.get_localRotation();
      _ftransResult.Scale = _Bone.Bone.transFrame.get_localScale();
      return true;
    }

    private bool TransformRotateAndTrans(
      FakeTransform _ftransResult,
      CBoneData _Bone,
      int _nIdx,
      FakeTransform _ftransBaseT,
      FakeTransform _ftransBaseR)
    {
      FakeTransform fakeTransform1 = new FakeTransform();
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      Quaternion quaternion1 = Quaternion.AngleAxis((float) this.m_listRot[_nIdx].x * _Bone.fScaleR, Vector3.get_right());
      Quaternion quaternion2 = Quaternion.AngleAxis((float) this.m_listRot[_nIdx].y * _Bone.fScaleR, Vector3.get_up());
      fakeTransform1.Rot = Quaternion.op_Multiply(quaternion1, quaternion2);
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(_Bone.fScaleT, _Bone.fScaleT, _Bone.fScaleT);
      if (this.m_listSpringCtrl[_nIdx].listPoint[0].vPos.y >= 0.0)
        vector3.y = (__Null) (double) _Bone.fScaleYT;
      fakeTransform1.Pos = Vector3.Scale(this.m_listSpringCtrl[_nIdx].listPoint[0].vPos, vector3);
      FakeTransform fakeTransform2 = fakeTransform1;
      fakeTransform2.Pos = Vector3.op_Addition(fakeTransform2.Pos, _ftransBaseT.Pos);
      float num1;
      float num2;
      float num3;
      if (fakeTransform1.Pos.z > 0.0)
      {
        float num4 = Mathf.InverseLerp(0.0f, (float) cswayParamDetail.vLimitMaxT.z, (float) fakeTransform1.Pos.z);
        num1 = Mathf.Lerp(1f, cswayParamDetail.fCrushZMax, num4) * (float) cswayParamDetail.vAddS.z;
        float num5 = Mathf.Lerp(1f, cswayParamDetail.fCrushXYMin, num4);
        num2 = num5 * (float) cswayParamDetail.vAddS.y;
        num3 = num5 * (float) cswayParamDetail.vAddS.x;
      }
      else
      {
        float num4 = Mathf.InverseLerp(0.0f, (float) cswayParamDetail.vLimitMinT.z, (float) fakeTransform1.Pos.z);
        num1 = Mathf.Lerp(1f, cswayParamDetail.fCrushZMin, num4) * (float) cswayParamDetail.vAddS.z;
        float num5 = Mathf.Lerp(1f, cswayParamDetail.fCrushXYMax, num4);
        num2 = num5 * (float) cswayParamDetail.vAddS.y;
        num3 = num5 * (float) cswayParamDetail.vAddS.x;
      }
      _ftransResult.Rot = Quaternion.op_Multiply(fakeTransform1.Rot, _ftransBaseR.Rot);
      _ftransResult.Pos = fakeTransform1.Pos;
      _ftransResult.Scale = Vector3.Scale(new Vector3(num3, num2, num1), _ftransBaseR.Scale);
      return true;
    }

    private bool TransformRotate(
      FakeTransform _ftransResult,
      CBoneData _Bone,
      int _nIdx,
      FakeTransform _ftransBaseT,
      FakeTransform _ftransBaseR)
    {
      FakeTransform fakeTransform = new FakeTransform();
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      Quaternion quaternion1 = Quaternion.AngleAxis((float) this.m_listRot[_nIdx].x * _Bone.fScaleR, Vector3.get_right());
      Quaternion quaternion2 = Quaternion.AngleAxis((float) this.m_listRot[_nIdx].y * _Bone.fScaleR, Vector3.get_up());
      fakeTransform.Rot = Quaternion.op_Multiply(quaternion1, quaternion2);
      _ftransResult.Rot = Quaternion.op_Multiply(fakeTransform.Rot, _ftransBaseR.Rot);
      _ftransResult.Pos = _ftransBaseR.Pos;
      _ftransResult.Scale = Vector3.Scale(cswayParamDetail.vAddS, _ftransBaseR.Scale);
      return true;
    }

    private bool TransformTrans(
      FakeTransform _ftransResult,
      CBoneData _Bone,
      int _nIdx,
      FakeTransform _ftransBaseT,
      FakeTransform _ftransBaseR)
    {
      FakeTransform fakeTransform1 = new FakeTransform();
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(_Bone.fScaleT, _Bone.fScaleT, _Bone.fScaleT);
      if (this.m_listSpringCtrl[_nIdx].listPoint[0].vPos.y >= 0.0)
        vector3.y = (__Null) (double) _Bone.fScaleYT;
      fakeTransform1.Pos = Vector3.Scale(this.m_listSpringCtrl[_nIdx].listPoint[0].vPos, vector3);
      FakeTransform fakeTransform2 = fakeTransform1;
      fakeTransform2.Pos = Vector3.op_Addition(fakeTransform2.Pos, _ftransBaseT.Pos);
      float num1;
      float num2;
      float num3;
      if (fakeTransform1.Pos.z > 0.0)
      {
        float num4 = Mathf.InverseLerp(0.0f, (float) cswayParamDetail.vLimitMaxT.z, (float) fakeTransform1.Pos.z);
        num1 = Mathf.Lerp(1f, cswayParamDetail.fCrushZMax, num4) * (float) cswayParamDetail.vAddS.z;
        float num5 = Mathf.Lerp(1f, cswayParamDetail.fCrushXYMin, num4);
        num2 = num5 * (float) cswayParamDetail.vAddS.y;
        num3 = num5 * (float) cswayParamDetail.vAddS.x;
      }
      else
      {
        float num4 = Mathf.InverseLerp(0.0f, (float) cswayParamDetail.vLimitMinT.z, (float) fakeTransform1.Pos.z);
        num1 = Mathf.Lerp(1f, cswayParamDetail.fCrushZMin, num4) * (float) cswayParamDetail.vAddS.z;
        float num5 = Mathf.Lerp(1f, cswayParamDetail.fCrushXYMax, num4);
        num2 = num5 * (float) cswayParamDetail.vAddS.y;
        num3 = num5 * (float) cswayParamDetail.vAddS.x;
      }
      _ftransResult.Rot = _ftransBaseT.Rot;
      _ftransResult.Pos = fakeTransform1.Pos;
      _ftransResult.Scale = Vector3.Scale(new Vector3(num3, num2, num1), _ftransBaseT.Scale);
      return true;
    }

    private bool MoveLimit(int _nIdx)
    {
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      SpringCtrl.CSpringPoint cspringPoint = this.m_listSpringCtrl[_nIdx].listPoint[0];
      cspringPoint.vPos.x = (__Null) (double) Mathf.Clamp((float) cspringPoint.vPos.x, (float) cswayParamDetail.vLimitMinT.x, (float) cswayParamDetail.vLimitMaxT.x);
      cspringPoint.vPos.y = (__Null) (double) Mathf.Clamp((float) cspringPoint.vPos.y, (float) cswayParamDetail.vLimitMinT.y, (float) cswayParamDetail.vLimitMaxT.y);
      cspringPoint.vPos.z = (__Null) (double) Mathf.Clamp((float) cspringPoint.vPos.z, (float) cswayParamDetail.vLimitMinT.z, (float) cswayParamDetail.vLimitMaxT.z);
      return true;
    }

    private bool CalcRot(int _nIdx)
    {
      CSwayParamDetail cswayParamDetail = this.m_Param.listDetail[_nIdx];
      SpringCtrl.CSpringPoint cspringPoint = this.m_listSpringCtrl[_nIdx].listPoint[0];
      this.m_listRot[_nIdx] = Vector3.get_zero();
      int index1 = cspringPoint.vPos.y < 0.0 ? 1 : 0;
      int index2 = cspringPoint.vPos.x < 0.0 ? 1 : 0;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float[] numArray1 = new float[2]
      {
        (float) cswayParamDetail.vLimitMaxT.y,
        (float) cswayParamDetail.vLimitMinT.y
      };
      float[] numArray2 = new float[2]
      {
        (float) cswayParamDetail.vLimitMaxT.x,
        (float) cswayParamDetail.vLimitMinT.x
      };
      float[] numArray3 = new float[2]
      {
        (float) cswayParamDetail.vLimitMaxR.y,
        (float) cswayParamDetail.vLimitMinR.y
      };
      float[] numArray4 = new float[2]
      {
        (float) cswayParamDetail.vLimitMaxR.x,
        (float) cswayParamDetail.vLimitMinR.x
      };
      if ((double) numArray1[index1] == 1.0)
        num1 = (float) -cspringPoint.vPos.y / numArray1[index1];
      if ((double) numArray2[index2] == 1.0)
        num2 = (float) cspringPoint.vPos.x / numArray2[index2];
      Vector3 vector3 = this.m_listRot[_nIdx];
      ((Vector3) ref vector3).Set(numArray4[index1] * num1, numArray3[index2] * num2, 0.0f);
      return true;
    }

    private void MemberInit()
    {
      this.m_bLR = false;
      this.m_nNumBone = 0;
      this.m_listBone.Clear();
      this.m_listPos.Clear();
      this.m_listRot.Clear();
      this.m_listOldWorldPos.Clear();
      this.m_listSpringCtrl.Clear();
      using (List<GameObject>.Enumerator enumerator = this.m_listObjCalc.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.Destroy((Object) enumerator.Current);
      }
      this.m_listObjCalc.Clear();
    }

    public bool setFrameInfo(CFrameInfo _Info, Transform _transFrame)
    {
      if (Object.op_Equality((Object) null, (Object) _transFrame))
        return false;
      _Info.transFrame = _transFrame;
      _Info.transParent = _transFrame.get_parent();
      return true;
    }

    public void setParamAll(CSwayParam _Param)
    {
      this.m_Param = (CSwayParam) _Param.DeepCopy();
      this.m_Param.bCalc = _Param.bCalc;
      this.m_Param.bEntry = _Param.bEntry;
      this.m_Param.fBlendTime = _Param.fBlendTime;
      this.m_Param.fMoveRate = _Param.fMoveRate;
      this.m_Param.nCatch = _Param.nCatch;
      this.m_Param.nPtn = _Param.nPtn;
      this.m_Param.strName = _Param.strName;
      this.m_Param.listDetail.Clear();
      for (int index = 0; index < _Param.listDetail.Count; ++index)
      {
        this.m_Param.listDetail.Add(new CSwayParamDetail());
        this.m_Param.listDetail[index].bAutoRotProc = _Param.listDetail[index].bAutoRotProc;
        this.m_Param.listDetail[index].bAutoRotUp = _Param.listDetail[index].bAutoRotUp;
        this.m_Param.listDetail[index].fAttenuation = _Param.listDetail[index].fAttenuation;
        this.m_Param.listDetail[index].fAutoRot = _Param.listDetail[index].fAutoRot;
        this.m_Param.listDetail[index].fCrushXYMax = _Param.listDetail[index].fCrushXYMax;
        this.m_Param.listDetail[index].fCrushXYMin = _Param.listDetail[index].fCrushXYMin;
        this.m_Param.listDetail[index].fCrushZMax = _Param.listDetail[index].fCrushZMax;
        this.m_Param.listDetail[index].fCrushZMin = _Param.listDetail[index].fCrushZMin;
        this.m_Param.listDetail[index].fDrag = _Param.listDetail[index].fDrag;
        this.m_Param.listDetail[index].fForceLimit = _Param.listDetail[index].fForceLimit;
        this.m_Param.listDetail[index].fForceScale = _Param.listDetail[index].fForceScale;
        this.m_Param.listDetail[index].fGravity = _Param.listDetail[index].fGravity;
        this.m_Param.listDetail[index].fInertiaScale = _Param.listDetail[index].fInertiaScale;
        this.m_Param.listDetail[index].fMass = _Param.listDetail[index].fMass;
        this.m_Param.listDetail[index].fShear = _Param.listDetail[index].fShear;
        this.m_Param.listDetail[index].fTension = _Param.listDetail[index].fTension;
        this.m_Param.listDetail[index].vAddR = _Param.listDetail[index].vAddR;
        this.m_Param.listDetail[index].vAddS = _Param.listDetail[index].vAddS;
        this.m_Param.listDetail[index].vAddT = _Param.listDetail[index].vAddT;
        this.m_Param.listDetail[index].vLimitMaxR = _Param.listDetail[index].vLimitMaxR;
        this.m_Param.listDetail[index].vLimitMaxT = _Param.listDetail[index].vLimitMaxT;
        this.m_Param.listDetail[index].vLimitMinR = _Param.listDetail[index].vLimitMinR;
        this.m_Param.listDetail[index].vLimitMinT = _Param.listDetail[index].vLimitMinT;
        this.m_Param.listDetail[index].Calc.fScaleR = _Param.listDetail[index].Calc.fScaleR;
        this.m_Param.listDetail[index].Calc.fScaleT = _Param.listDetail[index].Calc.fScaleT;
        this.m_Param.listDetail[index].Calc.fScaleYT = _Param.listDetail[index].Calc.fScaleYT;
        this.m_Param.listDetail[index].Calc.nCalcKind = _Param.listDetail[index].Calc.nCalcKind;
        this.m_Param.listDetail[index].Calc.nLocaterRIdx = _Param.listDetail[index].Calc.nLocaterRIdx;
        this.m_Param.listDetail[index].Calc.nLocaterTIdx = _Param.listDetail[index].Calc.nLocaterTIdx;
        this.m_Param.listDetail[index].Calc.nTransformKind = _Param.listDetail[index].Calc.nTransformKind;
      }
      this.setParamMoveRate(_Param.fMoveRate);
      this.setBoneCalc(_Param.bCalc);
      this.setBoneCatch(_Param.nCatch);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType5<CSwayParamDetail, int> anonType5 in _Param.listDetail.Select<CSwayParamDetail, \u003C\u003E__AnonType5<CSwayParamDetail, int>>((Func<CSwayParamDetail, int, \u003C\u003E__AnonType5<CSwayParamDetail, int>>) ((v, i) => new \u003C\u003E__AnonType5<CSwayParamDetail, int>(v, i))))
      {
        this.setParamLimitMaxT(anonType5.v.vLimitMaxT, anonType5.i);
        this.setParamLimitMinT(anonType5.v.vLimitMinT, anonType5.i);
        this.setParamLimitMaxR(anonType5.v.vLimitMaxR, anonType5.i);
        this.setParamLimitMinR(anonType5.v.vLimitMinR, anonType5.i);
        this.setParamAddR(anonType5.v.vAddR, anonType5.i);
        this.setParamAddT(anonType5.v.vAddT, anonType5.i);
        this.setParamAddS(anonType5.v.vAddS, anonType5.i);
        this.setParamForceScale(anonType5.v.fForceScale, anonType5.i);
        this.setParamForceLimit(anonType5.v.fForceLimit, anonType5.i);
        this.setParamCalcKind(anonType5.v.Calc.nCalcKind, anonType5.i);
        this.setParamTransformKind(anonType5.v.Calc.nTransformKind, anonType5.i);
        this.setParamScaleT(anonType5.v.Calc.fScaleT, anonType5.i);
        this.setParamScaleYT(anonType5.v.Calc.fScaleYT, anonType5.i);
        this.setParamScaleR(anonType5.v.Calc.fScaleR, anonType5.i);
        this.setCrushZScale(anonType5.v.fCrushZMax, anonType5.v.fCrushZMin, anonType5.i);
        this.setCrushXYScale(anonType5.v.fCrushXYMax, anonType5.v.fCrushXYMin, anonType5.i);
        this.setAutoRotProc(anonType5.v.bAutoRotProc, anonType5.i);
        this.setAutoRot(anonType5.v.fAutoRot, anonType5.i);
        this.setAutoRotUp(anonType5.v.bAutoRotUp, anonType5.i);
        this.m_listSpringCtrl[anonType5.i].SetParam(anonType5.v.fMass, anonType5.v.fTension, anonType5.v.fShear, anonType5.v.fAttenuation);
      }
    }

    public void getParamAll(ref CSwayParam _Param)
    {
      _Param = (CSwayParam) this.m_Param.DeepCopy();
    }

    public void setParamPtn(int _nPtn)
    {
      this.m_Param.nPtn = _nPtn;
    }

    public int getParamPtn()
    {
      return this.m_Param.nPtn;
    }

    public void setParamName(string _strName)
    {
      this.m_Param.strName = _strName;
    }

    public string getParamName()
    {
      return this.m_Param.strName;
    }

    public void setParamBlendTime(float _fBlendTime)
    {
      this.m_Param.fBlendTime = _fBlendTime;
    }

    public float getParamBlendTime()
    {
      return this.m_Param.fBlendTime;
    }

    public void setParamMoveRate(float _fMoveRate)
    {
      this.m_Param.fMoveRate = _fMoveRate;
    }

    public float getParamMoveRate()
    {
      return this.m_Param.fMoveRate;
    }

    public void setParamLimitMaxT(Vector3 _vLimitT, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vLimitMaxT = _vLimitT;
    }

    public void setParamLimitMinT(Vector3 _vLimitT, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vLimitMinT = _vLimitT;
    }

    public Vector3 getParamLimitMaxT(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vLimitMaxT;
    }

    public Vector3 getParamLimitMinT(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vLimitMinT;
    }

    public void setParamLimitMaxR(Vector3 _vLimitR, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vLimitMaxR = _vLimitR;
    }

    public void setParamLimitMinR(Vector3 _vLimitR, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vLimitMinR = _vLimitR;
    }

    public Vector3 getParamLimitMaxR(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vLimitMaxR;
    }

    public Vector3 getParamLimitMinR(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vLimitMinR;
    }

    public void setParamForceScale(float _fForceScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fForceScale = _fForceScale;
    }

    public float getParamForceScale(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fForceScale;
    }

    public void setParamForceLimit(float _fForceLimit, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fForceLimit = _fForceLimit;
    }

    public float getParamForceLimit(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fForceLimit;
    }

    public void setParamInertiaScale(float _fInertiaScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fInertiaScale = _fInertiaScale;
    }

    public float getParamInertiaScale(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fInertiaScale;
    }

    public void setParamCalcKind(byte _nCalcKind, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.nCalcKind = _nCalcKind;
      this.m_listBone[_nIdx].nCalcKind = _nCalcKind;
    }

    public byte getParamCalcKind(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.nCalcKind;
    }

    public void setParamLocaterTIdx(byte _nLocaterIdx, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.nLocaterTIdx = _nLocaterIdx;
    }

    public byte getParamLocaterTIdx(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.nLocaterTIdx;
    }

    public void setParamLocaterRIdx(byte _nLocaterIdx, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.nLocaterRIdx = _nLocaterIdx;
    }

    public byte getParamLocaterRIdx(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.nLocaterRIdx;
    }

    public void setParamTransformKind(byte _nTransformKind, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.nTransformKind = _nTransformKind;
      this.m_listBone[_nIdx].nTransformKind = _nTransformKind;
    }

    public byte getParamTransformKind(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.nTransformKind;
    }

    public void setParamScaleT(float _fScaleT, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.fScaleT = _fScaleT;
      this.m_listBone[_nIdx].fScaleT = _fScaleT;
    }

    public float getParamScaleT(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.fScaleT;
    }

    public void setParamScaleYT(float _fScaleYT, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.fScaleYT = _fScaleYT;
      this.m_listBone[_nIdx].fScaleYT = _fScaleYT;
    }

    public float getParamScaleYT(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.fScaleYT;
    }

    public void setParamScaleR(float _fScaleR, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].Calc.fScaleR = _fScaleR;
      this.m_listBone[_nIdx].fScaleR = _fScaleR;
    }

    public float getParamScaleR(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].Calc.fScaleR;
    }

    public void setParamGravity(float _fGravity, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fGravity = _fGravity;
      this.m_listSpringCtrl[_nIdx].setGravity = _fGravity;
    }

    public float getParamGravity(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fGravity;
    }

    public void setParamDrag(float _fDrag, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fDrag = _fDrag;
      this.m_listSpringCtrl[_nIdx].setDrag = _fDrag;
    }

    public float getParamDrag(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fDrag;
    }

    public void setParamTension(float _fTension, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fTension = _fTension;
      this.m_listSpringCtrl[_nIdx].setTension = _fTension;
    }

    public float getParamTension(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fTension;
    }

    public void setParamShear(float _fShear, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fShear = _fShear;
      this.m_listSpringCtrl[_nIdx].setShear = _fShear;
    }

    public float getParamShear(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fShear;
    }

    public void setParamAttenuation(float _fAttenuation, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fAttenuation = _fAttenuation;
      this.m_listSpringCtrl[_nIdx].setAttenuation = _fAttenuation;
    }

    public float getParamAttenuation(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fAttenuation;
    }

    public void setParamMass(float _fMass, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fMass = _fMass;
      this.m_listSpringCtrl[_nIdx].setMass = _fMass;
    }

    public float getParamMass(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fMass;
    }

    public void setBoneCalc(bool _bCalc)
    {
      this.m_Param.bCalc = _bCalc;
    }

    public bool getBoneCalc()
    {
      return this.m_Param.bCalc;
    }

    public void setCrushZScale(float _fScaleMax, float _fScaleMin, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushZMax = _fScaleMax;
      this.m_Param.listDetail[_nIdx].fCrushZMin = _fScaleMin;
    }

    public void setCrushZScaleMax(float _fScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushZMax = _fScale;
    }

    public void setCrushZScaleMin(float _fScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushZMin = _fScale;
    }

    public float getCrushZScaleMax(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fCrushZMax;
    }

    public float getCrushZScaleMin(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fCrushZMin;
    }

    public void setCrushXYScale(float _fScaleMax, float _fScaleMin, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushXYMax = _fScaleMax;
      this.m_Param.listDetail[_nIdx].fCrushXYMin = _fScaleMin;
    }

    public void setCrushXYScaleMax(float _fScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushXYMax = _fScale;
    }

    public void setCrushXYScaleMin(float _fScale, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fCrushXYMin = _fScale;
    }

    public float getCrushXYScaleMax(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fCrushXYMax;
    }

    public float getCrushXYScaleMin(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fCrushXYMin;
    }

    public void setAutoRotProc(bool _bAuto, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].bAutoRotProc = _bAuto;
    }

    public bool getAutoRotProc(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].bAutoRotProc;
    }

    public void setAutoRot(float _fRot, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].fAutoRot = _fRot;
    }

    public float getAutoRot(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].fAutoRot;
    }

    public void setAutoRotUp(bool _bUp, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].bAutoRotUp = _bUp;
    }

    public bool getAutoRotUp(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].bAutoRotUp;
    }

    public void setBoneCatch(byte _nCatch)
    {
      this.m_Param.nCatch = _nCatch;
    }

    public byte getBoneCatch()
    {
      return this.m_Param.nCatch;
    }

    public void initLocaterHist(int _nIdx)
    {
      if (Object.op_Equality((Object) this.m_listBone[_nIdx].listLocater[0].transFrame, (Object) null))
        return;
      this.m_listOldWorldPos[_nIdx] = this.m_listBone[_nIdx].listLocater[0].transFrame.get_position();
    }

    public void initLocalPos(int _nIdx)
    {
      this.m_listPos[_nIdx] = Vector3.get_zero();
    }

    public void initForce(int _nIdx)
    {
      this.m_listSpringCtrl[_nIdx].InitForce();
    }

    public void setParamAddR(Vector3 _vAddR, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vAddR = _vAddR;
    }

    public Vector3 getParamAddR(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vAddR;
    }

    public void setParamAddT(Vector3 _vAddT, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vAddT = _vAddT;
    }

    public Vector3 getParamAddT(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vAddT;
    }

    public void setParamAddS(Vector3 _vAddS, int _nIdx)
    {
      this.m_Param.listDetail[_nIdx].vAddS = _vAddS;
    }

    public Vector3 getParamAddS(int _nIdx)
    {
      return this.m_Param.listDetail[_nIdx].vAddS;
    }

    private delegate bool ResultMatrixFunc(
      FakeTransform _ftransResult,
      CBoneData _Bone,
      int _nBoneIdx);

    private delegate bool ResultTransformFunc(
      FakeTransform _ftransResult,
      CBoneData _Bone,
      int _nBoneIdx,
      FakeTransform _ftransT,
      FakeTransform _ftransR);
  }
}

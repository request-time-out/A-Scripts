// Decompiled with JetBrains decompiler
// Type: NeckLookCalcVer2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class NeckLookCalcVer2 : MonoBehaviour
{
  public bool isEnabled;
  public Transform transformAim;
  public Transform boneCalcAngle;
  public NeckObjectVer2[] aBones;
  public NeckTypeStateVer2[] neckTypeStates;
  [Tooltip("表示するパターン番号")]
  public int ptnDraw;
  public float drawLineLength;
  public float changeTypeLeapTime;
  public AnimationCurve changeTypeLerpCurve;
  [Tooltip("無条件でこれだけ回る")]
  public Vector2 nowAngle;
  [Range(0.0f, 1f)]
  public float calcLerp;
  public bool skipCalc;
  private int nowPtnNo;
  private bool initEnd;
  private Vector3 backupPos;
  private float changeTypeTimer;
  private NECK_LOOK_TYPE_VER2 lookType;

  public NeckLookCalcVer2()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    if (this.initEnd)
      return;
    this.Init();
  }

  private void Start()
  {
    if (this.initEnd)
      return;
    this.Init();
  }

  private void OnDrawGizmos()
  {
    if (Object.op_Implicit((Object) this.boneCalcAngle))
    {
      Gizmos.set_color(new Color(1f, 1f, 1f, 0.3f));
      if (this.neckTypeStates.Length > this.ptnDraw)
      {
        NeckTypeStateVer2 neckTypeState = this.neckTypeStates[this.ptnDraw];
        Gizmos.set_color(new Color(0.0f, 1f, 1f, 0.3f));
        Vector3 vector3_1 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, neckTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_1);
        Vector3 vector3_2 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -neckTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_2);
        Vector3 vector3_3 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(neckTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_3);
        Vector3 vector3_4 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-neckTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_4);
        Gizmos.DrawLine(vector3_1, vector3_4);
        Gizmos.DrawLine(vector3_4, vector3_2);
        Gizmos.DrawLine(vector3_2, vector3_3);
        Gizmos.DrawLine(vector3_3, vector3_1);
        if ((double) neckTypeState.limitBreakCorrectionValue != 0.0)
        {
          Gizmos.set_color(new Color(1f, 1f, 0.0f, 0.3f));
          Vector3 vector3_5 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, neckTypeState.hAngleLimit + neckTypeState.limitBreakCorrectionValue, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
          Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_5);
          Vector3 vector3_6 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -neckTypeState.hAngleLimit - neckTypeState.limitBreakCorrectionValue, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
          Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_6);
          Vector3 vector3_7 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(neckTypeState.vAngleLimit + neckTypeState.limitBreakCorrectionValue, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
          Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_7);
          Vector3 vector3_8 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-neckTypeState.vAngleLimit - neckTypeState.limitBreakCorrectionValue, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
          Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_8);
          Gizmos.DrawLine(vector3_5, vector3_8);
          Gizmos.DrawLine(vector3_8, vector3_6);
          Gizmos.DrawLine(vector3_6, vector3_7);
          Gizmos.DrawLine(vector3_7, vector3_5);
        }
        Gizmos.set_color(new Color(1f, 0.0f, 1f, 0.8f));
        float num1 = 0.0f;
        for (int index = 0; index < neckTypeState.aParam.Length; ++index)
          num1 += neckTypeState.aParam[index].maxBendingAngle;
        Vector3 vector3_9 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num1, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_9);
        Vector3 vector3_10 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num1 - neckTypeState.limitAway, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_10);
        Gizmos.DrawLine(vector3_9, vector3_10);
        float num2 = 0.0f;
        for (int index = 0; index < neckTypeState.aParam.Length; ++index)
          num2 += neckTypeState.aParam[index].minBendingAngle;
        Vector3 vector3_11 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num2, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_11);
        Vector3 vector3_12 = Vector3.op_Addition(this.boneCalcAngle.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num2 + neckTypeState.limitAway, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.boneCalcAngle.get_position());
        Gizmos.DrawLine(this.boneCalcAngle.get_position(), vector3_12);
        Gizmos.DrawLine(vector3_11, vector3_12);
      }
    }
    Gizmos.set_color(Color.get_white());
  }

  public void Init()
  {
    foreach (NeckObjectVer2 aBone in this.aBones)
    {
      if (Object.op_Equality((Object) aBone.referenceCalc, (Object) null))
        aBone.referenceCalc = ((Component) this).get_transform();
      aBone.angleH = 0.0f;
      aBone.angleV = 0.0f;
      aBone.angleHRate = 0.0f;
      aBone.angleVRate = 0.0f;
    }
    this.lookType = NECK_LOOK_TYPE_VER2.ANIMATION;
    this.initEnd = true;
  }

  public void UpdateCall(int ptnNo)
  {
    if (ptnNo >= this.neckTypeStates.Length)
      ptnNo = 0;
    if (this.lookType != this.neckTypeStates[ptnNo].lookType)
    {
      this.lookType = this.neckTypeStates[ptnNo].lookType;
      this.changeTypeTimer = 0.0f;
      for (int index = 0; index < this.aBones.Length; ++index)
        this.aBones[index].fixAngleBackup = this.aBones[index].fixAngle;
      if (this.lookType == NECK_LOOK_TYPE_VER2.FORWARD)
      {
        for (int index = 0; index < this.aBones.Length; ++index)
        {
          this.aBones[index].angleH = 0.0f;
          this.aBones[index].angleV = 0.0f;
        }
      }
    }
    if (this.lookType != NECK_LOOK_TYPE_VER2.TARGET)
      return;
    for (int index = 0; index < this.aBones.Length; ++index)
    {
      this.aBones[index].backupLocalRotaionByTarget = this.aBones[index].neckBone.get_localRotation();
      this.aBones[index].neckBone.set_localRotation(this.aBones[index].fixAngle);
    }
  }

  public void NeckUpdateCalc(Vector3 target, int ptnNo, bool _isUseBackUpPos = false)
  {
    if (!this.initEnd)
      return;
    this.nowPtnNo = ptnNo;
    if (!this.isEnabled || !this.skipCalc && (double) Time.get_deltaTime() == 0.0)
      return;
    NeckTypeStateVer2 neckTypeState = this.neckTypeStates[this.nowPtnNo];
    if (!_isUseBackUpPos)
      this.backupPos = target;
    if (neckTypeState.aParam.Length != this.aBones.Length)
    {
      Debug.LogWarning((object) "パラメーターの個数と骨の個数が合っていない");
    }
    else
    {
      if (this.skipCalc)
        this.changeTypeTimer = this.changeTypeLeapTime;
      this.changeTypeTimer = Mathf.Clamp(this.changeTypeTimer + Time.get_deltaTime(), 0.0f, this.changeTypeLeapTime);
      float num1 = Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer);
      if (this.changeTypeLerpCurve != null)
        num1 = this.changeTypeLerpCurve.Evaluate(num1);
      if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.ANIMATION)
      {
        for (int index = 0; index < this.aBones.Length; ++index)
        {
          this.aBones[index].fixAngle = this.aBones[index].neckBone.get_localRotation();
          this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, this.aBones[index].fixAngle, num1));
          if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
          {
            this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            if (((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf())
              ((Component) this.aBones[index].controlBone).get_gameObject().SetActive(false);
          }
        }
      }
      else
      {
        bool flag1 = false;
        if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.CONTROL)
        {
          for (int index = 0; index < this.aBones.Length; ++index)
          {
            if (!Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
            {
              flag1 = true;
              break;
            }
          }
        }
        if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.CONTROL && !flag1)
        {
          for (int index = 0; index < this.aBones.Length; ++index)
          {
            if (index != 0)
              ((Component) this.aBones[index].controlBone).get_gameObject().SetActive(!((Component) this.aBones[0].controlBone).get_gameObject().get_activeSelf());
            this.aBones[index].fixAngle = this.aBones[index].controlBone.get_localRotation();
            float num2 = Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer);
            this.aBones[index].neckBone.set_localRotation(Quaternion.Lerp(this.aBones[index].fixAngleBackup, this.aBones[index].fixAngle, num2));
          }
        }
        else
        {
          for (int index = 0; index < this.aBones.Length; ++index)
          {
            if (!Object.op_Equality((Object) this.aBones[index].controlBone, (Object) null) && ((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf())
              ((Component) this.aBones[index].controlBone).get_gameObject().SetActive(false);
          }
          if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.FORWARD || flag1)
          {
            for (int index = 0; index < this.aBones.Length; ++index)
            {
              this.aBones[index].fixAngle = Quaternion.get_identity();
              Quaternion quaternion = Quaternion.Slerp(this.aBones[index].neckBone.get_localRotation(), this.aBones[index].fixAngle, this.calcLerp);
              this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, quaternion, num1));
              if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
                this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            }
          }
          else if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.FIX)
          {
            for (int index = 0; index < this.aBones.Length; ++index)
            {
              Quaternion quaternion = Quaternion.Slerp(this.aBones[index].neckBone.get_localRotation(), this.aBones[index].fixAngle, this.calcLerp);
              this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, quaternion, num1));
              if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
                this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            }
          }
          else
          {
            Vector3 dirB = Vector3.op_Subtraction(target, this.boneCalcAngle.get_position());
            float num2 = this.AngleAroundAxis(this.boneCalcAngle.get_forward(), dirB, this.boneCalcAngle.get_up());
            float num3 = this.AngleAroundAxis(this.boneCalcAngle.get_forward(), dirB, this.boneCalcAngle.get_right());
            bool flag2 = false;
            float num4 = !neckTypeState.isLimitBreakBackup ? this.neckTypeStates[ptnNo].limitBreakCorrectionValue : 0.0f;
            if ((double) Mathf.Abs(num2) > (double) this.neckTypeStates[ptnNo].hAngleLimit + (double) num4 || (double) Mathf.Abs(num3) > (double) this.neckTypeStates[ptnNo].vAngleLimit + (double) num4)
              flag2 = true;
            neckTypeState.isLimitBreakBackup = flag2;
            if (flag2)
            {
              this.nowAngle = Vector2.get_zero();
            }
            else
            {
              if (_isUseBackUpPos)
                target = this.backupPos;
              this.nowAngle = this.GetAngleToTarget(target, this.aBones[this.aBones.Length - 1], 1f);
              if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.TARGET)
              {
                Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.transformAim.get_position(), this.boneCalcAngle.get_rotation(), this.transformAim.get_lossyScale());
                Matrix4x4 inverse1 = ((Matrix4x4) ref matrix4x4).get_inverse();
                Vector3 vector3_1 = ((Matrix4x4) ref inverse1).MultiplyPoint3x4(target);
                Matrix4x4 inverse2 = ((Matrix4x4) ref matrix4x4).get_inverse();
                Vector3 vector3_2 = ((Matrix4x4) ref inverse2).MultiplyPoint3x4(this.boneCalcAngle.get_position());
                if (vector3_1.z < 0.0 && vector3_1.y < 0.0)
                {
                  if (vector3_2.x < 0.0)
                  {
                    if (vector3_2.x < vector3_1.x && vector3_1.x < 0.0)
                      target = this.transformAim.get_position();
                  }
                  else if (vector3_2.x > vector3_1.x && vector3_1.x > 0.0)
                    target = this.transformAim.get_position();
                }
                Vector3 vector3_3 = Vector3.op_Subtraction(this.transformAim.get_position(), target);
                if ((double) ((Vector3) ref vector3_3).get_magnitude() == 0.0)
                {
                  for (int index = 0; index < this.aBones.Length; ++index)
                  {
                    this.CalcNeckBone(this.aBones[index]);
                    this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, this.aBones[index].fixAngle, num1));
                  }
                  return;
                }
              }
              else if (neckTypeState.lookType == NECK_LOOK_TYPE_VER2.AWAY)
              {
                float num5 = 0.0f;
                float num6 = 0.0f;
                for (int index = 0; index < this.aBones.Length; ++index)
                  num5 += this.aBones[index].angleH;
                if (this.nowAngle.y <= (double) num5)
                {
                  for (int index = 0; index < this.aBones.Length; ++index)
                    num6 += neckTypeState.aParam[index].maxBendingAngle;
                  if (this.nowAngle.y <= (double) num6 - (double) neckTypeState.limitAway || this.nowAngle.y < 0.0)
                  {
                    this.nowAngle.y = (__Null) (double) num6;
                  }
                  else
                  {
                    this.nowAngle.y = (__Null) 0.0;
                    for (int index = 0; index < this.aBones.Length; ++index)
                    {
                      ref Vector2 local = ref this.nowAngle;
                      local.y = (__Null) (local.y + (double) neckTypeState.aParam[index].minBendingAngle);
                    }
                  }
                }
                else if (this.nowAngle.y > (double) num5)
                {
                  for (int index = 0; index < this.aBones.Length; ++index)
                    num6 += neckTypeState.aParam[index].minBendingAngle;
                  if (this.nowAngle.y >= (double) num6 + (double) neckTypeState.limitAway || this.nowAngle.y > 0.0)
                  {
                    this.nowAngle.y = (__Null) (double) num6;
                  }
                  else
                  {
                    this.nowAngle.y = (__Null) 0.0;
                    for (int index = 0; index < this.aBones.Length; ++index)
                    {
                      ref Vector2 local = ref this.nowAngle;
                      local.y = (__Null) (local.y + (double) neckTypeState.aParam[index].maxBendingAngle);
                    }
                  }
                }
                this.nowAngle.x = -this.nowAngle.x;
              }
            }
            Vector2 nowAngle = this.nowAngle;
            for (int _boneNum = this.aBones.Length - 1; _boneNum > -1; --_boneNum)
              this.RotateToAngle(this.aBones[_boneNum], neckTypeState, _boneNum, ref nowAngle);
            for (int index = 0; index < this.aBones.Length; ++index)
            {
              Quaternion quaternion = Quaternion.Slerp(this.aBones[index].backupLocalRotaionByTarget, this.aBones[index].fixAngle, this.calcLerp);
              this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, quaternion, num1));
              if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
                this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            }
          }
        }
      }
    }
  }

  private Vector2 GetAngleToTarget(
    Vector3 _targetPosition,
    NeckObjectVer2 _bone,
    float _weight)
  {
    Quaternion quaternion1 = Quaternion.op_Multiply(Quaternion.FromToRotation(Quaternion.op_Multiply(this.transformAim.get_rotation(), Vector3.get_forward()), Vector3.op_Subtraction(_targetPosition, this.transformAim.get_position())), _bone.neckBone.get_rotation());
    float num = this.AngleAroundAxis(this.boneCalcAngle.get_forward(), Quaternion.op_Multiply(quaternion1, Vector3.get_forward()), this.boneCalcAngle.get_up());
    Quaternion quaternion2 = Quaternion.op_Multiply(Quaternion.AngleAxis(num, this.boneCalcAngle.get_up()), this.boneCalcAngle.get_rotation());
    Vector3 axis = Vector3.Cross(this.boneCalcAngle.get_up(), Quaternion.op_Multiply(quaternion2, Vector3.get_forward()));
    return new Vector2(this.AngleAroundAxis(Quaternion.op_Multiply(quaternion2, Vector3.get_forward()), Quaternion.op_Multiply(quaternion1, Vector3.get_forward()), axis), num);
  }

  private void RotateToAngle(
    NeckObjectVer2 _bone,
    NeckTypeStateVer2 _param,
    int _boneNum,
    ref Vector2 _Angle)
  {
    float num1 = Mathf.Clamp((float) _Angle.y, _param.aParam[_boneNum].minBendingAngle, _param.aParam[_boneNum].maxBendingAngle);
    float num2 = Mathf.Clamp((float) _Angle.x, _param.aParam[_boneNum].upBendingAngle, _param.aParam[_boneNum].downBendingAngle);
    _Angle = Vector2.op_Subtraction(_Angle, new Vector2(num2, num1));
    float num3 = Mathf.Clamp01(Time.get_deltaTime() * _param.leapSpeed);
    if (this.skipCalc)
      num3 = 1f;
    _bone.angleH = Mathf.Lerp(_bone.angleH, num1, num3);
    _bone.angleV = Mathf.Lerp(_bone.angleV, num2, num3);
    this.CalcNeckBone(_bone);
  }

  private void CalcNeckBone(NeckObjectVer2 _bone)
  {
    Quaternion quaternion = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(_bone.angleH, _bone.referenceCalc.get_up()), Quaternion.AngleAxis(_bone.angleV, _bone.referenceCalc.get_right())), _bone.referenceCalc.get_rotation());
    _bone.neckBone.set_rotation(quaternion);
    _bone.fixAngle = _bone.neckBone.get_localRotation();
  }

  private float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
  {
    dirA = Vector3.op_Subtraction(dirA, Vector3.Project(dirA, axis));
    dirB = Vector3.op_Subtraction(dirB, Vector3.Project(dirB, axis));
    return Vector3.Angle(dirA, dirB) * ((double) Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0.0 ? 1f : -1f);
  }

  public void setEnable(bool setFlag)
  {
    this.isEnabled = setFlag;
  }
}

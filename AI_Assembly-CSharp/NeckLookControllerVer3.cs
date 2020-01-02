// Decompiled with JetBrains decompiler
// Type: NeckLookControllerVer3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class NeckLookControllerVer3 : MonoBehaviour
{
  public int ptnNo;
  public Transform target;
  public float rate;
  public Transform rootNode;
  public bool isEnabled;
  public Transform m_transformAim;
  public Transform m_boneCalcAngle;
  private Transform transformAim;
  private Transform boneCalcAngle;
  private Quaternion transformAimWorldRotation;
  private Quaternion transformAimLocalRotation;
  private Quaternion boneCalcAngleWorldRotation;
  private Quaternion boneCalcAngleLocalRotation;
  public NeckObjectVer3[] aBones;
  public NeckTypeStateVer3[] neckTypeStates;
  [Tooltip("表示するパターン番号")]
  public int ptnDraw;
  public float drawLineLength;
  public float changeTypeLeapTime;
  public AnimationCurve changeTypeLerpCurve;
  [Tooltip("無条件でこれだけ回る")]
  public Vector2 nowAngle;
  [Range(0.0f, 1f)]
  public float calcLerp;
  private int nowPtnNo;
  private bool initEnd;
  private Vector3 backupPos;
  private float changeTypeTimer;
  private NECK_LOOK_TYPE_VER3 lookType;

  public NeckLookControllerVer3()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    this.Init();
  }

  private void Start()
  {
    if (!Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) Camera.get_main()))
      this.target = ((Component) Camera.get_main()).get_transform();
    this.Init();
  }

  private void LateUpdate()
  {
    this.UpdateCall(this.ptnNo);
    if (Object.op_Inequality((Object) this.target, (Object) null))
    {
      Vector3 position1 = this.transformAim.get_position();
      Vector3 position2 = this.target.get_position();
      for (int index = 0; index < 2; ++index)
        ((Vector3) ref position2).set_Item(index, Mathf.Lerp(((Vector3) ref position1).get_Item(index), ((Vector3) ref position2).get_Item(index), this.rate));
      this.NeckUpdateCalc(position2, this.ptnNo, false);
    }
    else
      this.NeckUpdateCalc(Vector3.get_zero(), this.ptnNo, true);
  }

  private void OnDrawGizmos()
  {
    Transform transform = !Object.op_Implicit((Object) this.boneCalcAngle) ? (!Object.op_Implicit((Object) this.m_boneCalcAngle) ? (Transform) null : this.m_boneCalcAngle) : this.boneCalcAngle;
    if (!Object.op_Implicit((Object) transform) || this.neckTypeStates.Length <= this.ptnDraw || this.ptnDraw < 0)
    {
      Gizmos.set_color(Color.get_white());
    }
    else
    {
      NeckTypeStateVer3 neckTypeState = this.neckTypeStates[this.ptnDraw];
      Vector3 position = transform.get_position();
      Gizmos.set_color(new Color(0.0f, 1f, 1f, 0.75f));
      Vector3 vector3_1 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, neckTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Vector3 vector3_2 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -neckTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Vector3 vector3_3 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(neckTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Vector3 vector3_4 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-neckTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Gizmos.DrawLine(position, vector3_1);
      Gizmos.DrawLine(position, vector3_2);
      Gizmos.DrawLine(position, vector3_3);
      Gizmos.DrawLine(position, vector3_4);
      Gizmos.DrawLine(vector3_1, vector3_4);
      Gizmos.DrawLine(vector3_4, vector3_2);
      Gizmos.DrawLine(vector3_2, vector3_3);
      Gizmos.DrawLine(vector3_3, vector3_1);
      if ((double) neckTypeState.limitBreakCorrectionValue != 0.0)
      {
        Gizmos.set_color(new Color(1f, 1f, 0.0f, 0.75f));
        Vector3 vector3_5 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, neckTypeState.hAngleLimit + neckTypeState.limitBreakCorrectionValue, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
        Vector3 vector3_6 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -neckTypeState.hAngleLimit - neckTypeState.limitBreakCorrectionValue, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
        Vector3 vector3_7 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(neckTypeState.vAngleLimit + neckTypeState.limitBreakCorrectionValue, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
        Vector3 vector3_8 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-neckTypeState.vAngleLimit - neckTypeState.limitBreakCorrectionValue, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
        Gizmos.DrawLine(position, vector3_5);
        Gizmos.DrawLine(position, vector3_6);
        Gizmos.DrawLine(position, vector3_7);
        Gizmos.DrawLine(position, vector3_8);
        Gizmos.DrawLine(vector3_5, vector3_8);
        Gizmos.DrawLine(vector3_8, vector3_6);
        Gizmos.DrawLine(vector3_6, vector3_7);
        Gizmos.DrawLine(vector3_7, vector3_5);
      }
      Gizmos.set_color(new Color(1f, 0.0f, 1f, 0.75f));
      float num1 = 0.0f;
      for (int index = 0; index < neckTypeState.aParam.Length; ++index)
        num1 += neckTypeState.aParam[index].rightBendingAngle;
      Vector3 vector3_9 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num1, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Vector3 vector3_10 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num1 - neckTypeState.limitAway, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Gizmos.DrawLine(position, vector3_9);
      Gizmos.DrawLine(position, vector3_10);
      Gizmos.DrawLine(vector3_9, vector3_10);
      float num2 = 0.0f;
      for (int index = 0; index < neckTypeState.aParam.Length; ++index)
        num2 += neckTypeState.aParam[index].leftBendingAngle;
      Vector3 vector3_11 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num2, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Vector3 vector3_12 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, num2 + neckTypeState.limitAway, 0.0f), Vector3.get_forward()), this.drawLineLength)), position);
      Gizmos.DrawLine(position, vector3_11);
      Gizmos.DrawLine(position, vector3_12);
      Gizmos.DrawLine(vector3_11, vector3_12);
      Gizmos.set_color(Color.get_white());
    }
  }

  private void CreateObj(
    Transform _origin,
    ref Transform _newObj,
    ref Quaternion _worldRotation,
    ref Quaternion _localRotation,
    string _name = "")
  {
    if (Object.op_Inequality((Object) _newObj, (Object) null) && Object.op_Inequality((Object) ((Component) _newObj).get_gameObject(), (Object) null))
      Object.Destroy((Object) ((Component) _newObj).get_gameObject());
    _newObj = (Transform) null;
    if (_name.IsNullOrEmpty())
      _name = ((Object) _origin).get_name() + "(Ref)";
    GameObject gameObject = new GameObject(_name);
    _newObj = gameObject.get_transform();
    _newObj.set_parent(_origin.get_parent());
    _newObj.set_rotation(Quaternion.get_identity());
    _newObj.set_localScale(_origin.get_localScale());
    _newObj.set_position(_origin.get_position());
    _worldRotation = Quaternion.op_Multiply(_origin.get_rotation(), Quaternion.Inverse(this.rootNode.get_rotation()));
    _localRotation = _origin.get_localRotation();
  }

  public void Init()
  {
    if (this.initEnd)
      return;
    if (!Object.op_Implicit((Object) this.rootNode))
      this.rootNode = ((Component) this).get_transform();
    Quaternion quaternion = (Quaternion) null;
    if (Object.op_Implicit((Object) this.rootNode))
    {
      quaternion = this.rootNode.get_rotation();
      this.rootNode.set_rotation(Quaternion.get_identity());
    }
    this.DestroyReference();
    if (Object.op_Inequality((Object) this.m_transformAim, (Object) null))
      this.CreateObj(this.m_transformAim, ref this.transformAim, ref this.transformAimWorldRotation, ref this.transformAimLocalRotation, ((Object) this.m_transformAim).get_name() + "(AimRef)");
    if (Object.op_Inequality((Object) this.m_boneCalcAngle, (Object) null))
      this.CreateObj(this.m_boneCalcAngle, ref this.boneCalcAngle, ref this.boneCalcAngleWorldRotation, ref this.boneCalcAngleLocalRotation, ((Object) this.m_boneCalcAngle).get_name() + "(CalcRef)");
    foreach (NeckObjectVer3 aBone in this.aBones)
    {
      if (Object.op_Equality((Object) aBone.m_referenceCalc, (Object) null))
        aBone.m_referenceCalc = ((Component) this).get_transform();
      if (Object.op_Inequality((Object) aBone.m_referenceCalc, (Object) null))
        this.CreateObj(aBone.m_referenceCalc, ref aBone.referenceCalc, ref aBone.referenceCalcWorldRotation, ref aBone.referenceCalcLocalRotation, ((Object) aBone.m_referenceCalc).get_name() + "(軸Ref)");
      aBone.neckBoneWorldRotation = Quaternion.op_Multiply(aBone.neckBone.get_rotation(), Quaternion.Inverse(this.rootNode.get_rotation()));
      aBone.neckBoneLocalRotation = aBone.neckBone.get_localRotation();
      aBone.angleH = aBone.angleV = aBone.angleHRate = aBone.angleVRate = 0.0f;
    }
    if (Object.op_Implicit((Object) this.rootNode))
      this.rootNode.set_rotation(quaternion);
    this.initEnd = true;
  }

  private void UpdateCall(int _ptnNo)
  {
    if (this.neckTypeStates.Length <= _ptnNo || _ptnNo < 0)
      _ptnNo = 0;
    if (this.neckTypeStates.Length == 0)
    {
      Debug.LogWarning((object) "動きパターンが設定されていません");
    }
    else
    {
      if (this.lookType != this.neckTypeStates[_ptnNo].lookType)
      {
        this.lookType = this.neckTypeStates[_ptnNo].lookType;
        this.changeTypeTimer = 0.0f;
        for (int index = 0; index < this.aBones.Length; ++index)
          this.aBones[index].fixAngleBackup = this.aBones[index].fixAngle;
      }
      if (this.lookType != NECK_LOOK_TYPE_VER3.TARGET)
        return;
      for (int index = 0; index < this.aBones.Length; ++index)
      {
        this.aBones[index].backupLocalRotationByTarget = this.aBones[index].neckBone.get_localRotation();
        this.aBones[index].neckBone.set_localRotation(this.aBones[index].fixAngle);
      }
    }
  }

  private void NeckUpdateCalc(Vector3 _target, int _ptnNo, bool _isUseBackUpPos = false)
  {
    if (!this.initEnd)
      return;
    this.nowPtnNo = _ptnNo;
    if (!this.isEnabled || this.nowPtnNo < 0 || (this.neckTypeStates.Length <= this.nowPtnNo || this.neckTypeStates.Length == 0))
      return;
    NeckTypeStateVer3 neckTypeState = this.neckTypeStates[this.nowPtnNo];
    if (!_isUseBackUpPos)
      this.backupPos = _target;
    if (neckTypeState.aParam.Length != this.aBones.Length)
    {
      Debug.LogWarning((object) "パラメーターと骨の個数が合っていない");
    }
    else
    {
      this.changeTypeTimer = Mathf.Clamp(this.changeTypeTimer + Time.get_deltaTime(), 0.0f, this.changeTypeLeapTime);
      float num1 = Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer);
      if (this.changeTypeLerpCurve != null)
        num1 = this.changeTypeLerpCurve.Evaluate(num1);
      if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.ANIMATION)
      {
        for (int index = 0; index < this.aBones.Length; ++index)
        {
          this.aBones[index].fixAngle = this.aBones[index].neckBone.get_localRotation();
          this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, this.aBones[index].fixAngle, num1));
          Transform controlBone = this.aBones[index].controlBone;
          if (Object.op_Inequality((Object) controlBone, (Object) null))
          {
            controlBone.set_localRotation(this.aBones[index].fixAngle);
            if (((Component) controlBone).get_gameObject().get_activeSelf())
              ((Component) controlBone).get_gameObject().SetActive(false);
          }
        }
      }
      else
      {
        bool flag1 = false;
        if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.CONTROL)
        {
          int index = 0;
          while (index < this.aBones.Length && !(flag1 = Object.op_Equality((Object) this.aBones[index].controlBone, (Object) null)))
            ++index;
        }
        if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.CONTROL && !flag1)
        {
          for (int index = 0; index < this.aBones.Length; ++index)
          {
            if (index != 0 && ((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf() == ((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf())
              ((Component) this.aBones[index].controlBone).get_gameObject().SetActive(!((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf());
            this.aBones[index].fixAngle = this.aBones[index].controlBone.get_localRotation();
            float num2 = Mathf.InverseLerp(0.0f, this.changeTypeLeapTime, this.changeTypeTimer);
            this.aBones[index].neckBone.set_localRotation(Quaternion.Lerp(this.aBones[index].fixAngleBackup, this.aBones[index].fixAngle, num2));
          }
        }
        else
        {
          for (int index = 0; index < this.aBones.Length; ++index)
          {
            if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null) && ((Component) this.aBones[index].controlBone).get_gameObject().get_activeSelf())
              ((Component) this.aBones[index].controlBone).get_gameObject().SetActive(false);
          }
          if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.FORWARD || flag1)
          {
            for (int index = 0; index < this.aBones.Length; ++index)
            {
              this.aBones[index].fixAngle = this.aBones[index].neckBoneLocalRotation;
              Quaternion quaternion = Quaternion.Slerp(this.aBones[index].neckBone.get_localRotation(), this.aBones[index].fixAngle, this.calcLerp);
              this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, quaternion, num1));
              if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
                this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            }
          }
          else if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.FIX)
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
            Vector3 _dirB = Vector3.op_Subtraction(_target, this.boneCalcAngle.get_position());
            Vector3 forward = this.boneCalcAngle.get_forward();
            float num2 = this.AngleAroundAxis(forward, _dirB, this.boneCalcAngle.get_up());
            float num3 = this.AngleAroundAxis(forward, _dirB, this.boneCalcAngle.get_right());
            bool flag2 = false;
            float num4 = !neckTypeState.isLimitBreakBackup ? this.neckTypeStates[_ptnNo].limitBreakCorrectionValue : 0.0f;
            if ((double) Mathf.Abs(num2) > (double) this.neckTypeStates[_ptnNo].hAngleLimit + (double) num4 || (double) Mathf.Abs(num3) > (double) this.neckTypeStates[_ptnNo].vAngleLimit + (double) num4)
              flag2 = true;
            neckTypeState.isLimitBreakBackup = flag2;
            if (flag2)
            {
              this.nowAngle = Vector2.get_zero();
            }
            else
            {
              if (_isUseBackUpPos)
                _target = this.backupPos;
              this.nowAngle = this.GetAngleToTarget(_target, this.aBones[this.aBones.Length - 1]);
              if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.TARGET)
              {
                Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.transformAim.get_position(), this.boneCalcAngle.get_rotation(), this.transformAim.get_lossyScale());
                Matrix4x4 inverse1 = ((Matrix4x4) ref matrix4x4).get_inverse();
                Vector3 vector3_1 = ((Matrix4x4) ref inverse1).MultiplyPoint3x4(_target);
                Matrix4x4 inverse2 = ((Matrix4x4) ref matrix4x4).get_inverse();
                Vector3 vector3_2 = ((Matrix4x4) ref inverse2).MultiplyPoint3x4(this.boneCalcAngle.get_position());
                if (vector3_1.z < 0.0 && vector3_1.y < 0.0)
                {
                  if (vector3_2.x < 0.0)
                  {
                    if (vector3_2.x < vector3_1.x && vector3_1.x < 0.0)
                      _target = this.transformAim.get_position();
                  }
                  else if (vector3_1.x < vector3_2.x && 0.0 < vector3_1.x)
                    _target = this.transformAim.get_position();
                }
                Vector3 vector3_3 = Vector3.op_Subtraction(this.transformAim.get_position(), _target);
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
              else if (neckTypeState.lookType == NECK_LOOK_TYPE_VER3.AWAY)
              {
                float num5 = 0.0f;
                float num6 = 0.0f;
                for (int index = 0; index < this.aBones.Length; ++index)
                  num5 += this.aBones[index].angleH;
                if (this.nowAngle.y <= (double) num5)
                {
                  for (int index = 0; index < this.aBones.Length; ++index)
                    num6 += neckTypeState.aParam[index].rightBendingAngle;
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
                      local.y = (__Null) (local.y + (double) neckTypeState.aParam[index].leftBendingAngle);
                    }
                  }
                }
                else if ((double) num5 < this.nowAngle.y)
                {
                  for (int index = 0; index < this.aBones.Length; ++index)
                    num6 += neckTypeState.aParam[index].leftBendingAngle;
                  if ((double) num6 + (double) neckTypeState.limitAway <= this.nowAngle.y || 0.0 < this.nowAngle.y)
                  {
                    this.nowAngle.y = (__Null) (double) num6;
                  }
                  else
                  {
                    this.nowAngle.y = (__Null) 0.0;
                    for (int index = 0; index < this.aBones.Length; ++index)
                    {
                      ref Vector2 local = ref this.nowAngle;
                      local.y = (__Null) (local.y + (double) neckTypeState.aParam[index].rightBendingAngle);
                    }
                  }
                }
                this.nowAngle.x = -this.nowAngle.x;
              }
            }
            Vector2 nowAngle = this.nowAngle;
            for (int _boneNum = this.aBones.Length - 1; 0 <= _boneNum; --_boneNum)
              this.RotateToAngle(this.aBones[_boneNum], neckTypeState, _boneNum, ref nowAngle);
            for (int index = 0; index < this.aBones.Length; ++index)
            {
              Quaternion quaternion = Quaternion.Slerp(this.aBones[index].backupLocalRotationByTarget, this.aBones[index].fixAngle, this.calcLerp);
              this.aBones[index].neckBone.set_localRotation(Quaternion.Slerp(this.aBones[index].fixAngleBackup, quaternion, num1));
              if (Object.op_Inequality((Object) this.aBones[index].controlBone, (Object) null))
                this.aBones[index].controlBone.set_localRotation(this.aBones[index].fixAngle);
            }
          }
        }
      }
    }
  }

  private float AngleAroundAxis(Vector3 _dirA, Vector3 _dirB, Vector3 _axis)
  {
    _dirA = Vector3.op_Subtraction(_dirA, Vector3.Project(_dirA, _axis));
    _dirB = Vector3.op_Subtraction(_dirB, Vector3.Project(_dirB, _axis));
    return Vector3.Angle(_dirA, _dirB) * ((double) Vector3.Dot(_axis, Vector3.Cross(_dirA, _dirB)) >= 0.0 ? 1f : -1f);
  }

  private Vector2 GetAngleToTarget(Vector3 _targetPosition, NeckObjectVer3 _bone)
  {
    Quaternion quaternion1 = Quaternion.op_Multiply(Quaternion.FromToRotation(Quaternion.op_Multiply(this.transformAim.get_rotation(), Vector3.get_forward()), Vector3.op_Subtraction(_targetPosition, this.transformAim.get_position())), Quaternion.op_Multiply(_bone.neckBone.get_rotation(), Quaternion.Inverse(_bone.neckBoneWorldRotation)));
    float num = this.AngleAroundAxis(this.boneCalcAngle.get_forward(), Quaternion.op_Multiply(quaternion1, Vector3.get_forward()), this.boneCalcAngle.get_up());
    Quaternion quaternion2 = Quaternion.op_Multiply(Quaternion.AngleAxis(num, this.boneCalcAngle.get_up()), this.boneCalcAngle.get_rotation());
    Vector3 _axis = Vector3.Cross(this.boneCalcAngle.get_up(), Quaternion.op_Multiply(quaternion2, Vector3.get_forward()));
    return new Vector2(this.AngleAroundAxis(Quaternion.op_Multiply(quaternion2, Vector3.get_forward()), Quaternion.op_Multiply(quaternion1, Vector3.get_forward()), _axis), num);
  }

  private void CalcNeckBone(NeckObjectVer3 _bone)
  {
    Quaternion quaternion = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(_bone.angleH, _bone.referenceCalc.get_up()), Quaternion.AngleAxis(_bone.angleV, _bone.referenceCalc.get_right())), _bone.referenceCalc.get_rotation());
    _bone.neckBone.set_rotation(Quaternion.op_Multiply(quaternion, _bone.neckBoneWorldRotation));
    _bone.fixAngle = _bone.neckBone.get_localRotation();
  }

  private void RotateToAngle(
    NeckObjectVer3 _bone,
    NeckTypeStateVer3 _param,
    int _boneNum,
    ref Vector2 _angle)
  {
    float num1 = Mathf.Clamp((float) _angle.y, _param.aParam[_boneNum].leftBendingAngle, _param.aParam[_boneNum].rightBendingAngle);
    float num2 = Mathf.Clamp((float) _angle.x, _param.aParam[_boneNum].upBendingAngle, _param.aParam[_boneNum].downBendingAngle);
    _angle = Vector2.op_Subtraction(_angle, new Vector2(num2, num1));
    float num3 = Mathf.Clamp01(Time.get_deltaTime() * _param.leapSpeed);
    _bone.angleH = Mathf.Lerp(_bone.angleH, num1, num3);
    _bone.angleV = Mathf.Lerp(_bone.angleV, num2, num3);
    this.CalcNeckBone(_bone);
  }

  public void SetEnable(bool _setFlag)
  {
    this.isEnabled = _setFlag;
  }

  private void DestroyReference()
  {
    if (Object.op_Inequality((Object) this.transformAim, (Object) null))
    {
      Object.Destroy((Object) ((Component) this.transformAim).get_gameObject());
      this.transformAim = (Transform) null;
    }
    if (Object.op_Inequality((Object) this.boneCalcAngle, (Object) null))
    {
      Object.Destroy((Object) ((Component) this.boneCalcAngle).get_gameObject());
      this.boneCalcAngle = (Transform) null;
    }
    foreach (NeckObjectVer3 aBone in this.aBones)
    {
      if (Object.op_Inequality((Object) aBone.referenceCalc, (Object) null))
      {
        Object.Destroy((Object) ((Component) aBone.referenceCalc).get_gameObject());
        aBone.referenceCalc = (Transform) null;
      }
    }
  }

  private void OnDestroy()
  {
    this.DestroyReference();
  }
}

// Decompiled with JetBrains decompiler
// Type: EyeLookControllerVer2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

public class EyeLookControllerVer2 : MonoBehaviour
{
  public static bool isEnabled = true;
  public int ptnNo;
  public Transform target;
  public Transform calcRootNode;
  private Transform calcRootNodeRef;
  public Transform objRootNode;
  public EyeObject_Ver2[] eyeObjs;
  public Vector3 headLookVector;
  public Vector3 headUpVector;
  public EyeTypeState_Ver2[] eyeTypeStates;
  public float[] angleHRate;
  public float angleVRate;
  public float awayRate;
  public bool isDebugDraw;
  public int ptnDraw;
  public float drawLineLength;
  private int nowPtnNo;
  private bool initEnd;
  public GameObject targetObj;
  private Transform originRootNode;
  private Vector3 targetPos;
  public Quaternion[] fixAngle;

  public EyeLookControllerVer2()
  {
    base.\u002Ector();
  }

  public Vector3 TargetPos
  {
    get
    {
      return this.targetPos;
    }
    set
    {
      this.targetPos = value;
    }
  }

  private void Awake()
  {
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

  private void OnDrawGizmos()
  {
    Transform transform = this.calcRootNode ?? this.objRootNode;
    if (!this.isDebugDraw || !Object.op_Implicit((Object) transform))
    {
      Gizmos.set_color(Color.get_white());
    }
    else
    {
      if (0 < this.eyeTypeStates.Length)
      {
        if (this.ptnDraw < 0)
          this.ptnDraw = 0;
        else if (this.eyeTypeStates.Length <= this.ptnDraw)
          this.ptnDraw = this.eyeTypeStates.Length - 1;
        Gizmos.set_color(new Color(1f, 1f, 1f, 0.8f));
        EyeTypeState_Ver2 eyeTypeState = this.eyeTypeStates[this.ptnDraw];
        Gizmos.set_color(new Color(0.0f, 1f, 1f, 0.8f));
        Vector3 position1 = transform.get_position();
        Vector3 vector3_1 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), position1);
        Vector3 vector3_2 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -eyeTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), position1);
        Vector3 vector3_3 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position1);
        Vector3 vector3_4 = Vector3.op_Addition(transform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-eyeTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position1);
        Gizmos.DrawLine(position1, vector3_1);
        Gizmos.DrawLine(position1, vector3_2);
        Gizmos.DrawLine(position1, vector3_3);
        Gizmos.DrawLine(position1, vector3_4);
        Gizmos.DrawLine(vector3_1, vector3_4);
        Gizmos.DrawLine(vector3_4, vector3_2);
        Gizmos.DrawLine(vector3_2, vector3_3);
        Gizmos.DrawLine(vector3_3, vector3_1);
        Gizmos.set_color(new Color(1f, 0.0f, 1f, 0.8f));
        for (int index = 0; index < this.eyeObjs.Length; ++index)
        {
          Vector3 position2 = this.eyeObjs[index].eyeTransform.get_position();
          Vector3 vector3_5 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.inBendingAngle * (float) this.eyeObjs[index].eyeLR, 0.0f), Vector3.get_forward()), this.drawLineLength)), position2);
          Vector3 vector3_6 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.outBendingAngle * (float) this.eyeObjs[index].eyeLR, 0.0f), Vector3.get_forward()), this.drawLineLength)), position2);
          Vector3 vector3_7 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.upBendingAngle, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position2);
          Vector3 vector3_8 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.downBendingAngle, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), position2);
          Gizmos.DrawLine(position2, vector3_5);
          Gizmos.DrawLine(position2, vector3_6);
          Gizmos.DrawLine(position2, vector3_7);
          Gizmos.DrawLine(position2, vector3_8);
          Gizmos.DrawLine(vector3_5, vector3_8);
          Gizmos.DrawLine(vector3_8, vector3_6);
          Gizmos.DrawLine(vector3_6, vector3_7);
          Gizmos.DrawLine(vector3_7, vector3_5);
        }
      }
      Gizmos.set_color(new Color(1f, 1f, 0.0f, 0.8f));
      for (int index = 0; index < this.eyeObjs.Length; ++index)
      {
        Vector3 position = this.eyeObjs[index].eyeTransform.get_position();
        Gizmos.DrawLine(position, Vector3.op_Addition(position, Vector3.op_Multiply(this.eyeObjs[index].eyeTransform.get_forward(), this.drawLineLength)));
      }
      Gizmos.set_color(Color.get_white());
    }
  }

  private void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.target))
      return;
    this.EyeUpdateCalc(this.target.get_position(), this.ptnNo);
  }

  private void Init()
  {
    if (this.initEnd)
      return;
    if (Object.op_Equality((Object) this.calcRootNode, (Object) null))
      this.calcRootNode = ((Component) this).get_transform();
    if (Object.op_Equality((Object) this.objRootNode, (Object) null) && Object.op_Inequality((Object) this.calcRootNode, (Object) ((Component) this).get_transform()) && Object.op_Inequality((Object) this.calcRootNode, (Object) ((Component) this).get_transform().get_parent()))
      this.objRootNode = ((Component) this).get_transform().get_parent() ?? ((Component) this).get_transform();
    Quaternion quaternion1 = Quaternion.get_identity();
    if (Object.op_Implicit((Object) this.objRootNode))
    {
      quaternion1 = this.objRootNode.get_rotation();
      this.objRootNode.set_rotation(Quaternion.get_identity());
    }
    this.originRootNode = this.calcRootNode ?? this.objRootNode;
    if (Object.op_Inequality((Object) this.calcRootNode, (Object) null))
    {
      GameObject gameObject = new GameObject(((Object) this.calcRootNode).get_name() + "(EyeRef)");
      gameObject.get_transform().set_parent(this.calcRootNode.get_parent());
      gameObject.get_transform().set_localPosition(this.calcRootNode.get_localPosition());
      gameObject.get_transform().set_rotation(Quaternion.get_identity());
      this.calcRootNodeRef = gameObject.get_transform();
    }
    foreach (EyeObject_Ver2 eyeObj in this.eyeObjs)
    {
      Quaternion rotation = eyeObj.eyeTransform.get_parent().get_rotation();
      eyeObj.parentFirstWorldRotation = rotation;
      eyeObj.parentFirstLocalRotation = eyeObj.eyeTransform.get_parent().get_localRotation();
      Quaternion quaternion2 = Quaternion.Inverse(rotation);
      Transform transform = this.calcRootNodeRef ?? this.calcRootNode;
      eyeObj.referenceLookDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion2, transform.get_rotation()), ((Vector3) ref this.headLookVector).get_normalized());
      eyeObj.referenceUpDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion2, transform.get_rotation()), ((Vector3) ref this.headUpVector).get_normalized());
      eyeObj.angleH = 0.0f;
      eyeObj.angleV = 0.0f;
      eyeObj.dirUp = eyeObj.referenceUpDir;
      eyeObj.origRotation = (Quaternion) null;
      eyeObj.origRotation = eyeObj.eyeTransform.get_localRotation();
      this.angleHRate = new float[2];
    }
    if (Object.op_Implicit((Object) this.objRootNode))
      this.objRootNode.set_rotation(quaternion1);
    this.initEnd = true;
  }

  private void EyeUpdateCalc(Vector3 _target, int _ptnNo)
  {
    if (!this.initEnd)
    {
      if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
        return;
      this.targetObj.SetActive(false);
    }
    else
    {
      this.nowPtnNo = _ptnNo;
      if (!EyeLookControllerVer2.isEnabled || (double) Time.get_deltaTime() == 0.0)
      {
        if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
          return;
        this.targetObj.SetActive(false);
      }
      else
      {
        EyeTypeState_Ver2 eyeTypeState = this.eyeTypeStates[_ptnNo];
        EYE_LOOK_TYPE_VER2 eyeLookTypeVeR2 = this.eyeTypeStates[_ptnNo].lookType;
        Transform calcRootNodeRef = this.calcRootNodeRef;
        if (eyeLookTypeVeR2 == EYE_LOOK_TYPE_VER2.NO_LOOK)
        {
          this.eyeObjs[0].eyeTransform.set_localRotation(this.fixAngle[0]);
          this.eyeObjs[1].eyeTransform.set_localRotation(this.fixAngle[1]);
          if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
            return;
          this.targetObj.SetActive(false);
        }
        else
        {
          Vector3 vector3_1 = calcRootNodeRef.InverseTransformPoint(_target);
          if ((double) ((Vector3) ref vector3_1).get_magnitude() < (double) this.eyeTypeStates[_ptnNo].nearDis)
          {
            vector3_1 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.eyeTypeStates[_ptnNo].nearDis);
            _target = calcRootNodeRef.TransformPoint(vector3_1);
          }
          Vector3 vector3_2;
          ((Vector3) ref vector3_2).\u002Ector((float) vector3_1.x, 0.0f, (float) vector3_1.z);
          float num1 = Vector3.Dot(vector3_2, Vector3.get_forward());
          float num2 = Vector3.Angle(vector3_2, Vector3.get_forward());
          ((Vector3) ref vector3_2).\u002Ector(0.0f, (float) vector3_1.y, (float) vector3_1.z);
          float num3 = Vector3.Dot(vector3_2, Vector3.get_forward());
          float num4 = Vector3.Angle(vector3_2, Vector3.get_forward());
          if ((double) num1 < 0.0 || (double) num3 < 0.0 || ((double) this.eyeTypeStates[_ptnNo].hAngleLimit < (double) num2 || (double) this.eyeTypeStates[_ptnNo].vAngleLimit < (double) num4))
            eyeLookTypeVeR2 = EYE_LOOK_TYPE_VER2.FORWARD;
          if (eyeLookTypeVeR2 == EYE_LOOK_TYPE_VER2.FORWARD)
            _target = Vector3.op_Addition(calcRootNodeRef.get_position(), Vector3.op_Multiply(this.originRootNode.get_forward(), this.eyeTypeStates[_ptnNo].frontTagDis));
          if (eyeLookTypeVeR2 == EYE_LOOK_TYPE_VER2.CONTROL || this.eyeTypeStates[_ptnNo].lookType == EYE_LOOK_TYPE_VER2.CONTROL)
          {
            if (Object.op_Inequality((Object) this.targetObj, (Object) null))
            {
              if (!this.targetObj.get_activeSelf())
                this.targetObj.SetActive(true);
              _target = Vector3.MoveTowards(((Component) calcRootNodeRef).get_transform().get_position(), this.targetObj.get_transform().get_position(), this.eyeTypeStates[_ptnNo].frontTagDis);
              this.targetObj.get_transform().set_position(Vector3.MoveTowards(((Component) calcRootNodeRef).get_transform().get_position(), _target, 0.5f));
            }
          }
          else if (Object.op_Inequality((Object) this.targetObj, (Object) null))
          {
            this.targetObj.get_transform().set_position(Vector3.MoveTowards(((Component) calcRootNodeRef).get_transform().get_position(), _target, 0.5f));
            if (this.targetObj.get_activeSelf())
              this.targetObj.SetActive(false);
          }
          float num5 = -1f;
          foreach (EyeObject_Ver2 eyeObj in this.eyeObjs)
          {
            eyeObj.eyeTransform.set_localRotation(eyeObj.origRotation);
            Quaternion rotation = eyeObj.eyeTransform.get_parent().get_rotation();
            Quaternion quaternion1 = Quaternion.Inverse(rotation);
            Vector3 vector3_3 = Vector3.op_Subtraction(_target, eyeObj.eyeTransform.get_position());
            Vector3 normalized = ((Vector3) ref vector3_3).get_normalized();
            Vector3 _dirB = Quaternion.op_Multiply(quaternion1, normalized);
            float num6 = this.AngleAroundAxis(eyeObj.referenceLookDir, _dirB, eyeObj.referenceUpDir);
            Vector3 _axis = Vector3.Cross(eyeObj.referenceUpDir, _dirB);
            float num7 = this.AngleAroundAxis(Vector3.op_Subtraction(_dirB, Vector3.Project(_dirB, eyeObj.referenceUpDir)), _dirB, _axis);
            float num8 = Mathf.Max(0.0f, Mathf.Abs(num6) - eyeTypeState.thresholdAngleDifference) * Mathf.Sign(num6);
            float num9 = Mathf.Max(0.0f, Mathf.Abs(num7) - eyeTypeState.thresholdAngleDifference) * Mathf.Sign(num7);
            float num10 = Mathf.Max(Mathf.Abs(num8) * Mathf.Abs(eyeTypeState.bendingMultiplier), Mathf.Abs(num6) - eyeTypeState.maxAngleDifference) * Mathf.Sign(num6) * Mathf.Sign(eyeTypeState.bendingMultiplier);
            float num11 = Mathf.Max(Mathf.Abs(num9) * Mathf.Abs(eyeTypeState.bendingMultiplier), Mathf.Abs(num7) - eyeTypeState.maxAngleDifference) * Mathf.Sign(num7) * Mathf.Sign(eyeTypeState.bendingMultiplier);
            float num12 = eyeObj.eyeLR != EYE_LR_VER2.EYE_L ? -eyeTypeState.inBendingAngle : eyeTypeState.outBendingAngle;
            float num13 = eyeObj.eyeLR != EYE_LR_VER2.EYE_L ? -eyeTypeState.outBendingAngle : eyeTypeState.inBendingAngle;
            float num14 = Mathf.Clamp(num10, num13, num12);
            float num15 = Mathf.Clamp(num11, eyeTypeState.upBendingAngle, eyeTypeState.downBendingAngle);
            Vector3 vector3_4 = Vector3.Cross(eyeObj.referenceUpDir, eyeObj.referenceLookDir);
            if (eyeLookTypeVeR2 == EYE_LOOK_TYPE_VER2.AWAY)
            {
              if ((double) num5 == -1.0)
              {
                float num16 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].outBendingAngle, -this.eyeTypeStates[this.nowPtnNo].inBendingAngle, eyeObj.angleH));
                float num17 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].outBendingAngle, -this.eyeTypeStates[this.nowPtnNo].inBendingAngle, num14));
                float num18 = num16 - num17;
                if ((double) Mathf.Abs(num18) < (double) this.awayRate)
                {
                  num5 = Mathf.InverseLerp(-1f, 1f, (double) num18 >= 0.0 ? (0.0 >= (double) num18 ? num17 + this.awayRate : ((double) this.awayRate >= (double) num17 ? num17 + this.awayRate : num17 - this.awayRate)) : ((double) num17 >= -(double) this.awayRate ? num17 - this.awayRate : num17 + this.awayRate));
                  num14 = Mathf.Lerp(-this.eyeTypeStates[this.nowPtnNo].outBendingAngle, -this.eyeTypeStates[this.nowPtnNo].inBendingAngle, num5);
                }
                else
                {
                  num5 = Mathf.InverseLerp(-1f, 1f, num16);
                  num14 = eyeObj.angleH;
                }
              }
              else
                num14 = Mathf.Lerp(-this.eyeTypeStates[this.nowPtnNo].outBendingAngle, -this.eyeTypeStates[this.nowPtnNo].inBendingAngle, num5);
              num15 = -num15;
            }
            eyeObj.angleH = Mathf.Lerp(eyeObj.angleH, num14, Time.get_deltaTime() * eyeTypeState.leapSpeed);
            eyeObj.angleV = Mathf.Lerp(eyeObj.angleV, num15, Time.get_deltaTime() * eyeTypeState.leapSpeed);
            _dirB = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(eyeObj.angleH, eyeObj.referenceUpDir), Quaternion.AngleAxis(eyeObj.angleV, vector3_4)), eyeObj.referenceLookDir);
            Vector3 referenceUpDir = eyeObj.referenceUpDir;
            Vector3.OrthoNormalize(ref _dirB, ref referenceUpDir);
            Vector3 vector3_5 = _dirB;
            eyeObj.dirUp = Vector3.Slerp(eyeObj.dirUp, referenceUpDir, Time.get_deltaTime());
            Vector3.OrthoNormalize(ref vector3_5, ref eyeObj.dirUp);
            Quaternion quaternion2 = Quaternion.op_Multiply(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(vector3_5, eyeObj.dirUp)), Quaternion.Inverse(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(eyeObj.referenceLookDir, eyeObj.referenceUpDir))));
            eyeObj.eyeTransform.set_rotation(Quaternion.op_Multiply(quaternion2, eyeObj.eyeTransform.get_rotation()));
          }
          this.targetPos = _target;
          this.fixAngle[0] = this.eyeObjs[0].eyeTransform.get_localRotation();
          this.fixAngle[1] = this.eyeObjs[1].eyeTransform.get_localRotation();
          this.AngleHRateCalc();
          this.angleVRate = this.AngleVRateCalc();
        }
      }
    }
  }

  public float AngleAroundAxis(Vector3 _dirA, Vector3 _dirB, Vector3 _axis)
  {
    _dirA = Vector3.op_Subtraction(_dirA, Vector3.Project(_dirA, _axis));
    _dirB = Vector3.op_Subtraction(_dirB, Vector3.Project(_dirB, _axis));
    return Vector3.Angle(_dirA, _dirB) * ((double) Vector3.Dot(_axis, Vector3.Cross(_dirA, _dirB)) >= 0.0 ? 1f : -1f);
  }

  public void SetEnable(bool _setFlag)
  {
    EyeLookControllerVer2.isEnabled = _setFlag;
  }

  private void AngleHRateCalc()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.eyeObjs[index] != null)
        this.angleHRate[index] = this.eyeObjs[index].eyeLR != EYE_LR_VER2.EYE_R ? Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(this.eyeTypeStates[this.nowPtnNo].inBendingAngle, this.eyeTypeStates[this.nowPtnNo].outBendingAngle, this.eyeObjs[index].angleH)) : Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].outBendingAngle, -this.eyeTypeStates[this.nowPtnNo].inBendingAngle, this.eyeObjs[index].angleH));
    }
  }

  private float AngleVRateCalc()
  {
    if (this.eyeObjs[0] == null)
      return 0.0f;
    EyeTypeState_Ver2 eyeTypeState = this.eyeTypeStates[this.nowPtnNo];
    return (double) eyeTypeState.downBendingAngle <= (double) eyeTypeState.upBendingAngle ? (0.0 <= (double) this.eyeObjs[0].angleV ? -Mathf.InverseLerp(0.0f, eyeTypeState.upBendingAngle, this.eyeObjs[0].angleV) : Mathf.InverseLerp(0.0f, eyeTypeState.downBendingAngle, this.eyeObjs[0].angleV)) : (0.0 <= (double) this.eyeObjs[0].angleV ? -Mathf.InverseLerp(0.0f, eyeTypeState.downBendingAngle, this.eyeObjs[0].angleV) : Mathf.InverseLerp(0.0f, eyeTypeState.upBendingAngle, this.eyeObjs[0].angleV));
  }

  public float GetAngleHRate(EYE_LR_VER2 _eyeLR)
  {
    return this.angleHRate[_eyeLR != EYE_LR_VER2.EYE_L ? 1 : 0];
  }

  public float GetAngleVRate()
  {
    return this.angleVRate;
  }

  public void SaveAngle(BinaryWriter _writer)
  {
    for (int index = 0; index < 2 && index < this.eyeObjs.Length; ++index)
    {
      this.fixAngle[index] = this.eyeObjs[index].eyeTransform.get_localRotation();
      _writer.Write((float) this.fixAngle[index].x);
      _writer.Write((float) this.fixAngle[index].y);
      _writer.Write((float) this.fixAngle[index].z);
      _writer.Write((float) this.fixAngle[index].w);
    }
  }

  public void LoadAngle(BinaryReader _reader)
  {
    for (int index = 0; index < 2 && index < this.eyeObjs.Length; ++index)
    {
      this.fixAngle[index] = new Quaternion(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
      this.eyeObjs[index].eyeTransform.set_localRotation(this.fixAngle[index]);
    }
  }
}

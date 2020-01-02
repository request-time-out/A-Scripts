// Decompiled with JetBrains decompiler
// Type: EyeLookCalc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

public class EyeLookCalc : MonoBehaviour
{
  public static bool isEnabled = true;
  public Transform rootNode;
  public EyeObject[] eyeObjs;
  public Vector3 headLookVector;
  public Vector3 headUpVector;
  public EyeTypeState[] eyeTypeStates;
  public float[] angleHRate;
  public float angleVRate;
  public float sorasiRate;
  public bool isDebugDraw;
  public int ptnDraw;
  public float drawLineLength;
  private int nowPtnNo;
  private bool initEnd;
  public GameObject targetObj;
  private Vector3 targetPos;
  public Quaternion[] fixAngle;

  public EyeLookCalc()
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
    if (!this.isDebugDraw)
      return;
    if (Object.op_Implicit((Object) this.rootNode))
    {
      Gizmos.set_color(new Color(1f, 1f, 1f, 0.8f));
      if (this.eyeTypeStates.Length > this.ptnDraw)
      {
        EyeTypeState eyeTypeState = this.eyeTypeStates[this.ptnDraw];
        Gizmos.set_color(new Color(0.0f, 1f, 1f, 0.8f));
        Vector3 vector3_1 = Vector3.op_Addition(this.rootNode.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.rootNode.get_position());
        Gizmos.DrawLine(this.rootNode.get_position(), vector3_1);
        Vector3 vector3_2 = Vector3.op_Addition(this.rootNode.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, -eyeTypeState.hAngleLimit, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.rootNode.get_position());
        Gizmos.DrawLine(this.rootNode.get_position(), vector3_2);
        Vector3 vector3_3 = Vector3.op_Addition(this.rootNode.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.rootNode.get_position());
        Gizmos.DrawLine(this.rootNode.get_position(), vector3_3);
        Vector3 vector3_4 = Vector3.op_Addition(this.rootNode.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(-eyeTypeState.vAngleLimit, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.rootNode.get_position());
        Gizmos.DrawLine(this.rootNode.get_position(), vector3_4);
        Gizmos.DrawLine(vector3_1, vector3_4);
        Gizmos.DrawLine(vector3_4, vector3_2);
        Gizmos.DrawLine(vector3_2, vector3_3);
        Gizmos.DrawLine(vector3_3, vector3_1);
        Gizmos.set_color(new Color(1f, 0.0f, 1f, 0.8f));
        for (int index = 0; index < this.eyeObjs.Length; ++index)
        {
          Vector3 vector3_5 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.minBendingAngle * (this.eyeObjs[index].eyeLR != EYE_LR.EYE_R ? -1f : 1f), 0.0f), Vector3.get_forward()), this.drawLineLength)), this.eyeObjs[index].eyeTransform.get_position());
          Gizmos.DrawLine(this.eyeObjs[index].eyeTransform.get_position(), vector3_5);
          Vector3 vector3_6 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(0.0f, eyeTypeState.maxBendingAngle * (this.eyeObjs[index].eyeLR != EYE_LR.EYE_R ? -1f : 1f), 0.0f), Vector3.get_forward()), this.drawLineLength)), this.eyeObjs[index].eyeTransform.get_position());
          Gizmos.DrawLine(this.eyeObjs[index].eyeTransform.get_position(), vector3_6);
          Vector3 vector3_7 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.upBendingAngle, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.eyeObjs[index].eyeTransform.get_position());
          Gizmos.DrawLine(this.eyeObjs[index].eyeTransform.get_position(), vector3_7);
          Vector3 vector3_8 = Vector3.op_Addition(this.eyeObjs[index].eyeTransform.TransformDirection(Vector3.op_Multiply(Quaternion.op_Multiply(Quaternion.Euler(eyeTypeState.downBendingAngle, 0.0f, 0.0f), Vector3.get_forward()), this.drawLineLength)), this.eyeObjs[index].eyeTransform.get_position());
          Gizmos.DrawLine(this.eyeObjs[index].eyeTransform.get_position(), vector3_8);
          Gizmos.DrawLine(vector3_5, vector3_8);
          Gizmos.DrawLine(vector3_8, vector3_6);
          Gizmos.DrawLine(vector3_6, vector3_7);
          Gizmos.DrawLine(vector3_7, vector3_5);
        }
      }
      Gizmos.set_color(new Color(1f, 1f, 0.0f, 0.8f));
      for (int index = 0; index < this.eyeObjs.Length; ++index)
        Gizmos.DrawLine(this.eyeObjs[index].eyeTransform.get_position(), Vector3.op_Addition(this.eyeObjs[index].eyeTransform.get_position(), Vector3.op_Multiply(this.eyeObjs[index].eyeTransform.get_forward(), this.drawLineLength)));
    }
    Gizmos.set_color(Color.get_white());
  }

  public void Init()
  {
    if (Object.op_Equality((Object) this.rootNode, (Object) null))
      this.rootNode = ((Component) this).get_transform();
    foreach (EyeObject eyeObj in this.eyeObjs)
    {
      if (!Object.op_Equality((Object) eyeObj.eyeTransform, (Object) null))
      {
        Quaternion quaternion = Quaternion.Inverse(eyeObj.eyeTransform.get_parent().get_rotation());
        eyeObj.referenceLookDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion, this.rootNode.get_rotation()), ((Vector3) ref this.headLookVector).get_normalized());
        eyeObj.referenceUpDir = Quaternion.op_Multiply(Quaternion.op_Multiply(quaternion, this.rootNode.get_rotation()), ((Vector3) ref this.headUpVector).get_normalized());
        eyeObj.angleH = 0.0f;
        eyeObj.angleV = 0.0f;
        eyeObj.dirUp = eyeObj.referenceUpDir;
        eyeObj.origRotation = (Quaternion) null;
        eyeObj.origRotation = eyeObj.eyeTransform.get_localRotation();
        this.angleHRate = new float[2];
      }
    }
    this.initEnd = true;
  }

  public void EyeUpdateCalc(Vector3 target, int ptnNo)
  {
    if (!this.initEnd)
    {
      if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
        return;
      this.targetObj.SetActive(false);
    }
    else
    {
      this.nowPtnNo = ptnNo;
      if (!EyeLookCalc.isEnabled)
      {
        if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
          return;
        this.targetObj.SetActive(false);
      }
      else if ((double) Time.get_deltaTime() == 0.0)
      {
        if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
          return;
        this.targetObj.SetActive(false);
      }
      else
      {
        EyeTypeState eyeTypeState = this.eyeTypeStates[ptnNo];
        EYE_LOOK_TYPE eyeLookType = this.eyeTypeStates[ptnNo].lookType;
        if (eyeLookType == EYE_LOOK_TYPE.NO_LOOK)
        {
          this.eyeObjs[0].eyeTransform.set_localRotation(this.fixAngle[0]);
          this.eyeObjs[1].eyeTransform.set_localRotation(this.fixAngle[1]);
          if (!Object.op_Inequality((Object) this.targetObj, (Object) null) || !this.targetObj.get_activeSelf())
            return;
          this.targetObj.SetActive(false);
        }
        else
        {
          Vector3 vector3_1 = this.rootNode.InverseTransformPoint(target);
          if ((double) ((Vector3) ref vector3_1).get_magnitude() < (double) this.eyeTypeStates[ptnNo].nearDis)
          {
            vector3_1 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.eyeTypeStates[ptnNo].nearDis);
            target = this.rootNode.TransformPoint(vector3_1);
          }
          Vector3 vector3_2;
          ((Vector3) ref vector3_2).\u002Ector((float) vector3_1.x, 0.0f, (float) vector3_1.z);
          float num1 = Vector3.Dot(vector3_2, Vector3.get_forward());
          float num2 = Vector3.Angle(vector3_2, Vector3.get_forward());
          ((Vector3) ref vector3_2).\u002Ector(0.0f, (float) vector3_1.y, (float) vector3_1.z);
          float num3 = Vector3.Dot(vector3_2, Vector3.get_forward());
          float num4 = Vector3.Angle(vector3_2, Vector3.get_forward());
          if ((double) num1 < 0.0 || (double) num3 < 0.0 || ((double) num2 > (double) this.eyeTypeStates[ptnNo].hAngleLimit || (double) num4 > (double) this.eyeTypeStates[ptnNo].vAngleLimit))
            eyeLookType = EYE_LOOK_TYPE.FORWARD;
          if (eyeLookType == EYE_LOOK_TYPE.FORWARD)
            target = Vector3.op_Addition(this.rootNode.get_position(), Vector3.op_Multiply(this.rootNode.get_forward(), this.eyeTypeStates[ptnNo].forntTagDis));
          if (eyeLookType == EYE_LOOK_TYPE.CONTROL || this.eyeTypeStates[ptnNo].lookType == EYE_LOOK_TYPE.CONTROL)
          {
            if (Object.op_Inequality((Object) this.targetObj, (Object) null))
            {
              if (!this.targetObj.get_activeSelf())
                this.targetObj.SetActive(true);
              target = Vector3.MoveTowards(((Component) this.rootNode).get_transform().get_position(), this.targetObj.get_transform().get_position(), this.eyeTypeStates[ptnNo].forntTagDis);
              this.targetObj.get_transform().set_position(Vector3.MoveTowards(((Component) this.rootNode).get_transform().get_position(), target, 0.5f));
            }
          }
          else if (Object.op_Inequality((Object) this.targetObj, (Object) null))
          {
            this.targetObj.get_transform().set_position(Vector3.MoveTowards(((Component) this.rootNode).get_transform().get_position(), target, 0.5f));
            if (this.targetObj.get_activeSelf())
              this.targetObj.SetActive(false);
          }
          float num5 = -1f;
          foreach (EyeObject eyeObj in this.eyeObjs)
          {
            eyeObj.eyeTransform.set_localRotation(eyeObj.origRotation);
            Quaternion rotation = eyeObj.eyeTransform.get_parent().get_rotation();
            Quaternion quaternion1 = Quaternion.Inverse(rotation);
            Vector3 vector3_3 = Vector3.op_Subtraction(target, eyeObj.eyeTransform.get_position());
            Vector3 normalized = ((Vector3) ref vector3_3).get_normalized();
            Vector3 dirB = Quaternion.op_Multiply(quaternion1, normalized);
            float num6 = EyeLookCalc.AngleAroundAxis(eyeObj.referenceLookDir, dirB, eyeObj.referenceUpDir);
            Vector3 axis = Vector3.Cross(eyeObj.referenceUpDir, dirB);
            float num7 = EyeLookCalc.AngleAroundAxis(Vector3.op_Subtraction(dirB, Vector3.Project(dirB, eyeObj.referenceUpDir)), dirB, axis);
            float num8 = Mathf.Max(0.0f, Mathf.Abs(num6) - eyeTypeState.thresholdAngleDifference) * Mathf.Sign(num6);
            float num9 = Mathf.Max(0.0f, Mathf.Abs(num7) - eyeTypeState.thresholdAngleDifference) * Mathf.Sign(num7);
            float num10 = Mathf.Max(Mathf.Abs(num8) * Mathf.Abs(eyeTypeState.bendingMultiplier), Mathf.Abs(num6) - eyeTypeState.maxAngleDifference) * Mathf.Sign(num6) * Mathf.Sign(eyeTypeState.bendingMultiplier);
            float num11 = Mathf.Max(Mathf.Abs(num9) * Mathf.Abs(eyeTypeState.bendingMultiplier), Mathf.Abs(num7) - eyeTypeState.maxAngleDifference) * Mathf.Sign(num7) * Mathf.Sign(eyeTypeState.bendingMultiplier);
            float num12 = eyeTypeState.maxBendingAngle;
            float num13 = eyeTypeState.minBendingAngle;
            if (eyeObj.eyeLR == EYE_LR.EYE_R)
            {
              num12 = -eyeTypeState.minBendingAngle;
              num13 = -eyeTypeState.maxBendingAngle;
            }
            float num14 = Mathf.Clamp(num10, num13, num12);
            float num15 = Mathf.Clamp(num11, eyeTypeState.upBendingAngle, eyeTypeState.downBendingAngle);
            Vector3 vector3_4 = Vector3.Cross(eyeObj.referenceUpDir, eyeObj.referenceLookDir);
            if (eyeLookType == EYE_LOOK_TYPE.AWAY)
            {
              if ((double) num5 == -1.0)
              {
                float num16 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, -this.eyeTypeStates[this.nowPtnNo].minBendingAngle, eyeObj.angleH));
                float num17 = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, -this.eyeTypeStates[this.nowPtnNo].minBendingAngle, num14));
                float num18 = num16 - num17;
                if ((double) Mathf.Abs(num18) < (double) this.sorasiRate)
                {
                  num5 = Mathf.InverseLerp(-1f, 1f, (double) num18 >= 0.0 ? ((double) num18 <= 0.0 ? num17 + this.sorasiRate : ((double) num17 <= (double) this.sorasiRate ? num17 + this.sorasiRate : num17 - this.sorasiRate)) : ((double) num17 >= -(double) this.sorasiRate ? num17 - this.sorasiRate : num17 + this.sorasiRate));
                  num14 = Mathf.Lerp(-this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, -this.eyeTypeStates[this.nowPtnNo].minBendingAngle, num5);
                }
                else
                {
                  num5 = Mathf.InverseLerp(-1f, 1f, num16);
                  num14 = eyeObj.angleH;
                }
              }
              else
                num14 = Mathf.Lerp(-this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, -this.eyeTypeStates[this.nowPtnNo].minBendingAngle, num5);
              num15 = -num15;
            }
            eyeObj.angleH = Mathf.Lerp(eyeObj.angleH, num14, Time.get_deltaTime() * eyeTypeState.leapSpeed);
            eyeObj.angleV = Mathf.Lerp(eyeObj.angleV, num15, Time.get_deltaTime() * eyeTypeState.leapSpeed);
            Vector3 vector3_5 = Quaternion.op_Multiply(Quaternion.op_Multiply(Quaternion.AngleAxis(eyeObj.angleH, eyeObj.referenceUpDir), Quaternion.AngleAxis(eyeObj.angleV, vector3_4)), eyeObj.referenceLookDir);
            Vector3 referenceUpDir = eyeObj.referenceUpDir;
            Vector3.OrthoNormalize(ref vector3_5, ref referenceUpDir);
            Vector3 vector3_6 = vector3_5;
            eyeObj.dirUp = Vector3.Slerp(eyeObj.dirUp, referenceUpDir, Time.get_deltaTime() * 5f);
            Vector3.OrthoNormalize(ref vector3_6, ref eyeObj.dirUp);
            Quaternion quaternion2 = Quaternion.op_Multiply(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(vector3_6, eyeObj.dirUp)), Quaternion.Inverse(Quaternion.op_Multiply(rotation, Quaternion.LookRotation(eyeObj.referenceLookDir, eyeObj.referenceUpDir))));
            eyeObj.eyeTransform.set_rotation(Quaternion.op_Multiply(quaternion2, eyeObj.eyeTransform.get_rotation()));
          }
          this.targetPos = target;
          this.fixAngle[0] = this.eyeObjs[0].eyeTransform.get_localRotation();
          this.fixAngle[1] = this.eyeObjs[1].eyeTransform.get_localRotation();
          this.AngleHRateCalc();
          this.angleVRate = this.AngleVRateCalc();
        }
      }
    }
  }

  public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
  {
    dirA = Vector3.op_Subtraction(dirA, Vector3.Project(dirA, axis));
    dirB = Vector3.op_Subtraction(dirB, Vector3.Project(dirB, axis));
    return Vector3.Angle(dirA, dirB) * ((double) Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0.0 ? 1f : -1f);
  }

  public void setEnable(bool setFlag)
  {
    EyeLookCalc.isEnabled = setFlag;
  }

  private void AngleHRateCalc()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.eyeObjs[index] != null)
        this.angleHRate[index] = this.eyeObjs[index].eyeLR != EYE_LR.EYE_R ? Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(this.eyeTypeStates[this.nowPtnNo].minBendingAngle, this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, this.eyeObjs[index].angleH)) : Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-this.eyeTypeStates[this.nowPtnNo].maxBendingAngle, -this.eyeTypeStates[this.nowPtnNo].minBendingAngle, this.eyeObjs[index].angleH));
    }
  }

  private float AngleVRateCalc()
  {
    if (this.eyeObjs[0] == null)
      return 0.0f;
    return (double) this.eyeTypeStates[this.nowPtnNo].downBendingAngle <= (double) this.eyeTypeStates[this.nowPtnNo].upBendingAngle ? (0.0 <= (double) this.eyeObjs[0].angleV ? -Mathf.InverseLerp(0.0f, this.eyeTypeStates[this.nowPtnNo].upBendingAngle, this.eyeObjs[0].angleV) : Mathf.InverseLerp(0.0f, this.eyeTypeStates[this.nowPtnNo].downBendingAngle, this.eyeObjs[0].angleV)) : (0.0 <= (double) this.eyeObjs[0].angleV ? -Mathf.InverseLerp(0.0f, this.eyeTypeStates[this.nowPtnNo].downBendingAngle, this.eyeObjs[0].angleV) : Mathf.InverseLerp(0.0f, this.eyeTypeStates[this.nowPtnNo].upBendingAngle, this.eyeObjs[0].angleV));
  }

  public float GetAngleHRate(EYE_LR eyeLR)
  {
    return eyeLR == EYE_LR.EYE_L ? this.angleHRate[0] : this.angleHRate[1];
  }

  public float GetAngleVRate()
  {
    return this.angleVRate;
  }

  public void SaveAngle(BinaryWriter writer)
  {
    this.fixAngle[0] = this.eyeObjs[0].eyeTransform.get_localRotation();
    this.fixAngle[1] = this.eyeObjs[1].eyeTransform.get_localRotation();
    writer.Write((float) this.fixAngle[0].x);
    writer.Write((float) this.fixAngle[0].y);
    writer.Write((float) this.fixAngle[0].z);
    writer.Write((float) this.fixAngle[0].w);
    writer.Write((float) this.fixAngle[1].x);
    writer.Write((float) this.fixAngle[1].y);
    writer.Write((float) this.fixAngle[1].z);
    writer.Write((float) this.fixAngle[1].w);
  }

  public void LoadAngle(BinaryReader reader)
  {
    this.fixAngle[0] = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    this.fixAngle[1] = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    this.eyeObjs[0].eyeTransform.set_localRotation(this.fixAngle[0]);
    this.eyeObjs[1].eyeTransform.set_localRotation(this.fixAngle[1]);
  }
}

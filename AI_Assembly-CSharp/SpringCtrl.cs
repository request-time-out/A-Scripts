// Decompiled with JetBrains decompiler
// Type: SpringCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SpringCtrl
{
  private List<SpringCtrl.CSpringPoint> m_listPoint = new List<SpringCtrl.CSpringPoint>();
  private List<SpringCtrl.CSpring> m_listSpring = new List<SpringCtrl.CSpring>();
  private float m_fGravity;
  private float m_fDrag;
  private float m_fMass;
  private float m_fTension;
  private float m_fShear;
  private float m_fAttenuation;
  private uint m_nPointNumW;
  private uint m_nPointNumH;
  private uint m_nNumPoint;
  private uint m_nNumSpring;
  private Vector3 m_vForce;
  private Vector3 m_vAutoForce;

  public SpringCtrl()
  {
    this.MemberInit();
  }

  public float setGravity
  {
    set
    {
      this.m_fGravity = value;
    }
  }

  public float setDrag
  {
    set
    {
      this.m_fDrag = value;
    }
  }

  public float setMass
  {
    set
    {
      this.m_fMass = value;
      uint num1 = this.m_nPointNumW * this.m_nPointNumH;
      float num2 = 0.0f;
      if (num1 != 0U)
        num2 = this.m_fMass / (float) num1;
      foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
      {
        cspringPoint.fMass = num2;
        cspringPoint.fInvMass = (double) cspringPoint.fMass == 0.0 ? 0.0f : 1f / cspringPoint.fMass;
      }
    }
  }

  public float setTension
  {
    set
    {
      this.m_fTension = value;
      foreach (SpringCtrl.CSpring cspring in this.m_listSpring)
      {
        if (!cspring.bShear)
          cspring.fConstant = this.m_fTension;
      }
    }
  }

  public float setShear
  {
    set
    {
      this.m_fShear = value;
      foreach (SpringCtrl.CSpring cspring in this.m_listSpring)
      {
        if (!cspring.bShear)
          cspring.fConstant = this.m_fShear;
      }
    }
  }

  public float setAttenuation
  {
    set
    {
      this.m_fAttenuation = value;
      foreach (SpringCtrl.CSpring cspring in this.m_listSpring)
        cspring.fAttenuation = this.m_fAttenuation;
    }
  }

  public List<SpringCtrl.CSpringPoint> listPoint
  {
    get
    {
      return this.m_listPoint;
    }
  }

  public Vector3 setForce
  {
    set
    {
      this.m_vForce = value;
    }
  }

  public Vector3 setAutoForce
  {
    set
    {
      this.m_vAutoForce = value;
    }
  }

  public bool Init(
    float _fMass,
    float _fTension,
    float _fShear,
    float _fAttenuation,
    float _fGravity,
    float _fDrag,
    uint _nPointNumW,
    uint _nPointNumH)
  {
    this.MemberInit();
    this.m_fGravity = _fGravity;
    this.m_fDrag = _fDrag;
    this.m_nPointNumW = _nPointNumW;
    this.m_nPointNumH = _nPointNumH;
    this.m_nNumPoint = this.m_nPointNumW * this.m_nPointNumH;
    if (this.m_nNumPoint == 0U)
    {
      this.MemberInit();
      return false;
    }
    for (int index = 0; (long) index < (long) this.m_nNumPoint; ++index)
      this.m_listPoint.Add(new SpringCtrl.CSpringPoint());
    uint[] numArray = new uint[4]
    {
      this.m_nPointNumH - 1U,
      (uint) (((int) this.m_nPointNumH - 1) * ((int) this.m_nPointNumW - 2)),
      this.m_nPointNumH - 1U,
      this.m_nPointNumW - 1U
    };
    this.m_nNumSpring = (uint) ((int) numArray[0] * 3 + (int) numArray[1] * 4 + (int) numArray[2] * 2) + numArray[3];
    if (this.m_nNumSpring == 0U)
    {
      this.MemberInit();
      return false;
    }
    for (int index = 0; (long) index < (long) this.m_nNumSpring; ++index)
      this.m_listSpring.Add(new SpringCtrl.CSpring());
    return this.SetParam(_fMass, _fTension, _fShear, _fAttenuation);
  }

  public bool UpdateEulerMethod(Transform _transfrom, float _ftime)
  {
    this.CalcForce(_transfrom);
    Vector3.get_zero();
    foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
    {
      cspringPoint.vAccel = Vector3.op_Multiply(cspringPoint.vAccel, cspringPoint.fInvMass);
      Vector3 vector3_1 = Vector3.op_Multiply(cspringPoint.vAccel, _ftime);
      cspringPoint.vVelocity = Vector3.op_Addition(cspringPoint.vVelocity, vector3_1);
      Vector3 vector3_2 = Vector3.op_Multiply(cspringPoint.vVelocity, _ftime);
      cspringPoint.vPos = Vector3.op_Addition(cspringPoint.vPos, vector3_2);
    }
    return true;
  }

  public bool ResetAllForce()
  {
    foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
    {
      cspringPoint.vVelocity = Vector3.get_zero();
      cspringPoint.vAccel = Vector3.get_zero();
      cspringPoint.vForce = Vector3.get_zero();
    }
    return true;
  }

  public void InitForce()
  {
    this.m_vAutoForce = Vector3.get_zero();
    foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
    {
      cspringPoint.vVelocity = Vector3.get_zero();
      cspringPoint.vAccel = Vector3.get_zero();
      cspringPoint.vForce = Vector3.get_zero();
    }
  }

  public bool SetParam(float _fMass, float _fTension, float _fShear, float _fAttenuation)
  {
    if (this.m_nPointNumW == 0U || this.m_nPointNumH == 0U)
      return false;
    this.m_fMass = _fMass;
    this.m_fTension = _fTension;
    this.m_fShear = _fShear;
    this.m_fAttenuation = _fAttenuation;
    int num1 = (int) this.m_nPointNumW - 1;
    int num2 = (int) this.m_nPointNumH - 1;
    float num3 = this.m_fMass / (float) (this.m_nPointNumW * this.m_nPointNumH);
    for (int index1 = 0; (long) index1 < (long) this.m_nPointNumH; ++index1)
    {
      for (int index2 = 0; (long) index2 < (long) this.m_nPointNumW; ++index2)
      {
        int index3 = index1 * (int) this.m_nPointNumH + index2;
        this.m_listPoint[index3].fMass = num3;
        this.m_listPoint[index3].fInvMass = (double) this.m_listPoint[index3].fMass == 0.0 ? 0.0f : 1f / this.m_listPoint[index3].fMass;
        this.m_listPoint[index3].vVelocity = Vector3.get_zero();
        this.m_listPoint[index3].vAccel = Vector3.get_zero();
        this.m_listPoint[index3].vForce = Vector3.get_zero();
        this.m_listPoint[index3].vPos = Vector3.get_zero();
      }
    }
    int[] numArray1 = new int[4]{ 1, 0, 1, -1 };
    int[] numArray2 = new int[4]{ 0, 1, 1, 1 };
    byte[] numArray3 = new byte[4];
    int index4 = 0;
    SpringCtrl.POINTS points1 = new SpringCtrl.POINTS();
    SpringCtrl.POINTS points2 = new SpringCtrl.POINTS();
    for (int index1 = 0; (long) index1 < (long) this.m_nPointNumH; ++index1)
    {
      for (int index2 = 0; (long) index2 < (long) this.m_nPointNumW; ++index2)
      {
        points1.x = (short) index2;
        points1.y = (short) index1;
        numArray3[0] = index2 >= num1 ? (byte) 0 : (byte) 1;
        numArray3[1] = index1 >= num2 ? (byte) 0 : (byte) 1;
        numArray3[2] = index1 >= num2 || index2 >= num1 ? (byte) 0 : (byte) 1;
        numArray3[3] = index2 <= 0 || index1 >= num2 ? (byte) 0 : (byte) 1;
        for (int index3 = 0; index3 < 4; ++index3)
        {
          if (numArray3[index3] != (byte) 0)
          {
            points2.x = (short) (index2 + numArray1[index3]);
            points2.y = (short) (index1 + numArray2[index3]);
            this.m_listSpring[index4].nIdx1 = (uint) ((ulong) points1.x + (ulong) points1.y * (ulong) this.m_nPointNumH);
            this.m_listSpring[index4].nIdx2 = (uint) ((ulong) points2.x + (ulong) points2.y * (ulong) this.m_nPointNumH);
            if (2 > index3)
            {
              this.m_listSpring[index4].bShear = false;
              this.m_listSpring[index4].fConstant = this.m_fTension;
            }
            else
            {
              this.m_listSpring[index4].bShear = true;
              this.m_listSpring[index4].fConstant = this.m_fShear;
            }
            this.m_listSpring[index4].fAttenuation = this.m_fAttenuation;
            uint nIdx1 = this.m_listSpring[index4].nIdx1;
            uint nIdx2 = this.m_listSpring[index4].nIdx2;
            this.m_listSpring[index4].fLength = Vector3.Distance(this.m_listPoint[(int) nIdx1].vPos, this.m_listPoint[(int) nIdx2].vPos);
            ++index4;
          }
        }
      }
    }
    return true;
  }

  private bool CalcForce(Transform _trans)
  {
    foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
      cspringPoint.vForce = Vector3.get_zero();
    foreach (SpringCtrl.CSpringPoint cspringPoint in this.m_listPoint)
    {
      if (!cspringPoint.bLock)
      {
        Vector3 vector3_1;
        ((Vector3) ref vector3_1).\u002Ector(0.0f, this.m_fGravity * cspringPoint.fMass, 0.0f);
        vector3_1 = _trans.InverseTransformDirection(vector3_1);
        cspringPoint.vForce = Vector3.op_Addition(cspringPoint.vForce, vector3_1);
        float magnitude = ((Vector3) ref cspringPoint.vVelocity).get_magnitude();
        Vector3 vector3_2 = Vector3.op_Multiply(Vector3.Normalize(Vector3.op_Multiply(cspringPoint.vVelocity, -1f)), magnitude * this.m_fDrag);
        cspringPoint.vForce = Vector3.op_Addition(cspringPoint.vForce, vector3_2);
      }
    }
    foreach (SpringCtrl.CSpring cspring in this.m_listSpring)
    {
      int nIdx1 = (int) cspring.nIdx1;
      int nIdx2 = (int) cspring.nIdx2;
      if (!this.m_listPoint[nIdx1].bLock || !this.m_listPoint[nIdx2].bLock)
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(this.m_listPoint[nIdx1].vPos, this.m_listPoint[nIdx2].vPos);
        float num1 = Vector3.Dot(Vector3.op_Subtraction(this.m_listPoint[nIdx1].vVelocity, this.m_listPoint[nIdx2].vVelocity), vector3_1);
        float magnitude = ((Vector3) ref vector3_1).get_magnitude();
        float num2 = (double) magnitude == 0.0 ? 0.0f : 1f / magnitude;
        float fLength = cspring.fLength;
        float fConstant = cspring.fConstant;
        float fAttenuation = cspring.fAttenuation;
        float num3 = (float) -((double) (fConstant * (magnitude - fLength)) + (double) (fAttenuation * num1 * num2));
        Vector3 vector3_2 = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), num3), this.m_vForce), this.m_vAutoForce);
        Vector3 vector3_3 = Vector3.op_Multiply(vector3_2, -1f);
        if (!this.m_listPoint[nIdx1].bLock)
          this.m_listPoint[nIdx1].vForce = Vector3.op_Addition(this.m_listPoint[nIdx1].vForce, vector3_2);
        if (!this.m_listPoint[nIdx2].bLock)
          this.m_listPoint[nIdx2].vForce = Vector3.op_Addition(this.m_listPoint[nIdx2].vForce, vector3_3);
      }
    }
    return true;
  }

  public void MemberInit()
  {
    this.m_fGravity = 0.0f;
    this.m_fDrag = 0.0f;
    this.m_fMass = 0.0f;
    this.m_fTension = 0.0f;
    this.m_fShear = 0.0f;
    this.m_fAttenuation = 0.0f;
    this.m_nPointNumW = 0U;
    this.m_nPointNumH = 0U;
    this.m_nNumPoint = 0U;
    this.m_listPoint.Clear();
    this.m_nNumSpring = 0U;
    this.m_listSpring.Clear();
    this.m_vForce = Vector3.get_zero();
    this.m_vAutoForce = Vector3.get_zero();
  }

  public class POINTS
  {
    public short x;
    public short y;

    public POINTS()
    {
      this.MemberInit();
    }

    public POINTS(short _x, short _y)
    {
      this.x = _x;
      this.y = _y;
    }

    public void MemberInit()
    {
      this.x = (short) 0;
      this.y = (short) 0;
    }
  }

  public class CSpringPoint
  {
    public bool bLock;
    public float fMass;
    public float fInvMass;
    public Vector3 vPos;
    public Vector3 vVelocity;
    public Vector3 vAccel;
    public Vector3 vForce;

    public void MemberInit()
    {
      this.bLock = false;
      this.fMass = 0.0f;
      this.fInvMass = 0.0f;
      this.vPos = Vector3.get_zero();
      this.vVelocity = Vector3.get_zero();
      this.vAccel = Vector3.get_zero();
      this.vForce = Vector3.get_zero();
    }
  }

  public class CSpring
  {
    public uint nIdx1;
    public uint nIdx2;
    public float fLength;
    public float fConstant;
    public float fAttenuation;
    public bool bShear;

    public void MemberInit()
    {
      this.nIdx1 = 0U;
      this.nIdx2 = 0U;
      this.fLength = 0.0f;
      this.fConstant = 0.0f;
      this.fAttenuation = 0.0f;
      this.bShear = false;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MorphFaceBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MorphAssist;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MorphFaceBase : MorphBase
{
  [Range(0.0f, 1f)]
  public float OpenMax = 1f;
  [Range(-0.1f, 1f)]
  public float FixedRate = -0.1f;
  private float correctOpenMax = -1f;
  protected int backPtn;
  [Range(0.0f, 255f)]
  public int NowPtn;
  [Range(0.0f, 1f)]
  protected float openRate;
  [Range(0.0f, 1f)]
  public float OpenMin;
  public bool BlendNormals;
  protected TimeProgressCtrl blendTimeCtrl;

  public bool Create(GameObject o)
  {
    if (!this.CreateCalcInfo(o))
      return false;
    this.blendTimeCtrl = new TimeProgressCtrl();
    return true;
  }

  public bool Init(List<MorphingTargetInfo> MorphTargetList)
  {
    this.ChangeRefTargetMesh(MorphTargetList);
    this.blendTimeCtrl = new TimeProgressCtrl();
    return true;
  }

  public void ChangePtn(int ptn, bool blend)
  {
    if (this.NowPtn == ptn)
      return;
    this.backPtn = this.NowPtn;
    this.NowPtn = ptn;
    if (!blend)
      this.blendTimeCtrl.End();
    else
      this.blendTimeCtrl.Start();
  }

  public void SetFixedRate(float value)
  {
    this.FixedRate = value;
  }

  public void SetCorrectOpenMax(float value)
  {
    this.correctOpenMax = value;
  }

  public void CalculateBlendVertex()
  {
    if (this.CalcInfo == null)
      return;
    float num1 = Mathf.Lerp(this.OpenMin, (double) this.correctOpenMax >= 0.0 ? this.correctOpenMax : this.OpenMax, this.openRate);
    if (0.0 <= (double) this.FixedRate)
      num1 = this.FixedRate;
    float num2 = 0.0f;
    if (this.blendTimeCtrl != null)
      num2 = this.blendTimeCtrl.Calculate();
    if ((double) num2 == 1.0)
    {
      foreach (MorphCalcInfo morphCalcInfo in this.CalcInfo)
      {
        if (!Object.op_Equality((Object) null, (Object) morphCalcInfo.TargetMesh) && this.NowPtn * 2 + 1 < morphCalcInfo.UpdateInfo.Length)
        {
          Vector3[] vertices = morphCalcInfo.TargetMesh.get_vertices();
          // ISSUE: object of a compiler-generated type is created
          foreach (\u003C\u003E__AnonType10<int, int> anonType10 in ((IEnumerable<int>) morphCalcInfo.UpdateIndex).Select<int, \u003C\u003E__AnonType10<int, int>>((Func<int, int, \u003C\u003E__AnonType10<int, int>>) ((value, index) => new \u003C\u003E__AnonType10<int, int>(value, index))))
            vertices[anonType10.value] = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.NowPtn * 2].Pos[anonType10.index], morphCalcInfo.UpdateInfo[this.NowPtn * 2 + 1].Pos[anonType10.index], num1);
          morphCalcInfo.TargetMesh.set_vertices(vertices);
          if (this.BlendNormals)
          {
            Vector3[] normals = morphCalcInfo.TargetMesh.get_normals();
            // ISSUE: object of a compiler-generated type is created
            foreach (\u003C\u003E__AnonType10<int, int> anonType10 in ((IEnumerable<int>) morphCalcInfo.UpdateIndex).Select<int, \u003C\u003E__AnonType10<int, int>>((Func<int, int, \u003C\u003E__AnonType10<int, int>>) ((value, index) => new \u003C\u003E__AnonType10<int, int>(value, index))))
              normals[anonType10.value] = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.NowPtn * 2].Normmal[anonType10.index], morphCalcInfo.UpdateInfo[this.NowPtn * 2 + 1].Normmal[anonType10.index], num1);
            morphCalcInfo.TargetMesh.set_normals(normals);
          }
        }
      }
    }
    else
    {
      foreach (MorphCalcInfo morphCalcInfo in this.CalcInfo)
      {
        if (!Object.op_Equality((Object) null, (Object) morphCalcInfo.TargetMesh) && this.NowPtn * 2 + 1 < morphCalcInfo.UpdateInfo.Length && this.backPtn * 2 + 1 < morphCalcInfo.UpdateInfo.Length)
        {
          Vector3[] vertices = morphCalcInfo.TargetMesh.get_vertices();
          // ISSUE: object of a compiler-generated type is created
          foreach (\u003C\u003E__AnonType10<int, int> anonType10 in ((IEnumerable<int>) morphCalcInfo.UpdateIndex).Select<int, \u003C\u003E__AnonType10<int, int>>((Func<int, int, \u003C\u003E__AnonType10<int, int>>) ((value, index) => new \u003C\u003E__AnonType10<int, int>(value, index))))
          {
            Vector3 vector3_1 = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.backPtn * 2].Pos[anonType10.index], morphCalcInfo.UpdateInfo[this.backPtn * 2 + 1].Pos[anonType10.index], num1);
            Vector3 vector3_2 = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.NowPtn * 2].Pos[anonType10.index], morphCalcInfo.UpdateInfo[this.NowPtn * 2 + 1].Pos[anonType10.index], num1);
            vertices[anonType10.value] = Vector3.Lerp(vector3_1, vector3_2, num2);
          }
          morphCalcInfo.TargetMesh.set_vertices(vertices);
          if (this.BlendNormals)
          {
            Vector3[] normals = morphCalcInfo.TargetMesh.get_normals();
            // ISSUE: object of a compiler-generated type is created
            foreach (\u003C\u003E__AnonType10<int, int> anonType10 in ((IEnumerable<int>) morphCalcInfo.UpdateIndex).Select<int, \u003C\u003E__AnonType10<int, int>>((Func<int, int, \u003C\u003E__AnonType10<int, int>>) ((value, index) => new \u003C\u003E__AnonType10<int, int>(value, index))))
            {
              Vector3 vector3_1 = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.backPtn * 2].Normmal[anonType10.index], morphCalcInfo.UpdateInfo[this.backPtn * 2 + 1].Normmal[anonType10.index], num1);
              Vector3 vector3_2 = Vector3.Lerp(morphCalcInfo.UpdateInfo[this.NowPtn * 2].Normmal[anonType10.index], morphCalcInfo.UpdateInfo[this.NowPtn * 2 + 1].Normmal[anonType10.index], num1);
              normals[anonType10.value] = Vector3.Lerp(vector3_1, vector3_2, num2);
            }
            morphCalcInfo.TargetMesh.set_normals(normals);
          }
        }
      }
    }
  }
}

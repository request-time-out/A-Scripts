// Decompiled with JetBrains decompiler
// Type: EliminateScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EliminateScale : MonoBehaviour
{
  [Tooltip("省きたいスケール軸")]
  public EliminateScale.EliminateScaleKind kind;
  public List<EliminateScale.ShapeMove> lstShape;
  [Header("Debug表示")]
  public Vector3 defScale;
  public Vector3 defPositon;
  public Quaternion defRotation;
  public ChaControl custom;

  public EliminateScale()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.defScale = ((Component) this).get_transform().get_lossyScale();
    this.defPositon = ((Component) this).get_transform().get_localPosition();
    this.defRotation = ((Component) this).get_transform().get_localRotation();
  }

  private void LateUpdate()
  {
    Vector3 lossyScale = ((Component) this).get_transform().get_lossyScale();
    Vector3 localScale = ((Component) this).get_transform().get_localScale();
    ((Component) this).get_transform().set_localScale(new Vector3(this.kind == EliminateScale.EliminateScaleKind.ALL || this.kind == EliminateScale.EliminateScaleKind.X || (this.kind == EliminateScale.EliminateScaleKind.XY || this.kind == EliminateScale.EliminateScaleKind.XZ) ? (float) (localScale.x / lossyScale.x * this.defScale.x) : (float) localScale.x, this.kind == EliminateScale.EliminateScaleKind.ALL || this.kind == EliminateScale.EliminateScaleKind.Y || (this.kind == EliminateScale.EliminateScaleKind.XY || this.kind == EliminateScale.EliminateScaleKind.YZ) ? (float) (localScale.y / lossyScale.y * this.defScale.y) : (float) localScale.y, this.kind == EliminateScale.EliminateScaleKind.ALL || this.kind == EliminateScale.EliminateScaleKind.Z || (this.kind == EliminateScale.EliminateScaleKind.XZ || this.kind == EliminateScale.EliminateScaleKind.YZ) ? (float) (localScale.z / lossyScale.z * this.defScale.z) : (float) localScale.z));
    if (!Object.op_Inequality((Object) this.custom, (Object) null))
      return;
    Vector3 vector3_1 = Vector3.get_zero();
    Vector3 vector3_2 = Vector3.get_zero();
    for (int index = 0; index < this.lstShape.Count; ++index)
    {
      float shapeBodyValue = this.custom.GetShapeBodyValue(Mathf.Max(this.lstShape[index].numShape, 0));
      vector3_1 = Vector3.op_Addition(vector3_1, (double) shapeBodyValue < 0.5 ? Vector3.Lerp(this.lstShape[index].posMin, this.lstShape[index].posMid, Mathf.InverseLerp(0.0f, 0.5f, shapeBodyValue)) : Vector3.Lerp(this.lstShape[index].posMid, this.lstShape[index].posMax, Mathf.InverseLerp(0.5f, 1f, shapeBodyValue)));
      vector3_2 = Vector3.op_Addition(vector3_2, (double) shapeBodyValue < 0.5 ? Vector3.Lerp(this.lstShape[index].rotMin, this.lstShape[index].rotMid, Mathf.InverseLerp(0.0f, 0.5f, shapeBodyValue)) : Vector3.Lerp(this.lstShape[index].rotMid, this.lstShape[index].rotMax, Mathf.InverseLerp(0.5f, 1f, shapeBodyValue)));
    }
    ((Component) this).get_transform().set_localPosition(Vector3.op_Addition(this.defPositon, vector3_1));
    ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(this.defRotation, Quaternion.Euler(vector3_2)));
  }

  public enum EliminateScaleKind
  {
    ALL,
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ,
    NONE,
  }

  [Serializable]
  public class ShapeMove
  {
    public int numShape;
    public Vector3 posMax;
    public Vector3 posMid;
    public Vector3 posMin;
    public Vector3 rotMax;
    public Vector3 rotMid;
    public Vector3 rotMin;
  }
}

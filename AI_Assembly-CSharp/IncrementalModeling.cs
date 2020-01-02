// Decompiled with JetBrains decompiler
// Type: IncrementalModeling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalModeling : ImplicitSurface
{
  public bool bSaveBrushHistory = true;
  [SerializeField]
  private List<IncrementalModeling.Brush> _brushHistory = new List<IncrementalModeling.Brush>();

  protected override void InitializePowerMap()
  {
    foreach (IncrementalModeling.Brush brush in this._brushHistory)
      brush.Draw(this);
  }

  [ContextMenu("Rebuild")]
  public void Rebuild()
  {
    this.ResetMaps();
    foreach (IncrementalModeling.Brush brush in this._brushHistory)
      brush.Draw(this);
    this.CreateMesh();
  }

  [ContextMenu("ClearHistory")]
  public void ClearHistory()
  {
    this._brushHistory.Clear();
  }

  public void AddSphere(
    Transform brushTransform,
    float radius,
    float powerScale,
    float fadeRadius)
  {
    IncrementalModeling.Brush brush = new IncrementalModeling.Brush(IncrementalModeling.Brush.Shape.sphere, Matrix4x4.op_Multiply(brushTransform.get_worldToLocalMatrix(), ((Component) this).get_transform().get_localToWorldMatrix()), fadeRadius, powerScale, radius, Vector3.get_one());
    brush.Draw(this);
    if (this.bSaveBrushHistory)
      this._brushHistory.Add(brush);
    this.CreateMesh();
  }

  public void AddBox(
    Transform brushTransform,
    Vector3 extents,
    float powerScale,
    float fadeRadius)
  {
    IncrementalModeling.Brush brush = new IncrementalModeling.Brush(IncrementalModeling.Brush.Shape.box, Matrix4x4.op_Multiply(brushTransform.get_worldToLocalMatrix(), ((Component) this).get_transform().get_localToWorldMatrix()), fadeRadius, powerScale, 1f, extents);
    brush.Draw(this);
    if (this.bSaveBrushHistory)
      this._brushHistory.Add(brush);
    this.CreateMesh();
  }

  [Serializable]
  public class Brush
  {
    public float fadeRadius = 0.1f;
    public float powerScale = 1f;
    public float sphereRadius = 0.5f;
    public Vector3 boxExtents = Vector3.op_Multiply(Vector3.get_one(), 0.5f);
    public Matrix4x4 invTransform;
    public IncrementalModeling.Brush.Shape shape;

    public Brush()
    {
    }

    public Brush(
      IncrementalModeling.Brush.Shape shape_,
      Matrix4x4 invTransformMtx_,
      float fadeRadius_,
      float powerScale_,
      float sphereRadius_,
      Vector3 boxExtents_)
    {
      this.shape = shape_;
      this.fadeRadius = fadeRadius_;
      this.powerScale = powerScale_;
      this.invTransform = invTransformMtx_;
      this.sphereRadius = sphereRadius_;
      this.boxExtents = boxExtents_;
    }

    public void Draw(IncrementalModeling model)
    {
      switch (this.shape)
      {
        case IncrementalModeling.Brush.Shape.sphere:
          this.DrawSphere(model);
          break;
        case IncrementalModeling.Brush.Shape.box:
          this.DrawBox(model);
          break;
      }
    }

    private void DrawSphere(IncrementalModeling model)
    {
      int num1 = model._countX * model._countY * model._countZ;
      for (int index = 0; index < num1; ++index)
      {
        Vector3 vector3 = ((Matrix4x4) ref this.invTransform).MultiplyPoint(model._positionMap[index]);
        float magnitude = ((Vector3) ref vector3).get_magnitude();
        if ((double) magnitude < (double) this.sphereRadius)
        {
          float num2 = 1f;
          if ((double) this.fadeRadius > 0.0)
            num2 = Mathf.Clamp01((this.sphereRadius - magnitude) / this.fadeRadius);
          model._powerMap[index] = Mathf.Clamp01(model._powerMap[index] + this.powerScale * num2);
          model._powerMap[index] *= model._powerMapMask[index];
        }
      }
    }

    private void DrawBox(IncrementalModeling model)
    {
      int num1 = model._countX * model._countY * model._countZ;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        float num2 = 1f;
        Vector3 vector3 = ((Matrix4x4) ref this.invTransform).MultiplyPoint(model._positionMap[index1]);
        for (int index2 = 0; index2 < 3; ++index2)
        {
          float num3 = Mathf.Abs(((Vector3) ref vector3).get_Item(index2));
          float num4 = ((Vector3) ref this.boxExtents).get_Item(index2);
          if ((double) num3 < (double) num4)
          {
            if ((double) this.fadeRadius > 0.0)
              num2 *= Mathf.Clamp01((num4 - num3) / this.fadeRadius);
          }
          else
          {
            num2 = 0.0f;
            break;
          }
        }
        if ((double) num2 > 0.0)
        {
          model._powerMap[index1] = Mathf.Clamp01(model._powerMap[index1] + this.powerScale * num2);
          model._powerMap[index1] *= model._powerMapMask[index1];
        }
      }
    }

    public enum Shape
    {
      sphere,
      box,
    }
  }
}

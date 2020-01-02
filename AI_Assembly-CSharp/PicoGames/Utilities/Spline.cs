// Decompiled with JetBrains decompiler
// Type: PicoGames.Utilities.Spline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PicoGames.Utilities
{
  [AddComponentMenu("PicoGames/Utilities/Spline")]
  [Serializable]
  public class Spline : MonoBehaviour
  {
    [SerializeField]
    public int outputResolution;
    [SerializeField]
    [HideInInspector]
    public bool hasChanged;
    [SerializeField]
    [HideInInspector]
    private List<Vector3> points;
    [SerializeField]
    [HideInInspector]
    private List<Spline.ControlPointMode> modes;
    [SerializeField]
    private bool isLooped;
    [SerializeField]
    private bool evenlyDistributPoints;
    [SerializeField]
    [HideInInspector]
    private float curveLength;

    public Spline()
    {
      base.\u002Ector();
    }

    public float CurveLength
    {
      get
      {
        if ((double) this.curveLength < 0.0)
          this.UpdateCurveLength(1000);
        return this.curveLength;
      }
    }

    public int ControlCount
    {
      get
      {
        return (this.points.Count - 1) / 3;
      }
    }

    public Vector3[] Points
    {
      get
      {
        return this.points.ToArray();
      }
    }

    public bool IsLooped
    {
      get
      {
        return this.isLooped;
      }
      set
      {
        if (value == this.isLooped)
          return;
        this.hasChanged = true;
        this.isLooped = value;
        if (!value)
          return;
        if (this.ControlCount < 2)
        {
          this.AddCurve(0, Spline.ControlPointMode.Mirrored);
          Vector3 vector3 = Vector3.op_Subtraction(this.GetPoint(1, (Space) 1), this.GetPoint(0, (Space) 1));
          this.SetControlPoint(1, Vector3.op_Subtraction(this.GetControlPoint(0, (Space) 1), Vector3.op_Multiply(new Vector3((float) vector3.y, (float) -vector3.x, 0.0f), 1.5f)), (Space) 1);
          this.SetPoint(4, Vector3.op_Subtraction(this.GetPoint(this.points.Count - 2, (Space) 1), Vector3.op_Multiply(new Vector3((float) vector3.y, (float) -vector3.x, 0.0f), 1.5f)), (Space) 1);
        }
        this.evenlyDistributPoints = true;
        this.modes[0] = this.modes[this.modes.Count - 1];
        this.SetPoint(this.points.Count - 1, this.points[0], (Space) 1);
      }
    }

    public bool EvenPointDistribution
    {
      get
      {
        return this.evenlyDistributPoints;
      }
      set
      {
        this.evenlyDistributPoints = this.isLooped || value;
      }
    }

    public void Reset()
    {
      this.points = new List<Vector3>((IEnumerable<Vector3>) new Vector3[4]
      {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, -1f, 0.0f),
        new Vector3(0.0f, -4f, 0.0f),
        new Vector3(0.0f, -5f, 0.0f)
      });
      this.modes = new List<Spline.ControlPointMode>((IEnumerable<Spline.ControlPointMode>) new Spline.ControlPointMode[2]
      {
        Spline.ControlPointMode.Mirrored,
        Spline.ControlPointMode.Mirrored
      });
      this.UpdateCurveLength(1000);
      this.hasChanged = true;
    }

    public void AddCurve(int _atIndex, Spline.ControlPointMode _defaultMode = Spline.ControlPointMode.Mirrored)
    {
      Vector3 point1 = this.points[_atIndex * 3];
      Vector3 point2 = this.points[(_atIndex + 1) * 3];
      Vector3 vector3_1 = Vector3.op_Multiply(Vector3.op_Addition(point1, point2), 0.5f);
      Vector3 vector3_2 = Vector3.op_Subtraction(point2, point1);
      Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
      this.points.InsertRange(_atIndex * 3 + 2, (IEnumerable<Vector3>) new Vector3[3]
      {
        Vector3.op_Subtraction(vector3_1, normalized),
        vector3_1,
        Vector3.op_Addition(vector3_1, normalized)
      });
      this.modes.Insert(_atIndex, _defaultMode);
      this.EnforceMode(_atIndex);
      if (this.isLooped)
      {
        this.points[this.points.Count - 1] = this.points[0];
        this.modes[this.modes.Count - 1] = this.modes[0];
        this.EnforceMode(0);
      }
      this.hasChanged = true;
    }

    public void RemoveCurve(int _index)
    {
      if (_index == 0 || _index == this.ControlCount)
        return;
      this.points.RemoveRange(_index * 3 - 1, 3);
      this.modes.RemoveAt(_index);
      this.hasChanged = true;
    }

    public Spline.ControlPointMode GetMode(int _index)
    {
      return this.modes[(_index + 1) / 3];
    }

    public void SetMode(int _index, Spline.ControlPointMode _mode)
    {
      int index = (_index + 1) / 3;
      this.modes[index] = _mode;
      if (this.isLooped)
      {
        if (index == 0)
          this.modes[this.modes.Count - 1] = _mode;
        else if (index == this.modes.Count - 1)
          this.modes[0] = _mode;
      }
      this.EnforceMode(_index);
      this.hasChanged = true;
    }

    private void EnforceMode(int _index)
    {
      int index1 = (_index + 1) / 3;
      Spline.ControlPointMode mode = this.modes[index1];
      if (mode == Spline.ControlPointMode.Free || !this.isLooped && (index1 == 0 || index1 == this.modes.Count - 1))
        return;
      int index2 = index1 * 3;
      int index3;
      int index4;
      if (_index <= index2)
      {
        index3 = index2 - 1;
        if (index3 < 0)
          index3 = this.points.Count - 2;
        index4 = index2 + 1;
        if (index4 >= this.points.Count - 2)
          index4 = 1;
      }
      else
      {
        index3 = index2 + 1;
        if (index3 >= this.points.Count)
          index3 = 1;
        index4 = index2 - 1;
        if (index4 < 0)
          index4 = this.points.Count - 2;
      }
      Vector3 point = this.points[index2];
      Vector3 vector3 = Vector3.op_Subtraction(point, this.points[index3]);
      if (mode == Spline.ControlPointMode.Aligned)
        vector3 = Vector3.op_Multiply(((Vector3) ref vector3).get_normalized(), Vector3.Distance(point, this.points[index4]));
      this.points[index4] = Vector3.op_Addition(point, vector3);
    }

    public Vector3 GetControlPoint(int _index, Space _space = 1)
    {
      return this.GetPoint(_index * 3, _space);
    }

    public void SetControlPoint(int _index, Vector3 _position, Space _space = 1)
    {
      this.SetPoint(_index * 3, _position, _space);
    }

    public void SetPoint(int _index, Vector3 _position, Space _space = 1)
    {
      if (_space == null)
        _position = ((Component) this).get_transform().InverseTransformPoint(_position);
      if (_index % 3 == 0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(_position, this.points[_index]);
        if (this.isLooped)
        {
          if (_index == 0)
          {
            List<Vector3> points1;
            (points1 = this.points)[1] = Vector3.op_Addition(points1[1], vector3);
            List<Vector3> points2;
            int index;
            (points2 = this.points)[index = this.points.Count - 2] = Vector3.op_Addition(points2[index], vector3);
            this.points[this.points.Count - 1] = _position;
          }
          else if (_index == this.points.Count - 1)
          {
            this.points[0] = _position;
            List<Vector3> points1;
            (points1 = this.points)[1] = Vector3.op_Addition(points1[1], vector3);
            List<Vector3> points2;
            int index;
            (points2 = this.points)[index = _index - 1] = Vector3.op_Addition(points2[index], vector3);
          }
          else
          {
            List<Vector3> points1;
            int index1;
            (points1 = this.points)[index1 = _index - 1] = Vector3.op_Addition(points1[index1], vector3);
            List<Vector3> points2;
            int index2;
            (points2 = this.points)[index2 = _index + 1] = Vector3.op_Addition(points2[index2], vector3);
          }
        }
        else
        {
          if (_index > 0)
          {
            List<Vector3> points;
            int index;
            (points = this.points)[index = _index - 1] = Vector3.op_Addition(points[index], vector3);
          }
          if (_index + 1 < this.points.Count)
          {
            List<Vector3> points;
            int index;
            (points = this.points)[index = _index + 1] = Vector3.op_Addition(points[index], vector3);
          }
        }
      }
      this.points[_index] = _position;
      this.EnforceMode(_index);
      this.UpdateCurveLength(1000);
      this.hasChanged = true;
    }

    public Vector3 GetPoint(int _index, Space _space = 1)
    {
      return _space == null ? ((Component) this).get_transform().TransformPoint(this.points[_index]) : this.points[_index];
    }

    public Vector3 GetPointOnCurve(float _t)
    {
      int index;
      if ((double) _t >= 1.0)
      {
        _t = 1f;
        index = this.points.Count - 4;
      }
      else
      {
        _t = Mathf.Clamp01(_t) * (float) ((this.points.Count - 1) / 3);
        int num = (int) _t;
        _t -= (float) num;
        index = num * 3;
      }
      return Spline.GetBezierPoint(this.points[index], this.points[index + 1], this.points[index + 2], this.points[index + 3], _t);
    }

    public SplinePoint[] GetSpacedPointsReversed(float _spacing)
    {
      List<SplinePoint> splinePointList = new List<SplinePoint>();
      Vector3 _position = this.GetPointOnCurve(1f);
      float num1 = _spacing * _spacing;
      float num2 = 1f / (float) this.outputResolution;
      splinePointList.Add(new SplinePoint(_position, Quaternion.get_identity()));
      for (float _t = 1f; (double) _t >= 0.0; _t -= num2)
      {
        Vector3 pointOnCurve = this.GetPointOnCurve(_t);
        if ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(pointOnCurve, _position)) >= (double) num1)
        {
          _position = pointOnCurve;
          splinePointList.Add(new SplinePoint(_position, Quaternion.get_identity()));
        }
      }
      if (splinePointList.Count <= 1)
        return new SplinePoint[0];
      Vector3 vector3_1 = this.GetPointOnCurve(0.0f);
      float num3 = Vector3.Distance(vector3_1, splinePointList[splinePointList.Count - 1].position) / (float) splinePointList.Count;
      for (int index = splinePointList.Count - 1; index >= 0; --index)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, splinePointList[index].position);
        Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
        if (this.evenlyDistributPoints)
        {
          SplinePoint splinePoint = splinePointList[index];
          splinePoint.position = Vector3.op_Addition(splinePoint.position, Vector3.op_Multiply(num3 * (float) index, normalized));
        }
        splinePointList[index].rotation = Quaternion.FromToRotation(Vector3.get_up(), normalized);
        vector3_1 = splinePointList[index].position;
      }
      return splinePointList.ToArray();
    }

    private void UpdateCurveLength(int _resolution = 1000)
    {
      float num = 1f / (float) _resolution;
      Vector3 vector3 = this.GetPointOnCurve(0.0f);
      this.curveLength = 0.0f;
      for (int index = 1; index <= _resolution; ++index)
      {
        Vector3 pointOnCurve = this.GetPointOnCurve((float) index * num);
        this.curveLength += Vector3.Distance(vector3, pointOnCurve);
        vector3 = pointOnCurve;
      }
    }

    public static Vector3 GetBezierPoint(
      Vector3 _p0,
      Vector3 _p1,
      Vector3 _p2,
      Vector3 _p3,
      float _t)
    {
      _t = Mathf.Clamp01(_t);
      float num = 1f - _t;
      return Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(num * num * num, _p0), Vector3.op_Multiply(3f * num * num * _t, _p1)), Vector3.op_Multiply(3f * num * _t * _t, _p2)), Vector3.op_Multiply(_t * _t * _t, _p3));
    }

    public enum ControlPointMode
    {
      Free,
      Aligned,
      Mirrored,
    }
  }
}

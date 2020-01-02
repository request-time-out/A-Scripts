// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.LocomotionArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  public class LocomotionArea
  {
    protected static RaycastHit[] _raycastHits = new RaycastHit[3];
    public List<Waypoint> points = new List<Waypoint>();
    private List<Waypoint> activePoints = new List<Waypoint>();
    public const LocomotionArea.AreaType NormalAreaType = LocomotionArea.AreaType.Normal | LocomotionArea.AreaType.Indoor;

    public int Count
    {
      get
      {
        return this.points.Count;
      }
    }

    public bool Empty
    {
      get
      {
        return ((IReadOnlyList<Waypoint>) this.points).IsNullOrEmpty<Waypoint>();
      }
    }

    public bool ActiveEmpty
    {
      get
      {
        return ((IReadOnlyList<Waypoint>) this.activePoints).IsNullOrEmpty<Waypoint>();
      }
    }

    public bool Active { get; private set; }

    public bool Complete { get; private set; }

    public void SetWaypoint(List<Waypoint> _points)
    {
      this.Clear();
      if (((IReadOnlyList<Waypoint>) _points).IsNullOrEmpty<Waypoint>())
        return;
      foreach (Waypoint point in _points)
      {
        if (!Object.op_Equality((Object) point, (Object) null))
          this.points.Add(point);
      }
      this.Complete = true;
      this.Active = !this.Empty;
    }

    public void Clear()
    {
      bool flag = false;
      this.Active = flag;
      this.Complete = flag;
      this.points.Clear();
      this.activePoints.Clear();
    }

    ~LocomotionArea()
    {
      this.Clear();
    }

    public bool ActivePoint(Waypoint _point)
    {
      return Object.op_Inequality((Object) _point, (Object) null) && ((Component) _point).get_gameObject().get_activeSelf() && _point.Reserver == null && Object.op_Inequality((Object) _point.OwnerArea, (Object) null);
    }

    public bool ActivePoint(Waypoint _point, LocomotionArea.AreaType _type)
    {
      if (!this.ActivePoint(_point))
        return false;
      MapArea.AreaType areaType = _point.AreaType;
      return ((((((false ? 1 : 0) | ((_type & LocomotionArea.AreaType.Normal) == (LocomotionArea.AreaType) 0 ? 0 : (areaType == MapArea.AreaType.Normal ? 1 : 0))) != 0 ? 1 : 0) | ((_type & LocomotionArea.AreaType.Indoor) == (LocomotionArea.AreaType) 0 ? 0 : (areaType == MapArea.AreaType.Indoor ? 1 : 0))) != 0 ? 1 : 0) | ((_type & LocomotionArea.AreaType.Private) == (LocomotionArea.AreaType) 0 ? 0 : (areaType == MapArea.AreaType.Private ? 1 : 0))) != 0;
    }

    public List<Waypoint> GetPointList(LocomotionArea.AreaType _areaType = LocomotionArea.AreaType.Normal | LocomotionArea.AreaType.Indoor)
    {
      this.SetActivePoints(_areaType);
      return this.activePoints;
    }

    public List<Waypoint> GetPointList(
      Vector3 _position,
      float _distance,
      LocomotionArea.AreaType _areaType = LocomotionArea.AreaType.Normal | LocomotionArea.AreaType.Indoor)
    {
      this.SetActivePoints(_position, _distance, _areaType);
      return this.activePoints;
    }

    public List<Waypoint> GetPointList(
      Vector3 _position,
      float _minDistance,
      float _maxDistance)
    {
      this.SetActivePoints(_position, _minDistance, _maxDistance);
      return this.activePoints;
    }

    public List<Waypoint> GetPointList(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      LocomotionArea.AreaType _areaType)
    {
      this.SetActivePoints(_position, _minDistance, _maxDistance, _areaType);
      return this.activePoints;
    }

    public List<Waypoint> GetPointList(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      MapArea _mapArea)
    {
      this.SetActivePoints(_position, _minDistance, _maxDistance, _mapArea);
      return this.activePoints;
    }

    public List<Waypoint> GetPointList(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      MapArea _mapArae,
      LocomotionArea.AreaType _areaType)
    {
      this.SetActivePoints(_position, _minDistance, _maxDistance, _mapArae, _areaType);
      return this.activePoints;
    }

    public List<Waypoint> GetRandomPointList(
      Vector3 _myPoint,
      Vector3 _targetPoint,
      float _createDistance,
      Vector3 _forward,
      float _angle,
      LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      if (this.Empty)
        return this.activePoints;
      float num1 = _createDistance * _createDistance;
      for (int index = 0; index < this.Count; ++index)
      {
        Waypoint point = this.points[index];
        if (this.ActivePoint(point, _areaType))
        {
          Vector3 position = ((Component) point).get_transform().get_position();
          Vector3 vector3_1 = Vector3.op_Subtraction(_myPoint, position);
          if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() > (double) num1)
          {
            Vector3 vector3_2 = Vector3.op_Subtraction(_targetPoint, position);
            if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() > (double) num1)
            {
              Vector2 vector2_1;
              ((Vector2) ref vector2_1).\u002Ector((float) _forward.x, (float) _forward.z);
              Vector2 normalized1 = ((Vector2) ref vector2_1).get_normalized();
              Vector2 vector2_2;
              ((Vector2) ref vector2_2).\u002Ector((float) (position.x - _targetPoint.x), (float) (position.z - _targetPoint.z));
              Vector2 normalized2 = ((Vector2) ref vector2_2).get_normalized();
              float num2 = Mathf.Acos(Mathf.Clamp(Vector2.Dot(normalized1, normalized2), -1f, 1f)) * 57.29578f;
              if ((double) _angle >= (double) num2 * 2.0)
                this.activePoints.Add(point);
            }
          }
        }
      }
      if (this.ActiveEmpty)
      {
        this.SetActivePoints(_myPoint, _targetPoint, _createDistance, _areaType);
        if (this.ActiveEmpty)
          this.SetActivePoints(_areaType);
      }
      return this.activePoints;
    }

    public bool GetRandomPoint(
      Vector3 _myPoint,
      Vector3 _targetPoint,
      float _createDistance,
      Vector3 _forward,
      float _angle,
      ref Waypoint _nextPoint,
      LocomotionArea.AreaType _areaType = LocomotionArea.AreaType.Normal | LocomotionArea.AreaType.Indoor)
    {
      this.GetRandomPointList(_myPoint, _targetPoint, _createDistance, _forward, _angle, _areaType);
      if (this.ActiveEmpty)
        return false;
      _nextPoint = this.activePoints.Rand<Waypoint>();
      return true;
    }

    private void SetActivePoints(LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      foreach (Waypoint point in this.points)
      {
        if (this.ActivePoint(point, _areaType))
          this.activePoints.Add(point);
      }
    }

    private void SetActivePoints(MapArea _mapArea, LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      if (Object.op_Equality((Object) _mapArea, (Object) null))
        return;
      foreach (Waypoint waypoint in _mapArea.Waypoints)
      {
        if (this.ActivePoint(waypoint, _areaType))
          this.activePoints.Add(waypoint);
      }
    }

    private void SetActivePoints(Vector3 _position, float _minDistance, float _maxDistance)
    {
      this.activePoints.Clear();
      _minDistance *= _minDistance;
      _maxDistance *= _maxDistance;
      foreach (Waypoint point in this.points)
      {
        if (this.ActivePoint(point))
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) point).get_transform().get_position(), _position);
          float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
          if ((double) _minDistance <= (double) sqrMagnitude && (double) sqrMagnitude <= (double) _maxDistance)
            this.activePoints.Add(point);
        }
      }
    }

    private void SetActivePoints(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      MapArea _mapArea)
    {
      this.activePoints.Clear();
      if (Object.op_Equality((Object) _mapArea, (Object) null))
        return;
      _minDistance *= _minDistance;
      _maxDistance *= _maxDistance;
      foreach (Waypoint waypoint in _mapArea.Waypoints)
      {
        if (this.ActivePoint(waypoint))
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) waypoint).get_transform().get_position(), _position);
          float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
          if ((double) _minDistance <= (double) sqrMagnitude && (double) sqrMagnitude <= (double) _maxDistance)
            this.activePoints.Add(waypoint);
        }
      }
    }

    private void SetActivePoints(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      MapArea _mapArea,
      LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      if (Object.op_Equality((Object) _mapArea, (Object) null))
        return;
      _minDistance *= _minDistance;
      _maxDistance *= _maxDistance;
      foreach (Waypoint waypoint in _mapArea.Waypoints)
      {
        if (this.ActivePoint(waypoint, _areaType))
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) waypoint).get_transform().get_position(), _position);
          float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
          if ((double) _minDistance <= (double) sqrMagnitude && (double) sqrMagnitude <= (double) _maxDistance)
            this.activePoints.Add(waypoint);
        }
      }
    }

    private void SetActivePoints(
      Vector3 _position,
      float _distance,
      LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      _distance *= _distance;
      foreach (Waypoint point in this.points)
      {
        if (this.ActivePoint(point, _areaType))
        {
          double num = (double) _distance;
          Vector3 vector3 = Vector3.op_Subtraction(((Component) point).get_transform().get_position(), _position);
          double sqrMagnitude = (double) ((Vector3) ref vector3).get_sqrMagnitude();
          if (num <= sqrMagnitude)
            this.activePoints.Add(point);
        }
      }
    }

    private void SetActivePoints(
      Vector3 _position,
      float _minDistance,
      float _maxDistance,
      LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      _minDistance *= _minDistance;
      _maxDistance *= _maxDistance;
      foreach (Waypoint point in this.points)
      {
        if (this.ActivePoint(point, _areaType))
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) point).get_transform().get_position(), _position);
          float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
          if ((double) _minDistance <= (double) sqrMagnitude && (double) sqrMagnitude <= (double) _maxDistance)
            this.activePoints.Add(point);
        }
      }
    }

    private void SetActivePoints(
      Vector3 _myPoint,
      Vector3 _targetPoint,
      float _distance,
      LocomotionArea.AreaType _areaType)
    {
      this.activePoints.Clear();
      _distance *= _distance;
      foreach (Waypoint point in this.points)
      {
        if (this.ActivePoint(point, _areaType))
        {
          Vector3 position = ((Component) point).get_transform().get_position();
          Vector3 vector3_1 = Vector3.op_Subtraction(position, _myPoint);
          float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
          Vector3 vector3_2 = Vector3.op_Subtraction(position, _targetPoint);
          float sqrMagnitude2 = ((Vector3) ref vector3_2).get_sqrMagnitude();
          if ((double) _distance <= (double) sqrMagnitude1 && (double) _distance <= (double) sqrMagnitude2)
            this.activePoints.Add(point);
        }
      }
    }

    [Flags]
    public enum AreaType
    {
      Normal = 1,
      Indoor = 2,
      Private = 4,
      All = Private | Indoor | Normal, // 0x00000007
    }
  }
}

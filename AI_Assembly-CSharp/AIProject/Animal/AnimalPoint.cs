// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class AnimalPoint : Point
  {
    private Color _pink = new Color(1f, 0.0f, 1f);
    [SerializeField]
    [Tooltip("このポイントのID")]
    protected int _id;
    [SerializeField]
    private LocateTypes _locateType;

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public void SetID(int id)
    {
      this._id = id;
    }

    public LocateTypes LocateType
    {
      get
      {
        return this._locateType;
      }
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
      set
      {
        ((Component) this).get_transform().set_position(value);
      }
    }

    public Vector3 EulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_eulerAngles();
      }
      set
      {
        ((Component) this).get_transform().set_eulerAngles(value);
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this).get_transform().get_rotation();
      }
      set
      {
        ((Component) this).get_transform().set_rotation(value);
      }
    }

    public Vector3 LocalPosition
    {
      get
      {
        return ((Component) this).get_transform().get_localPosition();
      }
      set
      {
        ((Component) this).get_transform().set_localPosition(value);
      }
    }

    public Vector3 LocalEulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_localEulerAngles();
      }
      set
      {
        ((Component) this).get_transform().set_localEulerAngles(value);
      }
    }

    public Quaternion LocalRotation
    {
      get
      {
        return ((Component) this).get_transform().get_localRotation();
      }
      set
      {
        ((Component) this).get_transform().set_localRotation(value);
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this).get_transform().get_forward();
      }
    }

    public Vector3 Up
    {
      get
      {
        return ((Component) this).get_transform().get_up();
      }
    }

    public Vector3 Right
    {
      get
      {
        return ((Component) this).get_transform().get_right();
      }
    }

    public Color Pink
    {
      get
      {
        return this._pink;
      }
    }

    public virtual bool Available
    {
      get
      {
        return true;
      }
    }

    public virtual void LoadObject()
    {
    }

    protected float DistanceXZ(Vector3 _p1, Vector3 _p2)
    {
      _p1.y = _p2.y;
      return Vector3.Distance(_p1, _p2);
    }

    protected float DistanceY(Vector3 _p1, Vector3 _p2)
    {
      return Mathf.Abs((float) (_p2.y - _p1.y));
    }

    public static void RelocationOnCollider(Transform t, float heightOnRaycast)
    {
      Vector3 vector3 = Vector3.op_Addition(t.get_position(), Vector3.op_Multiply(Vector3.get_up(), heightOnRaycast));
      Collider[] componentsInChildren = (Collider[]) ((Component) t).GetComponentsInChildren<Collider>();
      Ray ray;
      ((Ray) ref ray).\u002Ector(vector3, Vector3.get_down());
      int num = Mathf.Min(Physics.RaycastNonAlloc(ray, Point._raycastHits, 100f, LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.MapLayer)), Point._raycastHits.Length);
      if (0 >= num)
        return;
      RaycastHit raycastHit1 = (RaycastHit) null;
      for (int index = 0; index < num; ++index)
      {
        RaycastHit raycastHit2 = Point._raycastHits[index];
        bool flag = false;
        foreach (Collider collider in componentsInChildren)
        {
          flag |= Object.op_Equality((Object) ((Component) collider).get_gameObject(), (Object) ((Component) ((RaycastHit) ref raycastHit2).get_collider()).get_gameObject());
          if (flag)
            break;
        }
        if (!flag)
        {
          raycastHit1 = raycastHit2;
          break;
        }
      }
      if (Object.op_Equality((Object) ((RaycastHit) ref raycastHit1).get_collider(), (Object) null))
        return;
      t.set_position(((RaycastHit) ref raycastHit1).get_point());
    }

    public static void RelocationOnCollider(
      Transform t,
      float heightOnRaycast,
      LayerMask layerMask)
    {
      Vector3 vector3 = Vector3.op_Addition(t.get_position(), Vector3.op_Multiply(Vector3.get_up(), heightOnRaycast));
      Collider[] componentsInChildren = (Collider[]) ((Component) t).GetComponentsInChildren<Collider>();
      Ray ray;
      ((Ray) ref ray).\u002Ector(vector3, Vector3.get_down());
      int num = Mathf.Min(Physics.RaycastNonAlloc(ray, Point._raycastHits, 100f, LayerMask.op_Implicit(layerMask)), Point._raycastHits.Length);
      if (0 >= num)
        return;
      RaycastHit raycastHit1 = (RaycastHit) null;
      for (int index = 0; index < num; ++index)
      {
        RaycastHit raycastHit2 = Point._raycastHits[index];
        bool flag = false;
        foreach (Collider collider in componentsInChildren)
        {
          flag |= Object.op_Equality((Object) ((Component) collider).get_gameObject(), (Object) ((Component) ((RaycastHit) ref raycastHit2).get_collider()).get_gameObject());
          if (flag)
            break;
        }
        if (!flag)
        {
          raycastHit1 = raycastHit2;
          break;
        }
      }
      if (Object.op_Equality((Object) ((RaycastHit) ref raycastHit1).get_collider(), (Object) null))
        return;
      t.set_position(((RaycastHit) ref raycastHit1).get_point());
    }

    public static void RelocationOnNavMesh(Transform t, float searchDistance)
    {
      NavMeshHit navMeshHit;
      if (!NavMesh.SamplePosition(t.get_position(), ref navMeshHit, searchDistance, -1))
        return;
      t.set_position(((NavMeshHit) ref navMeshHit).get_position());
    }

    [Serializable]
    public class LocateInfo
    {
      [SerializeField]
      private float _raycastUpOffset = 3f;
      [SerializeField]
      private LayerMask _checkLayer = LayerMask.op_Implicit(0);
      [SerializeField]
      private float _checkNavMeshDistance = 10f;
      [SerializeField]
      private List<Transform> _colliderTarget = new List<Transform>();
      [SerializeField]
      private List<Transform> _navMeshTarget = new List<Transform>();
      private const int ButtonHeight = 20;
      private const int FontSize = 15;

      [Button("コライダーの高さに合わせる", ButtonHeight = 20)]
      public void LocateCollider()
      {
        if (((IReadOnlyList<Transform>) this._colliderTarget).IsNullOrEmpty<Transform>())
          return;
        using (List<Transform>.Enumerator enumerator = this._colliderTarget.GetEnumerator())
        {
          while (enumerator.MoveNext())
            AnimalPoint.RelocationOnCollider(enumerator.Current, this._raycastUpOffset, this._checkLayer);
        }
      }

      [Button("一番近いNavMeshの位置に合わせる", ButtonHeight = 20)]
      public void LocateNavMesh()
      {
        if (((IReadOnlyList<Transform>) this._navMeshTarget).IsNullOrEmpty<Transform>())
          return;
        using (List<Transform>.Enumerator enumerator = this._navMeshTarget.GetEnumerator())
        {
          while (enumerator.MoveNext())
            AnimalPoint.RelocationOnNavMesh(enumerator.Current, this._checkNavMeshDistance);
        }
      }

      [Button("両方合わせる", ButtonHeight = 20)]
      public void LocateAll()
      {
        this.LocateCollider();
        this.LocateNavMesh();
      }
    }
  }
}

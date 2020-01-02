// Decompiled with JetBrains decompiler
// Type: AIProject.Point
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public abstract class Point : SerializedMonoBehaviour
  {
    protected static RaycastHit[] _raycastHits = new RaycastHit[3];
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private MapArea _ownerArea;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private MapArea.AreaType _areaType;
    protected bool _initialized;

    protected Point()
    {
      base.\u002Ector();
    }

    public virtual int RegisterID { get; set; }

    public MapArea OwnerArea
    {
      get
      {
        return this._ownerArea;
      }
      set
      {
        this._ownerArea = value;
      }
    }

    public MapArea.AreaType AreaType
    {
      get
      {
        return this._areaType;
      }
      set
      {
        this._areaType = value;
      }
    }

    public bool Initialized
    {
      get
      {
        return this._initialized;
      }
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void LocateGround()
    {
      Point.LocateGround(((Component) this).get_transform());
    }

    public virtual void RefreshExistence()
    {
      NavMeshHit navMeshHit;
      if (NavMesh.FindClosestEdge(((Component) this).get_transform().get_position(), ref navMeshHit, -1))
        ((Component) this).get_gameObject().SetActive((double) ((NavMeshHit) ref navMeshHit).get_distance() >= (double) Singleton<Resources>.Instance.LocomotionProfile.PointDistanceMargin);
      else
        ((Component) this).get_gameObject().SetActive(false);
    }

    protected static void LocateGround(Transform t)
    {
      Vector3 vector3 = Vector3.op_Addition(t.get_position(), Vector3.op_Multiply(Vector3.get_up(), 15f));
      Collider[] componentsInChildren = (Collider[]) ((Component) t).GetComponentsInChildren<Collider>();
      Ray ray;
      ((Ray) ref ray).\u002Ector(vector3, Vector3.get_down());
      int num = Mathf.Min(Physics.RaycastNonAlloc(ray, Point._raycastHits, 100f, LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer)), Point._raycastHits.Length);
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
  }
}

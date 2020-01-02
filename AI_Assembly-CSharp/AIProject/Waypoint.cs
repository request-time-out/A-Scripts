// Decompiled with JetBrains decompiler
// Type: AIProject.Waypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace AIProject
{
  public class Waypoint : Point, IRoute
  {
    [SerializeField]
    private Waypoint.ElementType _type = (Waypoint.ElementType) -1;
    [SerializeField]
    private Waypoint.AffiliationType _affiliation = Waypoint.AffiliationType.Map;
    [SerializeField]
    [Tooltip("手動操作か？")]
    private bool _isManual;

    public Waypoint.ElementType Type
    {
      get
      {
        return this._type;
      }
    }

    public Waypoint.AffiliationType Affiliation
    {
      get
      {
        return this._affiliation;
      }
      set
      {
        this._affiliation = value;
      }
    }

    public int GroupID { get; set; }

    public int ID { get; set; }

    public INavMeshActor Reserver { get; set; }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public bool Available(INavMeshActor user)
    {
      if (!((Behaviour) this).get_isActiveAndEnabled())
        return false;
      return this.Reserver == null || this.Reserver == user;
    }

    protected override void OnEnable()
    {
      base.OnEnable();
    }

    protected override void OnDisable()
    {
    }

    public override void RefreshExistence()
    {
      if (this._isManual)
        return;
      base.RefreshExistence();
    }

    public void RefilterToActionPoint(Point[] points)
    {
      if (!((Component) this).get_gameObject().get_activeSelf())
        return;
      float pointDistanceMargin = Singleton<Resources>.Instance.LocomotionProfile.PointDistanceMargin;
      Vector3 position = ((Component) this).get_transform().get_position();
      foreach (Point point in points)
      {
        if ((double) Vector3.Distance(position, ((Component) point).get_transform().get_position()) < (double) pointDistanceMargin)
        {
          ((Component) this).get_gameObject().SetActive(false);
          break;
        }
      }
    }

    [Flags]
    public enum ElementType
    {
      HalfWay = 1,
      Destination = 2,
    }

    [Flags]
    public enum AffiliationType
    {
      Map = 1,
      Housing = 2,
      Item = 4,
    }
  }
}

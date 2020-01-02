// Decompiled with JetBrains decompiler
// Type: AIProject.MapArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AIProject
{
  public class MapArea : MonoBehaviour
  {
    [SerializeField]
    private MapArea.AreaType _type;
    [SerializeField]
    private int _areaID;
    [SerializeField]
    private List<Waypoint> _waypoints;
    [SerializeField]
    private List<BasePoint> _basePoints;
    [SerializeField]
    private List<DevicePoint> _devicePoints;
    [SerializeField]
    private List<FarmPoint> _farmPoints;
    [SerializeField]
    private List<ShipPoint> _shipPoints;
    [SerializeField]
    private List<ActionPoint> _actionPoints;
    [SerializeField]
    private List<MerchantPoint> _merchantPoints;
    [SerializeField]
    private List<EventPoint> _eventPoints;
    [SerializeField]
    private List<StoryPoint> _storyPoints;
    [SerializeField]
    private List<AnimalPoint> _animalPoints;
    [SerializeField]
    private List<ActionPoint> _appendActionPoints;
    [SerializeField]
    private List<FarmPoint> _appendFarmPoints;
    private List<PetHomePoint> _appendPetHomePoints;
    private List<JukePoint> _appendJukePoints;
    private List<CraftPoint> _appendCraftPoints;
    private List<LightSwitchPoint> _appendLightSwitchPoints;
    [SerializeField]
    private List<HPoint> _appendHPoints;
    [SerializeField]
    private Collider[] _colliders;
    [SerializeField]
    private Collider[] _hColliders;

    public MapArea()
    {
      base.\u002Ector();
    }

    public MapArea.AreaType Type
    {
      get
      {
        return this._type;
      }
    }

    public int ChunkID { get; set; }

    public int AreaID
    {
      get
      {
        return this._areaID;
      }
    }

    public List<Waypoint> Waypoints
    {
      get
      {
        return this._waypoints;
      }
    }

    public List<BasePoint> BasePoints
    {
      get
      {
        return this._basePoints;
      }
    }

    public List<DevicePoint> DevicePoints
    {
      get
      {
        return this._devicePoints;
      }
    }

    public List<FarmPoint> FarmPoints
    {
      get
      {
        return this._farmPoints;
      }
    }

    public List<ShipPoint> ShipPoints
    {
      get
      {
        return this._shipPoints;
      }
    }

    public List<ActionPoint> ActionPoints
    {
      get
      {
        return this._actionPoints;
      }
    }

    public List<MerchantPoint> MerchantPoints
    {
      get
      {
        return this._merchantPoints;
      }
    }

    public List<EventPoint> EventPoints
    {
      get
      {
        return this._eventPoints;
      }
    }

    public List<StoryPoint> StoryPoints
    {
      get
      {
        return this._storyPoints;
      }
    }

    public List<AnimalPoint> AnimalPoints
    {
      get
      {
        return this._animalPoints;
      }
    }

    public List<ActionPoint> AppendActionPoints
    {
      get
      {
        return this._appendActionPoints;
      }
    }

    public List<FarmPoint> AppendFarmPoints
    {
      get
      {
        return this._appendFarmPoints;
      }
    }

    public List<PetHomePoint> AppendPetHomePoints
    {
      get
      {
        return this._appendPetHomePoints;
      }
    }

    public List<JukePoint> AppendJukePoints
    {
      get
      {
        return this._appendJukePoints;
      }
    }

    public List<CraftPoint> AppendCraftPoints
    {
      get
      {
        return this._appendCraftPoints;
      }
    }

    public List<LightSwitchPoint> AppendLightSwitchPoints
    {
      get
      {
        return this._appendLightSwitchPoints;
      }
    }

    public List<HPoint> AppendHPoints
    {
      get
      {
        return this._appendHPoints;
      }
    }

    public Collider[] hColliders
    {
      set
      {
        this._hColliders = value;
      }
    }

    [DebuggerHidden]
    public IEnumerator Load(
      Waypoint[] wp,
      BasePoint[] bp,
      DevicePoint[] dp,
      FarmPoint[] fp,
      ShipPoint[] shipPt,
      ActionPoint[] ap,
      MerchantPoint[] mp,
      EventPoint[] ep,
      StoryPoint[] sp,
      AnimalPoint[] pap)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapArea.\u003CLoad\u003Ec__Iterator0()
      {
        wp = wp,
        bp = bp,
        dp = dp,
        fp = fp,
        shipPt = shipPt,
        ap = ap,
        mp = mp,
        ep = ep,
        sp = sp,
        pap = pap,
        \u0024this = this
      };
    }

    public void AddPoints<T>(T[] pts, List<T> list, LayerMask layer, LayerMask roofLayer) where T : Point
    {
      foreach (T pt in pts)
        this.CheckPointOnTheArea<T>(pt, list, layer, roofLayer);
    }

    public void CheckPointOnTheArea<T>(
      T point,
      List<T> pointList,
      LayerMask layer,
      LayerMask roofLayer)
      where T : Point
    {
      Vector3 vector3 = Vector3.op_Addition(point.get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 10f));
      RaycastHit raycastHit;
      if (!Physics.Raycast(vector3, Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(layer)) || !this.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
        return;
      pointList.Add(point);
      point.OwnerArea = this;
      point.AreaType = !Physics.Raycast(vector3, Vector3.get_up(), ref raycastHit, 1000f, LayerMask.op_Implicit(roofLayer)) ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
    }

    public bool CheckPointOnTheArea<T>(
      T point,
      LayerMask layer,
      LayerMask roofLayer,
      float offsetY = 10f)
      where T : Point
    {
      Vector3 vector3 = Vector3.op_Addition(point.get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), offsetY));
      RaycastHit raycastHit;
      if (!Physics.Raycast(vector3, Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(layer)) || !this.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
        return false;
      point.OwnerArea = this;
      point.AreaType = !Physics.Raycast(vector3, Vector3.get_up(), ref raycastHit, 1000f, LayerMask.op_Implicit(roofLayer)) ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
      return true;
    }

    public void AddHPoints(HPoint[] pts, List<HPoint> list, LayerMask layer)
    {
      foreach (HPoint pt in pts)
        this.CheckHPointOnTheArea(pt, list, layer);
    }

    private void CheckHPointOnTheArea(HPoint point, List<HPoint> pointList, LayerMask layer)
    {
      RaycastHit raycastHit;
      if (!Physics.Raycast(Vector3.op_Addition(((Component) point).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 10f)), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(layer)) || !this.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
        return;
      pointList.Add(point);
    }

    public bool ContainsCollider(Collider source)
    {
      foreach (Object collider in this._colliders)
      {
        if (Object.op_Equality(collider, (Object) source))
          return true;
      }
      return false;
    }

    public void Clear()
    {
      this._waypoints.Clear();
      this._actionPoints.Clear();
      this._basePoints.Clear();
      this._devicePoints.Clear();
      this._farmPoints.Clear();
      this._shipPoints.Clear();
      this._animalPoints.Clear();
    }

    public enum AreaType
    {
      Normal,
      Indoor,
      Private,
    }
  }
}

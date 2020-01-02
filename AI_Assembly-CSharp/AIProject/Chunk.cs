// Decompiled with JetBrains decompiler
// Type: AIProject.Chunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public class Chunk : MonoBehaviour
  {
    [SerializeField]
    private int _id;
    [SerializeField]
    private Chunk[] _connectedChunks;
    [Space(8f)]
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private MapArea[] _mapAreas;

    public Chunk()
    {
      base.\u002Ector();
    }

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public Chunk[] ConnectedChunks
    {
      get
      {
        return this._connectedChunks;
      }
    }

    public MapArea[] MapAreas
    {
      get
      {
        return this._mapAreas;
      }
    }

    public List<Waypoint> Waypoints { get; private set; }

    public List<ActionPoint> ActionPoints { get; private set; }

    public List<BasePoint> BasePoints { get; private set; }

    public List<DevicePoint> DevicePoints { get; private set; }

    public List<FarmPoint> FarmPoints { get; private set; }

    public List<ShipPoint> ShipPoints { get; private set; }

    public List<MerchantPoint> MerchantPoints { get; private set; }

    public List<EventPoint> EventPoints { get; private set; }

    public List<StoryPoint> StoryPoints { get; private set; }

    public List<AnimalPoint> AnimalPoints { get; private set; }

    public List<ActionPoint> AppendActionPoints { get; private set; }

    public List<FarmPoint> AppendFarmPoints { get; private set; }

    public List<PetHomePoint> AppendPetHomePoints { get; private set; }

    public List<JukePoint> AppendJukePoints { get; private set; }

    public List<CraftPoint> AppendCraftPoints { get; private set; }

    public List<LightSwitchPoint> AppendLightSwitchPoints { get; private set; }

    public List<HPoint> AppendHPoints { get; private set; }

    private void Awake()
    {
      if (Singleton<Manager.Map>.IsInstance())
        Singleton<Manager.Map>.Instance.ChunkTable[this._id] = this;
      this._mapAreas = (MapArea[]) ((Component) this).GetComponentsInChildren<MapArea>(true);
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.ChunkID = this._id;
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
      return (IEnumerator) new Chunk.\u003CLoad\u003Ec__Iterator0()
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

    public void LoadAppendActionPoints(ActionPoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point appendActionPoint in mapArea.AppendActionPoints)
          appendActionPoint.OwnerArea = (MapArea) null;
        mapArea.AppendActionPoints.Clear();
      }
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AddPoints<ActionPoint>(pts, mapArea.AppendActionPoints, areaDetectionLayer, roofLayer);
      this.AppendActionPoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (ActionPoint appendActionPoint in mapArea.AppendActionPoints)
          this.AppendActionPoints.Add(appendActionPoint);
      }
    }

    public void LoadAppendFarmPoints(FarmPoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point appendFarmPoint in mapArea.AppendFarmPoints)
          appendFarmPoint.OwnerArea = (MapArea) null;
        mapArea.AppendFarmPoints.Clear();
      }
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AddPoints<FarmPoint>(pts, mapArea.AppendFarmPoints, areaDetectionLayer, roofLayer);
      this.AppendFarmPoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (FarmPoint appendFarmPoint in mapArea.AppendFarmPoints)
          this.AppendFarmPoints.Add(appendFarmPoint);
      }
    }

    public void LoadAppendPetHomePoints(PetHomePoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point appendPetHomePoint in mapArea.AppendPetHomePoints)
          appendPetHomePoint.OwnerArea = (MapArea) null;
        mapArea.AppendPetHomePoints.Clear();
      }
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AddPoints<PetHomePoint>(pts, mapArea.AppendPetHomePoints, areaDetectionLayer, roofLayer);
      this.AppendPetHomePoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (PetHomePoint appendPetHomePoint in mapArea.AppendPetHomePoints)
          this.AppendPetHomePoints.Add(appendPetHomePoint);
      }
    }

    public void LoadAppendJukePoints(JukePoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point appendJukePoint in mapArea.AppendJukePoints)
          appendJukePoint.OwnerArea = (MapArea) null;
        mapArea.AppendJukePoints.Clear();
      }
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AddPoints<JukePoint>(pts, mapArea.AppendJukePoints, areaDetectionLayer, roofLayer);
      this.AppendJukePoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (JukePoint appendJukePoint in mapArea.AppendJukePoints)
          this.AppendJukePoints.Add(appendJukePoint);
      }
    }

    public void LoadAppendCraftPoints(CraftPoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point appendCraftPoint in mapArea.AppendCraftPoints)
          appendCraftPoint.OwnerArea = (MapArea) null;
        mapArea.AppendCraftPoints.Clear();
      }
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AddPoints<CraftPoint>(pts, mapArea.AppendCraftPoints, areaDetectionLayer, roofLayer);
      this.AppendCraftPoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (CraftPoint appendCraftPoint in mapArea.AppendCraftPoints)
          this.AppendCraftPoints.Add(appendCraftPoint);
      }
    }

    public void LoadAppendLightSwitchPoints(LightSwitchPoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (Point lightSwitchPoint in mapArea.AppendLightSwitchPoints)
          lightSwitchPoint.OwnerArea = (MapArea) null;
        mapArea.AppendLightSwitchPoints.Clear();
      }
      if (!pts.IsNullOrEmpty<LightSwitchPoint>())
      {
        foreach (MapArea mapArea in this._mapAreas)
          mapArea.AddPoints<LightSwitchPoint>(pts, mapArea.AppendLightSwitchPoints, areaDetectionLayer, roofLayer);
      }
      this.AppendLightSwitchPoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (LightSwitchPoint lightSwitchPoint in mapArea.AppendLightSwitchPoints)
          this.AppendLightSwitchPoints.Add(lightSwitchPoint);
      }
    }

    public bool CheckPointOnTheArea<T>(
      T point,
      LayerMask layer,
      LayerMask roofLayer,
      float offsetY = 10f)
      where T : Point
    {
      if (this._mapAreas.IsNullOrEmpty<MapArea>())
        return false;
      foreach (MapArea mapArea in this._mapAreas)
      {
        if (mapArea.CheckPointOnTheArea<T>(point, layer, roofLayer, offsetY))
          return true;
      }
      return false;
    }

    public void LoadAppendHPoints(HPoint[] pts)
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      foreach (MapArea mapArea in this._mapAreas)
        mapArea.AppendHPoints.Clear();
      if (!pts.IsNullOrEmpty<HPoint>())
      {
        foreach (MapArea mapArea in this._mapAreas)
          mapArea.AddHPoints(pts, mapArea.AppendHPoints, areaDetectionLayer);
      }
      this.AppendHPoints.Clear();
      foreach (MapArea mapArea in this._mapAreas)
      {
        foreach (HPoint appendHpoint in mapArea.AppendHPoints)
          this.AppendHPoints.Add(appendHpoint);
      }
    }
  }
}

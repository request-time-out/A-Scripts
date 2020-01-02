// Decompiled with JetBrains decompiler
// Type: AIProject.PointManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.SaveData;
using Housing;
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject
{
  public class PointManager : MonoBehaviour
  {
    private Waypoint[] _waypoints;
    private ActionPoint[] _actionPoints;
    private BasePoint[] _basePoints;
    private DevicePoint[] _devicePoints;
    private FarmPoint[] _farmPoints;
    private ShipPoint[] _shipPoints;
    private LightSwitchPoint[] _lightSwitchPoints;
    private MerchantPoint[] _merchantPoints;
    private EventPoint[] _eventPoints;
    private StoryPoint[] _storyPoints;
    private AnimalPoint[] _animalPoints;
    private AnimalActionPoint[] _animalActionPoints;
    private Dictionary<int, FarmPoint> _runtimeFarmPointTable;
    private Dictionary<int, PetHomePoint> _petHomePointTable;
    private Dictionary<int, List<JukePoint>> _jukePointTable;
    [SerializeField]
    private Transform _routeParent;
    [SerializeField]
    private Transform _actionPointRoot;
    [SerializeField]
    private Transform _basePointRoot;
    [SerializeField]
    private Transform _devicePointRoot;
    [SerializeField]
    private Transform _harvestPointRoot;
    private Transform _merchantPointRoot;
    private Transform _eventPointRoot;
    private Transform _storyPointRoot;
    private Transform _animalPointRoot;
    private Transform _lightSwitchPointRoot;
    private Transform _shipPointRoot;
    private Dictionary<int, List<ValueTuple<Vector3, Waypoint>>> _housingWaypointCacheTable;
    private Dictionary<int, List<Waypoint>> _housingWaypointTable;
    private Dictionary<int, Transform> _housingWaypointParentTable;

    public PointManager()
    {
      base.\u002Ector();
    }

    public List<ActionPoint> AppendActionPoints { get; private set; }

    public List<HPoint> AppendHPoints { get; private set; }

    public Transform ActionPointRoot
    {
      get
      {
        return this._actionPointRoot;
      }
      set
      {
        this._actionPointRoot = value;
      }
    }

    public Transform BasePointRoot
    {
      get
      {
        return this._basePointRoot;
      }
      set
      {
        this._basePointRoot = value;
      }
    }

    public Transform DevicePointRoot
    {
      get
      {
        return this._devicePointRoot;
      }
      set
      {
        this._devicePointRoot = value;
      }
    }

    public Transform HarvestPointRoot
    {
      get
      {
        return this._harvestPointRoot;
      }
      set
      {
        this._harvestPointRoot = value;
      }
    }

    public Transform MerchantPointRoot
    {
      get
      {
        return this._merchantPointRoot;
      }
      set
      {
        this._merchantPointRoot = value;
      }
    }

    public Transform EventPointRoot
    {
      get
      {
        return this._eventPointRoot;
      }
      set
      {
        this._eventPointRoot = value;
      }
    }

    public Transform StoryPointRoot
    {
      get
      {
        return this._storyPointRoot;
      }
      set
      {
        this._storyPointRoot = value;
      }
    }

    public Transform AnimalPointRoot
    {
      get
      {
        return this._animalPointRoot;
      }
      set
      {
        this._animalPointRoot = value;
      }
    }

    public Transform LightSwitchPointRoot
    {
      get
      {
        return this._lightSwitchPointRoot;
      }
      set
      {
        this._lightSwitchPointRoot = value;
      }
    }

    public Transform ShipPointRoot
    {
      get
      {
        return this._shipPointRoot;
      }
      set
      {
        this._shipPointRoot = value;
      }
    }

    public bool ReadyTo { get; private set; }

    [DebuggerHidden]
    public IEnumerator Load()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PointManager.\u003CLoad\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Release()
    {
      foreach (Component waypoint in this._waypoints)
        Object.Destroy((Object) waypoint.get_gameObject());
      this._waypoints = (Waypoint[]) null;
      Object.Destroy((Object) ((Component) this._actionPointRoot).get_gameObject());
      this._actionPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._basePointRoot).get_gameObject());
      this._basePointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._devicePointRoot).get_gameObject());
      this._devicePointRoot = (Transform) null;
      this.DevicePointDic.Clear();
      Object.Destroy((Object) ((Component) this._harvestPointRoot).get_gameObject());
      this._harvestPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._shipPointRoot).get_gameObject());
      this._shipPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._lightSwitchPointRoot).get_gameObject());
      this._lightSwitchPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._merchantPointRoot).get_gameObject());
      this._merchantPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._eventPointRoot).get_gameObject());
      this._eventPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._storyPointRoot).get_gameObject());
      this._storyPointRoot = (Transform) null;
      Object.Destroy((Object) ((Component) this._animalPointRoot).get_gameObject());
      this._animalPointRoot = (Transform) null;
      using (Dictionary<int, Transform>.Enumerator enumerator = this._housingWaypointParentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Object.Destroy((Object) ((Component) enumerator.Current.Value).get_gameObject());
      }
      this._housingWaypointParentTable.Clear();
    }

    private bool AnySamePoint(List<ActionPoint> list, ActionPoint point)
    {
      foreach (ActionPoint actionPoint in list)
      {
        bool flag = true & actionPoint.ID == point.ID;
      }
      return false;
    }

    private bool AnySamePoint(List<AnimalPoint> list, AnimalPoint point)
    {
      return false;
    }

    public void AddRuntimeFarmPoints(FarmPoint[] runtimeFarmPoints)
    {
      this._runtimeFarmPointTable.Clear();
      if (runtimeFarmPoints.IsNullOrEmpty<FarmPoint>())
        return;
      foreach (FarmPoint runtimeFarmPoint in runtimeFarmPoints)
      {
        if (!Object.op_Equality((Object) runtimeFarmPoint, (Object) null))
          this._runtimeFarmPointTable[runtimeFarmPoint.RegisterID] = runtimeFarmPoint;
      }
    }

    public void RemoveRuntimeFarmPoint(FarmPoint farmPoint)
    {
      if (Object.op_Equality((Object) farmPoint, (Object) null))
        return;
      int registerId = farmPoint.RegisterID;
      if (!this._runtimeFarmPointTable.ContainsKey(registerId))
        return;
      this._runtimeFarmPointTable.Remove(registerId);
    }

    public void AddPetHomePoints(PetHomePoint[] petHomePoints)
    {
      this._petHomePointTable.Clear();
      if (petHomePoints.IsNullOrEmpty<PetHomePoint>())
        return;
      foreach (PetHomePoint petHomePoint in petHomePoints)
      {
        if (!Object.op_Equality((Object) petHomePoint, (Object) null))
          this._petHomePointTable[petHomePoint.RegisterID] = petHomePoint;
      }
    }

    public void RemovePetHomePoint(PetHomePoint petHomePoint)
    {
      if (Object.op_Equality((Object) petHomePoint, (Object) null))
        return;
      int registerId = petHomePoint.RegisterID;
      if (!this._petHomePointTable.ContainsKey(registerId))
        return;
      this._petHomePointTable.Remove(registerId);
    }

    public void AddJukePoints(JukePoint[] jukePoints)
    {
      foreach (KeyValuePair<int, List<JukePoint>> keyValuePair in this._jukePointTable)
      {
        if (!keyValuePair.Value.IsNullOrEmpty<JukePoint>())
          keyValuePair.Value.Clear();
      }
      if (jukePoints.IsNullOrEmpty<JukePoint>())
        return;
      foreach (JukePoint jukePoint in jukePoints)
      {
        if (!Object.op_Equality((Object) jukePoint, (Object) null))
        {
          int areaId = jukePoint.AreaID;
          List<JukePoint> jukePointList;
          if (!this._jukePointTable.TryGetValue(areaId, out jukePointList) || jukePointList == null)
            this._jukePointTable[areaId] = jukePointList = new List<JukePoint>();
          jukePointList.Add(jukePoint);
        }
      }
    }

    public void RemoveJukePoint(JukePoint jukePoint)
    {
      List<JukePoint> source;
      if (Object.op_Equality((Object) jukePoint, (Object) null) || !this._jukePointTable.TryGetValue(jukePoint.AreaID, out source) || source.IsNullOrEmpty<JukePoint>())
        return;
      source.Remove(jukePoint);
    }

    public Waypoint[] Waypoints
    {
      get
      {
        return this._waypoints;
      }
    }

    public ActionPoint[] ActionPoints
    {
      get
      {
        return this._actionPoints;
      }
    }

    public BasePoint[] BasePoints
    {
      get
      {
        return this._basePoints;
      }
    }

    public DevicePoint[] DevicePoints
    {
      get
      {
        return this._devicePoints;
      }
    }

    public FarmPoint[] FarmPoints
    {
      get
      {
        return this._farmPoints;
      }
    }

    public ShipPoint[] ShipPoints
    {
      get
      {
        return this._shipPoints;
      }
    }

    public LightSwitchPoint[] LightSwitchPoints
    {
      get
      {
        return this._lightSwitchPoints;
      }
    }

    public Dictionary<int, DevicePoint> DevicePointDic { get; set; }

    public AnimalPoint[] AnimalPoints
    {
      get
      {
        return this._animalPoints;
      }
    }

    public AnimalActionPoint[] AnimalActionPoints
    {
      get
      {
        return this._animalActionPoints;
      }
    }

    public MerchantPoint[] MerchantPoints
    {
      get
      {
        return this._merchantPoints;
      }
    }

    public EventPoint[] EventPoints
    {
      get
      {
        return this._eventPoints;
      }
    }

    public StoryPoint[] StoryPoints
    {
      get
      {
        return this._storyPoints;
      }
    }

    public Dictionary<int, StoryPoint> StoryPointTable { get; private set; }

    public IReadOnlyDictionary<int, FarmPoint> RuntimeFarmPointTable
    {
      get
      {
        return (IReadOnlyDictionary<int, FarmPoint>) this._runtimeFarmPointTable;
      }
    }

    public IReadOnlyDictionary<int, PetHomePoint> PetHomePointTable
    {
      get
      {
        return (IReadOnlyDictionary<int, PetHomePoint>) this._petHomePointTable;
      }
    }

    public IReadOnlyDictionary<int, List<JukePoint>> JukePointTable
    {
      get
      {
        return (IReadOnlyDictionary<int, List<JukePoint>>) this._jukePointTable;
      }
    }

    public Dictionary<int, List<Waypoint>> HousingWaypointTable
    {
      get
      {
        return this._housingWaypointTable;
      }
    }

    public void CreateHousingWaypoint(int housingID)
    {
      Dictionary<int, CraftInfo> craftInfos = (!Singleton<Game>.IsInstance() ? (HousingData) null : Singleton<Game>.Instance.WorldData?.HousingData)?.CraftInfos;
      CraftInfo craftInfo;
      if (craftInfos.IsNullOrEmpty<int, CraftInfo>() || !craftInfos.TryGetValue(housingID, out craftInfo) || craftInfo == null)
        return;
      GameObject objRoot = craftInfo.ObjRoot ?? (!Singleton<Manager.Housing>.IsInstance() ? (GameObject) null : Singleton<Manager.Housing>.Instance.GetRoot(housingID));
      this.CreateHousingWaypoint(housingID, objRoot, craftInfo.LimitSize);
    }

    public void CreateHousingWaypoint()
    {
      if (!Singleton<Manager.Housing>.IsInstance())
        return;
      Dictionary<int, CraftInfo> craftInfos = (!Singleton<Game>.IsInstance() ? (HousingData) null : Singleton<Game>.Instance.WorldData?.HousingData)?.CraftInfos;
      if (craftInfos == null)
        return;
      foreach (KeyValuePair<int, CraftInfo> keyValuePair in craftInfos)
      {
        CraftInfo craftInfo = keyValuePair.Value;
        GameObject objRoot = craftInfo.ObjRoot ?? Singleton<Manager.Housing>.Instance.GetRoot(keyValuePair.Key);
        if (Singleton<Manager.Map>.IsInstance() && Singleton<Manager.Map>.Instance.HousingPointTable.ContainsKey(keyValuePair.Key))
          this.CreateHousingWaypoint(keyValuePair.Key, objRoot, craftInfo.LimitSize);
      }
    }

    private void CreateHousingWaypoint(int housingID, GameObject objRoot, Vector3 areaSize)
    {
      if (Object.op_Equality((Object) objRoot, (Object) null) || !Singleton<Resources>.IsInstance())
        return;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      if (Object.op_Equality((Object) locomotionProfile, (Object) null))
        return;
      List<ValueTuple<Vector3, Waypoint>> waypointList;
      if (!this._housingWaypointCacheTable.TryGetValue(housingID, out waypointList) || waypointList == null)
      {
        this._housingWaypointCacheTable[housingID] = waypointList = new List<ValueTuple<Vector3, Waypoint>>();
        Transform transform = objRoot.get_transform();
        if (Object.op_Equality((Object) transform, (Object) null))
          return;
        float installationDistance = locomotionProfile.HousingWaypointSetting.InstallationDistance;
        float installationHeight = locomotionProfile.HousingWaypointSetting.InstallationHeight;
        SpiralPoint spiralPoint = new SpiralPoint(999);
        for (int index = 0; (double) installationHeight * (double) index < areaSize.y; ++index)
        {
          float num = installationHeight * (float) index;
          spiralPoint.Clear();
          spiralPoint.Limit = 999;
          while (!spiralPoint.End)
          {
            Vector3 vector3 = Vector3.op_Multiply(new Vector3((float) ((Vector2Int) ref spiralPoint.Current).get_x(), 0.0f, (float) ((Vector2Int) ref spiralPoint.Current).get_y()), installationDistance);
            vector3.y = (__Null) (double) num;
            if (areaSize.y > vector3.y && vector3.x > -areaSize.x / 2.0 && (areaSize.x / 2.0 > vector3.x && vector3.z > -areaSize.z / 2.0) && areaSize.z / 2.0 > vector3.z)
            {
              vector3 = Vector3.op_Addition(transform.get_position(), Quaternion.op_Multiply(transform.get_rotation(), vector3));
              waypointList.Add(new ValueTuple<Vector3, Waypoint>(vector3, (Waypoint) null));
              spiralPoint.Next();
            }
            else
              break;
          }
        }
      }
      this.RefreshWaypoints(housingID, waypointList);
    }

    protected void RefreshWaypoints(int housingID, List<ValueTuple<Vector3, Waypoint>> waypointList)
    {
      if (!Singleton<Resources>.IsInstance() || waypointList.IsNullOrEmpty<ValueTuple<Vector3, Waypoint>>())
        return;
      List<Waypoint> source = (List<Waypoint>) null;
      if (!this._housingWaypointTable.TryGetValue(housingID, out source) || source == null)
        this._housingWaypointTable[housingID] = source = new List<Waypoint>();
      else
        source.Clear();
      float closestEdgeDistance = Singleton<Resources>.Instance.LocomotionProfile.HousingWaypointSetting.ClosestEdgeDistance;
      float sampleDistance = Singleton<Resources>.Instance.LocomotionProfile.HousingWaypointSetting.SampleDistance;
      for (int pointID = 0; pointID < waypointList.Count; ++pointID)
      {
        ValueTuple<Vector3, Waypoint> waypoint1 = waypointList[pointID];
        Vector3 position = (Vector3) waypoint1.Item1;
        NavMeshHit navMeshHit;
        bool flag = true & NavMesh.SamplePosition(position, ref navMeshHit, sampleDistance, -1);
        if (flag)
          position = ((NavMeshHit) ref navMeshHit).get_position();
        if (flag)
          flag &= NavMesh.FindClosestEdge(position, ref navMeshHit, -1);
        if (flag)
          flag &= (double) closestEdgeDistance <= (double) ((NavMeshHit) ref navMeshHit).get_distance();
        Waypoint waypoint2 = (Waypoint) waypoint1.Item2;
        if (Object.op_Inequality((Object) waypoint2, (Object) null))
        {
          ((Component) waypoint2).get_transform().set_position(position);
          if (((Component) waypoint2).get_gameObject().get_activeSelf() != flag)
            ((Component) waypoint2).get_gameObject().SetActive(flag);
        }
        else if (flag)
        {
          waypoint2 = this.CreateWaypoint(housingID, pointID);
          ((Component) waypoint2).get_transform().set_position(position);
          waypoint1.Item2 = (__Null) waypoint2;
          waypointList[pointID] = waypoint1;
          if (((Component) waypoint2).get_gameObject().get_activeSelf() != flag)
            ((Component) waypoint2).get_gameObject().SetActive(flag);
        }
        if (flag)
          source.Add(waypoint2);
      }
      if (!Singleton<Manager.Map>.IsInstance() || source.IsNullOrEmpty<Waypoint>())
        return;
      Dictionary<int, Chunk> chunkTable = Singleton<Manager.Map>.Instance.ChunkTable;
      if (chunkTable.IsNullOrEmpty<int, Chunk>())
        return;
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      for (int index = 0; index < source.Count; ++index)
      {
        bool flag = false;
        Waypoint point = source[index];
        foreach (KeyValuePair<int, Chunk> keyValuePair in chunkTable)
        {
          Chunk chunk = keyValuePair.Value;
          flag = Object.op_Inequality((Object) chunk, (Object) null) && chunk.CheckPointOnTheArea<Waypoint>(point, areaDetectionLayer, roofLayer, 1f);
          if (flag)
            break;
        }
        if (!flag)
        {
          ((Component) point).get_gameObject().SetActive(false);
          source.RemoveAt(index);
          --index;
        }
      }
    }

    private Waypoint CreateWaypoint(int housingID, int pointID)
    {
      Transform transform1 = (Transform) null;
      if (!this._housingWaypointParentTable.TryGetValue(housingID, out transform1) || Object.op_Equality((Object) transform1, (Object) null))
      {
        this._housingWaypointParentTable[housingID] = transform1 = new GameObject(string.Format("Housing Area Waypoint [{0:00}]", (object) housingID)).get_transform();
        transform1.SetParent(((Component) this).get_transform(), false);
      }
      Transform transform2 = new GameObject(string.Format("housing{0:00}_waypoint{1:0000}", (object) housingID, (object) pointID)).get_transform();
      transform2.SetParent(transform1, false);
      Waypoint orAddComponent = ((Component) transform2).GetOrAddComponent<Waypoint>();
      orAddComponent.GroupID = housingID;
      orAddComponent.ID = pointID;
      orAddComponent.Affiliation = Waypoint.AffiliationType.Housing;
      return orAddComponent;
    }

    public void ClearHousingWaypoint()
    {
      using (Dictionary<int, Transform>.Enumerator enumerator = this._housingWaypointParentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform transform = enumerator.Current.Value;
          if (Object.op_Inequality((Object) transform, (Object) null) && Object.op_Inequality((Object) ((Component) transform).get_gameObject(), (Object) null))
            Object.Destroy((Object) ((Component) transform).get_gameObject());
        }
      }
      this._housingWaypointParentTable.Clear();
      foreach (KeyValuePair<int, List<Waypoint>> keyValuePair in this._housingWaypointTable)
        keyValuePair.Value?.Clear();
      this._housingWaypointTable.Clear();
      using (Dictionary<int, List<ValueTuple<Vector3, Waypoint>>>.Enumerator enumerator = this._housingWaypointCacheTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value?.Clear();
      }
      this._housingWaypointCacheTable.Clear();
    }
  }
}

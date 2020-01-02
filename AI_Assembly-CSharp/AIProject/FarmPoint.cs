// Decompiled with JetBrains decompiler
// Type: AIProject.FarmPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.Player;
using Housing;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class FarmPoint : Point, ICommandable
  {
    private static RaycastHit[] _chickenWaypointRaycastHits = new RaycastHit[10];
    [SerializeField]
    private bool _enabledRangeCheck = true;
    [SerializeField]
    private float _radius = 1f;
    private List<Waypoint> _chickenWaypointList = new List<Waypoint>();
    private List<Waypoint> _chickenWaypointCacheList = new List<Waypoint>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private FarmSection[] _harvestSections;
    [SerializeField]
    private FarmPoint.FarmKind _farmKind;
    [SerializeField]
    private Transform _commandBasePoint;
    private int? _hashCode;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;
    private List<Vector3> _chickenWaypointPositionList;
    private Transform _chickenWaypointRoot;
    private Transform _animalRoot;

    public override int RegisterID
    {
      get
      {
        return this._id;
      }
      set
      {
        this._id = value;
      }
    }

    public FarmSection[] HarvestSections
    {
      get
      {
        return this._harvestSections;
      }
    }

    public float Radius
    {
      get
      {
        return this._radius;
      }
    }

    public FarmPoint.FarmKind Kind
    {
      get
      {
        return this._farmKind;
      }
    }

    public int AreaIDOnHousingArea { get; set; } = -1;

    public int InstanceID
    {
      get
      {
        if (!this._hashCode.HasValue)
          this._hashCode = new int?(((Object) this).GetInstanceID());
        return this._hashCode.Value;
      }
    }

    public bool IsImpossible { get; private set; }

    public bool SetImpossible(bool value, Actor actor)
    {
      return true;
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      if (this.TutorialHideMode() || (double) distance > (double) radiusA)
        return false;
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num = angle / 2f;
      return (double) Vector3.Angle(Vector3.op_Subtraction(commandCenter, basePosition), forward) <= (double) num;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        bool flag2 = false;
        using (List<Transform>.Enumerator enumerator = this.NavMeshPoints.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Transform current = enumerator.Current;
            nmAgent.CalculatePath(this.Position, this._pathForCalc);
            if (this._pathForCalc.get_status() == null)
            {
              float num1 = 0.0f;
              Vector3[] corners = this._pathForCalc.get_corners();
              float num2 = (this.CommandType != CommandType.Forward ? radiusB : radiusA) + this._radius;
              for (int index = 0; index < corners.Length - 1; ++index)
              {
                float num3 = Vector3.Distance(corners[index], corners[index + 1]);
                num1 += num3;
              }
              if ((double) num1 < (double) num2)
                break;
              if (!flag2)
                flag1 = false;
            }
          }
        }
      }
      else
        flag1 = false;
      return flag1;
    }

    public bool IsNeutralCommand
    {
      get
      {
        return !this.TutorialHideMode();
      }
    }

    public bool TutorialHideMode()
    {
      return Manager.Map.TutorialMode;
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public Vector3 CommandCenter
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public List<Transform> NavMeshPoints { get; set; } = new List<Transform>();

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
        return Object.op_Inequality((Object) playerActor, (Object) null) && playerActor.PlayerController.State is Onbu ? (CommandLabel.CommandInfo[]) null : this._labels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels { get; private set; }

    public ObjectLayer Layer { get; } = ObjectLayer.Command;

    public CommandType CommandType { get; }

    private void Awake()
    {
      this._harvestSections = (FarmSection[]) ((Component) this).GetComponentsInChildren<FarmSection>();
      int num = 0;
      foreach (FarmSection harvestSection in this._harvestSections)
      {
        harvestSection.HarvestID = this._id;
        harvestSection.SectionID = num++;
      }
    }

    protected override void Start()
    {
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      base.Start();
      this.NavMeshPoints.Add(((Component) this).get_transform());
      List<GameObject> gameObjectList = ListPool<GameObject>.Get();
      ((Component) this).get_transform().FindLoopPrefix(gameObjectList, mapDefines.NavMeshTargetName);
      if (!gameObjectList.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = gameObjectList.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.NavMeshPoints.Add(enumerator.Current.get_transform());
        }
      }
      ListPool<GameObject>.Release(gameObjectList);
      Sprite sprite1;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.FarmIconID, out sprite1);
      Sprite sprite2;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.ChickenCoopIconID, out sprite2);
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.WellIconID, out Sprite _);
      Transform transform = ((Component) this).get_transform().FindLoop(mapDefines.FarmPointLabelTargetName)?.get_transform() ?? ((Component) this).get_transform();
      if (this._farmKind == FarmPoint.FarmKind.Plant)
        this._labels = new CommandLabel.CommandInfo[1]
        {
          new CommandLabel.CommandInfo()
          {
            Text = "畑",
            Icon = sprite1,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) (x => this._farmKind == FarmPoint.FarmKind.Plant),
            Event = (Action) (() =>
            {
              MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
              List<AIProject.SaveData.Environment.PlantInfo> plantInfoList1;
              if (!Singleton<Game>.Instance.Environment.FarmlandTable.TryGetValue(this._id, out plantInfoList1))
              {
                List<AIProject.SaveData.Environment.PlantInfo> plantInfoList2 = new List<AIProject.SaveData.Environment.PlantInfo>();
                Singleton<Game>.Instance.Environment.FarmlandTable[this._id] = plantInfoList2;
                plantInfoList1 = plantInfoList2;
                foreach (FarmSection harvestSection in this._harvestSections)
                  plantInfoList1.Add((AIProject.SaveData.Environment.PlantInfo) null);
              }
              MapUIContainer.FarmlandUI.currentPlant = plantInfoList1;
              MapUIContainer.SetActiveFarmlandUI(true);
              Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Harvest");
            })
          }
        };
      else if (this._farmKind == FarmPoint.FarmKind.ChickenCoop)
        this._labels = new CommandLabel.CommandInfo[1]
        {
          new CommandLabel.CommandInfo()
          {
            Text = "鶏小屋",
            Icon = sprite2,
            IsHold = true,
            TargetSpriteInfo = icon.ActionSpriteInfo,
            Transform = transform,
            Condition = (Func<PlayerActor, bool>) (x => this._farmKind == FarmPoint.FarmKind.ChickenCoop),
            Event = (Action) (() =>
            {
              PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
              if (!Object.op_Inequality((Object) playerActor, (Object) null))
                return;
              playerActor.CurrentFarmPoint = this;
              playerActor.PlayerController.ChangeState("ChickenCoopMenu");
            })
          }
        };
      else if (this._farmKind != FarmPoint.FarmKind.Well)
        ;
    }

    private int GetHousingAreaID()
    {
      if (!Singleton<Resources>.IsInstance())
        return -1;
      ItemComponent componentInParent1 = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
      if (Object.op_Equality((Object) componentInParent1, (Object) null))
        return -1;
      Vector3 vector3 = Vector3.op_Addition(componentInParent1.position, Vector3.op_Multiply(Vector3.get_up(), 5f));
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      int num = Mathf.Min(Physics.RaycastNonAlloc(vector3, Vector3.get_down(), Point._raycastHits, 1000f, LayerMask.op_Implicit(areaDetectionLayer)), Point._raycastHits.Length);
      if (num <= 0)
        return -1;
      for (int index = 0; index < num; ++index)
      {
        RaycastHit raycastHit = Point._raycastHits[index];
        if (!Object.op_Equality((Object) ((RaycastHit) ref raycastHit).get_transform(), (Object) null))
        {
          MapArea componentInParent2 = (MapArea) ((Component) ((RaycastHit) ref raycastHit).get_transform()).GetComponentInParent<MapArea>();
          if (!Object.op_Equality((Object) componentInParent2, (Object) null))
            return componentInParent2.AreaID;
        }
      }
      return -1;
    }

    public IReadOnlyList<Vector3> ChickenWaypointPositionList
    {
      get
      {
        return (IReadOnlyList<Vector3>) this._chickenWaypointPositionList;
      }
    }

    public IReadOnlyList<Waypoint> ChickenWaypointList
    {
      get
      {
        return (IReadOnlyList<Waypoint>) this._chickenWaypointList;
      }
    }

    public Transform AnimalRoot
    {
      get
      {
        if (Object.op_Equality((Object) this._animalRoot, (Object) null))
        {
          this._animalRoot = new GameObject("Animal Root").get_transform();
          ItemComponent componentInParent = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
          this._animalRoot.SetParent(!Object.op_Inequality((Object) componentInParent, (Object) null) || !Object.op_Implicit((Object) ((Component) componentInParent).get_transform()) ? ((Component) this).get_transform() : ((Component) componentInParent).get_transform(), false);
        }
        return this._animalRoot;
      }
    }

    public void SetChickenWayPoint()
    {
      if (this._farmKind != FarmPoint.FarmKind.ChickenCoop)
        return;
      this.AreaIDOnHousingArea = this.GetHousingAreaID();
      if (!Singleton<Resources>.IsInstance())
        return;
      ItemComponent componentInParent = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
      Collider[] componentsInChildren = (Collider[]) ((Component) componentInParent)?.GetComponentsInChildren<Collider>();
      if (componentsInChildren.IsNullOrEmpty<Collider>())
        return;
      AnimalDefinePack.ChickenCoopWaypointSettings coopWaypointSetting = Singleton<Resources>.Instance.AnimalDefinePack.ChickenCoopWaypointSetting;
      Collider collider1 = (Collider) null;
      LayerMask layer = coopWaypointSetting.Layer;
      string tagName = coopWaypointSetting.TagName;
      foreach (Collider collider2 in componentsInChildren)
      {
        if (!Object.op_Equality((Object) collider2, (Object) null) && !Object.op_Equality((Object) ((Component) collider2).get_gameObject(), (Object) null) && ((1 << ((Component) collider2).get_gameObject().get_layer() & LayerMask.op_Implicit(layer)) != 0 && ((Component) collider2).get_gameObject().get_tag() == tagName))
        {
          collider1 = collider2;
          break;
        }
      }
      if (Object.op_Equality((Object) collider1, (Object) null))
        return;
      Transform transform = ((Component) componentInParent).get_transform();
      Vector3 position = ((Component) collider1).get_transform().get_position();
      Quaternion rotation = ((Component) collider1).get_transform().get_rotation();
      if (this._chickenWaypointPositionList == null)
        this._chickenWaypointPositionList = new List<Vector3>();
      else
        this._chickenWaypointPositionList.Clear();
      if (Object.op_Equality((Object) this._chickenWaypointRoot, (Object) null))
      {
        this._chickenWaypointRoot = new GameObject("Chicken Waypoint Root").get_transform();
        this._chickenWaypointRoot.SetParent(transform, false);
        Transform chickenWaypointRoot = this._chickenWaypointRoot;
        Vector3 zero = Vector3.get_zero();
        this._chickenWaypointRoot.set_localEulerAngles(zero);
        Vector3 vector3 = zero;
        chickenWaypointRoot.set_localPosition(vector3);
      }
      Vector3 installation = coopWaypointSetting.Installation;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      while (++num3 <= 999999)
      {
        int num4 = 0;
        int num5 = 0;
        while (true)
        {
          Vector3 vector3_1;
          ((Vector3) ref vector3_1).\u002Ector((float) this.ReversiNumber(num2), 0.0f, (float) this.ReversiNumber(num5));
          Vector3 vector3_2 = Vector3.Scale(vector3_1, installation);
          vector3_1 = Vector3.op_Addition(position, Quaternion.op_Multiply(rotation, vector3_2));
          int num6 = Physics.RaycastNonAlloc(Vector3.op_Addition(vector3_1, Vector3.get_up()), Vector3.get_down(), FarmPoint._chickenWaypointRaycastHits, 2f, LayerMask.op_Implicit(layer));
          int num7 = Mathf.Min(FarmPoint._chickenWaypointRaycastHits.Length, num6);
          bool flag = 0 < num7;
          if (flag)
          {
            flag = false;
            for (int index = 0; index < num7; ++index)
            {
              RaycastHit waypointRaycastHit = FarmPoint._chickenWaypointRaycastHits[index];
              if (Object.op_Equality((Object) collider1, (Object) ((RaycastHit) ref waypointRaycastHit).get_collider()))
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            ++num4;
            if (num5 == 0)
              break;
          }
          else
          {
            num1 = 0;
            num4 = 0;
            this._chickenWaypointPositionList.Add(vector3_1);
          }
          if (2 > num4)
            ++num5;
          else
            goto label_32;
        }
        ++num1;
label_32:
        ++num2;
        if (2 <= num1)
          break;
      }
      foreach (Waypoint chickenWaypoint in this._chickenWaypointList)
        this.ReturnChickenWaypoint(chickenWaypoint);
      this._chickenWaypointList.Clear();
      if (this._chickenWaypointPositionList.IsNullOrEmpty<Vector3>())
        return;
      float closestEdgeDistance = coopWaypointSetting.ClosestEdgeDistance;
      float sampleDistance = coopWaypointSetting.SampleDistance;
      int agentAreaMask = coopWaypointSetting.AgentAreaMask;
      for (int id = 0; id < this._chickenWaypointPositionList.Count; ++id)
      {
        Vector3 vector3 = this._chickenWaypointPositionList[id];
        NavMeshHit navMeshHit;
        bool flag = true & NavMesh.SamplePosition(vector3, ref navMeshHit, sampleDistance, agentAreaMask);
        if (flag)
          vector3 = ((NavMeshHit) ref navMeshHit).get_position();
        if (flag)
          flag &= NavMesh.FindClosestEdge(vector3, ref navMeshHit, agentAreaMask);
        if (flag)
          flag &= (double) closestEdgeDistance <= (double) ((NavMeshHit) ref navMeshHit).get_distance();
        if (flag)
        {
          int num4 = Physics.RaycastNonAlloc(Vector3.op_Addition(vector3, Vector3.get_up()), Vector3.get_down(), FarmPoint._chickenWaypointRaycastHits, sampleDistance * 2f, LayerMask.op_Implicit(layer));
          flag = 0 < num4;
          if (flag)
          {
            int num5 = Mathf.Min(num4, FarmPoint._chickenWaypointRaycastHits.Length);
            for (int index = 0; index < num5; ++index)
            {
              RaycastHit waypointRaycastHit = FarmPoint._chickenWaypointRaycastHits[index];
              if (flag = Object.op_Equality((Object) collider1, (Object) ((RaycastHit) ref waypointRaycastHit).get_collider()))
                break;
            }
          }
        }
        if (flag)
        {
          Waypoint chickenWaypoint = this.GetOrCreateChickenWaypoint(id);
          ((Component) chickenWaypoint).get_transform().set_position(vector3);
          if (!((Component) chickenWaypoint).get_gameObject().get_activeSelf())
            ((Component) chickenWaypoint).get_gameObject().SetActive(true);
          this._chickenWaypointList.Add(chickenWaypoint);
        }
      }
    }

    private int ReversiNumber(int num)
    {
      return num % 2 == 0 ? num * -1 + num / 2 : num - num / 2;
    }

    private Waypoint GetOrCreateChickenWaypoint(int id)
    {
      Waypoint waypoint = this._chickenWaypointCacheList.PopFront<Waypoint>();
      if (Object.op_Inequality((Object) waypoint, (Object) null))
        return waypoint;
      Waypoint orAddComponent = new GameObject(string.Format("chicken_waypoint_{0:00}", (object) id)).GetOrAddComponent<Waypoint>();
      orAddComponent.GroupID = this.RegisterID;
      orAddComponent.ID = id;
      ((Component) orAddComponent).get_transform().SetParent(this._chickenWaypointRoot, false);
      orAddComponent.Affiliation = Waypoint.AffiliationType.Item;
      return orAddComponent;
    }

    private bool ReturnChickenWaypoint(Waypoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null) || Object.op_Equality((Object) ((Component) point).get_gameObject(), (Object) null) || this._chickenWaypointCacheList.Contains(point))
        return false;
      if (((Component) point).get_gameObject().get_activeSelf())
        ((Component) point).get_gameObject().SetActive(false);
      this._chickenWaypointCacheList.Add(point);
      return true;
    }

    public void ClearChickenWayPoint()
    {
      if (Object.op_Equality((Object) this._chickenWaypointRoot, (Object) null))
        return;
      Object.Destroy((Object) ((Component) this._chickenWaypointRoot).get_gameObject());
      this._chickenWaypointRoot = (Transform) null;
      if (this._chickenWaypointPositionList != null)
        this._chickenWaypointPositionList.Clear();
      this._chickenWaypointPositionList = (List<Vector3>) null;
      this._chickenWaypointList.Clear();
      this._chickenWaypointCacheList.Clear();
    }

    public List<PetChicken> ChickenList { get; private set; }

    public void CreateChicken()
    {
      if (this._farmKind != FarmPoint.FarmKind.ChickenCoop)
        return;
      if (this.ChickenList != null)
      {
        this.ChickenList.RemoveAll((Predicate<PetChicken>) (x => Object.op_Equality((Object) x, (Object) null)));
      }
      else
      {
        if (!Singleton<Game>.IsInstance() || !Singleton<AnimalManager>.IsInstance())
          return;
        AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
        if (environment == null)
          return;
        this.ChickenList = ListPool<PetChicken>.Get();
        Dictionary<int, List<AIProject.SaveData.Environment.ChickenInfo>> chickenTable = environment.ChickenTable;
        if (chickenTable == null)
          return;
        List<AIProject.SaveData.Environment.ChickenInfo> source1 = (List<AIProject.SaveData.Environment.ChickenInfo>) null;
        if (!chickenTable.TryGetValue(this.RegisterID, out source1) || source1.IsNullOrEmpty<AIProject.SaveData.Environment.ChickenInfo>())
        {
          if (Singleton<Manager.Map>.IsInstance())
          {
            int mapId = Singleton<Manager.Map>.Instance.MapID;
            int areaIdOnHousingArea = this.AreaIDOnHousingArea;
            Dictionary<int, Dictionary<int, AIProject.SaveData.AnimalData>> source2;
            Dictionary<int, AIProject.SaveData.AnimalData> source3;
            if (!environment.HousingChickenDataTable.TryGetValue(mapId, out source2) || source2.IsNullOrEmpty<int, Dictionary<int, AIProject.SaveData.AnimalData>>() || (!source2.TryGetValue(areaIdOnHousingArea, out source3) || source3.IsNullOrEmpty<int, AIProject.SaveData.AnimalData>()))
              return;
            int num = source3.Keys.Max();
            if (source1 == null)
              chickenTable[this.RegisterID] = source1 = new List<AIProject.SaveData.Environment.ChickenInfo>();
            while (source1.Count <= num)
              source1.Add((AIProject.SaveData.Environment.ChickenInfo) null);
            for (int index = 0; index < source1.Count; ++index)
              source1[index] = (AIProject.SaveData.Environment.ChickenInfo) null;
            foreach (KeyValuePair<int, AIProject.SaveData.AnimalData> keyValuePair in source3)
            {
              int key = keyValuePair.Key;
              AIProject.SaveData.AnimalData animalData = keyValuePair.Value;
              if (animalData == null)
                source1[key] = (AIProject.SaveData.Environment.ChickenInfo) null;
              else
                source1[key] = new AIProject.SaveData.Environment.ChickenInfo()
                {
                  name = animalData.Nickname,
                  AnimalData = animalData
                };
            }
          }
          if (source1.IsNullOrEmpty<AIProject.SaveData.Environment.ChickenInfo>())
            return;
        }
        for (int index = 0; index < source1.Count; ++index)
        {
          AIProject.SaveData.AnimalData animalData = source1.GetElement<AIProject.SaveData.Environment.ChickenInfo>(index)?.AnimalData;
          if (animalData != null)
          {
            if (!animalData.InitAnimalTypeID || animalData.AnimalTypeID < 0)
            {
              animalData.AnimalTypeID = AIProject.Animal.AnimalData.GetAnimalTypeID(animalData.AnimalType);
              animalData.InitAnimalTypeID = true;
            }
            AnimalBase animalBase = Singleton<AnimalManager>.Instance.CreateBase(animalData.AnimalTypeID, (int) animalData.BreedingType);
            if (!Object.op_Equality((Object) animalBase, (Object) null))
            {
              ((Component) animalBase).get_transform().SetParent(this.AnimalRoot, true);
              animalData.AnimalID = animalBase.AnimalID;
              if (animalBase is IPetAnimal petAnimal)
                petAnimal.AnimalData = animalData;
              PetChicken chicken = animalBase as PetChicken;
              this.AddChicken(index, chicken);
              if (Object.op_Inequality((Object) chicken, (Object) null))
                chicken.Initialize(this);
            }
          }
        }
      }
    }

    public void AddChicken(int index, PetChicken chicken)
    {
      if (Object.op_Equality((Object) chicken, (Object) null) || this.ChickenList == null || this.ChickenList.Contains(chicken))
        return;
      this.ChickenList.Add(chicken);
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      int mapId = Singleton<Manager.Map>.Instance.MapID;
      int areaIdOnHousingArea = this.AreaIDOnHousingArea;
      Dictionary<int, Dictionary<int, Dictionary<int, AIProject.SaveData.AnimalData>>> chickenDataTable = environment.HousingChickenDataTable;
      Dictionary<int, Dictionary<int, AIProject.SaveData.AnimalData>> dictionary1;
      if (!chickenDataTable.TryGetValue(mapId, out dictionary1) || dictionary1 == null)
        chickenDataTable[mapId] = dictionary1 = new Dictionary<int, Dictionary<int, AIProject.SaveData.AnimalData>>();
      Dictionary<int, AIProject.SaveData.AnimalData> dictionary2;
      if (!dictionary1.TryGetValue(areaIdOnHousingArea, out dictionary2) || dictionary2 == null)
      {
        Dictionary<int, AIProject.SaveData.AnimalData> dictionary3;
        dictionary1[areaIdOnHousingArea] = dictionary3 = new Dictionary<int, AIProject.SaveData.AnimalData>();
      }
      dictionary1[areaIdOnHousingArea][index] = chicken.AnimalData;
    }

    public void RemoveChicken(int index, PetChicken chicken)
    {
      if (Object.op_Equality((Object) chicken, (Object) null) || this.ChickenList.IsNullOrEmpty<PetChicken>() || !this.ChickenList.Contains(chicken))
        return;
      this.ChickenList.Remove(chicken);
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      int mapId = Singleton<Manager.Map>.Instance.MapID;
      int areaIdOnHousingArea = this.AreaIDOnHousingArea;
      Dictionary<int, Dictionary<int, AIProject.SaveData.AnimalData>> source1;
      Dictionary<int, AIProject.SaveData.AnimalData> source2;
      if (!environment.HousingChickenDataTable.TryGetValue(mapId, out source1) || source1.IsNullOrEmpty<int, Dictionary<int, AIProject.SaveData.AnimalData>>() || (!source1.TryGetValue(areaIdOnHousingArea, out source2) || source2.IsNullOrEmpty<int, AIProject.SaveData.AnimalData>()))
        return;
      source2.Remove(index);
    }

    public void DestroyChicken()
    {
      if (this._farmKind != FarmPoint.FarmKind.ChickenCoop)
        return;
      if (this.ChickenList != null)
      {
        this.ChickenList.RemoveAll((Predicate<PetChicken>) (x => Object.op_Equality((Object) x, (Object) null)));
        foreach (PetChicken chicken in this.ChickenList)
        {
          IPetAnimal petAnimal = (IPetAnimal) chicken;
          if (petAnimal != null)
            petAnimal.Release();
          else
            chicken.Release();
        }
        ListPool<PetChicken>.Release(this.ChickenList);
        this.ChickenList = (List<PetChicken>) null;
      }
      AIProject.SaveData.Environment environment = !Singleton<Game>.IsInstance() ? (AIProject.SaveData.Environment) null : Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      Dictionary<int, List<AIProject.SaveData.Environment.ChickenInfo>> chickenTable = environment.ChickenTable;
      if (!chickenTable.TryGetValue(this.RegisterID, out List<AIProject.SaveData.Environment.ChickenInfo> _))
        return;
      chickenTable.Remove(this.RegisterID);
    }

    public enum FarmKind
    {
      Plant,
      ChickenCoop,
      Well,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.PetHomePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Player;
using AIProject.SaveData;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;

namespace AIProject.Animal
{
  public class PetHomePoint : Point, ICommandable
  {
    private static CommandLabel.CommandInfo[] _emptyLabels = new CommandLabel.CommandInfo[0];
    [SerializeField]
    [HideInEditorMode]
    [ReadOnly]
    private int _pointID = -1;
    [SerializeField]
    [HideInEditorMode]
    [ReadOnly]
    private int _animalID = -1;
    private IntReactiveProperty _commandAreaIndex = new IntReactiveProperty(-1);
    private Vector3 _firstLabelPosition = Vector3.get_zero();
    [SerializeField]
    private bool _enableRangeCheck = true;
    [SerializeField]
    private float _checkRadius = 1f;
    private int? _instanceID;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;
    [SerializeField]
    private CommandType _commandType;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _labelPoint;
    [SerializeField]
    private PetHomePoint.HomeKind _homeKind;
    [SerializeField]
    private int _allowableSizeID;
    [SerializeField]
    private Transform[] _rootPoints;
    [SerializeField]
    private PetHomePoint.CommandAreaInfo[] _commandAreaInfos;

    public override int RegisterID
    {
      get
      {
        return this._pointID;
      }
      set
      {
        this._pointID = value;
      }
    }

    public int AnimalID
    {
      get
      {
        return this._animalID;
      }
      protected set
      {
        this._animalID = value;
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

    public Vector3 CommandCenter
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint.get_position() : ((Component) this).get_transform().get_position();
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this).get_transform().get_rotation();
      }
    }

    public Vector3 EulerAngle
    {
      get
      {
        return ((Component) this).get_transform().get_eulerAngles();
      }
    }

    public AIProject.SaveData.Environment.PetHomeInfo SaveData { get; set; }

    public int HousingID { get; set; }

    public Vector3Int GridPoint { get; set; } = Vector3Int.get_zero();

    public int InstanceID
    {
      get
      {
        return (!this._instanceID.HasValue ? (this._instanceID = new int?(((Object) this).GetInstanceID())) : this._instanceID).Value;
      }
    }

    public bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      Vector3 commandCenter = this.CommandCenter;
      commandCenter.y = (__Null) 0.0;
      float num1 = Vector3.Distance(basePosition, commandCenter);
      bool flag;
      if (this.CommandType == CommandType.Forward)
      {
        if ((!this._enableRangeCheck ? (double) radiusA : (double) radiusA + (double) this._checkRadius) < (double) num1)
          return false;
        Vector3 vector3 = commandCenter;
        vector3.y = (__Null) 0.0;
        float num2 = angle / 2f;
        flag = (double) Vector3.Angle(Vector3.op_Subtraction(vector3, basePosition), forward) <= (double) num2;
      }
      else
      {
        float num2 = !this._enableRangeCheck ? radiusB : radiusB + this._checkRadius;
        flag = (double) distance <= (double) num2;
      }
      if (!flag)
        return false;
      PlayerActor player = Manager.Map.GetPlayer();
      return (!Object.op_Inequality((Object) player, (Object) null) || !(player.PlayerController.State is Onbu)) && flag;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (Object.op_Equality((Object) nmAgent, (Object) null) || !((Behaviour) nmAgent).get_isActiveAndEnabled())
        return false;
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag = true;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        switch (this._homeKind)
        {
          case PetHomePoint.HomeKind.PetMat:
            nmAgent.CalculatePath(this.Position, this._pathForCalc);
            flag &= this._pathForCalc.get_status() == 0;
            break;
          case PetHomePoint.HomeKind.FishTank:
            flag = false;
            int num1 = -1;
            if (!((IReadOnlyList<PetHomePoint.CommandAreaInfo>) this._commandAreaInfos).IsNullOrEmpty<PetHomePoint.CommandAreaInfo>())
            {
              List<ValueTuple<int, Vector3>> toRelease = ListPool<ValueTuple<int, Vector3>>.Get();
              for (int index = 0; index < this._commandAreaInfos.Length; ++index)
              {
                PetHomePoint.CommandAreaInfo commandAreaInfo = this._commandAreaInfos[index];
                if (commandAreaInfo != null && commandAreaInfo.IsActive)
                  toRelease.Add(new ValueTuple<int, Vector3>(index, commandAreaInfo.BasePoint.get_position()));
              }
              Vector3 position = ((Component) nmAgent).get_transform().get_position();
              toRelease.Sort((Comparison<ValueTuple<int, Vector3>>) ((x, y) => (int) ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction((Vector3) x.Item2, position)) - (double) Vector3.SqrMagnitude(Vector3.op_Subtraction((Vector3) y.Item2, position)))));
              for (int index = 0; index < toRelease.Count; ++index)
              {
                ValueTuple<int, Vector3> valueTuple = toRelease[index];
                nmAgent.CalculatePath((Vector3) valueTuple.Item2, this._pathForCalc);
                if (flag = this._pathForCalc.get_status() == 0)
                {
                  num1 = (int) valueTuple.Item1;
                  break;
                }
              }
              ListPool<ValueTuple<int, Vector3>>.Release(toRelease);
            }
            ((ReactiveProperty<int>) this._commandAreaIndex).set_Value(num1);
            if (!flag)
            {
              nmAgent.CalculatePath(this.Position, this._pathForCalc);
              flag = this._pathForCalc.get_status() == 0;
              break;
            }
            break;
        }
        float num2 = 0.0f;
        Vector3[] corners = this._pathForCalc.get_corners();
        for (int index = 0; index < corners.Length - 1; ++index)
        {
          float num3 = Vector3.Distance(corners[index], corners[index + 1]);
          num2 += num3;
          float num4 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
          if ((double) num2 > (double) num4)
          {
            flag = false;
            break;
          }
        }
      }
      return flag;
    }

    public bool IsImpossible { get; protected set; }

    public Actor CommandPartner { get; set; }

    public bool SetImpossible(bool _value, Actor _actor)
    {
      if (this.IsImpossible == _value || _value && Object.op_Inequality((Object) this.CommandPartner, (Object) null))
        return false;
      this.IsImpossible = _value;
      this.CommandPartner = !_value ? (Actor) null : _actor;
      return true;
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

    public CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return this._labels;
      }
    }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return PetHomePoint._emptyLabels;
      }
    }

    public ObjectLayer Layer
    {
      get
      {
        return ObjectLayer.Command;
      }
    }

    public CommandType CommandType
    {
      get
      {
        return this._commandType;
      }
    }

    public Transform CommandBasePoint
    {
      get
      {
        return Object.op_Inequality((Object) this._commandBasePoint, (Object) null) ? this._commandBasePoint : ((Component) this).get_transform();
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return Object.op_Inequality((Object) this._labelPoint, (Object) null) ? this._labelPoint : ((Component) this).get_transform();
      }
    }

    public bool EnableRangeCheck
    {
      get
      {
        return this._enableRangeCheck;
      }
    }

    public float CheckRadius
    {
      get
      {
        return this._checkRadius;
      }
    }

    public PetHomePoint.HomeKind Kind
    {
      get
      {
        return this._homeKind;
      }
    }

    public int AllowableSizeID
    {
      get
      {
        return this._allowableSizeID;
      }
    }

    public Transform[] RootPoints
    {
      get
      {
        return this._rootPoints;
      }
    }

    public IPetAnimal User { get; protected set; }

    public bool UsedPoint()
    {
      return this.User != null;
    }

    protected override void Start()
    {
      base.Start();
      this._firstLabelPosition = this.LabelPoint.get_position();
      ObservableExtensions.Subscribe<int>(Observable.DistinctUntilChanged<int>((IObservable<M0>) this._commandAreaIndex), (Action<M0>) (index =>
      {
        Transform labelPoint = this.LabelPoint;
        if (Object.op_Equality((Object) labelPoint, (Object) null))
          return;
        if (0 <= index && !((IReadOnlyList<PetHomePoint.CommandAreaInfo>) this._commandAreaInfos).IsNullOrEmpty<PetHomePoint.CommandAreaInfo>() && index < this._commandAreaInfos.Length)
        {
          PetHomePoint.CommandAreaInfo commandAreaInfo = this._commandAreaInfos[index];
          if (commandAreaInfo != null && Object.op_Inequality((Object) commandAreaInfo.LabelPoint, (Object) null))
          {
            labelPoint.set_position(commandAreaInfo.LabelPoint.get_position());
            return;
          }
        }
        labelPoint.set_position(this._firstLabelPosition);
      }));
      if (!Singleton<Resources>.IsInstance())
        return;
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName);
        this._commandBasePoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (Object.op_Equality((Object) this._labelPoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.PetHomePointLabelTargetName);
        this._labelPoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (((IReadOnlyList<Transform>) this._rootPoints).IsNullOrEmpty<Transform>())
      {
        this.SetSizeRootPoints();
        if (((IReadOnlyList<Transform>) this._rootPoints).IsNullOrEmpty<Transform>())
          this._rootPoints = new Transform[1]
          {
            ((Component) this).get_transform()
          };
      }
      this.InitializeCommandLabels();
    }

    private void InitializeCommandLabels()
    {
      Resources resources = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
      if (Object.op_Equality((Object) resources, (Object) null))
        return;
      CommonDefine.CommonIconGroup icon = resources.CommonDefine.Icon;
      ValueTuple<int, List<string>> valueTuple;
      resources.AnimalTable.PetHomeUIInfoTable.TryGetValue((int) this._homeKind, out valueTuple);
      Dictionary<int, Sprite> actionIconTable = resources.itemIconTables.ActionIconTable;
      Sprite sprite = (Sprite) null;
      actionIconTable.TryGetValue((int) valueTuple.Item1, out sprite);
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      switch (this._homeKind)
      {
        case PetHomePoint.HomeKind.PetMat:
          this._labels = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = ((List<string>) valueTuple.Item2).GetElement<string>(index),
              Icon = sprite,
              IsHold = true,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (Action) (() =>
              {
                PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
                if (Object.op_Equality((Object) playerActor, (Object) null))
                  return;
                playerActor.CurrentPetHomePoint = this;
                MapUIContainer.SetActivePetHomeUI(true);
                playerActor.PlayerController.ChangeState("Harvest");
              })
            }
          };
          break;
        case PetHomePoint.HomeKind.FishTank:
          this._labels = new CommandLabel.CommandInfo[1]
          {
            new CommandLabel.CommandInfo()
            {
              Text = ((List<string>) valueTuple.Item2).GetElement<string>(index),
              Icon = sprite,
              IsHold = true,
              TargetSpriteInfo = icon.ActionSpriteInfo,
              Transform = this.LabelPoint,
              Condition = (Func<PlayerActor, bool>) null,
              Event = (Action) (() =>
              {
                PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
                if (Object.op_Equality((Object) playerActor, (Object) null))
                  return;
                playerActor.CurrentPetHomePoint = this;
                MapUIContainer.SetActivePetHomeUI(true);
                playerActor.PlayerController.ChangeState("Harvest");
              })
            }
          };
          break;
      }
    }

    public void SetUser(IPetAnimal animal)
    {
      if (animal == null)
        return;
      if (this.User != null)
        this.User.Release();
      this.User = animal;
      this.SaveData.AnimalData = animal.AnimalData;
      animal.Initialize(this);
    }

    public void RemoveUser()
    {
      this.SaveData.AnimalData = (AIProject.SaveData.AnimalData) null;
      if (this.User == null)
        return;
      this.User.Release();
      this.User = (IPetAnimal) null;
    }

    public void SetRootPoint(int pointID)
    {
      if (this.User == null)
        return;
      Transform transform = this._rootPoints.GetElement<Transform>(pointID) ?? ((Component) this).get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      this.User.Position = transform.get_position();
      this.User.Rotation = transform.get_rotation();
    }

    public void SetRootPoint(int pointID, IPetAnimal animal)
    {
      if (animal == null)
        return;
      Transform element = this._rootPoints.GetElement<Transform>(pointID);
      Transform transform = !Object.op_Inequality((Object) element, (Object) null) ? ((Component) this).get_transform() : element;
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      animal.Position = transform.get_position();
      animal.Rotation = transform.get_rotation();
    }

    public Transform GetRootPoint(int pointID)
    {
      Transform element = this._rootPoints.GetElement<Transform>(pointID);
      return !Object.op_Inequality((Object) element, (Object) null) ? ((Component) this).get_transform() : element;
    }

    private void OnDestroy()
    {
      if (this.User == null)
        return;
      this.User.Release();
    }

    public void SetSizeRootPoints()
    {
      List<Transform> list = ListPool<Transform>.Get();
      ((Component) this).get_transform().FindMatchLoop("^pet_point_[0-9]+$", ref list);
      if (!((IReadOnlyList<Transform>) list).IsNullOrEmpty<Transform>())
      {
        Dictionary<int, Transform> toRelease = DictionaryPool<int, Transform>.Get();
        int num = 0;
        for (int index = 0; index < list.Count; ++index)
        {
          Transform transform = list[index];
          Match match = Regex.Match(((Object) transform).get_name(), "[0-9]+");
          int result;
          if (match.Success && int.TryParse(match.Value, out result))
          {
            toRelease[result] = transform;
            num = Mathf.Max(num, result + 1);
          }
        }
        this._rootPoints = new Transform[Mathf.Min(num, 100)];
        using (Dictionary<int, Transform>.Enumerator enumerator = toRelease.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, Transform> current = enumerator.Current;
            int key = current.Key;
            if (key >= 0 && this._rootPoints.Length > key)
              this._rootPoints[current.Key] = current.Value;
          }
        }
        DictionaryPool<int, Transform>.Release(toRelease);
      }
      ListPool<Transform>.Release(list);
    }

    public bool CanDelete()
    {
      if (this.SaveData == null || this.SaveData.AnimalData == null || (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance()))
        return true;
      AIProject.SaveData.AnimalData animalData = this.SaveData.AnimalData;
      int itemCategoryId = animalData.ItemCategoryID;
      int itemId = animalData.ItemID;
      if (Singleton<Resources>.Instance.GameInfo.GetItem(itemCategoryId, itemId) == null)
        return true;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      List<ValueTuple<int, List<StuffItem>>> inventoryList = instance.GetInventoryList();
      if (((IReadOnlyList<ValueTuple<int, List<StuffItem>>>) inventoryList).IsNullOrEmpty<ValueTuple<int, List<StuffItem>>>())
      {
        instance.ReturnInventoryList(inventoryList);
        return true;
      }
      StuffItem stuffItem = new StuffItem(itemCategoryId, itemId, 1);
      bool flag = false;
      using (List<ValueTuple<int, List<StuffItem>>>.Enumerator enumerator = inventoryList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<StuffItem>> current = enumerator.Current;
          int capacity = (int) current.Item1;
          List<StuffItem> self = (List<StuffItem>) current.Item2;
          int possible = 0;
          StuffItemExtensions.CanAddItem((IReadOnlyCollection<StuffItem>) self, capacity, stuffItem, out possible);
          flag = stuffItem.Count <= possible;
          if (flag)
          {
            self.AddItem(stuffItem);
            break;
          }
        }
      }
      return flag;
    }

    public enum HomeKind
    {
      PetMat,
      FishTank,
    }

    [Serializable]
    public class CommandAreaInfo
    {
      [SerializeField]
      private Transform _labelPoint;
      [SerializeField]
      private Transform _basePoint;

      public bool IsActive
      {
        get
        {
          return Object.op_Inequality((Object) this._labelPoint, (Object) null) && Object.op_Inequality((Object) this._basePoint, (Object) null);
        }
      }

      public Transform LabelPoint
      {
        get
        {
          return this._labelPoint;
        }
      }

      public Transform BasePoint
      {
        get
        {
          return this._basePoint;
        }
      }
    }
  }
}

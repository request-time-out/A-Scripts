// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.FishTankPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class FishTankPoint : Point, ICommandable
  {
    private CommandLabel.CommandInfo[] emptyLabel = new CommandLabel.CommandInfo[0];
    [SerializeField]
    private bool _enableRangeCheck = true;
    [SerializeField]
    private float _checkRadius = 1f;
    [SerializeField]
    private Transform[] _sizeRootPoints = new Transform[0];
    private int? _instanceID;
    private NavMeshPath _pathForCalc;
    [SerializeField]
    private CommandType _commandType;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _labelPoint;
    [SerializeField]
    private int _allowableSizeID;

    public PetFish Fish { get; set; }

    public int TankID { get; private set; } = -1;

    public int ItemID { get; private set; } = -1;

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
      if (this._commandType == CommandType.Forward)
      {
        if ((!this._enableRangeCheck ? (double) radiusA : (double) radiusA + (double) this._checkRadius) < (double) num1)
          return false;
        Vector3 vector3 = commandCenter;
        vector3.y = (__Null) 0.0;
        float num2 = angle / 2f;
        return (double) Vector3.Angle(Vector3.op_Subtraction(vector3, basePosition), forward) <= (double) num2;
      }
      float num3 = !this._enableRangeCheck ? radiusB : radiusB + this._checkRadius;
      return (double) distance <= (double) num3;
    }

    public bool IsReachable(NavMeshAgent nmAgent, float radiusA, float radiusB)
    {
      if (this._pathForCalc == null)
        this._pathForCalc = new NavMeshPath();
      bool flag1 = true;
      bool flag2;
      if (((Behaviour) nmAgent).get_isActiveAndEnabled())
      {
        nmAgent.CalculatePath(this.Position, this._pathForCalc);
        flag2 = flag1 & this._pathForCalc.get_status() == 0;
        float num1 = 0.0f;
        Vector3[] corners = this._pathForCalc.get_corners();
        for (int index = 0; index < corners.Length - 1; ++index)
        {
          float num2 = Vector3.Distance(corners[index], corners[index + 1]);
          num1 += num2;
          float num3 = this.CommandType != CommandType.Forward ? radiusB : radiusA;
          if ((double) num1 > (double) num3)
          {
            flag2 = false;
            break;
          }
        }
      }
      else
        flag2 = false;
      return flag2;
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
        return true;
      }
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

    public CommandLabel.CommandInfo[] Labels { get; private set; }

    public CommandLabel.CommandInfo[] DateLabels
    {
      get
      {
        return this.emptyLabel;
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
        return this._commandBasePoint ?? ((Component) this).get_transform();
      }
    }

    public Transform LabelPoint
    {
      get
      {
        return this._labelPoint ?? ((Component) this).get_transform();
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

    public int AllowableSizeID
    {
      get
      {
        return this._allowableSizeID;
      }
    }

    public Transform[] SizeRootPoints
    {
      get
      {
        return this._sizeRootPoints;
      }
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName);
        this._commandBasePoint = !Object.op_Inequality((Object) loop, (Object) null) ? ((Component) this).get_transform() : loop.get_transform();
      }
      if (!Object.op_Equality((Object) this._labelPoint, (Object) null))
        ;
      if (((IReadOnlyList<Transform>) this._sizeRootPoints).IsNullOrEmpty<Transform>())
        this.SetSizeRootPoints();
      this.InitializeCommandLabels();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.AddCommandableObject();
    }

    protected override void OnDisable()
    {
      this.RemoveCommandableObject();
      base.OnDisable();
    }

    private void InitializeCommandLabels()
    {
      if (!((IReadOnlyList<CommandLabel.CommandInfo>) this.Labels).IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      Sprite sprite = (Sprite) null;
      Resources instance1 = Singleton<Resources>.Instance;
      Manager.Map instance2 = Singleton<Manager.Map>.Instance;
      instance1.itemIconTables.ActionIconTable.TryGetValue(icon.FishTankIconID, out sprite);
      this.Labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = "水槽",
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = this.LabelPoint,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() => Debug.Log((object) "水槽：魚追加UIを表示する"))
        }
      };
    }

    private void AddCommandableObject()
    {
      CommandArea commandArea = (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController?.CommandArea;
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      commandArea.AddCommandableObject((ICommandable) this);
    }

    private void RemoveCommandableObject()
    {
      CommandArea commandArea = (!Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player)?.PlayerController?.CommandArea;
      if (Object.op_Equality((Object) commandArea, (Object) null))
        return;
      bool flag = commandArea.ContainsConsiderationObject((ICommandable) this);
      commandArea.RemoveCommandableObject((ICommandable) this);
      if (!flag)
        return;
      commandArea.RefreshCommands();
    }

    private void SetSizeRootPoints()
    {
      List<Transform> list = ListPool<Transform>.Get();
      ((Component) this).get_transform().FindMatchLoop("^fish_point_[0-9]+$", ref list);
      if (!((IReadOnlyList<Transform>) list).IsNullOrEmpty<Transform>())
      {
        Dictionary<int, Transform> toRelease = DictionaryPool<int, Transform>.Get();
        int length = 0;
        for (int index = 0; index < list.Count; ++index)
        {
          Transform transform = list[index];
          Match match = Regex.Match(((Object) transform).get_name(), "[0-9]+");
          int result;
          if (match.Success && int.TryParse(match.Value, out result))
          {
            toRelease[result] = transform;
            length = Mathf.Max(length, result + 1);
          }
        }
        this._sizeRootPoints = new Transform[length];
        using (Dictionary<int, Transform>.Enumerator enumerator = toRelease.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, Transform> current = enumerator.Current;
            this._sizeRootPoints[current.Key] = current.Value;
          }
        }
        DictionaryPool<int, Transform>.Release(toRelease);
      }
      ListPool<Transform>.Release(list);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.BasePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class BasePoint : Point, ICommandable
  {
    [SerializeField]
    private bool _enabledRangeCheck = true;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private bool _isHousing = true;
    [SerializeField]
    private List<Transform> _recoverPoints = new List<Transform>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private int _areaIDInHousing;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _housingCenter;
    [SerializeField]
    private Transform _warpPoint;
    private int? _hashCode;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public int AreaIDInHousing
    {
      get
      {
        return this._areaIDInHousing;
      }
    }

    public float Radius
    {
      get
      {
        return this._radius;
      }
    }

    public bool IsHousing
    {
      get
      {
        return this._isHousing;
      }
    }

    public Transform WarpPoint
    {
      get
      {
        return this._warpPoint;
      }
    }

    public List<Transform> RecoverPoints
    {
      get
      {
        return this._recoverPoints;
      }
    }

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

    public bool IsNeutralCommand
    {
      get
      {
        return !this.TutorialHideMode();
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

    protected override void Start()
    {
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      if (Object.op_Equality((Object) this._warpPoint, (Object) null))
        this._warpPoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.BasePointWarpTargetName)?.get_transform() ?? ((Component) this).get_transform();
      if (Object.op_Equality((Object) this._housingCenter, (Object) null))
        this._housingCenter = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.HousingCenterTargetName)?.get_transform() ?? ((Component) this).get_transform();
      if (this._recoverPoints.IsNullOrEmpty<Transform>() || this._recoverPoints.Count < 4)
      {
        this._recoverPoints.Clear();
        foreach (string recoveryTargetName in Singleton<Resources>.Instance.DefinePack.MapDefines.DevicePointRecoveryTargetNames)
        {
          GameObject loop = ((Component) this).get_transform().FindLoop(recoveryTargetName);
          if (Object.op_Inequality((Object) loop, (Object) null))
            this._recoverPoints.Add(loop.get_transform());
        }
      }
      Singleton<Manager.Map>.Instance.HousingRecoveryPointTable[this._id] = this._recoverPoints;
      if (this._isHousing)
        Singleton<Manager.Map>.Instance.HousingPointTable[this._id] = this._housingCenter;
      if (Singleton<Game>.Instance.WorldData.Cleared && this._id >= 3)
        Singleton<Manager.Map>.Instance.SetBaseOpenState(this._id, true);
      base.Start();
      if (!this._isHousing)
        return;
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.BaseIconID, out sprite);
      Transform transform = ((Component) this).get_transform().FindLoop(mapDefines.BasePointLabelTargetName)?.get_transform() ?? ((Component) this).get_transform();
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = "拠点",
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = transform,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            if (Singleton<Manager.Map>.Instance.SetBaseOpenState(this._id, true))
            {
              string str;
              if (Singleton<Resources>.Instance.itemIconTables.BaseName.TryGetValue(this._id, out str))
                MapUIContainer.AddSystemLog(string.Format("{0} に移動できるようになりました。", (object) str), true);
              else
                MapUIContainer.AddSystemLog(string.Format("拠点{0}に移動できるようになりました。", (object) this._id), true);
            }
            if (Manager.Map.GetTutorialProgress() == 10)
              Manager.Map.SetTutorialProgress(11);
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            Singleton<Manager.Map>.Instance.HousingID = this._id;
            Singleton<Manager.Map>.Instance.HousingAreaID = this._areaIDInHousing;
            Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("BaseMenu");
          })
        }
      };
    }

    public bool TutorialHideMode()
    {
      if (!Manager.Map.TutorialMode)
        return false;
      int num1 = 10;
      int num2 = 12;
      int tutorialProgress = Manager.Map.GetTutorialProgress();
      return num1 > tutorialProgress || tutorialProgress > num2;
    }
  }
}

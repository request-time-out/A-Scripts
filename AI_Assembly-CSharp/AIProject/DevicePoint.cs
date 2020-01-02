// Decompiled with JetBrains decompiler
// Type: AIProject.DevicePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using AIProject.SaveData;
using IllusionUtility.GetUtility;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class DevicePoint : Point, ICommandable
  {
    [SerializeField]
    private bool _enabledRangeCheck = true;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField]
    private List<Transform> _recoverPoints = new List<Transform>();
    private Queue<PlayState.Info> _inQueue = new Queue<PlayState.Info>();
    private Queue<PlayState.Info> _outQueue = new Queue<PlayState.Info>();
    [SerializeField]
    private int _id;
    [SerializeField]
    private Transform _commandBasePoint;
    [SerializeField]
    private Transform _pivotPoint;
    [SerializeField]
    private Transform _playerRecoverPoint;
    private int? _hashCode;
    private NavMeshPath _pathForCalc;
    private CommandLabel.CommandInfo[] _labels;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private Animator _animator;
    private IEnumerator _inAnimEnumerator;
    private IDisposable _inAnimDisposable;
    private IEnumerator _outAnimEnumerator;
    private IDisposable _outAnimDisposable;

    public int ID
    {
      get
      {
        return this._id;
      }
    }

    public Transform PivotPoint
    {
      get
      {
        return this._pivotPoint;
      }
    }

    public List<Transform> RecoverPoints
    {
      get
      {
        return this._recoverPoints;
      }
    }

    public Transform PlayerRecoverPoint
    {
      get
      {
        return this._playerRecoverPoint;
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
      Vector3 position = this.Position;
      position.y = (__Null) 0.0;
      float num = angle / 2f;
      return (double) Vector3.Angle(Vector3.op_Subtraction(position, basePosition), forward) <= (double) num;
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
        AgentData agentData;
        return !this.TutorialHideMode() && Singleton<Game>.Instance.WorldData.AgentTable.TryGetValue(this._id, out agentData) && agentData.OpenState;
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

    public Animator Animator
    {
      get
      {
        return this._animator;
      }
    }

    protected override void Start()
    {
      if (DevicePointAnimData.AnimatorItemTable.TryGetValue(this._id, ref this._animator))
        this._animator.set_runtimeAnimatorController(Singleton<Resources>.Instance.Animation.GetItemAnimator(Singleton<Resources>.Instance.CommonDefine.ItemAnims.PodAnimatorID));
      if (Object.op_Equality((Object) this._commandBasePoint, (Object) null))
        this._commandBasePoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.CommandTargetName)?.get_transform() ?? ((Component) this).get_transform();
      if (Object.op_Equality((Object) this._pivotPoint, (Object) null))
        this._pivotPoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.DevicePointPivotTargetName)?.get_transform() ?? ((Component) this).get_transform();
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
      if (Object.op_Equality((Object) this._playerRecoverPoint, (Object) null))
        this._playerRecoverPoint = ((Component) this).get_transform().FindLoop(Singleton<Resources>.Instance.DefinePack.MapDefines.DevicePointPlayerRecoveryTargetName)?.get_transform();
      base.Start();
      CommonDefine.CommonIconGroup icon = Singleton<Resources>.Instance.CommonDefine.Icon;
      DefinePack.MapGroup mapDefines = Singleton<Resources>.Instance.DefinePack.MapDefines;
      Sprite sprite;
      Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.DeviceIconID, out sprite);
      Transform transform = ((Component) this).get_transform().FindLoop(mapDefines.DevicePointLabelTargetName)?.get_transform() ?? ((Component) this).get_transform();
      this._labels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = "データ端末",
          Icon = sprite,
          IsHold = true,
          TargetSpriteInfo = icon.ActionSpriteInfo,
          Transform = transform,
          Condition = (Func<PlayerActor, bool>) null,
          Event = (Action) (() =>
          {
            Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.BootDevice);
            MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
            Singleton<Manager.Map>.Instance.Player.CurrentDevicePoint = this;
            Singleton<Manager.Map>.Instance.Player.StashData();
            Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("DeviceMenu");
          })
        }
      };
    }

    public void PlayInAnimation()
    {
      this._inQueue.Clear();
      foreach (string podInState in Singleton<Resources>.Instance.CommonDefine.ItemAnims.PodInStates)
        this._inQueue.Enqueue(new PlayState.Info(podInState, 0));
      this._inAnimEnumerator = this.StartInAnimation();
      this._inAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._inAnimEnumerator), false));
    }

    public void StopInAnimation()
    {
      if (this._inAnimDisposable == null)
        return;
      this._inAnimDisposable.Dispose();
      this._inAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartInAnimation()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DevicePoint.\u003CStartInAnimation\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void PlayOutAnimation()
    {
      this._outQueue.Clear();
      foreach (string podOutState in Singleton<Resources>.Instance.CommonDefine.ItemAnims.PodOutStates)
        this._outQueue.Enqueue(new PlayState.Info(podOutState, 0));
      this._outAnimEnumerator = this.StartOutAnimation();
      this._outAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._outAnimEnumerator), false));
    }

    public void StopOutAnimation()
    {
      if (this._outAnimDisposable == null)
        return;
      this._outAnimDisposable.Dispose();
      this._outAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartOutAnimation()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DevicePoint.\u003CStartOutAnimation\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public bool PlayingInAnimation
    {
      get
      {
        return this._inAnimEnumerator != null;
      }
    }

    public bool PlayingOutAnimation
    {
      get
      {
        return this._outAnimEnumerator != null;
      }
    }
  }
}

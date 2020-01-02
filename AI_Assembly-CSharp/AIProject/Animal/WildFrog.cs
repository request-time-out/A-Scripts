// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.WildFrog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class WildFrog : GroundInsect
  {
    [SerializeField]
    [MinMaxSlider(0.0f, 200f, true)]
    [Tooltip("次の座標を生成する時の最短/最大距離")]
    private Vector2 nextPointDistance = new Vector2(5f, 15f);
    [SerializeField]
    [MinMaxSlider(0.1f, 60f, true)]
    [Tooltip("待機時間")]
    private Vector2 idleTimeSecond = Vector2.get_zero();
    [SerializeField]
    [Min(0.001f)]
    [Tooltip("逃げの状態が何秒続いたら消えるか")]
    private float escapeDestroyTimeSecond = 4f;
    [SerializeField]
    [Min(0.001f)]
    [Tooltip("透明になるフェードアウトの時間")]
    private float fadeOutTimeSecond = 0.8f;
    [SerializeField]
    private string materialAlphaParamName = "_AlphaEx";
    protected List<Vector3> movePoints = new List<Vector3>();
    protected NavMeshHit navHit = (NavMeshHit) null;
    [SerializeField]
    [ReadOnly]
    [HideInEditorMode]
    private FrogHabitatPoint habitatPoint;
    [SerializeField]
    [ReadOnly]
    private Material material;
    private int materialAlphaParamID;
    protected int locomotionCount;
    protected NavMeshPath navPath;
    private bool locomotionPossible;
    private IDisposable setLocomotionPointDisposable;
    private float prevNormalizedTime;
    private IDisposable addMovePointDisposable;
    private NavMeshPath path;

    public string MaterialAlphaParamName
    {
      get
      {
        return this.materialAlphaParamName;
      }
      set
      {
        this.materialAlphaParamName = value;
        this.materialAlphaParamID = Shader.PropertyToID(this.materialAlphaParamName);
      }
    }

    public override bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      return !Manager.Map.TutorialMode && base.Entered(basePosition, distance, radiusA, radiusB, angle, forward);
    }

    protected override void InitializeCommandLabels()
    {
      if (!((IReadOnlyList<CommandLabel.CommandInfo>) this.getLabels).IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
      CommonDefine.CommonIconGroup commonIconGroup = !Object.op_Inequality((Object) commonDefine, (Object) null) ? (CommonDefine.CommonIconGroup) null : commonDefine.Icon;
      Resources instance = Singleton<Resources>.Instance;
      int guideCancelId = commonIconGroup.GuideCancelID;
      Sprite sprite;
      instance.itemIconTables.InputIconTable.TryGetValue(guideCancelId, out sprite);
      List<string> source;
      instance.Map.EventPointCommandLabelTextTable.TryGetValue(18, out source);
      int index = !Singleton<GameSystem>.IsInstance() ? 0 : Singleton<GameSystem>.Instance.languageInt;
      this.getLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = source.GetElement<string>(index),
          Transform = this.LabelPoint,
          IsHold = false,
          Icon = sprite,
          TargetSpriteInfo = commonIconGroup?.CharaSpriteInfo,
          Event = (System.Action) (() =>
          {
            bool flag1 = true;
            if (Object.op_Inequality((Object) this.habitatPoint, (Object) null) && Singleton<Resources>.IsInstance() && Singleton<Manager.Map>.IsInstance())
            {
              PlayerActor player = Singleton<Manager.Map>.Instance.Player;
              Dictionary<int, ItemTableElement> tableInFrogPoint = Singleton<Resources>.Instance.GameInfo.GetItemTableInFrogPoint(this.habitatPoint.ItemID);
              Actor.SearchInfo searchInfo = player.RandomAddItem(tableInFrogPoint, true);
              if (searchInfo.IsSuccess)
              {
                bool flag2 = false;
                foreach (Actor.ItemSearchInfo itemSearchInfo in searchInfo.ItemList)
                {
                  PlayerData playerData = player.PlayerData;
                  List<StuffItem> itemList = playerData.ItemList;
                  StuffItem stuffItem = new StuffItem(itemSearchInfo.categoryID, itemSearchInfo.id, itemSearchInfo.count);
                  int possible;
                  if (StuffItemExtensions.CanAddItem((IReadOnlyCollection<StuffItem>) itemList, playerData.InventorySlotMax, stuffItem, out possible) && 0 < possible)
                  {
                    int count = Mathf.Min(possible, itemSearchInfo.count);
                    itemList.AddItem(stuffItem, count, playerData.InventorySlotMax);
                    MapUIContainer.AddSystemItemLog(Singleton<Resources>.Instance.GameInfo.GetItem(itemSearchInfo.categoryID, itemSearchInfo.id), count, true);
                    flag2 = true;
                  }
                }
                if (!flag2)
                {
                  flag1 = false;
                  MapUIContainer.PushWarningMessage(Popup.Warning.Type.PouchIsFull);
                }
              }
              else
                MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
            }
            if (!flag1)
              return;
            this.Destroy();
          })
        }
      };
    }

    protected bool EmptyMovePoints
    {
      get
      {
        return ((IReadOnlyList<Vector3>) this.movePoints).IsNullOrEmpty<Vector3>();
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (!this.materialAlphaParamName.IsNullOrEmpty())
        this.materialAlphaParamID = Shader.PropertyToID(this.materialAlphaParamName);
      if (Object.op_Equality((Object) this.NavMeshCon, (Object) null))
        return;
      this.NavMeshCon.SetEnabled(false);
      this.navPath = new NavMeshPath();
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
      {
        this.habitatPoint.StopUse(this);
        this.habitatPoint = (FrogHabitatPoint) null;
      }
      base.OnDestroy();
    }

    public void Initialize(FrogHabitatPoint _habitatPoint)
    {
      this.Clear();
      if (Object.op_Equality((Object) (this.habitatPoint = _habitatPoint), (Object) null))
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      else if (!this.habitatPoint.SetUse(this))
      {
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      }
      else
      {
        MapArea ownerArea = this.habitatPoint.OwnerArea;
        this.ChunkID = !Object.op_Inequality((Object) ownerArea, (Object) null) ? 0 : ownerArea.ChunkID;
        this.LoadBody();
        this.SetStateData();
        this.NavMeshCon.Animator = this.animator;
        if (Object.op_Inequality((Object) this.animator, (Object) null))
          this.rootMotion = ((Component) this.animator).GetOrAddComponent<AnimalRootMotion>();
        SkinnedMeshRenderer componentInChildren = (SkinnedMeshRenderer) ((Component) this).GetComponentInChildren<SkinnedMeshRenderer>(true);
        this.material = !Object.op_Inequality((Object) componentInChildren, (Object) null) ? (Material) null : ((Renderer) componentInChildren).get_material();
        this.rootMotion.OnMove = (System.Action<Vector3, Quaternion>) ((p, q) =>
        {
          this.NavMeshCon.Move(Vector3.op_Subtraction(p, this.Position));
          Quaternion quaternion = Quaternion.op_Multiply(q, Quaternion.Inverse(this.Rotation));
          Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
          eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
          WildFrog wildFrog = this;
          // ISSUE: explicit non-virtual call
          // ISSUE: explicit non-virtual call
          __nonvirtual (wildFrog.Rotation) = Quaternion.op_Multiply(__nonvirtual (wildFrog.Rotation), Quaternion.Euler(eulerAngles));
        });
        bool flag = false;
        this.BodyEnabled = flag;
        this.MarkerEnabled = flag;
        this.NavMeshCon.SetEnabled(false);
        this.TargetMapArea = this.habitatPoint.OwnerArea;
        this.IsNearPlayer = false;
        this.SetState(AnimalState.Start, (System.Action) null);
      }
    }

    public bool IsNearPlayer { get; private set; }

    private void ToEscapeState(PlayerActor _player)
    {
      this.IsNearPlayer = true;
      this.TargetActor = (Actor) _player;
      if (this.CurrentState != AnimalState.Idle && this.CurrentState != AnimalState.Locomotion)
        return;
      this.SetState(AnimalState.Escape, (System.Action) null);
    }

    protected override void OnNearPlayerActorEnter(PlayerActor _player)
    {
      if (this.IsNearPlayer)
        return;
      this.ToEscapeState(_player);
    }

    protected override void OnNearPlayerActorStay(PlayerActor _player)
    {
      if (this.IsNearPlayer)
        return;
      this.ToEscapeState(_player);
    }

    protected override void OnFarPlayerActorExit(PlayerActor _player)
    {
      this.IsNearPlayer = false;
      if (this.CurrentState != AnimalState.Escape)
        return;
      this.SetState(AnimalState.Idle, (System.Action) null);
    }

    protected override void EnterStart()
    {
      this.ClearMovePointList();
    }

    protected override void OnStart()
    {
      if (!NavMesh.SamplePosition(this.GetRandomMoveAreaPoint(), ref this.navHit, 5f, this.NavMeshCon.AreaMask) || !((NavMeshHit) ref this.navHit).get_hit())
        return;
      this.Position = ((NavMeshHit) ref this.navHit).get_position();
      bool flag = true;
      this.BodyEnabled = flag;
      this.MarkerEnabled = flag;
      this.StartAddMovePoint();
      this.NavMeshCon.SetEnabled(true);
      this.Active = true;
      this.SetState(AnimalState.Idle, (System.Action) null);
    }

    protected override void EnterIdle()
    {
      this.PlayInAnim(AnimationCategoryID.Idle, (System.Action) null);
      this.NavMeshCon.MoveUpdateEnabled = false;
      this.StateTimeLimit = Random.Range((float) this.idleTimeSecond.x, (float) this.idleTimeSecond.y);
    }

    protected override void OnIdle()
    {
      this.StateCounter += Time.get_deltaTime();
      if ((double) this.StateCounter < (double) this.StateTimeLimit)
        return;
      this.SetState(AnimalState.Locomotion, (System.Action) null);
    }

    protected override void ExitIdle()
    {
    }

    private void SetLocomotionPoint()
    {
      this.locomotionPossible = false;
      if (this.setLocomotionPointDisposable != null)
        this.setLocomotionPointDisposable.Dispose();
      this.setLocomotionPointDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeWhile<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => !this.NavMeshCon.IsMoving)), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => !((IReadOnlyList<Vector3>) this.movePoints).IsNullOrEmpty<Vector3>())), (Func<M0, bool>) (_ => !this.NavMeshCon.Agent.get_pathPending())), (System.Action<M0>) (_ => this.NavMeshCon.SetDestination(this.movePoints.Pop<Vector3>(), false)), (System.Action) (() => this.locomotionPossible = true));
    }

    protected override void EnterLocomotion()
    {
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (System.Action) null);
      this.NavMeshCon.MoveUpdateEnabled = true;
      this.locomotionCount = Random.Range(1, 3);
      this.SetLocomotionPoint();
    }

    protected override void OnLocomotion()
    {
      if (!this.locomotionPossible || this.NavMeshCon.IsMoving || !this.NavMeshCon.IsReached)
        return;
      --this.locomotionCount;
      if (this.locomotionCount <= 0)
        this.SetState(AnimalState.Idle, (System.Action) null);
      else
        this.SetLocomotionPoint();
    }

    protected override void ExitLocomotion()
    {
      if (this.setLocomotionPointDisposable != null)
        this.setLocomotionPointDisposable.Dispose();
      this.setLocomotionPointDisposable = (IDisposable) null;
      this.NavMeshCon.Refresh();
    }

    protected override void EnterEscape()
    {
      this.PlayInAnim(AnimationCategoryID.Locomotion, 1, (System.Action) null);
      this.StopAddMovePoint();
      this.ClearMovePointList();
      this.Rotation = this.LookOppositeAxisY(this.TargetActor.Position);
      this.prevNormalizedTime = 0.0f;
      this.StopPlayAnimChange();
      this.StateTimeLimit = this.escapeDestroyTimeSecond;
      this.NavMeshCon.MoveUpdateEnabled = false;
    }

    protected override void OnEscape()
    {
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      float num = Mathf.Repeat(((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime(), 1f);
      if ((double) num < (double) this.prevNormalizedTime)
        this.Rotation = this.LookOppositeAxisY(this.TargetActor.Position);
      this.prevNormalizedTime = num;
      if ((double) this.StateTimeLimit >= (double) (this.StateCounter += (float) (double) Time.get_deltaTime()))
        return;
      this.SetState(AnimalState.Depop, (System.Action) null);
    }

    protected override void ExitEscape()
    {
      if (this.CurrentState == AnimalState.Depop)
        return;
      this.NavMeshCon.Refresh();
      this.StartAddMovePoint();
      this.AutoChangeAnimation = true;
    }

    protected override void EnterDepop()
    {
      this.AutoChangeAnimation = false;
      if (Object.op_Equality((Object) this.material, (Object) null))
      {
        this.SetState(AnimalState.Destroyed, (System.Action) null);
      }
      else
      {
        float _startAlpha = this.material.GetFloat(this.materialAlphaParamID);
        float _endAlpha = 0.0f;
        ObservableExtensions.Subscribe<float>(Observable.TakeUntilDestroy<float>((IObservable<M0>) ObservableEasing.Linear(this.fadeOutTimeSecond, false), ((Component) this).get_gameObject()), (System.Action<M0>) (x => this.material.SetFloat(this.materialAlphaParamID, Mathf.Lerp(_startAlpha, _endAlpha, x))), (System.Action) (() =>
        {
          if (this.CurrentState != AnimalState.Depop)
            return;
          this.SetState(AnimalState.Destroyed, (System.Action) null);
        }));
      }
    }

    protected override void OnDepop()
    {
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      float num = Mathf.Repeat(((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime(), 1f);
      if ((double) num < (double) this.prevNormalizedTime)
        this.Rotation = this.LookOppositeAxisY(this.TargetActor.Position);
      this.prevNormalizedTime = num;
    }

    protected override void ExitDepop()
    {
      if (!Object.op_Inequality((Object) this.habitatPoint, (Object) null))
        return;
      this.habitatPoint.StopUse(this);
      this.habitatPoint = (FrogHabitatPoint) null;
    }

    private Quaternion LookTargetAxisY(Vector3 _target)
    {
      Vector3 vector3 = Vector3.op_Subtraction(_target, this.Position);
      vector3.y = (__Null) 0.0;
      return Quaternion.LookRotation(((Vector3) ref vector3).get_normalized(), Vector3.get_up());
    }

    private Quaternion LookOppositeAxisY(Vector3 _target)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.Position, _target);
      vector3.y = (__Null) 0.0;
      return Quaternion.LookRotation(((Vector3) ref vector3).get_normalized(), Vector3.get_up());
    }

    private float DistanceXY(Vector3 _p1, Vector3 _p2)
    {
      _p1.y = _p2.y;
      return Vector3.Distance(_p1, _p2);
    }

    public void StartAddMovePoint()
    {
      this.path = new NavMeshPath();
      if (this.addMovePointDisposable != null)
        this.addMovePointDisposable.Dispose();
      this.addMovePointDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.IntervalFrame(10, (FrameCountType) 0), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.movePoints.Count < 3)), (System.Action<M0>) (_ =>
      {
        Vector3 vector3 = !((IReadOnlyList<Vector3>) this.movePoints).IsNullOrEmpty<Vector3>() ? this.movePoints[this.movePoints.Count - 1] : this.Position;
        Vector3 randomMoveAreaPoint = this.GetRandomMoveAreaPoint();
        if (this.OnValue(Vector2.op_Implicit(this.nextPointDistance), Vector3.Distance(vector3, randomMoveAreaPoint)) || !NavMesh.CalculatePath(vector3, randomMoveAreaPoint, this.NavMeshCon.AreaMask, this.path) || (this.path.get_status() != null || ((IReadOnlyList<Vector3>) this.path.get_corners()).IsNullOrEmpty<Vector3>()))
          return;
        this.movePoints.Add(this.path.get_corners()[this.path.get_corners().Length - 1]);
      }));
    }

    public void StopAddMovePoint()
    {
      if (this.addMovePointDisposable != null)
        this.addMovePointDisposable.Dispose();
      this.addMovePointDisposable = (IDisposable) null;
    }

    public void ClearMovePointList()
    {
      this.movePoints.Clear();
    }

    public Vector3 GetRandomMoveAreaPoint()
    {
      return Vector3.op_Addition(this.GetRandomPosOnCircle(this.habitatPoint.MoveRadius), this.habitatPoint.Position);
    }

    public bool OnValue(Vector3 _minMax, float _value)
    {
      return _minMax.x <= (double) _value && (double) _value <= _minMax.y;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.Player.CatchAnimal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.SaveData;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class CatchAnimal : PlayerStateBase
  {
    private string animalName = string.Empty;
    private bool isEnd;
    private AnimalBase hasAnimal;
    private float getPercent;
    private bool getAnimalFlag;
    private StuffItemInfo getIteminfo;
    private StuffItem addItem;
    private bool isCameraBlend;
    private PlayState playerPlayState;
    private Subject<Unit> onStart;
    private Subject<Unit> onEndInAnimation;
    private Subject<Unit> onEndGetAnimal;
    private Subject<Unit> onEndAction;
    private CompositeDisposable disposable;
    private Func<PlayerActor, bool> isWait;
    private bool isNextEnabled;

    protected override void OnAwake(PlayerActor player)
    {
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      this.isEnd = false;
      this.isWait = (Func<PlayerActor, bool>) null;
      this.isNextEnabled = false;
      if (this.disposable != null)
        this.disposable.Clear();
      this.disposable = new CompositeDisposable();
      if (Object.op_Equality((Object) (this.hasAnimal = player.Animal), (Object) null))
      {
        this.isEnd = true;
        this.ToErrorEnd(player);
      }
      else
      {
        this.animalName = this.hasAnimal.Name;
        this.isCameraBlend = true;
        this.getPercent = 30f;
        if (this.hasAnimal is WildGround)
          this.getPercent = (this.hasAnimal as WildGround).GetPercent;
        this.getAnimalFlag = (double) Random.Range(0.0f, 100f) <= (double) this.getPercent;
        if (this.getAnimalFlag && Singleton<Resources>.IsInstance() && Singleton<Manager.Map>.IsInstance())
        {
          List<StuffItem> itemList = Singleton<Manager.Map>.Instance.Player?.PlayerData?.ItemList;
          if (itemList != null)
          {
            this.getIteminfo = this.hasAnimal.ItemInfo;
            if (this.getIteminfo != null)
            {
              this.addItem = new StuffItem(this.getIteminfo.CategoryID, this.getIteminfo.ID, 1);
              itemList.AddItem(this.addItem);
            }
          }
        }
        if (Singleton<Manager.Map>.IsInstance())
        {
          List<StuffItem> itemList = Singleton<Manager.Map>.Instance.Player?.PlayerData?.ItemList;
          if (itemList != null && this.hasAnimal is WildGround)
          {
            ItemIDKeyPair getItemId = (this.hasAnimal as WildGround).GetItemID;
            itemList.RemoveItem(new StuffItem(getItemId.categoryID, getItemId.itemID, 1));
          }
        }
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
        if (this.hasAnimal.AnimalType == AnimalTypes.Cat || this.hasAnimal.AnimalType == AnimalTypes.Chicken)
          this.Initialize(player);
        else
          this.ToErrorEnd(player);
      }
    }

    private void ToErrorEnd(PlayerActor player)
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Component) player), (Action<M0>) (_ => player.PlayerController.ChangeState("Normal"))), (ICollection<IDisposable>) this.disposable);
    }

    private void ToEnd(PlayerActor player)
    {
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.End((Actor) player)), false), (Component) player)), (ICollection<IDisposable>) this.disposable);
    }

    private void Initialize(PlayerActor player)
    {
      this.hasAnimal.SetState(AnimalState.WithPlayer, (Action) null);
      player.CameraControl.CrossFade.FadeStart(-1f);
      Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(this.hasAnimal.Position, player.Position), Vector3.get_up());
      Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
      eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
      player.Rotation = Quaternion.Euler(eulerAngles);
      this.hasAnimal.SetWithActorGetPoint((Actor) player);
      this.hasAnimal.PlayInAnim(AnimationCategoryID.Idle, 0, (Action) null);
      int _poseID = !this.getAnimalFlag ? 1 : 0;
      this.SetPlayerAnimationState(player, _poseID);
      this.onEndGetAnimal = new Subject<Unit>();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.onEndGetAnimal, (Component) player), 1), (Action<M0>) (_ =>
      {
        this.hasAnimal.Destroy();
        this.ToEnd(player);
      })), (ICollection<IDisposable>) this.disposable);
      this.onEndInAnimation = new Subject<Unit>();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.onEndInAnimation, (Component) player), 1), (Action<M0>) (_ =>
      {
        if (this.getAnimalFlag)
        {
          this.hasAnimal.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
          this.hasAnimal.SetFloat(Singleton<Resources>.Instance.AnimalDefinePack.AnimatorInfo.LocomotionParamName, 0.5f);
          this.isWait = (Func<PlayerActor, bool>) (actor => !this.isNextEnabled);
          this.onEndAction = this.onEndGetAnimal;
          IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(1f, true), false));
          iconnectableObservable.Connect();
          Vector3 _start = this.hasAnimal.Position;
          Vector3 _end = Vector3.op_Addition(_start, Vector3.op_Multiply(this.hasAnimal.Forward, 1f));
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Component) player), (Action<M0>) (x => this.hasAnimal.Position = Vector3.Lerp(_start, _end, ((TimeInterval<float>) ref x).get_Value()))), (ICollection<IDisposable>) this.disposable);
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<TimeInterval<float>[]>(Observable.TakeUntilDestroy<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
          {
            (IObservable<TimeInterval<float>>) iconnectableObservable
          }), (Component) player), (Action<M0>) (__ => this.isNextEnabled = true)), (ICollection<IDisposable>) this.disposable);
          if (!Singleton<Resources>.IsInstance())
            return;
          SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
          AnimalDefinePack.SoundIDInfo soundId = Singleton<Resources>.Instance.AnimalDefinePack.SoundID;
          AudioSource audioSource = (AudioSource) null;
          switch (this.hasAnimal.AnimalType)
          {
            case AnimalTypes.Cat:
              audioSource = soundPack.Play(soundId.GetCat, Sound.Type.GameSE3D, 0.0f);
              break;
            case AnimalTypes.Chicken:
              audioSource = soundPack.Play(soundId.GetChicken, Sound.Type.GameSE3D, 0.0f);
              break;
          }
          if (!Object.op_Inequality((Object) audioSource, (Object) null))
            return;
          audioSource.Stop();
          ((Component) audioSource).get_transform().SetPositionAndRotation(this.hasAnimal.Position, this.hasAnimal.Rotation);
          audioSource.Play();
        }
        else
        {
          if (this.hasAnimal is WildGround)
          {
            (this.hasAnimal as WildGround).StartAvoid(player.Position, (Action) null);
          }
          else
          {
            this.hasAnimal.BadMood = true;
            this.hasAnimal.PlayOutAnim((Action) (() => this.hasAnimal.SetState(AnimalState.Locomotion, (Action) null)));
          }
          this.isWait = (Func<PlayerActor, bool>) (actor => true);
          this.ToEnd(player);
        }
      })), (ICollection<IDisposable>) this.disposable);
      this.onStart = new Subject<Unit>();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.onStart, (Component) player), 1), (Action<M0>) (_ =>
      {
        player.Animation.PlayInAnimation(this.playerPlayState.MainStateInfo.InStateInfo.EnableFade, this.playerPlayState.MainStateInfo.InStateInfo.FadeSecond, this.playerPlayState.MainStateInfo.FadeOutTime, this.playerPlayState.Layer);
        this.isWait = (Func<PlayerActor, bool>) (actor => actor.Animation.PlayingInAnimation);
        this.onEndAction = this.onEndInAnimation;
      })), (ICollection<IDisposable>) this.disposable);
      this.onEndAction = this.onStart;
    }

    private void SetPlayerAnimationState(PlayerActor _player, int _poseID)
    {
      this.playerPlayState = (PlayState) null;
      Dictionary<int, PoseKeyPair> dictionary;
      PoseKeyPair poseKeyPair;
      if (!Singleton<Resources>.Instance.AnimalTable.PlayerCatchAnimalPoseTable.TryGetValue((int) _player.ChaControl.sex, out dictionary) || !dictionary.TryGetValue(_poseID, out poseKeyPair))
        return;
      PlayState playState = Singleton<Resources>.Instance.Animation.PlayerActionAnimTable[(int) _player.ChaControl.sex][poseKeyPair.postureID][poseKeyPair.poseID];
      _player.Animation.InitializeStates(this.playerPlayState = playState);
      _player.Animation.LoadEventKeyTable(poseKeyPair.postureID, poseKeyPair.poseID);
      _player.LoadEventItems(playState);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      info.move = Vector3.get_zero();
      if (this.isEnd)
        return;
      if (this.isCameraBlend)
      {
        this.isCameraBlend = actor.CameraControl.CinemachineBrain.get_IsBlending();
        if (this.isCameraBlend)
          return;
      }
      bool? nullable = this.isWait != null ? new bool?(this.isWait(actor)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 || this.onEndAction == null)
        return;
      this.onEndAction.OnNext(Unit.get_Default());
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CatchAnimal.\u003COnEnd\u003Ec__Iterator0()
      {
        player = player,
        \u0024this = this
      };
    }

    private void OnEndSet(PlayerActor player)
    {
      if (this.getAnimalFlag)
      {
        if (this.getIteminfo != null && this.addItem != null)
          MapUIContainer.AddSystemItemLog(this.getIteminfo, this.addItem.Count, true);
        else
          MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
      }
      player.ClearItems();
      player.Animal = (AnimalBase) null;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      player.PlayerController.ChangeState("Normal");
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (this.disposable != null)
        this.disposable.Clear();
      this.disposable = (CompositeDisposable) null;
    }

    ~CatchAnimal()
    {
      if (this.disposable != null)
        this.disposable.Clear();
      this.disposable = (CompositeDisposable) null;
    }
  }
}

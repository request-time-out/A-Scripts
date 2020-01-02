// Decompiled with JetBrains decompiler
// Type: AIProject.Env3DSEPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using Housing;
using Manager;
using Sound;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class Env3DSEPoint : Point, EnvArea3DSE.IPlayInfo
  {
    [SerializeField]
    private string _summary = string.Empty;
    [SerializeField]
    private int _clipID = -1;
    [SerializeField]
    private Vector2 _decay = (Vector2) null;
    [SerializeField]
    private bool _isLoop = true;
    [SerializeField]
    private Vector2 _interval = (Vector2) null;
    private bool _firstPlaying = true;
    [SerializeField]
    private Transform _playRoot;
    [SerializeField]
    private bool _playOnAwake;
    [SerializeField]
    private bool _setFirstFadeTime;
    [SerializeField]
    private float _firstFadeTime;
    private AudioSource _audio;
    private FadePlayer _fadePlayer;
    private float _delayTime;
    private float _elapsedTime;
    private bool _isPlay;
    private bool _isEnableDistance;
    private ItemComponent _itemComponent;

    public AudioSource Audio
    {
      get
      {
        return this._audio;
      }
    }

    public void Stop()
    {
      this.SoundStop();
    }

    public bool IsHousingItem
    {
      get
      {
        return Object.op_Inequality((Object) this._itemComponent, (Object) null);
      }
    }

    public int AreaID { get; private set; } = -1;

    public bool PlayEnabled { get; set; }

    protected override void Start()
    {
      base.Start();
      this.OwnerArea = (MapArea) null;
      this.PlayEnabled = this._playOnAwake;
      this._itemComponent = (ItemComponent) ((Component) this).GetComponent<ItemComponent>();
      if (Object.op_Equality((Object) this._itemComponent, (Object) null))
        this._itemComponent = (ItemComponent) ((Component) this).GetComponentInParent<ItemComponent>();
      if (Object.op_Inequality((Object) this._itemComponent, (Object) null) && Singleton<Manager.Map>.IsInstance() && Singleton<Resources>.IsInstance())
      {
        LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
        RaycastHit raycastHit;
        if (Physics.Raycast(Vector3.op_Addition(this._itemComponent.position, Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(areaDetectionLayer)))
        {
          Dictionary<int, Chunk> chunkTable = Singleton<Manager.Map>.Instance.ChunkTable;
          bool flag = false;
          foreach (KeyValuePair<int, Chunk> keyValuePair in chunkTable)
          {
            foreach (MapArea mapArea in keyValuePair.Value.MapAreas)
            {
              if (flag = mapArea.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
              {
                this.OwnerArea = mapArea;
                break;
              }
            }
            if (flag)
              break;
          }
        }
        bool flag1 = Object.op_Inequality((Object) this.OwnerArea, (Object) null);
        this.PlayEnabled = flag1;
        if (flag1)
        {
          this.AreaID = this.OwnerArea.AreaID;
          Dictionary<int, ValueTuple<bool, List<Env3DSEPoint>>> housingEnvSePointTable = Singleton<Manager.Map>.Instance.HousingEnvSEPointTable;
          ValueTuple<bool, List<Env3DSEPoint>> valueTuple1;
          if (!housingEnvSePointTable.TryGetValue(this.AreaID, out valueTuple1))
          {
            Dictionary<int, ValueTuple<bool, List<Env3DSEPoint>>> dictionary = housingEnvSePointTable;
            int areaId = this.AreaID;
            valueTuple1 = (ValueTuple<bool, List<Env3DSEPoint>>) null;
            ValueTuple<bool, List<Env3DSEPoint>> valueTuple2 = valueTuple1;
            dictionary[areaId] = valueTuple2;
            valueTuple1.Item1 = (__Null) (Singleton<Manager.Map>.Instance.ActiveEnvAreaID(this.AreaID) ? 1 : 0);
            valueTuple1.Item2 = (__Null) new List<Env3DSEPoint>();
          }
          if ((((Behaviour) this).get_enabled() ? 1 : 0) != valueTuple1.Item1)
            ((Behaviour) this).set_enabled((bool) valueTuple1.Item1);
          if (!((List<Env3DSEPoint>) valueTuple1.Item2).Contains(this))
            ((List<Env3DSEPoint>) valueTuple1.Item2).Add(this);
          housingEnvSePointTable[this.AreaID] = valueTuple1;
        }
      }
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.ResetDelay();
    }

    protected override void OnDisable()
    {
      this.SoundStop();
      base.OnDisable();
    }

    private void OnDestroy()
    {
      ValueTuple<bool, List<Env3DSEPoint>> valueTuple;
      if (!Object.op_Inequality((Object) this._itemComponent, (Object) null) || !Singleton<Manager.Map>.IsInstance() || (0 > this.AreaID || !Singleton<Manager.Map>.Instance.HousingEnvSEPointTable.TryGetValue(this.AreaID, out valueTuple)) || (((List<Env3DSEPoint>) valueTuple.Item2).IsNullOrEmpty<Env3DSEPoint>() || !((List<Env3DSEPoint>) valueTuple.Item2).Contains(this)))
        return;
      ((List<Env3DSEPoint>) valueTuple.Item2).Remove(this);
    }

    private void OnUpdate()
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Manager.Map instance1 = Singleton<Manager.Map>.Instance;
      Resources instance2 = Singleton<Resources>.Instance;
      Transform root = !Object.op_Inequality((Object) this._playRoot, (Object) null) ? ((Component) this).get_transform() : this._playRoot;
      bool isEnableDistance = this._isEnableDistance;
      this._isEnableDistance = this.PlayEnabled && (!this._isEnableDistance ? this.EnableDistance(instance2, instance1, root) : !this.DisableDistance(instance2, instance1, root));
      if (this._isPlay)
      {
        bool flag = Object.op_Equality((Object) this._audio, (Object) null) || !this._isLoop && !this._audio.get_isPlaying();
        if (flag && this._isLoop)
          this._delayTime = 1f;
        if (flag || !this._isEnableDistance)
          this.SoundReset();
      }
      else if (this._isEnableDistance)
      {
        this._elapsedTime += Time.get_deltaTime();
        if ((double) this._delayTime <= (double) this._elapsedTime)
        {
          this._elapsedTime = 0.0f;
          this.ResetDelay();
          this.SoundPlay(root);
        }
      }
      if (Object.op_Inequality((Object) this._audio, (Object) null))
        ((Component) this._audio).get_transform().SetPositionAndRotation(root.get_position(), root.get_rotation());
      if (this._isEnableDistance || !isEnableDistance)
        return;
      this.ResetDelay();
      this._elapsedTime = 0.0f;
      this._firstPlaying = true;
    }

    public float GetSqrDistanceFromCamera(Transform cameraT, Vector3 p1)
    {
      return Vector3.SqrMagnitude(Vector3.op_Subtraction(p1, cameraT.get_position()));
    }

    public float GetSqrDistanceFromCamera(Transform camera, Transform t1)
    {
      return Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position()));
    }

    public int GetSqrDistanceSort(Transform camera, Transform t1, Transform t2)
    {
      return (int) ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position())) - (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t2.get_position(), camera.get_position())));
    }

    private bool EnableDistance(Resources res, Manager.Map map, Transform root)
    {
      Camera cameraComponent = Manager.Map.GetCameraComponent();
      Transform transform = !Object.op_Inequality((Object) cameraComponent, (Object) null) ? (Transform) null : ((Component) cameraComponent).get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return false;
      float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(root.get_position(), transform.get_position()));
      float num2 = (float) this._decay.y + res.SoundPack.EnviroInfo.EnableDistance;
      float num3 = num2 * num2;
      return (double) num1 <= (double) num3;
    }

    private bool DisableDistance(Resources res, Manager.Map map, Transform root)
    {
      Camera cameraComponent = Manager.Map.GetCameraComponent();
      Transform transform = !Object.op_Inequality((Object) cameraComponent, (Object) null) ? (Transform) null : ((Component) cameraComponent).get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return true;
      float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(root.get_position(), transform.get_position()));
      float num2 = (float) this._decay.y + res.SoundPack.EnviroInfo.DisableDistance;
      return (double) (num2 * num2) < (double) num1;
    }

    private void SoundPlay(Transform root)
    {
      this._isPlay = true;
      List<EnvArea3DSE.IPlayInfo> playList = EnvArea3DSE.PlayAudioSourceList;
      playList.RemoveAll((Predicate<EnvArea3DSE.IPlayInfo>) (ax => ax == null || Object.op_Equality((Object) ax.Audio, (Object) null) || Object.op_Equality((Object) ((Component) ax.Audio).get_gameObject(), (Object) null)));
      SoundPack.SoundSystemInfoGroup soundSystemInfo = Singleton<Resources>.Instance.SoundPack.SoundSystemInfo;
      if (soundSystemInfo.EnviroSEMaxCount <= playList.Count)
      {
        bool flag = true;
        int num = playList.Count - soundSystemInfo.EnviroSEMaxCount + 1;
        List<EnvArea3DSE.IPlayInfo> playInfoList = ListPool<EnvArea3DSE.IPlayInfo>.Get();
        playInfoList.AddRange((IEnumerable<EnvArea3DSE.IPlayInfo>) playList);
        Transform cameraT = ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent).get_transform();
        playInfoList.Sort((Comparison<EnvArea3DSE.IPlayInfo>) ((a1, a2) => this.GetSqrDistanceSort(cameraT, ((Component) a2.Audio).get_transform(), ((Component) a1.Audio).get_transform())));
        float distanceFromCamera = this.GetSqrDistanceFromCamera(cameraT, root);
        for (int index = 0; index < num; ++index)
        {
          EnvArea3DSE.IPlayInfo element = playInfoList.GetElement<EnvArea3DSE.IPlayInfo>(index);
          if ((double) distanceFromCamera < (double) element.GetSqrDistanceFromCamera(cameraT, ((Component) element.Audio).get_transform().get_position()))
          {
            element.Stop();
            flag = false;
          }
        }
        ListPool<EnvArea3DSE.IPlayInfo>.Release(playInfoList);
        if (flag)
          return;
      }
      float fadeTime = 0.0f;
      if (this._firstPlaying)
      {
        fadeTime = !this._setFirstFadeTime ? Singleton<Resources>.Instance.SoundPack.EnviroInfo.FadeTime : this._firstFadeTime;
        this._firstPlaying = false;
      }
      this._audio = Singleton<Resources>.Instance.SoundPack.PlayEnviroSE(this._clipID, fadeTime);
      if (Object.op_Equality((Object) this._audio, (Object) null))
      {
        Object.Destroy((Object) this);
      }
      else
      {
        this._fadePlayer = (FadePlayer) ((Component) this._audio).GetComponentInChildren<FadePlayer>(true);
        this._audio.set_loop(this._isLoop);
        this._audio.set_minDistance((float) this._decay.x);
        this._audio.set_maxDistance((float) this._decay.y);
        if (!playList.Contains((EnvArea3DSE.IPlayInfo) this))
          playList.Add((EnvArea3DSE.IPlayInfo) this);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) this._audio), (Action<M0>) (_ => playList.Remove((EnvArea3DSE.IPlayInfo) this)));
      }
    }

    private void ResetDelay()
    {
      this._delayTime = !this._isLoop ? this._interval.RandomRange() : 0.0f;
    }

    private void SoundReset()
    {
      this._isPlay = false;
      this._elapsedTime = 0.0f;
      this.SoundStop();
    }

    private void SoundStop()
    {
      if (Object.op_Inequality((Object) this._fadePlayer, (Object) null))
        this._fadePlayer.Stop(!Singleton<Resources>.IsInstance() ? 0.0f : Singleton<Resources>.Instance.SoundPack.EnviroInfo.FadeTime);
      else if (Object.op_Inequality((Object) this._audio, (Object) null) && Object.op_Inequality((Object) ((Component) this._audio).get_transform(), (Object) null))
      {
        if (Singleton<Manager.Sound>.IsInstance())
        {
          Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.ENV, ((Component) this._audio).get_transform());
        }
        else
        {
          this._audio.Stop();
          Object.Destroy((Object) ((Component) this._audio).get_gameObject());
        }
      }
      this._audio = (AudioSource) null;
      this._fadePlayer = (FadePlayer) null;
    }

    public void SoundForcedPlay(bool useFadeTime = false)
    {
      this.PlayEnabled = true;
      this._firstPlaying = useFadeTime;
      if (!Object.op_Equality((Object) this._audio, (Object) null) && !Object.op_Equality((Object) this._fadePlayer, (Object) null) || (!Singleton<Manager.Map>.IsInstance() || !Singleton<Manager.Map>.Instance.ActiveEnvAreaID(this.AreaID)))
        return;
      this._elapsedTime = 0.0f;
      this.ResetDelay();
      this.SoundPlay(!Object.op_Inequality((Object) this._playRoot, (Object) null) ? ((Component) this).get_transform() : this._playRoot);
    }

    public void SoundForcedStop()
    {
      if (Object.op_Inequality((Object) this._audio, (Object) null) && Object.op_Inequality((Object) ((Component) this._audio).get_gameObject(), (Object) null))
        Object.Destroy((Object) ((Component) this._audio).get_gameObject());
      this._audio = (AudioSource) null;
      this._fadePlayer = (FadePlayer) null;
      this.PlayEnabled = false;
      this._firstPlaying = true;
    }

    public void RefreshMapAreaObject()
    {
      if (Singleton<Manager.Map>.IsInstance())
        ;
    }
  }
}

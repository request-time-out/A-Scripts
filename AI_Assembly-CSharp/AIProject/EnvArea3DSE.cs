// Decompiled with JetBrains decompiler
// Type: AIProject.EnvArea3DSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace AIProject
{
  public class EnvArea3DSE : SerializedMonoBehaviour
  {
    private static List<EnvArea3DSE.IPlayInfo> _playAudioSourceList = new List<EnvArea3DSE.IPlayInfo>();
    [SerializeField]
    private string _summary;
    [SerializeField]
    [FormerlySerializedAs("SEList")]
    private EnvArea3DSE.EnvironmentSEInfo[] _seInfoList;
    [SerializeField]
    [HideInInspector]
    private bool _showAllGizmos;
    private EnvArea3DSE.PlayInfo[] _playInfos;
    private bool _initFlag;

    public EnvArea3DSE()
    {
      base.\u002Ector();
    }

    public IEnumerable<EnvArea3DSE.EnvironmentSEInfo> InfoList
    {
      get
      {
        return (IEnumerable<EnvArea3DSE.EnvironmentSEInfo>) this._seInfoList;
      }
    }

    public static List<EnvArea3DSE.IPlayInfo> PlayAudioSourceList
    {
      get
      {
        return EnvArea3DSE._playAudioSourceList;
      }
    }

    public bool ShowAllGizmos
    {
      get
      {
        return this._showAllGizmos;
      }
    }

    public bool Playing { get; private set; }

    private void Awake()
    {
      this.Initialize();
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.Playing)), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnEnable()
    {
      this.Initialize();
    }

    private void OnDisable()
    {
      this.End();
    }

    private void OnDestroy()
    {
      this.End();
    }

    private void Initialize()
    {
      this.End();
      if (this._initFlag)
      {
        if (!this._playInfos.IsNullOrEmpty<EnvArea3DSE.PlayInfo>())
        {
          foreach (EnvArea3DSE.PlayInfo playInfo in this._playInfos)
            playInfo.ResetDelay();
        }
        this.Playing = true;
      }
      else
      {
        this._initFlag = true;
        if (!this._seInfoList.IsNullOrEmpty<EnvArea3DSE.EnvironmentSEInfo>())
        {
          this._playInfos = new EnvArea3DSE.PlayInfo[this._seInfoList.Length];
          for (int index = 0; index < this._seInfoList.Length; ++index)
          {
            this._playInfos[index] = EnvArea3DSE.PlayInfo.Convert(this._seInfoList[index]);
            this._playInfos[index].ResetDelay();
          }
        }
        else
          this._playInfos = new EnvArea3DSE.PlayInfo[0];
        this.Playing = true;
      }
    }

    private void End()
    {
      if (!this.Playing)
        return;
      if (!this._playInfos.IsNullOrEmpty<EnvArea3DSE.PlayInfo>())
      {
        foreach (EnvArea3DSE.PlayInfo playInfo in this._playInfos)
          playInfo?.Release();
      }
      this.Playing = false;
    }

    private void OnUpdate()
    {
      if (!Singleton<Manager.Map>.IsInstance() || this._playInfos.IsNullOrEmpty<EnvArea3DSE.PlayInfo>())
        return;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      Resources res = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
      EnvironmentSimulator simulator = instance.Simulator;
      foreach (EnvArea3DSE.PlayInfo playInfo in this._playInfos)
        playInfo.Update(simulator.Weather, simulator.TimeZone, res, instance, ((Component) this).get_transform());
    }

    public void LoadFromExcelData(ExcelData data)
    {
      if (Object.op_Equality((Object) data, (Object) null))
        return;
      this._seInfoList = Enumerable.Range(1, data.MaxCell - 1).Select<int, EnvArea3DSE.EnvironmentSEInfo>((Func<int, EnvArea3DSE.EnvironmentSEInfo>) (rowIdx =>
      {
        List<string> list = data.list[rowIdx].list;
        if (list.IsNullOrEmpty<string>())
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        int num1 = 0;
        List<string> source1 = list;
        int index1 = num1;
        int num2 = index1 + 1;
        string str = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = list;
        int index2 = num2;
        int num3 = index2 + 1;
        int result1;
        if (!int.TryParse(source2.GetElement<string>(index2) ?? string.Empty, out result1))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source3 = list;
        int index3 = num3;
        int num4 = index3 + 1;
        int result2;
        if (!int.TryParse(source3.GetElement<string>(index3) ?? string.Empty, out result2))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source4 = list;
        int index4 = num4;
        int num5 = index4 + 1;
        int result3;
        if (!int.TryParse(source4.GetElement<string>(index4) ?? string.Empty, out result3))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source5 = list;
        int index5 = num5;
        int num6 = index5 + 1;
        int result4;
        if (!int.TryParse(source5.GetElement<string>(index5) ?? string.Empty, out result4))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source6 = list;
        int index6 = num6;
        int num7 = index6 + 1;
        int result5;
        if (!int.TryParse(source6.GetElement<string>(index6) ?? string.Empty, out result5))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source7 = list;
        int index7 = num7;
        int num8 = index7 + 1;
        int result6;
        if (!int.TryParse(source7.GetElement<string>(index7) ?? string.Empty, out result6))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source8 = list;
        int index8 = num8;
        int num9 = index8 + 1;
        int result7;
        if (!int.TryParse(source8.GetElement<string>(index8) ?? string.Empty, out result7))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source9 = list;
        int index9 = num9;
        int num10 = index9 + 1;
        int result8;
        if (!int.TryParse(source9.GetElement<string>(index9) ?? string.Empty, out result8))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source10 = list;
        int index10 = num10;
        int num11 = index10 + 1;
        float result9;
        if (!float.TryParse(source10.GetElement<string>(index10) ?? string.Empty, out result9))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source11 = list;
        int index11 = num11;
        int num12 = index11 + 1;
        float result10;
        if (!float.TryParse(source11.GetElement<string>(index11) ?? string.Empty, out result10))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source12 = list;
        int index12 = num12;
        int num13 = index12 + 1;
        int result11;
        if (!int.TryParse(source12.GetElement<string>(index12) ?? string.Empty, out result11))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source13 = list;
        int index13 = num13;
        int num14 = index13 + 1;
        float result12;
        if (!float.TryParse(source13.GetElement<string>(index13) ?? string.Empty, out result12))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        List<string> source14 = list;
        int index14 = num14;
        int num15 = index14 + 1;
        float result13;
        if (!float.TryParse(source14.GetElement<string>(index14) ?? string.Empty, out result13))
          return (EnvArea3DSE.EnvironmentSEInfo) null;
        return new EnvArea3DSE.EnvironmentSEInfo()
        {
          Summary = str,
          ClipID = result1,
          IsMooning = result2 != 0,
          IsNoon = result3 != 0,
          IsNight = result4 != 0,
          IsClear = result5 != 0,
          IsCloud = result6 != 0,
          IsRain = result7 != 0,
          IsFog = result8 != 0,
          Decay = new Threshold(result9, result10),
          IsLoop = result11 != 0,
          Interval = new Threshold(result12, result13)
        };
      })).Where<EnvArea3DSE.EnvironmentSEInfo>((Func<EnvArea3DSE.EnvironmentSEInfo, bool>) (x => x != null)).ToArray<EnvArea3DSE.EnvironmentSEInfo>();
    }

    public void DrawElementGizmos(Transform t)
    {
    }

    public interface IPlayInfo
    {
      AudioSource Audio { get; }

      float GetSqrDistanceFromCamera(Transform cameraT, Vector3 p1);

      void Stop();
    }

    public class PlayInfo : EnvArea3DSE.IPlayInfo
    {
      private FadePlayer FadePlayer;

      public bool IsActive { get; set; }

      public bool FirstPlaying { get; private set; } = true;

      public Transform Root { get; set; }

      public int ClipID { get; set; } = -1;

      public bool IsMooning { get; set; }

      public bool IsNoon { get; set; }

      public bool IsNight { get; set; }

      public bool IsClear { get; set; }

      public bool IsCloud { get; set; }

      public bool IsRain { get; set; }

      public bool IsFog { get; set; }

      public Threshold Decay { get; set; } = new Threshold();

      public bool IsLoop { get; set; }

      public Threshold Interval { get; set; } = new Threshold();

      public AudioSource Audio { get; private set; }

      public float ElapsedTime { get; private set; }

      public float DelayTime { get; private set; }

      public bool IsPlay { get; private set; }

      public bool IsEnableDistance { get; private set; }

      public bool PlayEnable { get; private set; }

      public bool LoadSuccess { get; private set; }

      public float GetSqrDistanceFromCamera(Transform camera, Transform t1)
      {
        return Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position()));
      }

      public float GetSqrDistanceFromCamera(Transform cameraT, Vector3 p1)
      {
        return Vector3.SqrMagnitude(Vector3.op_Subtraction(p1, cameraT.get_position()));
      }

      public int GetSqrDistanceSort(Transform camera, Transform t1, Transform t2)
      {
        return (int) ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position())) - (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t2.get_position(), camera.get_position())));
      }

      public bool EnableDistance()
      {
        if (!Singleton<Resources>.IsInstance() || !Singleton<Manager.Map>.IsInstance() || Object.op_Equality((Object) this.Root, (Object) null))
          return false;
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(this.Root.get_position(), ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent).get_transform().get_position()));
        float num2 = this.Decay.max + Singleton<Resources>.Instance.SoundPack.EnviroInfo.EnableDistance;
        float num3 = num2 * num2;
        return (double) num1 <= (double) num3;
      }

      public bool EnableDistance(Resources res, Manager.Map map, Transform root)
      {
        if (Object.op_Equality((Object) res, (Object) null))
          return false;
        Transform transform = ((Component) map.Player.CameraControl.CameraComponent).get_transform();
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(root.get_position(), transform.get_position()));
        float num2 = this.Decay.max + res.SoundPack.EnviroInfo.EnableDistance;
        float num3 = num2 * num2;
        return (double) num1 <= (double) num3;
      }

      public bool DisableDistance()
      {
        if (!Singleton<Resources>.IsInstance() || !Singleton<Manager.Map>.IsInstance() || Object.op_Equality((Object) this.Root, (Object) null))
          return true;
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(this.Root.get_position(), ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent).get_transform().get_position()));
        float num2 = this.Decay.max + Singleton<Resources>.Instance.SoundPack.EnviroInfo.DisableDistance;
        return (double) (num2 * num2) < (double) num1;
      }

      public bool DisableDistance(Resources res, Manager.Map map, Transform root)
      {
        if (Object.op_Equality((Object) res, (Object) null))
          return true;
        Transform transform = ((Component) map.Player.CameraControl.CameraComponent).get_transform();
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(root.get_position(), transform.get_position()));
        float num2 = this.Decay.max + res.SoundPack.EnviroInfo.DisableDistance;
        return (double) (num2 * num2) < (double) num1;
      }

      public static EnvArea3DSE.PlayInfo Convert(EnvArea3DSE.EnvironmentSEInfo envInfo)
      {
        return new EnvArea3DSE.PlayInfo()
        {
          IsActive = false,
          FirstPlaying = true,
          Root = envInfo.Root,
          ClipID = envInfo.ClipID,
          IsMooning = envInfo.IsMooning,
          IsNoon = envInfo.IsNoon,
          IsNight = envInfo.IsNight,
          IsClear = envInfo.IsClear,
          IsCloud = envInfo.IsCloud,
          IsRain = envInfo.IsRain,
          IsFog = envInfo.IsFog,
          Decay = envInfo.Decay,
          IsLoop = envInfo.IsLoop,
          Interval = envInfo.Interval,
          Audio = (AudioSource) null,
          FadePlayer = (FadePlayer) null,
          ElapsedTime = 0.0f,
          DelayTime = 0.0f,
          IsPlay = false,
          IsEnableDistance = false,
          PlayEnable = false,
          LoadSuccess = false
        };
      }

      public bool Equal(EnvArea3DSE.EnvironmentSEInfo eInfo)
      {
        return Object.op_Equality((Object) this.Root, (Object) eInfo.Root) && this.ClipID == eInfo.ClipID && (this.IsMooning == eInfo.IsMooning && this.IsNoon == eInfo.IsNoon) && (this.IsNight == eInfo.IsNight && this.IsClear == eInfo.IsClear && (this.IsCloud == eInfo.IsCloud && this.IsRain == eInfo.IsRain)) && this.IsFog == eInfo.IsFog && (double) this.Decay.min == (double) eInfo.Decay.min && ((double) this.Decay.max == (double) eInfo.Decay.max && this.IsLoop == eInfo.IsLoop && (double) this.Interval.min == (double) eInfo.Interval.min) && (double) this.Interval.max == (double) eInfo.Interval.max;
      }

      public bool CheckPlayEnable(Weather weather, TimeZone timeZone)
      {
        bool flag = true;
        switch (weather)
        {
          case Weather.Clear:
          case Weather.Cloud1:
          case Weather.Cloud2:
            flag &= this.IsClear;
            break;
          case Weather.Cloud3:
          case Weather.Cloud4:
            flag &= this.IsCloud;
            break;
          case Weather.Fog:
            flag &= this.IsFog;
            break;
          case Weather.Rain:
          case Weather.Storm:
            flag &= this.IsRain;
            break;
        }
        switch (timeZone)
        {
          case TimeZone.Morning:
            flag &= this.IsMooning;
            break;
          case TimeZone.Day:
            flag &= this.IsNoon;
            break;
          case TimeZone.Night:
            flag &= this.IsNight;
            break;
        }
        return flag;
      }

      private void Play(Transform root)
      {
        this.IsPlay = true;
        EnvArea3DSE._playAudioSourceList.RemoveAll((Predicate<EnvArea3DSE.IPlayInfo>) (ax => ax == null || Object.op_Equality((Object) ax.Audio, (Object) null) || Object.op_Equality((Object) ((Component) ax.Audio).get_gameObject(), (Object) null)));
        SoundPack.SoundSystemInfoGroup soundSystemInfo = Singleton<Resources>.Instance.SoundPack.SoundSystemInfo;
        if (soundSystemInfo.EnviroSEMaxCount <= EnvArea3DSE._playAudioSourceList.Count)
        {
          bool flag = true;
          int num = EnvArea3DSE._playAudioSourceList.Count - soundSystemInfo.EnviroSEMaxCount + 1;
          List<EnvArea3DSE.IPlayInfo> playInfoList = ListPool<EnvArea3DSE.IPlayInfo>.Get();
          playInfoList.AddRange((IEnumerable<EnvArea3DSE.IPlayInfo>) EnvArea3DSE._playAudioSourceList);
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
        if (this.FirstPlaying)
        {
          fadeTime = Singleton<Resources>.Instance.SoundPack.EnviroInfo.FadeTime;
          this.FirstPlaying = false;
        }
        this.Audio = Singleton<Resources>.Instance.SoundPack.PlayEnviroSE(this.ClipID, fadeTime);
        if (Object.op_Equality((Object) this.Audio, (Object) null))
          return;
        this.FadePlayer = (FadePlayer) ((Component) this.Audio).GetComponentInChildren<FadePlayer>(true);
        this.Audio.set_loop(this.IsLoop);
        this.Audio.set_minDistance(this.Decay.min);
        this.Audio.set_maxDistance(this.Decay.max);
        this.LoadSuccess = true;
        if (EnvArea3DSE._playAudioSourceList.Contains((EnvArea3DSE.IPlayInfo) this))
          return;
        EnvArea3DSE._playAudioSourceList.Add((EnvArea3DSE.IPlayInfo) this);
      }

      public void Update(
        Weather weather,
        TimeZone timeZone,
        Resources res,
        Manager.Map map,
        Transform root)
      {
        Transform root1 = !Object.op_Equality((Object) this.Root, (Object) null) ? this.Root : root;
        this.IsEnableDistance = !this.IsEnableDistance ? this.EnableDistance(res, map, root1) : !this.DisableDistance(res, map, root1);
        bool playEnable = this.PlayEnable;
        this.PlayEnable = this.CheckPlayEnable(weather, timeZone) && this.IsEnableDistance;
        if (this.IsPlay)
        {
          bool flag = Object.op_Equality((Object) this.Audio, (Object) null) || !this.IsLoop && !this.Audio.get_isPlaying();
          if (flag && this.IsLoop)
            this.DelayTime = 1f;
          if (flag || !this.PlayEnable)
            this.Reset();
        }
        else if (this.PlayEnable)
        {
          this.ElapsedTime += Time.get_deltaTime();
          if ((double) this.DelayTime <= (double) this.ElapsedTime)
          {
            this.ElapsedTime = 0.0f;
            this.ResetDelay();
            this.Play(root1);
          }
        }
        if (Object.op_Inequality((Object) this.Audio, (Object) null))
        {
          ((Component) this.Audio).get_transform().set_position(root1.get_position());
          ((Component) this.Audio).get_transform().set_rotation(root1.get_rotation());
        }
        if (this.PlayEnable || !playEnable)
          return;
        this.ResetDelay();
        this.ElapsedTime = 0.0f;
        this.FirstPlaying = true;
      }

      public void ResetDelay()
      {
        this.DelayTime = !this.IsLoop ? this.Interval.RandomValue : 0.0f;
      }

      public void Stop()
      {
        if (EnvArea3DSE._playAudioSourceList.Contains((EnvArea3DSE.IPlayInfo) this))
          EnvArea3DSE._playAudioSourceList.Remove((EnvArea3DSE.IPlayInfo) this);
        if (Object.op_Inequality((Object) this.FadePlayer, (Object) null))
          this.FadePlayer.Stop(Singleton<Resources>.Instance.SoundPack.EnviroInfo.FadeTime);
        else if (Object.op_Inequality((Object) this.Audio, (Object) null) && Singleton<Manager.Sound>.IsInstance())
          Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.ENV, ((Component) this.Audio).get_transform());
        this.FadePlayer = (FadePlayer) null;
        this.Audio = (AudioSource) null;
        this.LoadSuccess = false;
      }

      private void Reset()
      {
        this.IsPlay = false;
        this.ElapsedTime = 0.0f;
        this.Stop();
      }

      public void Release()
      {
        this.Reset();
        this.IsEnableDistance = false;
        this.PlayEnable = false;
      }

      ~PlayInfo()
      {
        this.Release();
      }
    }

    [Serializable]
    public class EnvironmentSEInfo
    {
      [SerializeField]
      private string _summary = string.Empty;
      [SerializeField]
      private int _clipID = -1;
      [SerializeField]
      private Threshold _decay = new Threshold(1f, 500f);
      [SerializeField]
      private Threshold _interval = new Threshold(0.0f, 0.0f);
      [SerializeField]
      private Transform _root;
      [SerializeField]
      private bool _isMooning;
      [SerializeField]
      private bool _isNoon;
      [SerializeField]
      private bool _isNight;
      [SerializeField]
      private bool _isClear;
      [SerializeField]
      private bool _isCloud;
      [SerializeField]
      private bool _isRain;
      [SerializeField]
      private bool _isFog;
      [SerializeField]
      private bool _isLoop;

      public string Summary
      {
        get
        {
          return this._summary;
        }
        set
        {
          this._summary = value;
        }
      }

      public int ClipID
      {
        get
        {
          return this._clipID;
        }
        set
        {
          this._clipID = value;
        }
      }

      public Transform Root
      {
        get
        {
          return this._root;
        }
      }

      public bool IsMooning
      {
        get
        {
          return this._isMooning;
        }
        set
        {
          this._isMooning = value;
        }
      }

      public bool IsNoon
      {
        get
        {
          return this._isNoon;
        }
        set
        {
          this._isNoon = value;
        }
      }

      public bool IsNight
      {
        get
        {
          return this._isNight;
        }
        set
        {
          this._isNight = value;
        }
      }

      public bool IsClear
      {
        get
        {
          return this._isClear;
        }
        set
        {
          this._isClear = value;
        }
      }

      public bool IsCloud
      {
        get
        {
          return this._isCloud;
        }
        set
        {
          this._isCloud = value;
        }
      }

      public bool IsRain
      {
        get
        {
          return this._isRain;
        }
        set
        {
          this._isRain = value;
        }
      }

      public bool IsFog
      {
        get
        {
          return this._isFog;
        }
        set
        {
          this._isFog = value;
        }
      }

      public Threshold Decay
      {
        get
        {
          return this._decay;
        }
        set
        {
          this._decay = value;
        }
      }

      public bool IsLoop
      {
        get
        {
          return this._isLoop;
        }
        set
        {
          this._isLoop = value;
        }
      }

      public Threshold Interval
      {
        get
        {
          return this._interval;
        }
        set
        {
          this._interval = value;
        }
      }
    }
  }
}

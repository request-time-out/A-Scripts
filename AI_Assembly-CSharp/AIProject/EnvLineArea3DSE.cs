// Decompiled with JetBrains decompiler
// Type: AIProject.EnvLineArea3DSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using Sound;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace AIProject
{
  public class EnvLineArea3DSE : SerializedMonoBehaviour
  {
    [SerializeField]
    [LabelText("概要")]
    private string _summary;
    [SerializeField]
    [FormerlySerializedAs("SEList")]
    [LabelText("環境音リスト")]
    private List<EnvLineArea3DSE.EnvironmentSEInfo> _seInfoList;
    private EnvLineArea3DSE.PlayInfo[] _playInfos;
    private bool _initFlag;

    public EnvLineArea3DSE()
    {
      base.\u002Ector();
    }

    public List<EnvLineArea3DSE.EnvironmentSEInfo> SEInfoList
    {
      get
      {
        return this._seInfoList;
      }
    }

    public bool Playing { get; private set; }

    public EnvLineArea3DSE.PlayInfo[] PlayInfos
    {
      get
      {
        return this._playInfos;
      }
    }

    private static List<EnvArea3DSE.IPlayInfo> PlayAudioSourceList
    {
      get
      {
        return EnvArea3DSE.PlayAudioSourceList;
      }
    }

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
        if (!this._playInfos.IsNullOrEmpty<EnvLineArea3DSE.PlayInfo>())
        {
          foreach (EnvLineArea3DSE.PlayInfo playInfo in this._playInfos)
            playInfo?.ResetDelay();
        }
        this.Playing = true;
      }
      else
      {
        this._initFlag = true;
        if (!this._seInfoList.IsNullOrEmpty<EnvLineArea3DSE.EnvironmentSEInfo>())
        {
          this._playInfos = new EnvLineArea3DSE.PlayInfo[this._seInfoList.Count];
          for (int index = 0; index < this._playInfos.Length; ++index)
          {
            this._playInfos[index] = EnvLineArea3DSE.PlayInfo.Convert(this._seInfoList[index]);
            this._playInfos[index].ResetDelay();
          }
        }
        this.Playing = true;
      }
    }

    private void End()
    {
      if (!this.Playing)
        return;
      if (!this._playInfos.IsNullOrEmpty<EnvLineArea3DSE.PlayInfo>())
      {
        foreach (EnvLineArea3DSE.PlayInfo playInfo in this._playInfos)
          playInfo?.Release();
      }
      this.Playing = false;
    }

    private void OnUpdate()
    {
      if (!Singleton<Manager.Map>.IsInstance() || this._playInfos.IsNullOrEmpty<EnvLineArea3DSE.PlayInfo>())
        return;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      Resources res = !Singleton<Resources>.IsInstance() ? (Resources) null : Singleton<Resources>.Instance;
      EnvironmentSimulator simulator = instance.Simulator;
      foreach (EnvLineArea3DSE.PlayInfo playInfo in this._playInfos)
        playInfo.Update(simulator.Weather, simulator.TimeZone, res, instance);
    }

    public void LoadFromExcelData(ExcelData data)
    {
    }

    public class PlayInfo : EnvArea3DSE.IPlayInfo
    {
      private FadePlayer FadePlayer;

      public bool FirstPlaying { get; private set; } = true;

      public List<Transform> Roots { get; set; } = new List<Transform>();

      public List<EnvLineArea3DSE.LineT> Lines { get; set; } = new List<EnvLineArea3DSE.LineT>();

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

      public EnvLineArea3DSE.LineT NearLine { get; private set; } = new EnvLineArea3DSE.LineT();

      private bool EnableDistance(Resources res, Manager.Map map, Vector3 hitPos)
      {
        if (Object.op_Equality((Object) res, (Object) null))
          return false;
        Vector3 position = ((Component) map.Player.CameraControl.CameraComponent).get_transform().get_position();
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(hitPos, position));
        float num2 = this.Decay.max + res.SoundPack.EnviroInfo.EnableDistance;
        float num3 = num2 * num2;
        return (double) num1 <= (double) num3;
      }

      private bool DisableDistance(Resources res, Manager.Map map, Vector3 hitPos)
      {
        if (Object.op_Equality((Object) res, (Object) null))
          return true;
        Vector3 position = ((Component) map.Player.CameraControl.CameraComponent).get_transform().get_position();
        float num1 = Vector3.SqrMagnitude(Vector3.op_Subtraction(hitPos, position));
        float num2 = this.Decay.max + res.SoundPack.EnviroInfo.DisableDistance;
        return (double) (num2 * num2) < (double) num1;
      }

      public float GetSqrDistanceFromCamera(Transform camera, Vector3 p1)
      {
        return Vector3.SqrMagnitude(Vector3.op_Subtraction(p1, camera.get_position()));
      }

      public int GetSqrDistanceSort(Transform camera, Transform t1, Transform t2)
      {
        return (int) ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position())) - (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t2.get_position(), camera.get_position())));
      }

      public static EnvLineArea3DSE.PlayInfo Convert(
        EnvLineArea3DSE.EnvironmentSEInfo envInfo)
      {
        EnvLineArea3DSE.PlayInfo playInfo = new EnvLineArea3DSE.PlayInfo();
        playInfo.FirstPlaying = true;
        playInfo.Roots.Clear();
        playInfo.Roots.AddRange((IEnumerable<Transform>) envInfo.Roots);
        playInfo.ClipID = envInfo.ClipID;
        playInfo.IsMooning = envInfo.IsMooning;
        playInfo.IsNoon = envInfo.IsNoon;
        playInfo.IsNight = envInfo.IsNight;
        playInfo.IsClear = envInfo.IsClear;
        playInfo.IsCloud = envInfo.IsCloud;
        playInfo.IsRain = envInfo.IsRain;
        playInfo.IsFog = envInfo.IsFog;
        playInfo.Decay = envInfo.Decay;
        playInfo.IsLoop = envInfo.IsLoop;
        playInfo.Interval = envInfo.Interval;
        playInfo.Audio = (AudioSource) null;
        playInfo.FadePlayer = (FadePlayer) null;
        playInfo.ElapsedTime = 0.0f;
        playInfo.DelayTime = 0.0f;
        playInfo.IsPlay = false;
        playInfo.IsEnableDistance = false;
        playInfo.PlayEnable = false;
        playInfo.LoadSuccess = false;
        playInfo.NearLine = new EnvLineArea3DSE.LineT();
        if (!envInfo.Roots.IsNullOrEmpty<Transform>() && 2 <= envInfo.Roots.Count)
        {
          for (int index = 0; index < envInfo.Roots.Count - 1; ++index)
          {
            EnvLineArea3DSE.LineT lineT = new EnvLineArea3DSE.LineT()
            {
              P1 = envInfo.Roots[index],
              P2 = envInfo.Roots[index + 1]
            };
            playInfo.Lines.Add(lineT);
          }
        }
        return playInfo;
      }

      public bool Equal(EnvLineArea3DSE.EnvironmentSEInfo eInfo)
      {
        if (this.Roots == null && eInfo.Roots != null || this.Roots != null && eInfo.Roots == null)
          return false;
        if (this.Roots != null && eInfo.Roots != null)
        {
          if (this.Roots.Count != eInfo.Roots.Count)
            return false;
          for (int index = 0; index < this.Roots.Count; ++index)
          {
            if (Object.op_Inequality((Object) this.Roots[index], (Object) eInfo.Roots[index]))
              return false;
          }
        }
        return this.ClipID == eInfo.ClipID && this.IsMooning == eInfo.IsMooning && (this.IsNoon == eInfo.IsNoon && this.IsNight == eInfo.IsNight) && (this.IsClear == eInfo.IsClear && this.IsCloud == eInfo.IsCloud && (this.IsRain == eInfo.IsRain && this.IsFog == eInfo.IsFog)) && (double) this.Decay.min == (double) eInfo.Decay.min && ((double) this.Decay.max == (double) eInfo.Decay.max && this.IsLoop == eInfo.IsLoop && (double) this.Interval.min == (double) eInfo.Interval.min) && (double) this.Interval.max == (double) eInfo.Interval.max;
      }

      public bool TryGetNearPoint(out Vector3 getHitPoint, out EnvLineArea3DSE.LineT getHitLine)
      {
        getHitPoint = Vector3.get_zero();
        getHitLine = new EnvLineArea3DSE.LineT();
        if (!Singleton<Resources>.IsInstance() || !Singleton<Manager.Map>.IsInstance() || this.Lines.IsNullOrEmpty<EnvLineArea3DSE.LineT>())
          return false;
        Transform transform = ((Component) Singleton<Manager.Map>.Instance.Player?.CameraControl?.CameraComponent)?.get_transform();
        if (Object.op_Equality((Object) transform, (Object) null))
          return false;
        Vector3 position1 = transform.get_position();
        int index1 = -1;
        float num1 = float.MaxValue;
        for (int index2 = 0; index2 < this.Lines.Count; ++index2)
        {
          EnvLineArea3DSE.LineT line = this.Lines[index2];
          if (!Object.op_Equality((Object) line.P1, (Object) null) && !Object.op_Equality((Object) line.P2, (Object) null))
          {
            Vector3 position2 = line.P1.get_position();
            Vector3 position3 = line.P2.get_position();
            float maxDistance = Vector3.SqrMagnitude(Vector3.op_Subtraction(position3, position2));
            line.HitPoint = this.NearPointOnLine(position1, position2, position3, maxDistance);
            line.Distance = Vector3.Distance(line.HitPoint, position1);
            line.SqrDistance = line.Distance * line.Distance;
            if ((double) line.SqrDistance < (double) num1)
            {
              num1 = line.SqrDistance;
              index1 = index2;
            }
            this.Lines[index2] = line;
          }
        }
        if (0 > index1)
          return false;
        float areaSeBlendDistance = Singleton<Resources>.Instance.SoundPack.EnviroInfo.LineAreaSEBlendDistance;
        getHitLine = this.Lines[index1];
        if (this.Lines.Count == 1)
        {
          getHitPoint = getHitLine.HitPoint;
          return true;
        }
        if (index1 == 0)
        {
          EnvLineArea3DSE.LineT line1 = this.Lines[0];
          EnvLineArea3DSE.LineT line2 = this.Lines[1];
          float num2 = Mathf.Abs(line1.Distance - line2.Distance);
          if ((double) num2 <= (double) areaSeBlendDistance)
          {
            float num3 = Mathf.InverseLerp(0.0f, areaSeBlendDistance, num2);
            getHitPoint = Vector3.Lerp(Vector3.op_Division(Vector3.op_Addition(line1.HitPoint, line2.HitPoint), 2f), line1.HitPoint, num3);
            return true;
          }
        }
        else if (index1 == this.Lines.Count - 1)
        {
          int index2 = this.Lines.Count - 1;
          EnvLineArea3DSE.LineT line1 = this.Lines[index2];
          EnvLineArea3DSE.LineT line2 = this.Lines[index2 - 1];
          float num2 = Mathf.Abs(line1.Distance - line2.Distance);
          if ((double) num2 <= (double) areaSeBlendDistance)
          {
            float num3 = Mathf.InverseLerp(0.0f, areaSeBlendDistance, num2);
            getHitPoint = Vector3.Lerp(Vector3.op_Division(Vector3.op_Addition(line1.HitPoint, line2.HitPoint), 2f), line1.HitPoint, num3);
            return true;
          }
        }
        else
        {
          EnvLineArea3DSE.LineT line = this.Lines[index1];
          EnvLineArea3DSE.LineT lineT = (double) this.Lines[index1 - 1].Distance > (double) this.Lines[index1 + 1].Distance ? this.Lines[index1 + 1] : this.Lines[index1 - 1];
          float num2 = Mathf.Abs(line.Distance - lineT.Distance);
          if ((double) num2 <= (double) areaSeBlendDistance)
          {
            float num3 = Mathf.InverseLerp(0.0f, areaSeBlendDistance, num2);
            getHitPoint = Vector3.Lerp(Vector3.op_Division(Vector3.op_Addition(line.HitPoint, lineT.HitPoint), 2f), line.HitPoint, num3);
            return true;
          }
        }
        getHitPoint = getHitLine.HitPoint;
        return true;
      }

      private Vector3 NearPointOnLine(
        Vector3 pc,
        Vector3 p1,
        Vector3 p2,
        float maxDistance)
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(p2, p1);
        Vector3 vector3_2 = Vector3.op_Subtraction(pc, p1);
        Vector3 normalized = ((Vector3) ref vector3_1).get_normalized();
        float num = Vector3.Dot(normalized, vector3_2);
        if ((double) num < 0.0)
          num = 0.0f;
        else if ((double) maxDistance < (double) num * (double) num)
          num = Vector3.Distance(p2, p1);
        return Vector3.op_Addition(p1, Vector3.op_Multiply(normalized, num));
      }

      private bool CheckEnableEnvironment(Weather weather, TimeZone timeZone)
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

      private void Play(Vector3 point)
      {
        this.IsPlay = true;
        EnvLineArea3DSE.PlayAudioSourceList.RemoveAll((Predicate<EnvArea3DSE.IPlayInfo>) (ax => ax == null || Object.op_Equality((Object) ax.Audio, (Object) null) || Object.op_Equality((Object) ((Component) ax.Audio).get_gameObject(), (Object) null)));
        SoundPack.SoundSystemInfoGroup soundSystemInfo = Singleton<Resources>.Instance.SoundPack.SoundSystemInfo;
        if (soundSystemInfo.EnviroSEMaxCount <= EnvLineArea3DSE.PlayAudioSourceList.Count)
        {
          bool flag = true;
          int num = EnvLineArea3DSE.PlayAudioSourceList.Count - soundSystemInfo.EnviroSEMaxCount + 1;
          List<EnvArea3DSE.IPlayInfo> playInfoList = ListPool<EnvArea3DSE.IPlayInfo>.Get();
          playInfoList.AddRange((IEnumerable<EnvArea3DSE.IPlayInfo>) EnvLineArea3DSE.PlayAudioSourceList);
          Transform cameraT = ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent).get_transform();
          playInfoList.Sort((Comparison<EnvArea3DSE.IPlayInfo>) ((a1, a2) => this.GetSqrDistanceSort(cameraT, ((Component) a2.Audio).get_transform(), ((Component) a1.Audio).get_transform())));
          float distanceFromCamera = this.GetSqrDistanceFromCamera(cameraT, point);
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
        if (EnvLineArea3DSE.PlayAudioSourceList.Contains((EnvArea3DSE.IPlayInfo) this))
          return;
        EnvLineArea3DSE.PlayAudioSourceList.Add((EnvArea3DSE.IPlayInfo) this);
      }

      public void Update(Weather weather, TimeZone timeZone, Resources res, Manager.Map map)
      {
        Vector3 getHitPoint;
        bool nearPoint = this.TryGetNearPoint(out getHitPoint, out EnvLineArea3DSE.LineT _);
        this.IsEnableDistance = !this.IsEnableDistance ? nearPoint && this.EnableDistance(res, map, getHitPoint) : nearPoint && !this.DisableDistance(res, map, getHitPoint);
        bool playEnable = this.PlayEnable;
        this.PlayEnable = this.CheckEnableEnvironment(weather, timeZone) && this.IsEnableDistance;
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
            this.Play(getHitPoint);
          }
        }
        if (Object.op_Inequality((Object) this.Audio, (Object) null) && nearPoint)
          ((Component) this.Audio).get_transform().set_position(getHitPoint);
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
        if (EnvLineArea3DSE.PlayAudioSourceList.Contains((EnvArea3DSE.IPlayInfo) this))
          EnvLineArea3DSE.PlayAudioSourceList.Remove((EnvArea3DSE.IPlayInfo) this);
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

    public struct LineT
    {
      public Transform P1;
      public Transform P2;
      public Vector3 HitPoint;
      public float Distance;
      public float SqrDistance;
    }

    [Serializable]
    public class EnvironmentSEInfo
    {
      [SerializeField]
      [LabelText("概要")]
      private string _summary = string.Empty;
      [SerializeField]
      private int _clipID = -1;
      [SerializeField]
      [LabelText("減衰値")]
      private Threshold _decay = new Threshold(1f, 500f);
      [SerializeField]
      [LabelText("再生間隔")]
      [HideIf("_isLoop", true)]
      private Threshold _interval = new Threshold(0.0f, 0.0f);
      [SerializeField]
      private List<Transform> _roots;
      [SerializeField]
      [LabelText("朝")]
      [FoldoutGroup("時間帯", 0)]
      [ToggleLeft]
      private bool _isMooning;
      [SerializeField]
      [LabelText("昼")]
      [FoldoutGroup("時間帯", 0)]
      [ToggleLeft]
      private bool _isNoon;
      [SerializeField]
      [LabelText("夜")]
      [FoldoutGroup("時間帯", 0)]
      [ToggleLeft]
      private bool _isNight;
      [SerializeField]
      [LabelText("晴")]
      [FoldoutGroup("天候", 0)]
      [ToggleLeft]
      private bool _isClear;
      [SerializeField]
      [LabelText("曇")]
      [FoldoutGroup("天候", 0)]
      [ToggleLeft]
      private bool _isCloud;
      [SerializeField]
      [LabelText("雨")]
      [FoldoutGroup("天候", 0)]
      [ToggleLeft]
      private bool _isRain;
      [SerializeField]
      [LabelText("霧")]
      [FoldoutGroup("天候", 0)]
      [ToggleLeft]
      private bool _isFog;
      [SerializeField]
      [LabelText("ループ専用")]
      [ToggleLeft]
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

      public List<Transform> Roots
      {
        get
        {
          return this._roots;
        }
        set
        {
          this._roots = value;
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

// Decompiled with JetBrains decompiler
// Type: Manager.SoundPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using ReMotion;
using Sound;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;

namespace Manager
{
  public class SoundPlayer : Singleton<SoundPlayer>
  {
    private Dictionary<int, Dictionary<int, AudioClip>> jukeBoxAudioClipCacheTable = new Dictionary<int, Dictionary<int, AudioClip>>();
    private Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> housingAreaAudioTable = new Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>();
    private Dictionary<int, ValueTuple<AudioSource, FadePlayer>> usedWideEnvSE = new Dictionary<int, ValueTuple<AudioSource, FadePlayer>>();
    private IDisposable delayPlayBGMDisposable;
    private uAudio.uAudio_backend.uAudio _uAudio;
    private IDisposable delayPlayWideEnvDisposable;
    private IDisposable timeSubscribeDisposable;
    private IDisposable areaSubscribeDisposable;
    private IDisposable weatherSubscribeDisposable;

    public bool PlayActiveAll
    {
      set
      {
        bool flag = value;
        this.EnvPlayActive = flag;
        this.BGMPlayActive = flag;
      }
    }

    public bool BGMPlayActive { get; set; }

    public bool EnvPlayActive { get; set; }

    public AssetBundleInfo CurrentBGMAudioABInfo { get; private set; } = (AssetBundleInfo) null;

    public AssetBundleInfo PrevBGMAudioABInfo { get; private set; } = (AssetBundleInfo) null;

    public AudioSource LastBGMAudio { get; private set; }

    private SoundPlayer.UpdateType MapBGMUpdateFlag { get; set; }

    private bool BGMChangePossible { get; set; } = true;

    public uAudio.uAudio_backend.uAudio uAudio
    {
      get
      {
        if (this._uAudio == null)
        {
          this._uAudio = new uAudio.uAudio_backend.uAudio();
          this._uAudio.set_Volume(1f);
          this._uAudio.set_CurrentTime(TimeSpan.Zero);
        }
        return this._uAudio;
      }
    }

    private SoundPlayer.UpdateType WideEnvUpdateFlag { get; set; }

    private SoundPlayer.UpdateType PrevWideEnvUpdateFlag { get; set; }

    private bool WideEnvChangePossible { get; set; } = true;

    protected override void Awake()
    {
      if (this.CheckInstance())
        ;
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.BGMPlayActive)), (Func<M0, bool>) (_ => this.BGMChangePossible)), (Func<M0, bool>) (_ => this.MapBGMUpdateFlag != (SoundPlayer.UpdateType) 0)), (Action<M0>) (_ => this.PlayMapBGM()));
      IObservable<long> observable = (IObservable<long>) Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.EnvPlayActive));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.Where<long>((IObservable<M0>) observable, (Func<M0, bool>) (_ => !this.WideEnvChangePossible)), (Func<M0, bool>) (_ => this.WideEnvUpdateFlag.Contains(SoundPlayer.UpdateType.Area))), (Func<M0, bool>) (_ => !this.PrevWideEnvUpdateFlag.Contains(SoundPlayer.UpdateType.Area))), (Action<M0>) (_ =>
      {
        this.PlayWideEnvSE(true);
        this.PrevWideEnvUpdateFlag = this.WideEnvUpdateFlag;
      }));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>((IObservable<M0>) observable, (Func<M0, bool>) (_ => this.WideEnvChangePossible)), (Func<M0, bool>) (_ => this.WideEnvUpdateFlag != (SoundPlayer.UpdateType) 0)), (Action<M0>) (_ =>
      {
        this.PlayWideEnvSE(this.WideEnvUpdateFlag.Contains(SoundPlayer.UpdateType.Area));
        this.PrevWideEnvUpdateFlag = this.WideEnvUpdateFlag;
      }));
    }

    private void ChangedArea(PlayerActor _player)
    {
      this.MapBGMUpdateFlag |= SoundPlayer.UpdateType.Area;
      this.WideEnvUpdateFlag |= SoundPlayer.UpdateType.Area;
    }

    private void ChangedTime(PlayerActor _player)
    {
      this.MapBGMUpdateFlag |= SoundPlayer.UpdateType.Time;
    }

    private void ChangedWeather(PlayerActor _player)
    {
      this.WideEnvUpdateFlag |= SoundPlayer.UpdateType.Weather;
    }

    public void StartAllSubscribe()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      this.StartTimeSubscribe();
      this.StartAreaSubScribe();
      this.StartWeatherSubscribe();
    }

    public void StopAllSubscribe()
    {
      this.DisposeDelayPlayBGM();
      this.DisposeDelayPlayWideEnv();
      this.StopTimeSubscribe();
      this.StopAreaSubscribe();
      this.StopWeatherSubscribe();
    }

    public void StartTimeSubscribe()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      Map _map = Singleton<Map>.Instance;
      EnvironmentSimulator simulator = _map.Simulator;
      if (this.timeSubscribeDisposable != null)
        this.timeSubscribeDisposable.Dispose();
      this.timeSubscribeDisposable = ObservableExtensions.Subscribe<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) simulator.OnMinuteAsObservable(), (Component) _map), (Action<M0>) (_ => this.ChangedTime(_map.Player)));
    }

    public void StopTimeSubscribe()
    {
      if (this.timeSubscribeDisposable == null)
        return;
      this.timeSubscribeDisposable.Dispose();
      this.timeSubscribeDisposable = (IDisposable) null;
    }

    public void StartAreaSubScribe()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      Map instance = Singleton<Map>.Instance;
      PlayerActor _player = instance.Player;
      if (this.areaSubscribeDisposable != null)
        this.areaSubscribeDisposable.Dispose();
      this.areaSubscribeDisposable = ObservableExtensions.Subscribe<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) _player.OnMapAreaChangedAsObservable(), (Component) instance), (Action<M0>) (_ => this.ChangedArea(_player)));
    }

    public void StopAreaSubscribe()
    {
      if (this.areaSubscribeDisposable == null)
        return;
      this.areaSubscribeDisposable.Dispose();
      this.areaSubscribeDisposable = (IDisposable) null;
    }

    public void StartWeatherSubscribe()
    {
      if (!Singleton<Map>.IsInstance())
        return;
      Map _map = Singleton<Map>.Instance;
      EnvironmentSimulator simulator = _map.Simulator;
      if (this.weatherSubscribeDisposable != null)
        this.weatherSubscribeDisposable.Dispose();
      this.weatherSubscribeDisposable = ObservableExtensions.Subscribe<Weather>(Observable.TakeUntilDestroy<Weather>((IObservable<M0>) simulator.OnWeatherChangedAsObservable(), (Component) _map), (Action<M0>) (_ => this.ChangedWeather(_map.Player)));
    }

    public void StopWeatherSubscribe()
    {
      if (this.weatherSubscribeDisposable == null)
        return;
      this.weatherSubscribeDisposable.Dispose();
      this.weatherSubscribeDisposable = (IDisposable) null;
    }

    public void PlayMapBGM()
    {
      this.MapBGMUpdateFlag = (SoundPlayer.UpdateType) 0;
      if (!Singleton<Resources>.IsInstance() || !Singleton<Map>.IsInstance() || !Singleton<Manager.Sound>.IsInstance())
        return;
      int mapId = Singleton<Map>.Instance.MapID;
      PlayerActor player = Singleton<Map>.Instance.Player;
      MapArea mapArea = !Object.op_Inequality((Object) player, (Object) null) ? (MapArea) null : player.MapArea;
      if (Object.op_Equality((Object) mapArea, (Object) null))
        return;
      int index = mapArea.AreaID;
      if (mapId == 1)
        index = 1;
      string self = (string) null;
      Dictionary<int, string> dictionary1 = (Dictionary<int, string>) null;
      if (Singleton<Manager.Game>.IsInstance())
      {
        if (mapId == 0)
        {
          dictionary1 = Singleton<Manager.Game>.Instance.Environment?.JukeBoxAudioNameTable;
        }
        else
        {
          Dictionary<int, Dictionary<int, string>> boxAudioNameTable = Singleton<Manager.Game>.Instance.Environment?.AnotherJukeBoxAudioNameTable;
          if (boxAudioNameTable != null && (!boxAudioNameTable.TryGetValue(mapId, out dictionary1) || dictionary1 == null))
          {
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            boxAudioNameTable[mapId] = dictionary2;
            dictionary1 = dictionary2;
          }
        }
      }
      bool? nullable = dictionary1?.TryGetValue(index, out self);
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) != 0 && !self.IsNullOrEmpty())
      {
        AudioClip _audioClip = (AudioClip) null;
        Dictionary<int, AudioClip> dictionary2 = (Dictionary<int, AudioClip>) null;
        if (!this.jukeBoxAudioClipCacheTable.TryGetValue(mapId, out dictionary2) || dictionary2 == null)
        {
          Dictionary<int, AudioClip> dictionary3 = new Dictionary<int, AudioClip>();
          this.jukeBoxAudioClipCacheTable[mapId] = dictionary3;
          dictionary2 = dictionary3;
        }
        if (dictionary2.TryGetValue(index, out _audioClip) && Object.op_Inequality((Object) _audioClip, (Object) null))
        {
          Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary3 = (Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>) null;
          if (!this.housingAreaAudioTable.TryGetValue(mapId, out dictionary3) || dictionary3 == null)
          {
            Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary4 = new Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>();
            this.housingAreaAudioTable[mapId] = dictionary4;
            dictionary3 = dictionary4;
          }
          ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple;
          if (dictionary3.TryGetValue(index, out valueTuple) && Object.op_Inequality((Object) valueTuple.Item1, (Object) null) && (((AudioSource) valueTuple.Item1).get_isPlaying() && Object.op_Equality((Object) ((AudioSource) valueTuple.Item1).get_clip(), (Object) _audioClip)))
          {
            bool flag = false;
            if (valueTuple.Item3 != null)
            {
              ((IDisposable) valueTuple.Item3).Dispose();
              valueTuple.Item3 = null;
              flag = true;
            }
            if (valueTuple.Item2 == null)
            {
              float mapBgmFadeTime = Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime;
              AudioSource _audio = (AudioSource) valueTuple.Item1;
              float _startVolume = _audio.get_volume();
              FadePlayer _fadePlayer = (FadePlayer) ((Component) _audio).GetComponent<FadePlayer>();
              valueTuple.Item2 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(mapBgmFadeTime, false), false), (Component) _audio), (Action<M0>) (x =>
              {
                float num = Mathf.Lerp(_startVolume, 1f, ((TimeInterval<float>) ref x).get_Value());
                if (Object.op_Inequality((Object) _fadePlayer, (Object) null))
                  _audio.set_volume(_fadePlayer.currentVolume = num);
                else
                  _audio.set_volume(num);
              }));
              this.StopMapAreaBGM(mapBgmFadeTime);
              this.MuteHousingAreaBGM(mapId, index, mapBgmFadeTime, false);
              flag = true;
            }
            this.LastBGMAudio = (AudioSource) valueTuple.Item1;
            if (!flag)
              return;
            dictionary3[index] = valueTuple;
            return;
          }
        }
        if (Object.op_Equality((Object) _audioClip, (Object) null))
        {
          bool loadedTarget = false;
          _audioClip = SoundPlayer.LoadAudioClip(SoundPlayer.Directory.AudioFile + self, ref loadedTarget, this.uAudio);
          if (Object.op_Inequality((Object) _audioClip, (Object) null))
            dictionary2[index] = _audioClip;
        }
        if (Object.op_Inequality((Object) _audioClip, (Object) null))
        {
          this.PlayJukeAreaBGM(_audioClip, mapId, index);
          return;
        }
      }
      this.PlayDefaultBGM(mapId, index);
    }

    private void PlayDefaultBGM(int _mapID, int _areaID)
    {
      Resources.SoundTable sound = Singleton<Resources>.Instance.Sound;
      Dictionary<int, SoundPlayer.MapBGMInfo> dictionary;
      SoundPlayer.MapBGMInfo _bgmInfo;
      if (!sound.MapBGMInfoTable.TryGetValue(_mapID, out dictionary) || !dictionary.TryGetValue(_areaID, out _bgmInfo))
        return;
      EnvironmentSimulator simulator = Singleton<Map>.Instance.Simulator;
      int id = _bgmInfo.GetID(simulator.Now);
      AssetBundleInfo _assetInfo;
      if (!sound.MapBGMABTable.TryGetValue(id, out _assetInfo))
        return;
      Transform asset = Singleton<Manager.Sound>.Instance.FindAsset(Manager.Sound.Type.BGM, (string) _assetInfo.asset, (string) _assetInfo.assetbundle);
      if (Object.op_Inequality((Object) asset, (Object) null))
      {
        float mapBgmFadeTime = Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime;
        AudioSource component = (AudioSource) ((Component) asset).GetComponent<AudioSource>();
        if (Object.op_Inequality((Object) component, (Object) null) && !component.get_isPlaying())
          Singleton<Manager.Sound>.Instance.PlayBGM(mapBgmFadeTime);
        if (Object.op_Inequality((Object) component, (Object) null))
          this.LastBGMAudio = component;
        this.MuteHousingAreaBGM(mapBgmFadeTime, false);
      }
      else
        this.PlayMapBGM(_bgmInfo, _assetInfo);
    }

    private void PlayMapBGM(SoundPlayer.MapBGMInfo _bgmInfo, AssetBundleInfo _assetInfo)
    {
      Illusion.Game.Utils.Sound.SettingBGM setting = _bgmInfo.Setting;
      setting.assetBundleName = (string) _assetInfo.assetbundle;
      setting.assetName = (string) _assetInfo.asset;
      float _fadeTime = setting.fadeTime = Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime;
      Transform transform = Illusion.Game.Utils.Sound.Play((Illusion.Game.Utils.Sound.Setting) setting);
      AudioSource audioSource = !Object.op_Inequality((Object) transform, (Object) null) ? (AudioSource) null : (AudioSource) ((Component) transform).GetComponent<AudioSource>();
      if (!Object.op_Inequality((Object) audioSource, (Object) null))
        return;
      this.MuteHousingAreaBGM(_fadeTime, false);
      this.LastBGMAudio = audioSource;
      this.PrevBGMAudioABInfo = this.CurrentBGMAudioABInfo;
      this.CurrentBGMAudioABInfo = _assetInfo;
      if (this.delayPlayBGMDisposable != null)
        this.delayPlayBGMDisposable.Dispose();
      this.BGMChangePossible = false;
      this.delayPlayBGMDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) _fadeTime)), ((Component) this).get_gameObject()), (Action<M0>) (_ =>
      {
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }), (Action<Exception>) (ex =>
      {
        Debug.LogException(ex);
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }), (Action) (() =>
      {
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }));
    }

    private void PlayJukeAreaBGM(AudioClip _audioClip, int _mapID, int _areaID)
    {
      if (Object.op_Equality((Object) _audioClip, (Object) null))
        return;
      LoadSound bgm = Illusion.Game.Utils.Sound.GetBGM();
      if (Object.op_Inequality((Object) bgm, (Object) null) && Object.op_Inequality((Object) bgm.audioSource, (Object) null) && (bgm.audioSource.get_isPlaying() && Object.op_Equality((Object) bgm.audioSource.get_clip(), (Object) _audioClip)))
        return;
      float mapBgmFadeTime = Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime;
      AudioSource _audioSource = Illusion.Game.Utils.Sound.Play(Manager.Sound.Type.BGM, _audioClip, mapBgmFadeTime);
      if (Object.op_Equality((Object) _audioSource, (Object) null))
        return;
      _audioSource.set_loop(true);
      Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary1 = (Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>) null;
      if (!this.housingAreaAudioTable.TryGetValue(_mapID, out dictionary1) || dictionary1 == null)
      {
        Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary2 = new Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>();
        this.housingAreaAudioTable[_mapID] = dictionary2;
        dictionary1 = dictionary2;
      }
      dictionary1[_areaID] = new ValueTuple<AudioSource, IDisposable, IDisposable>(_audioSource, ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) mapBgmFadeTime))), (IDisposable) null);
      this.StopMapAreaBGM(mapBgmFadeTime);
      this.MuteHousingAreaBGM(_mapID, _areaID, mapBgmFadeTime, false);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) _audioSource), (Action<M0>) (_ =>
      {
        if (!_audioSource.get_isPlaying() || !mp3AudioClip.flare_SongEnd)
          return;
        if (_audioSource.get_loop())
        {
          mp3AudioClip.SongDone = mp3AudioClip.flare_SongEnd = false;
          this.uAudio.set_CurrentTime(TimeSpan.Zero);
        }
        else
          _audioSource.Stop();
      }));
      this.LastBGMAudio = _audioSource;
      this.PrevBGMAudioABInfo = this.CurrentBGMAudioABInfo;
      this.CurrentBGMAudioABInfo = (AssetBundleInfo) null;
      if (this.delayPlayBGMDisposable != null)
        this.delayPlayBGMDisposable.Dispose();
      this.BGMChangePossible = false;
      this.delayPlayBGMDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) mapBgmFadeTime)), ((Component) this).get_gameObject()), (Action<M0>) (_ =>
      {
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }), (Action<Exception>) (ex =>
      {
        if (Debug.get_isDebugBuild())
          Debug.LogException(ex);
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }), (Action) (() =>
      {
        this.delayPlayBGMDisposable = (IDisposable) null;
        this.BGMChangePossible = true;
      }));
    }

    public void StopMapAreaBGM(float _fadeTime = 0.0f)
    {
      if (!Singleton<Manager.Sound>.IsInstance())
        return;
      GameObject currentBgm = Singleton<Manager.Sound>.Instance.currentBGM;
      if (!Object.op_Inequality((Object) currentBgm, (Object) null))
        return;
      FadePlayer component = (FadePlayer) currentBgm.GetComponent<FadePlayer>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.Stop(_fadeTime);
      else
        Object.Destroy((Object) currentBgm);
      Singleton<Manager.Sound>.Instance.currentBGM = (GameObject) null;
    }

    private void ResizeHousingAreaAudioTable()
    {
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>) this.housingAreaAudioTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>())
        return;
      List<ValueTuple<int, int>> toRelease = ListPool<ValueTuple<int, int>>.Get();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>.Enumerator enumerator1 = this.housingAreaAudioTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> current1 = enumerator1.Current;
          Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary = current1.Value;
          if (!((IReadOnlyDictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>) dictionary).IsNullOrEmpty<int, ValueTuple<AudioSource, IDisposable, IDisposable>>())
          {
            using (Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>.Enumerator enumerator2 = dictionary.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, ValueTuple<AudioSource, IDisposable, IDisposable>> current2 = enumerator2.Current;
                if (Object.op_Equality((Object) current2.Value.Item1, (Object) null))
                  toRelease.Add(new ValueTuple<int, int>(current1.Key, current2.Key));
              }
            }
          }
        }
      }
      using (List<ValueTuple<int, int>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, int> current = enumerator.Current;
          this.housingAreaAudioTable[(int) current.Item1].Remove((int) current.Item2);
        }
      }
      ListPool<ValueTuple<int, int>>.Release(toRelease);
    }

    public void ForcedMuteHousingAreaBGM(float _fadeTime = 0.0f, bool _ignoreTimeScale = false)
    {
      this.ResizeHousingAreaAudioTable();
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>) this.housingAreaAudioTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>())
        return;
      List<ValueTuple<int, List<int>>> toRelease = ListPool<ValueTuple<int, List<int>>>.Get();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>.Enumerator enumerator = this.housingAreaAudioTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> current = enumerator.Current;
          List<int> intList = ListPool<int>.Get();
          intList.AddRange((IEnumerable<int>) current.Value.Keys);
          toRelease.Add(new ValueTuple<int, List<int>>(current.Key, intList));
        }
      }
      using (List<ValueTuple<int, List<int>>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<int>> current = enumerator.Current;
          foreach (int index in (List<int>) current.Item2)
          {
            ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple = this.housingAreaAudioTable[(int) current.Item1][index];
            if (valueTuple.Item2 != null)
            {
              ((IDisposable) valueTuple.Item2).Dispose();
              valueTuple.Item2 = null;
            }
            AudioSource _audio = (AudioSource) valueTuple.Item1;
            float _startVolume = _audio.get_volume();
            FadePlayer _fadePlayer = (FadePlayer) ((Component) _audio).GetComponent<FadePlayer>();
            ((IDisposable) valueTuple.Item3)?.Dispose();
            valueTuple.Item3 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, _ignoreTimeScale), _ignoreTimeScale), (Component) _audio), (Action<M0>) (x =>
            {
              if (Object.op_Inequality((Object) _fadePlayer, (Object) null))
                _audio.set_volume(_fadePlayer.currentVolume = Mathf.Lerp(_startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
              else
                _audio.set_volume(Mathf.Lerp(_startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
            }), (Action<Exception>) (ex =>
            {
              if (Object.op_Equality((Object) _audio, (Object) null))
                return;
              if (Object.op_Inequality((Object) _fadePlayer, (Object) null))
                _audio.set_volume(_fadePlayer.currentVolume = 0.0f);
              else
                _audio.set_volume(0.0f);
            }), (Action) (() =>
            {
              if (Object.op_Equality((Object) _audio, (Object) null))
                return;
              if (Object.op_Inequality((Object) _fadePlayer, (Object) null))
                _audio.set_volume(_fadePlayer.currentVolume = 0.0f);
              else
                _audio.set_volume(0.0f);
            }));
            this.housingAreaAudioTable[(int) current.Item1][index] = valueTuple;
          }
        }
      }
      using (List<ValueTuple<int, List<int>>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
          ListPool<int>.Release((List<int>) enumerator.Current.Item2);
      }
      ListPool<ValueTuple<int, List<int>>>.Release(toRelease);
    }

    public void MuteHousingAreaBGM(float _fadeTime = 0.0f, bool _ignoreTimeScale = false)
    {
      this.ResizeHousingAreaAudioTable();
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>) this.housingAreaAudioTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>())
        return;
      List<ValueTuple<int, List<int>>> toRelease = ListPool<ValueTuple<int, List<int>>>.Get();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>.Enumerator enumerator = this.housingAreaAudioTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> current = enumerator.Current;
          List<int> intList = ListPool<int>.Get();
          intList.AddRange((IEnumerable<int>) current.Value.Keys);
          toRelease.Add(new ValueTuple<int, List<int>>(current.Key, intList));
        }
      }
      using (List<ValueTuple<int, List<int>>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<int>> current = enumerator.Current;
          foreach (int index in (List<int>) current.Item2)
          {
            ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple = this.housingAreaAudioTable[(int) current.Item1][index];
            bool flag = false;
            if (valueTuple.Item2 != null)
            {
              ((IDisposable) valueTuple.Item2).Dispose();
              valueTuple.Item2 = null;
              flag = true;
            }
            if (valueTuple.Item3 == null)
            {
              AudioSource audio = (AudioSource) valueTuple.Item1;
              FadePlayer fadePlayer = (FadePlayer) ((Component) audio).GetComponent<FadePlayer>();
              float startVolume = audio.get_volume();
              valueTuple.Item3 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, _ignoreTimeScale), _ignoreTimeScale), (Component) audio), (Action<M0>) (x =>
              {
                float num = Mathf.Lerp(startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value());
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = num);
                else
                  audio.set_volume(num);
              }), (Action<Exception>) (ex =>
              {
                if (Object.op_Equality((Object) audio, (Object) null))
                  return;
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = 0.0f);
                else
                  audio.set_volume(0.0f);
              }), (Action) (() =>
              {
                if (Object.op_Equality((Object) audio, (Object) null))
                  return;
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = 0.0f);
                else
                  audio.set_volume(0.0f);
              }));
              flag = true;
            }
            if (flag)
              this.housingAreaAudioTable[(int) current.Item1][index] = valueTuple;
          }
        }
      }
      using (List<ValueTuple<int, List<int>>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
          ListPool<int>.Release((List<int>) enumerator.Current.Item2);
      }
      ListPool<ValueTuple<int, List<int>>>.Release(toRelease);
    }

    public void MuteHousingAreaBGM(
      int _mapID,
      int _areaID,
      float _fadeTime = 0.0f,
      bool _ignoreTimeScale = false)
    {
      this.ResizeHousingAreaAudioTable();
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>) this.housingAreaAudioTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>())
        return;
      Dictionary<int, List<int>> toRelease1 = DictionaryPool<int, List<int>>.Get();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>.Enumerator enumerator1 = this.housingAreaAudioTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> current1 = enumerator1.Current;
          Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary = current1.Value;
          if (!((IReadOnlyDictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>) dictionary).IsNullOrEmpty<int, ValueTuple<AudioSource, IDisposable, IDisposable>>())
          {
            int key1 = current1.Key;
            using (Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>.Enumerator enumerator2 = dictionary.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, ValueTuple<AudioSource, IDisposable, IDisposable>> current2 = enumerator2.Current;
                ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple = current2.Value;
                if (!Object.op_Equality((Object) valueTuple.Item1, (Object) null) && valueTuple.Item3 == null)
                {
                  int key2 = current2.Key;
                  if (key1 != _mapID || key2 != _areaID)
                  {
                    List<int> intList = (List<int>) null;
                    if (!toRelease1.TryGetValue(key1, out intList) || intList == null)
                      toRelease1[key1] = intList = ListPool<int>.Get();
                    if (!intList.Contains(key2))
                      intList.Add(key2);
                  }
                }
              }
            }
          }
        }
      }
      if (!((IReadOnlyDictionary<int, List<int>>) toRelease1).IsNullOrEmpty<int, List<int>>())
      {
        foreach (KeyValuePair<int, List<int>> keyValuePair in toRelease1)
        {
          List<int> intList = keyValuePair.Value;
          if (!((IReadOnlyList<int>) intList).IsNullOrEmpty<int>())
          {
            int key = keyValuePair.Key;
            foreach (int index in intList)
            {
              ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple = this.housingAreaAudioTable[key][index];
              if (valueTuple.Item2 != null)
              {
                ((IDisposable) valueTuple.Item2).Dispose();
                valueTuple.Item2 = null;
              }
              AudioSource audio = (AudioSource) valueTuple.Item1;
              FadePlayer fadePlayer = (FadePlayer) ((Component) audio).GetComponent<FadePlayer>();
              float startVolume = audio.get_volume();
              ((IDisposable) valueTuple.Item3)?.Dispose();
              valueTuple.Item3 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, _ignoreTimeScale), _ignoreTimeScale), (Component) valueTuple.Item1), (Action<M0>) (x =>
              {
                float num = Mathf.Lerp(startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value());
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = num);
                else
                  audio.set_volume(num);
              }), (Action<Exception>) (ex =>
              {
                if (Object.op_Equality((Object) audio, (Object) null))
                  return;
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = 0.0f);
                else
                  audio.set_volume(0.0f);
              }), (Action) (() =>
              {
                if (Object.op_Equality((Object) audio, (Object) null))
                  return;
                if (Object.op_Inequality((Object) fadePlayer, (Object) null))
                  audio.set_volume(fadePlayer.currentVolume = 0.0f);
                else
                  audio.set_volume(0.0f);
              }));
              this.housingAreaAudioTable[key][index] = valueTuple;
            }
          }
        }
      }
      if (!((IReadOnlyDictionary<int, List<int>>) toRelease1).IsNullOrEmpty<int, List<int>>())
      {
        foreach (KeyValuePair<int, List<int>> keyValuePair in toRelease1)
        {
          List<int> toRelease2 = keyValuePair.Value;
          if (toRelease2 != null)
            ListPool<int>.Release(toRelease2);
        }
      }
      DictionaryPool<int, List<int>>.Release(toRelease1);
    }

    public void ActivateMapBGM()
    {
      this.BGMPlayActive = true;
      this.BGMChangePossible = true;
      this.PlayMapBGM();
    }

    public void PauseMapBGM()
    {
      if (Singleton<Manager.Sound>.IsInstance())
        Singleton<Manager.Sound>.Instance.PauseBGM();
      this.BGMPlayActive = false;
    }

    public void StopMapBGM(float _fadeTime = 0.0f)
    {
      this.BGMPlayActive = false;
      this.DisposeDelayPlayBGM();
      if (!Singleton<Manager.Sound>.IsInstance())
        return;
      GameObject currentBgm = Singleton<Manager.Sound>.Instance.currentBGM;
      if (Object.op_Equality((Object) currentBgm, (Object) null))
        return;
      AudioSource _audio = (AudioSource) currentBgm.GetComponent<AudioSource>();
      FadePlayer fadePlayer = !Object.op_Inequality((Object) _audio, (Object) null) ? (FadePlayer) null : (FadePlayer) ((Component) _audio).GetComponent<FadePlayer>();
      if (Object.op_Inequality((Object) _audio, (Object) null) && Object.op_Equality((Object) _audio, (Object) this.LastBGMAudio))
        this.LastBGMAudio = (AudioSource) null;
      if (Object.op_Inequality((Object) fadePlayer, (Object) null))
        fadePlayer.Stop(_fadeTime);
      else if (Object.op_Inequality((Object) _audio, (Object) null))
      {
        float _startVolume = _audio.get_volume();
        ObservableExtensions.Subscribe<FrameInterval<float>>(Observable.TakeUntilDestroy<FrameInterval<float>>((IObservable<M0>) Observable.FrameInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, false)), (Component) _audio), (Action<M0>) (x => _audio.set_volume(Mathf.Lerp(_startVolume, 0.0f, ((FrameInterval<float>) ref x).get_Value()))), (Action<Exception>) (ex =>
        {
          if (!Object.op_Inequality((Object) _audio, (Object) null) || !Object.op_Inequality((Object) ((Component) _audio).get_gameObject(), (Object) null))
            return;
          Object.Destroy((Object) ((Component) _audio).get_gameObject());
        }), (Action) (() =>
        {
          if (!Object.op_Inequality((Object) _audio, (Object) null) || !Object.op_Inequality((Object) ((Component) _audio).get_gameObject(), (Object) null))
            return;
          Object.Destroy((Object) ((Component) _audio).get_gameObject());
        }));
      }
      Singleton<Manager.Sound>.Instance.currentBGM = (GameObject) null;
    }

    public void StopHousingAreaBGM(
      int _mapID,
      int _areaID,
      float _fadeTime = 0.0f,
      bool _ignoreTimeScale = false)
    {
      Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary1 = (Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>) null;
      if (!this.housingAreaAudioTable.TryGetValue(_mapID, out dictionary1) || dictionary1 == null)
      {
        Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>> dictionary2 = new Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>();
        this.housingAreaAudioTable[_mapID] = dictionary2;
        dictionary1 = dictionary2;
      }
      ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple;
      if (!dictionary1.TryGetValue(_areaID, out valueTuple))
        return;
      ((IDisposable) valueTuple.Item2)?.Dispose();
      ((IDisposable) valueTuple.Item3)?.Dispose();
      AudioSource audio = (AudioSource) valueTuple.Item1;
      FadePlayer fadePlayer = !Object.op_Inequality((Object) audio, (Object) null) ? (FadePlayer) null : (FadePlayer) ((Component) audio).GetComponent<FadePlayer>();
      if (Object.op_Inequality((Object) audio, (Object) null))
      {
        float startVolume = audio.get_volume();
        valueTuple.Item3 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, _ignoreTimeScale), _ignoreTimeScale), (Component) audio), (Action<M0>) (x =>
        {
          if (Object.op_Inequality((Object) fadePlayer, (Object) null))
            audio.set_volume(fadePlayer.currentVolume = Mathf.Lerp(startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
          else
            audio.set_volume(Mathf.Lerp(startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value()));
        }), (Action<Exception>) (ex =>
        {
          if (!Object.op_Inequality((Object) audio, (Object) null) || !Object.op_Inequality((Object) ((Component) audio).get_gameObject(), (Object) null))
            return;
          Object.Destroy((Object) ((Component) audio).get_gameObject());
        }), (Action) (() =>
        {
          if (!Object.op_Inequality((Object) audio, (Object) null) || !Object.op_Inequality((Object) ((Component) audio).get_gameObject(), (Object) null))
            return;
          Object.Destroy((Object) ((Component) audio).get_gameObject());
        }));
      }
      dictionary1.Remove(_areaID);
    }

    public void StopHousingAreaBGM(float _fadeTime = 0.0f)
    {
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>) this.housingAreaAudioTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>())
        return;
      List<ValueTuple<int, List<int>>> valueTupleList = ListPool<ValueTuple<int, List<int>>>.Get();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>>.Enumerator enumerator = this.housingAreaAudioTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, IDisposable, IDisposable>>> current = enumerator.Current;
          List<int> intList = ListPool<int>.Get();
          intList.AddRange((IEnumerable<int>) current.Value.Keys);
          valueTupleList.Add(new ValueTuple<int, List<int>>(current.Key, intList));
        }
      }
      using (List<ValueTuple<int, List<int>>>.Enumerator enumerator = valueTupleList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, List<int>> current = enumerator.Current;
          foreach (int index in (List<int>) current.Item2)
          {
            ValueTuple<AudioSource, IDisposable, IDisposable> valueTuple = this.housingAreaAudioTable[(int) current.Item1][index];
            ((IDisposable) valueTuple.Item2)?.Dispose();
            ((IDisposable) valueTuple.Item3)?.Dispose();
            AudioSource audio = (AudioSource) valueTuple.Item1;
            FadePlayer fadePlayer = !Object.op_Inequality((Object) audio, (Object) null) ? (FadePlayer) null : (FadePlayer) ((Component) audio).GetComponent<FadePlayer>();
            if (Object.op_Inequality((Object) fadePlayer, (Object) null))
            {
              fadePlayer.Stop(_fadeTime);
            }
            else
            {
              float startVolume = audio.get_volume();
              valueTuple.Item3 = (__Null) ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(_fadeTime, false), false), (Component) audio), (Action<M0>) (x => audio.set_volume(Mathf.Lerp(startVolume, 0.0f, ((TimeInterval<float>) ref x).get_Value()))), (Action<Exception>) (ex =>
              {
                if (!Object.op_Inequality((Object) audio, (Object) null) || !Object.op_Inequality((Object) ((Component) audio).get_gameObject(), (Object) null))
                  return;
                Object.Destroy((Object) ((Component) audio).get_gameObject());
              }), (Action) (() =>
              {
                if (!Object.op_Inequality((Object) audio, (Object) null) || !Object.op_Inequality((Object) ((Component) audio).get_gameObject(), (Object) null))
                  return;
                Object.Destroy((Object) ((Component) audio).get_gameObject());
              }));
            }
            if (Object.op_Inequality((Object) audio, (Object) null) && Object.op_Inequality((Object) ((Component) audio).get_gameObject(), (Object) null))
              Object.Destroy((Object) ((Component) audio).get_gameObject());
          }
        }
      }
      this.housingAreaAudioTable.Clear();
    }

    public void RemoveAreaAudioClip(int _mapID, int _areaID)
    {
      if (!this.jukeBoxAudioClipCacheTable.ContainsKey(_mapID))
        return;
      this.jukeBoxAudioClipCacheTable[_mapID].Remove(_areaID);
    }

    private void DisposeDelayPlayBGM()
    {
      if (this.delayPlayBGMDisposable != null)
      {
        this.delayPlayBGMDisposable.Dispose();
        this.delayPlayBGMDisposable = (IDisposable) null;
      }
      this.BGMChangePossible = true;
    }

    public void PlayWideEnvSE(bool _quickChange = false)
    {
      if (_quickChange)
        this.DisposeDelayPlayWideEnv();
      this.WideEnvUpdateFlag = (SoundPlayer.UpdateType) 0;
      if (!Singleton<Resources>.IsInstance() || !Singleton<Map>.IsInstance() || !Singleton<Manager.Sound>.IsInstance())
        return;
      MapArea mapArea = !Object.op_Inequality((Object) Singleton<Map>.Instance.Player, (Object) null) ? (MapArea) null : Singleton<Map>.Instance.Player.MapArea;
      if (Object.op_Equality((Object) mapArea, (Object) null))
        return;
      float fadeTime = !_quickChange ? Singleton<Resources>.Instance.SoundPack.EnviroInfo.WideRangeSlowFadeTime : Singleton<Resources>.Instance.SoundPack.EnviroInfo.WideRangeQuickFadeTime;
      int mapId = Singleton<Map>.Instance.MapID;
      bool flag = Object.op_Inequality((Object) mapArea, (Object) null) && Singleton<Resources>.Instance.SoundPack.WideEnvMuteArea(mapId, mapArea.AreaID);
      SoundPack.PlayAreaType areaType = SoundPack.PlayAreaType.Normal;
      Weather weather = Singleton<Map>.Instance.Simulator.Weather;
      List<int> idList = ListPool<int>.Get();
      if (!Singleton<Resources>.Instance.SoundPack.TryGetWideEnvIDList(areaType, weather, ref idList) || flag)
      {
        if (!((IReadOnlyDictionary<int, ValueTuple<AudioSource, FadePlayer>>) this.usedWideEnvSE).IsNullOrEmpty<int, ValueTuple<AudioSource, FadePlayer>>())
        {
          using (Dictionary<int, ValueTuple<AudioSource, FadePlayer>>.Enumerator enumerator = this.usedWideEnvSE.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ValueTuple<AudioSource, FadePlayer> valueTuple = enumerator.Current.Value;
              if (Object.op_Inequality((Object) valueTuple.Item2, (Object) null))
                ((FadePlayer) valueTuple.Item2).Stop(fadeTime);
              else if (Object.op_Inequality((Object) valueTuple.Item1, (Object) null))
                Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.ENV, ((Component) valueTuple.Item1).get_transform());
            }
          }
          this.usedWideEnvSE.Clear();
        }
        ListPool<int>.Release(idList);
      }
      else
      {
        List<int> toRelease1 = ListPool<int>.Get();
        List<int> toRelease2 = ListPool<int>.Get();
        List<int> toRelease3 = ListPool<int>.Get();
        using (Dictionary<int, ValueTuple<AudioSource, FadePlayer>>.Enumerator enumerator = this.usedWideEnvSE.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ValueTuple<AudioSource, FadePlayer>> current = enumerator.Current;
            if (Object.op_Equality((Object) current.Value.Item1, (Object) null) || Object.op_Equality((Object) ((Component) current.Value.Item1).get_gameObject(), (Object) null))
              toRelease1.Add(current.Key);
            else if (!((AudioSource) current.Value.Item1).get_isPlaying())
              toRelease2.Add(current.Key);
            else if (!idList.Contains(current.Key))
              toRelease3.Add(current.Key);
          }
        }
        for (int index = 0; index < toRelease1.Count; ++index)
          this.usedWideEnvSE.Remove(toRelease1[index]);
        for (int index = 0; index < toRelease2.Count; ++index)
        {
          int key = toRelease2[index];
          Object.Destroy((Object) ((Component) this.usedWideEnvSE[key].Item1).get_gameObject());
          this.usedWideEnvSE.Remove(key);
        }
        for (int index = 0; index < toRelease3.Count; ++index)
        {
          int key = toRelease3[index];
          ValueTuple<AudioSource, FadePlayer> valueTuple = this.usedWideEnvSE[key];
          if (Object.op_Inequality((Object) valueTuple.Item2, (Object) null))
            ((FadePlayer) valueTuple.Item2).Stop(fadeTime);
          else if (Object.op_Inequality((Object) valueTuple.Item1, (Object) null))
            Object.Destroy((Object) ((Component) valueTuple.Item1).get_gameObject());
          this.usedWideEnvSE.Remove(key);
        }
        ListPool<int>.Release(toRelease1);
        ListPool<int>.Release(toRelease2);
        ListPool<int>.Release(toRelease3);
        Camera cameraComponent = Map.GetCameraComponent();
        Transform _cameraT = !Object.op_Inequality((Object) cameraComponent, (Object) null) ? (Transform) null : ((Component) cameraComponent).get_transform();
        if (Object.op_Inequality((Object) _cameraT, (Object) null))
        {
          for (int index = 0; index < idList.Count; ++index)
          {
            int _id = idList[index];
            if (this.usedWideEnvSE.ContainsKey(_id))
              idList.RemoveAll((Predicate<int>) (x => x == _id));
          }
          for (int index = 0; index < idList.Count; ++index)
          {
            int clipID = idList[index];
            AudioSource _audio = Singleton<Resources>.Instance.SoundPack.PlayEnviroSE(clipID, fadeTime);
            if (!Object.op_Equality((Object) _audio, (Object) null))
            {
              FadePlayer componentInChildren = (FadePlayer) ((Component) _audio).GetComponentInChildren<FadePlayer>(true);
              _audio.set_loop(true);
              this.usedWideEnvSE[clipID] = new ValueTuple<AudioSource, FadePlayer>(_audio, componentInChildren);
              ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) _audio), (Component) _cameraT), (Component) _audio), (Action<M0>) (_ => ((Component) _audio).get_transform().SetPositionAndRotation(_cameraT.get_position(), _cameraT.get_rotation())));
            }
          }
        }
        ListPool<int>.Release(idList);
        if (this.delayPlayWideEnvDisposable != null)
          this.delayPlayWideEnvDisposable.Dispose();
        this.WideEnvChangePossible = false;
        this.delayPlayWideEnvDisposable = ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) fadeTime)), ((Component) this).get_gameObject()), (Action<M0>) (_ =>
        {
          this.delayPlayWideEnvDisposable = (IDisposable) null;
          this.WideEnvChangePossible = true;
        }), (Action<Exception>) (ex =>
        {
          Debug.LogException(ex);
          this.delayPlayWideEnvDisposable = (IDisposable) null;
          this.WideEnvChangePossible = true;
        }), (Action) (() =>
        {
          this.delayPlayWideEnvDisposable = (IDisposable) null;
          this.WideEnvChangePossible = true;
        }));
      }
    }

    public void ActivateWideEnvSE(bool _quickChange = false)
    {
      this.EnvPlayActive = true;
      this.WideEnvChangePossible = true;
      SoundPlayer.UpdateType updateType = (SoundPlayer.UpdateType) 0;
      this.PrevWideEnvUpdateFlag = updateType;
      this.WideEnvUpdateFlag = updateType;
      this.PlayWideEnvSE(_quickChange);
    }

    public void StopWideEnvSE(float _fadeTime = 0.0f)
    {
      this.DisposeDelayPlayWideEnv();
      if (!((IReadOnlyDictionary<int, ValueTuple<AudioSource, FadePlayer>>) this.usedWideEnvSE).IsNullOrEmpty<int, ValueTuple<AudioSource, FadePlayer>>())
      {
        using (Dictionary<int, ValueTuple<AudioSource, FadePlayer>>.Enumerator enumerator = this.usedWideEnvSE.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ValueTuple<AudioSource, FadePlayer>> current = enumerator.Current;
            if (Object.op_Inequality((Object) current.Value.Item2, (Object) null))
              ((FadePlayer) current.Value.Item2).Stop(_fadeTime);
            else if (Object.op_Inequality((Object) current.Value.Item1, (Object) null) && Object.op_Inequality((Object) ((Component) current.Value.Item1).get_gameObject(), (Object) null))
              Object.Destroy((Object) ((Component) current.Value.Item1).get_gameObject());
          }
        }
        this.usedWideEnvSE.Clear();
      }
      this.EnvPlayActive = false;
    }

    public void DisposeDelayPlayWideEnv()
    {
      if (this.delayPlayWideEnvDisposable != null)
      {
        this.delayPlayWideEnvDisposable.Dispose();
        this.delayPlayWideEnvDisposable = (IDisposable) null;
      }
      this.WideEnvChangePossible = true;
    }

    public void ActivateAllSound()
    {
      this.ActivateMapBGM();
      this.ActivateWideEnvSE(true);
    }

    public void StopAllSound(float _fadeTime = 0.0f)
    {
      this.StopMapBGM(_fadeTime);
      this.MuteHousingAreaBGM(_fadeTime, false);
      this.StopWideEnvSE(_fadeTime);
    }

    public void Release()
    {
      float _fadeTime = !Singleton<Resources>.IsInstance() ? 0.0f : Singleton<Resources>.Instance.SoundPack.BGMInfo.MapBGMFadeTime;
      this.StopMapBGM(_fadeTime);
      this.StopHousingAreaBGM(_fadeTime);
      this.StopWideEnvSE(_fadeTime);
      this.StopAllSubscribe();
    }

    public static AudioClip LoadAudioClip(
      string fileFullPath,
      ref bool loadedTarget,
      uAudio.uAudio_backend.uAudio uAudio)
    {
      ExternalAudioClip.LoadFile(fileFullPath, fileFullPath, ref loadedTarget, uAudio);
      AudioClip audioClip = (AudioClip) null;
      try
      {
        mp3AudioClip.SongDone = false;
        mp3AudioClip.flare_SongEnd = false;
        uAudio.targetFile = (__Null) fileFullPath;
        if (uAudio.LoadMainOutputStream())
        {
          string szErrorMs = (string) null;
          audioClip = ExternalAudioClip.Load(fileFullPath, (long) uAudio.get_SongLength(), uAudio, ref szErrorMs);
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) "uAudioPlayer - Play #j356j356j356j356");
        Debug.LogWarning((object) ex);
      }
      return audioClip;
    }

    public void RefreshJukeBoxTable(Map mapMana)
    {
      if (Object.op_Equality((Object) mapMana, (Object) null))
        return;
      int mapId = mapMana.MapID;
      Dictionary<int, string> dictionary1 = (Dictionary<int, string>) null;
      if (Singleton<Manager.Game>.IsInstance())
      {
        Manager.Game instance = Singleton<Manager.Game>.Instance;
        if (mapId == 0)
        {
          dictionary1 = instance.Environment?.JukeBoxAudioNameTable;
        }
        else
        {
          Dictionary<int, Dictionary<int, string>> boxAudioNameTable = instance.Environment?.AnotherJukeBoxAudioNameTable;
          if (boxAudioNameTable != null && (!boxAudioNameTable.TryGetValue(mapId, out dictionary1) || dictionary1 == null))
          {
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            boxAudioNameTable[mapId] = dictionary2;
            dictionary1 = dictionary2;
          }
        }
      }
      if (((IReadOnlyDictionary<int, string>) dictionary1).IsNullOrEmpty<int, string>())
        return;
      List<int> toRelease1 = ListPool<int>.Get();
      PointManager pointAgent = mapMana.PointAgent;
      if (Object.op_Inequality((Object) pointAgent, (Object) null))
      {
        BasePoint[] basePoints = pointAgent.BasePoints;
        if (!((IReadOnlyList<BasePoint>) basePoints).IsNullOrEmpty<BasePoint>())
        {
          foreach (BasePoint basePoint in basePoints)
          {
            if (!Object.op_Equality((Object) basePoint, (Object) null) && basePoint.IsHousing)
            {
              int areaIdInHousing = basePoint.AreaIDInHousing;
              if (!toRelease1.Contains(areaIdInHousing))
                toRelease1.Add(areaIdInHousing);
            }
          }
        }
      }
      if (((IReadOnlyList<int>) toRelease1).IsNullOrEmpty<int>())
      {
        dictionary1.Clear();
        ListPool<int>.Release(toRelease1);
      }
      else
      {
        List<int> toRelease2 = ListPool<int>.Get();
        foreach (KeyValuePair<int, string> keyValuePair in dictionary1)
        {
          if (!toRelease1.Contains(keyValuePair.Key) && !toRelease2.Contains(keyValuePair.Key))
            toRelease2.Add(keyValuePair.Key);
        }
        foreach (int key in toRelease2)
          dictionary1.Remove(key);
        ListPool<int>.Release(toRelease1);
        ListPool<int>.Release(toRelease2);
      }
    }

    public static class Directory
    {
      public static string AudioFile { get; } = UserData.Create("audio");
    }

    [Flags]
    public enum UpdateType
    {
      Area = 1,
      Weather = 2,
      Time = 4,
    }

    public struct MapBGMInfo
    {
      public MapBGMInfo(
        int _noonID,
        SoundPlayer.MapBGMInfo.Period[] _noonPeriod,
        int _nightID,
        SoundPlayer.MapBGMInfo.Period[] _nightPeriod)
      {
        this.NoonID = _noonID;
        this.NoonPeriod = _noonPeriod;
        this.NightID = _nightID;
        this.NightPeriod = _nightPeriod;
        this.Setting = new Illusion.Game.Utils.Sound.SettingBGM();
      }

      public int NoonID { get; private set; }

      public SoundPlayer.MapBGMInfo.Period[] NoonPeriod { get; private set; }

      public int NightID { get; private set; }

      public SoundPlayer.MapBGMInfo.Period[] NightPeriod { get; private set; }

      public Illusion.Game.Utils.Sound.SettingBGM Setting { get; private set; }

      private bool OverTime(DateTime _base, DateTime _time)
      {
        return _base.Year <= _time.Year && _base.Month <= _time.Month && (_base.Day <= _time.Day && _base.Hour <= _time.Hour) && _base.Minute <= _time.Minute;
      }

      private bool UnderTime(DateTime _base, DateTime _time)
      {
        return _time.Year <= _base.Year && _time.Month <= _base.Month && (_time.Day <= _base.Day && _time.Hour <= _base.Hour) && _time.Minute <= _base.Minute;
      }

      public int GetID(DateTime _time)
      {
        if (!((IReadOnlyList<SoundPlayer.MapBGMInfo.Period>) this.NoonPeriod).IsNullOrEmpty<SoundPlayer.MapBGMInfo.Period>())
        {
          foreach (SoundPlayer.MapBGMInfo.Period period in this.NoonPeriod)
          {
            if (this.OverTime(period.Start, _time) && this.UnderTime(period.End, _time))
              return this.NoonID;
          }
        }
        if (!((IReadOnlyList<SoundPlayer.MapBGMInfo.Period>) this.NightPeriod).IsNullOrEmpty<SoundPlayer.MapBGMInfo.Period>())
        {
          foreach (SoundPlayer.MapBGMInfo.Period period in this.NightPeriod)
          {
            if (this.OverTime(period.Start, _time) && this.UnderTime(period.End, _time))
              return this.NightID;
          }
        }
        return -1;
      }

      public struct Period
      {
        public DateTime Start;
        public DateTime End;
      }
    }
  }
}

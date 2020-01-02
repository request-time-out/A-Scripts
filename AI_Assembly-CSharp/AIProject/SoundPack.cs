// Decompiled with JetBrains decompiler
// Type: AIProject.SoundPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEx;

namespace AIProject
{
  public class SoundPack : SerializedScriptableObject
  {
    private static Dictionary<string, Dictionary<string, ValueTuple<int, AudioClip>>> _usedEnviroSEInfo = new Dictionary<string, Dictionary<string, ValueTuple<int, AudioClip>>>();
    private static Dictionary<string, Dictionary<string, AudioClip>> _audioClipTable = new Dictionary<string, Dictionary<string, AudioClip>>();
    [SerializeField]
    [FoldoutGroup("定義", 0)]
    [LabelText("サウンド全般的定義")]
    private SoundPack.SoundSystemInfoGroup _soundSystemInfo;
    [SerializeField]
    [FoldoutGroup("定義", 0)]
    [LabelText("BGM関連定義")]
    private SoundPack.BGMInfoGroup _bgmInfo;
    [SerializeField]
    [FoldoutGroup("定義", 0)]
    [LabelText("3DSE関連定義")]
    private SoundPack.Game3DSEInfoGroup _game3DSEInfo;
    [SerializeField]
    [FoldoutGroup("定義", 0)]
    [LabelText("環境音関連定義")]
    private SoundPack.EnviroSEInfoGroup _enviroInfo;
    [SerializeField]
    [FoldoutGroup("定義", 0)]
    [LabelText("足音関連定義")]
    private SoundPack.FootStepInfoGroup _footStepInfo;
    [SerializeField]
    [FoldoutGroup("定義/再生IDテーブル", 0)]
    [DictionaryDrawerSettings(KeyLabel = "ドアの材質", ValueLabel = "SE ID")]
    private Dictionary<DoorMatType, SoundPack.DoorSEIDInfo> _doorIDTable;
    [NonSerialized]
    private List<AudioSource> PlayingAudioList;
    [SerializeField]
    [FoldoutGroup("アクションSE関連", 0)]
    [DictionaryDrawerSettings(KeyLabel = "ID", ValueLabel = "Info")]
    private Dictionary<int, List<SoundPack.Data3D>> _actionSEDataTable;
    [SerializeField]
    [FoldoutGroup("システムSE関係", 0)]
    [DictionaryDrawerSettings(KeyLabel = "ID", ValueLabel = "Info")]
    private Dictionary<int, SoundPack.Data2D> _systemSEDataTable;
    [SerializeField]
    [FoldoutGroup("システムSE関係", 0)]
    private Dictionary<SoundPack.SystemSE, int> _systemSETable;
    [SerializeField]
    [Header("男女共通")]
    [FoldoutGroup("足音関係", 0)]
    [DictionaryDrawerSettings(KeyLabel = "Tag Name", ValueLabel = "SE Type")]
    private Dictionary<string, AIProject.Definitions.Map.FootStepSE> _footStepSETagTable;
    [SerializeField]
    [FoldoutGroup("足音関係/男性", 0)]
    [HideInInspector]
    [LabelText("音量スケール")]
    [Range(0.0f, 1f)]
    private float _maleFootStepVolumeScale;
    [SerializeField]
    [FoldoutGroup("足音関係/男性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE ID", ValueLabel = "SE Info")]
    private Dictionary<int, SoundPack.Data2D> _maleFootStepSEData;
    [SerializeField]
    [FoldoutGroup("足音関係/男性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _maleFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("足音関係/男性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _maleBareFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("足音関係/男性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _maleRainFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("足音関係/女性", 0)]
    [HideInInspector]
    [LabelText("音量スケール")]
    [Range(0.0f, 1f)]
    private float _femaleFootStepVolumeScale;
    [SerializeField]
    [FoldoutGroup("足音関係/女性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE ID", ValueLabel = "SE Info")]
    private Dictionary<int, SoundPack.Data2D> _femaleFootStepSEData;
    [SerializeField]
    [FoldoutGroup("足音関係/女性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _femaleFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("足音関係/女性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _femaleBareFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("足音関係/女性", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE Type", ValueLabel = "SE ID List")]
    private Dictionary<AIProject.Definitions.Map.FootStepSE, List<int>> _femaleRainFootStepSEMeshTable;
    [SerializeField]
    [FoldoutGroup("環境音関係", 0)]
    [DictionaryDrawerSettings(KeyLabel = "SE ID", ValueLabel = "Info")]
    private Dictionary<int, List<SoundPack.Data2D>> _env3DSEData;
    [SerializeField]
    [FoldoutGroup("環境音関係/広範囲系", 0)]
    [DictionaryDrawerSettings(KeyLabel = "Area Type", ValueLabel = "ID Info")]
    private Dictionary<SoundPack.PlayAreaType, SoundPack.WideRangeEnviroInfo> _wideRangeEnvTable;
    [SerializeField]
    [FoldoutGroup("環境音関係/広範囲系", 0)]
    [DictionaryDrawerSettings(KeyLabel = "Map ID", ValueLabel = "Area ID")]
    private Dictionary<int, int[]> _muteAreaTable;

    public SoundPack()
    {
      base.\u002Ector();
    }

    public SoundPack.SoundSystemInfoGroup SoundSystemInfo
    {
      get
      {
        return this._soundSystemInfo;
      }
    }

    public SoundPack.BGMInfoGroup BGMInfo
    {
      get
      {
        return this._bgmInfo;
      }
    }

    public SoundPack.Game3DSEInfoGroup Game3DInfo
    {
      get
      {
        return this._game3DSEInfo;
      }
    }

    public SoundPack.EnviroSEInfoGroup EnviroInfo
    {
      get
      {
        return this._enviroInfo;
      }
    }

    public SoundPack.FootStepInfoGroup FootStepInfo
    {
      get
      {
        return this._footStepInfo;
      }
    }

    public Dictionary<DoorMatType, SoundPack.DoorSEIDInfo> DoorIDTable
    {
      get
      {
        return this._doorIDTable;
      }
    }

    public float GetFootStepSEVolumeScale(byte sex)
    {
      if (sex == (byte) 0)
        return this._maleFootStepVolumeScale;
      return sex == (byte) 1 ? this._femaleFootStepVolumeScale : 1f;
    }

    public void Play(SoundPack.SystemSE se)
    {
      int key;
      SoundPack.Data2D data2D;
      if (!this._systemSETable.TryGetValue(se, out key) || !this._systemSEDataTable.TryGetValue(key, out data2D) || !data2D.IsActive)
        return;
      this.Play((SoundPack.IData) data2D);
    }

    public AudioSource Play(int key, Manager.Sound.Type soundType, float fadeTime = 0.0f)
    {
      List<SoundPack.Data3D> source;
      if (!this._actionSEDataTable.TryGetValue(key, out source))
        return (AudioSource) null;
      return source.IsNullOrEmpty<SoundPack.Data3D>() ? (AudioSource) null : this.Play((SoundPack.IData) source[Random.Range(0, source.Count)], soundType, fadeTime);
    }

    public AudioSource PlayFootStep(
      byte sex,
      bool bareFoot,
      AIProject.Definitions.Map.FootStepSE seType,
      Weather weather,
      SoundPack.PlayAreaType areaType)
    {
      if (sex != (byte) 0 && sex != (byte) 1)
        return (AudioSource) null;
      List<int> source = (List<int>) null;
      if (areaType == SoundPack.PlayAreaType.Normal && source.IsNullOrEmpty<int>() && (weather == Weather.Rain || weather == Weather.Storm))
      {
        if (!(sex != (byte) 0 ? this._femaleRainFootStepSEMeshTable : this._maleRainFootStepSEMeshTable).TryGetValue(seType, out source))
          source = (List<int>) null;
        else if (source.IsNullOrEmpty<int>())
          return (AudioSource) null;
      }
      if (bareFoot && source.IsNullOrEmpty<int>())
      {
        if (!(sex != (byte) 0 ? this._femaleBareFootStepSEMeshTable : this._maleBareFootStepSEMeshTable).TryGetValue(seType, out source))
          source = (List<int>) null;
        else if (source.IsNullOrEmpty<int>())
          return (AudioSource) null;
      }
      if (source.IsNullOrEmpty<int>())
      {
        if (!(sex != (byte) 0 ? this._femaleFootStepSEMeshTable : this._maleFootStepSEMeshTable).TryGetValue(seType, out source))
          return (AudioSource) null;
        if (source.IsNullOrEmpty<int>())
          return (AudioSource) null;
      }
      if (source.IsNullOrEmpty<int>())
        return (AudioSource) null;
      int key = source[Random.Range(0, source.Count)];
      SoundPack.Data2D data2D;
      return !(sex != (byte) 0 ? this._femaleFootStepSEData : this._maleFootStepSEData).TryGetValue(key, out data2D) ? (AudioSource) null : this.Play((SoundPack.IData) data2D, Manager.Sound.Type.GameSE3D, 0.0f);
    }

    public AudioSource PlayFootStep(
      byte sex,
      bool bareFoot,
      string tag,
      Weather weather,
      SoundPack.PlayAreaType areaType)
    {
      if (tag == "Untagged")
        return (AudioSource) null;
      if (sex != (byte) 0 && sex != (byte) 1)
        return (AudioSource) null;
      AIProject.Definitions.Map.FootStepSE seType;
      return !this._footStepSETagTable.TryGetValue(tag ?? string.Empty, out seType) ? (AudioSource) null : this.PlayFootStep(sex, bareFoot, seType, weather, areaType);
    }

    public AudioSource PlayFootStep(
      byte sex,
      bool bareFoot,
      int mapID,
      int areaID,
      Weather weather,
      SoundPack.PlayAreaType areaType)
    {
      if (!Singleton<Resources>.IsInstance())
        return (AudioSource) null;
      if (sex != (byte) 0 && sex != (byte) 1)
        return (AudioSource) null;
      int[] source = (int[]) null;
      Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>> dictionary1;
      Dictionary<int, ValueTuple<int[], int[], int[]>> dictionary2;
      ValueTuple<int[], int[], int[]> valueTuple;
      if (Singleton<Resources>.Instance.Sound.DefaultFootStepSETable.TryGetValue((int) sex, out dictionary1) && dictionary1.TryGetValue(mapID, out dictionary2) && dictionary2.TryGetValue(areaID, out valueTuple))
      {
        if (areaType == SoundPack.PlayAreaType.Normal && source.IsNullOrEmpty<int>() && (weather == Weather.Rain || weather == Weather.Storm))
          source = (int[]) valueTuple.Item3;
        if (bareFoot && source.IsNullOrEmpty<int>())
          source = (int[]) valueTuple.Item2;
        if (source.IsNullOrEmpty<int>())
          source = (int[]) valueTuple.Item1;
      }
      if (source.IsNullOrEmpty<int>())
        return (AudioSource) null;
      int key = source[Random.Range(0, source.Length)];
      SoundPack.Data2D data2D;
      return !(sex != (byte) 0 ? this._femaleFootStepSEData : this._maleFootStepSEData).TryGetValue(key, out data2D) ? (AudioSource) null : this.Play((SoundPack.IData) data2D, Manager.Sound.Type.GameSE3D, 0.0f);
    }

    public AudioClip LoadEnviroSEClip(int clipID, out int idx)
    {
      List<SoundPack.Data2D> source;
      if (!this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>())
      {
        idx = -1;
        return (AudioClip) null;
      }
      idx = Random.Range(0, source.Count);
      return this.Load((SoundPack.IData) source[idx]);
    }

    public AudioClip LoadEnviroSEClip(int clipID, int idx)
    {
      List<SoundPack.Data2D> source;
      return !this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>() || (idx < 0 || source.Count <= idx) ? (AudioClip) null : this.Load((SoundPack.IData) source[idx]);
    }

    public static int EnviroSECount { get; private set; } = 0;

    private static void AddCountUsedEnviroSEInfo(string assetBundle, string asset, AudioClip clip)
    {
      Dictionary<string, ValueTuple<int, AudioClip>> dictionary;
      if (!SoundPack._usedEnviroSEInfo.TryGetValue(assetBundle, out dictionary) || dictionary == null)
        SoundPack._usedEnviroSEInfo[assetBundle] = dictionary = new Dictionary<string, ValueTuple<int, AudioClip>>();
      ValueTuple<int, AudioClip> valueTuple;
      if (dictionary.TryGetValue(asset, out valueTuple) && Object.op_Inequality((Object) valueTuple.Item2, (Object) null))
      {
        valueTuple.Item1 = (__Null) (valueTuple.Item1 + 1);
        dictionary[asset] = valueTuple;
        ++SoundPack.EnviroSECount;
      }
      else
      {
        if (!Object.op_Inequality((Object) clip, (Object) null))
          return;
        dictionary[asset] = new ValueTuple<int, AudioClip>(1, clip);
        ++SoundPack.EnviroSECount;
      }
    }

    private static void RemoveUsedEnviroSEInfo(string assetBundle, string asset)
    {
      Dictionary<string, ValueTuple<int, AudioClip>> dictionary;
      ValueTuple<int, AudioClip> valueTuple;
      if (!SoundPack._usedEnviroSEInfo.TryGetValue(assetBundle, out dictionary) || !dictionary.TryGetValue(asset, out valueTuple))
        return;
      if (Object.op_Equality((Object) valueTuple.Item2, (Object) null))
      {
        dictionary.Remove(asset);
      }
      else
      {
        valueTuple.Item1 = (__Null) (valueTuple.Item1 - 1);
        --SoundPack.EnviroSECount;
        if (valueTuple.Item1 <= 0)
        {
          Resources.UnloadAsset((Object) valueTuple.Item2);
          dictionary.Remove(asset);
        }
        else
          dictionary[asset] = valueTuple;
      }
    }

    public AudioSource PlayEnviroSE(int clipID, float fadeTime = 0.0f)
    {
      List<SoundPack.Data2D> source;
      if (!this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>())
        return (AudioSource) null;
      int index = source.Count != 0 ? Random.Range(0, source.Count) : 0;
      SoundPack.Data2D data2D = source[index];
      AudioClip clip = this.Load((SoundPack.IData) data2D);
      if (Object.op_Equality((Object) clip, (Object) null))
        return (AudioSource) null;
      AudioSource audioSource = Illusion.Game.Utils.Sound.Play(Manager.Sound.Type.ENV, clip, fadeTime);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return (AudioSource) null;
      string bundle = data2D.AssetBundleName;
      string asset = data2D.AssetName;
      SoundPack.AddCountUsedEnviroSEInfo(bundle, asset, clip);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) audioSource), (System.Action<M0>) (_ => SoundPack.RemoveUsedEnviroSEInfo(bundle, asset)));
      audioSource.set_dopplerLevel(0.0f);
      audioSource.set_rolloffMode(this.EnviroInfo.RolloffMode);
      return audioSource;
    }

    public AudioSource PlayEnviroSE(int clipID, out int idx, float fadeTime = 0.0f)
    {
      List<SoundPack.Data2D> source;
      if (!this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>())
      {
        idx = -1;
        return (AudioSource) null;
      }
      idx = source.Count != 1 ? Random.Range(0, source.Count) : 0;
      SoundPack.Data2D data2D = source[idx];
      AudioClip clip = this.Load((SoundPack.IData) data2D);
      if (Object.op_Equality((Object) clip, (Object) null))
      {
        idx = -1;
        return (AudioSource) null;
      }
      AudioSource audioSource = Illusion.Game.Utils.Sound.Play(Manager.Sound.Type.ENV, clip, fadeTime);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return (AudioSource) null;
      string bundle = data2D.AssetBundleName;
      string asset = data2D.AssetName;
      SoundPack.AddCountUsedEnviroSEInfo(bundle, asset, clip);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) audioSource), (System.Action<M0>) (_ => SoundPack.RemoveUsedEnviroSEInfo(bundle, asset)));
      return audioSource;
    }

    public AudioSource PlayEnviroSE(int clipID, int idx, float fadeTime = 0.0f)
    {
      List<SoundPack.Data2D> source;
      if (!this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>() || (idx < 0 || source.Count <= idx))
        return (AudioSource) null;
      SoundPack.Data2D data2D = source[idx];
      AudioClip clip = this.Load((SoundPack.IData) data2D);
      if (Object.op_Equality((Object) clip, (Object) null))
        return (AudioSource) null;
      AudioSource audioSource = Illusion.Game.Utils.Sound.Play(Manager.Sound.Type.ENV, clip, fadeTime);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return (AudioSource) null;
      string bundle = data2D.AssetBundleName;
      string asset = data2D.AssetName;
      SoundPack.AddCountUsedEnviroSEInfo(bundle, asset, clip);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) audioSource), (System.Action<M0>) (_ => SoundPack.RemoveUsedEnviroSEInfo(bundle, asset)));
      return audioSource;
    }

    public int EnviroSEClipCount(int clipID)
    {
      List<SoundPack.Data2D> source;
      return !this._env3DSEData.TryGetValue(clipID, out source) || source.IsNullOrEmpty<SoundPack.Data2D>() ? 0 : source.Count;
    }

    private int[] GetWideEnviroID(SoundPack.WideRangeEnviroInfo envInfo, Weather weather)
    {
      switch (weather)
      {
        case Weather.Clear:
        case Weather.Cloud1:
        case Weather.Cloud2:
          return envInfo.ClearID;
        case Weather.Cloud3:
          return envInfo.LightCloudID;
        case Weather.Cloud4:
          return envInfo.HeavyCloudID;
        case Weather.Fog:
          return envInfo.FogID;
        case Weather.Rain:
          return envInfo.LightRainID;
        case Weather.Storm:
          return envInfo.HeavyRainID;
        default:
          return (int[]) null;
      }
    }

    public bool TryGetWideEnvIDList(
      SoundPack.PlayAreaType areaType,
      Weather weather,
      ref List<int> idList)
    {
      if (idList == null)
        return false;
      SoundPack.WideRangeEnviroInfo envInfo1;
      if (this._wideRangeEnvTable.TryGetValue(areaType, out envInfo1))
      {
        int[] wideEnviroId = this.GetWideEnviroID(envInfo1, weather);
        if (!wideEnviroId.IsNullOrEmpty<int>())
        {
          idList.AddRange((IEnumerable<int>) wideEnviroId);
          return true;
        }
      }
      SoundPack.WideRangeEnviroInfo envInfo2;
      if (this._wideRangeEnvTable.TryGetValue(SoundPack.PlayAreaType.Normal, out envInfo2))
      {
        int[] wideEnviroId = this.GetWideEnviroID(envInfo2, weather);
        if (!wideEnviroId.IsNullOrEmpty<int>())
        {
          idList.AddRange((IEnumerable<int>) wideEnviroId);
          return true;
        }
      }
      return false;
    }

    public bool WideEnvMuteArea(int mapID, int areaID)
    {
      int[] source;
      return !this._muteAreaTable.IsNullOrEmpty<int, int[]>() && this._muteAreaTable.TryGetValue(mapID, out source) && !source.IsNullOrEmpty<int>() && ((IEnumerable<int>) source).Contains<int>(areaID);
    }

    public static void UnloadAudioClipAll()
    {
      using (Dictionary<string, Dictionary<string, AudioClip>>.Enumerator enumerator1 = SoundPack._audioClipTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<string, Dictionary<string, AudioClip>> current1 = enumerator1.Current;
          if (!current1.Value.IsNullOrEmpty<string, AudioClip>())
          {
            using (Dictionary<string, AudioClip>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<string, AudioClip> current2 = enumerator2.Current;
                if (Object.op_Inequality((Object) current2.Value, (Object) null))
                  Resources.UnloadAsset((Object) current2.Value);
              }
            }
          }
        }
      }
      SoundPack._audioClipTable.Clear();
      SoundPack._usedEnviroSEInfo.Clear();
    }

    private void Play(SoundPack.IData data)
    {
      Illusion.Game.Utils.Sound.Get(Manager.Sound.Type.SystemSE, data.AssetBundleName, data.AssetName, (string) null)?.Play();
    }

    private AudioClip Load(SoundPack.IData data)
    {
      if (data.AssetBundleName.IsNullOrEmpty() || data.AssetName.IsNullOrEmpty())
      {
        if (Debug.get_isDebugBuild())
          Debug.LogWarning((object) string.Format("SoundPack:Load BundleName[{0}] AssetName[{1}] に未記入があります", (object) (data.AssetBundleName ?? string.Empty), (object) (data.AssetName ?? string.Empty)));
        return (AudioClip) null;
      }
      Dictionary<string, AudioClip> dictionary1;
      if (!SoundPack._audioClipTable.TryGetValue(data.AssetBundleName, out dictionary1))
      {
        Dictionary<string, AudioClip> dictionary2 = new Dictionary<string, AudioClip>();
        SoundPack._audioClipTable[data.AssetBundleName] = dictionary2;
        dictionary1 = dictionary2;
      }
      AudioClip audioClip1;
      if (!dictionary1.TryGetValue(data.AssetName, out audioClip1) || Object.op_Equality((Object) audioClip1, (Object) null))
      {
        AudioClip audioClip2 = CommonLib.LoadAsset<AudioClip>(data.AssetBundleName, data.AssetName, false, string.Empty);
        dictionary1[data.AssetName] = audioClip2;
        audioClip1 = audioClip2;
      }
      return audioClip1;
    }

    public bool TryGetActionSEData(int clipID, out SoundPack.Data3D data)
    {
      List<SoundPack.Data3D> source;
      if (this._actionSEDataTable.TryGetValue(clipID, out source) && !source.IsNullOrEmpty<SoundPack.Data3D>())
      {
        data = source[Random.Range(0, source.Count)];
        return true;
      }
      data = new SoundPack.Data3D();
      return false;
    }

    private int GetSqrDistanceSort(Transform camera, Transform t1, Transform t2)
    {
      return (int) ((double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t1.get_position(), camera.get_position())) - (double) Vector3.SqrMagnitude(Vector3.op_Subtraction(t2.get_position(), camera.get_position())));
    }

    public AudioSource Play(SoundPack.IData data, Manager.Sound.Type type, float fadeTime = 0.0f)
    {
      AudioClip clip = this.Load(data);
      if (Object.op_Equality((Object) clip, (Object) null))
        return (AudioSource) null;
      AudioSource audio = Illusion.Game.Utils.Sound.Play(type, clip, fadeTime);
      if (type == Manager.Sound.Type.GameSE3D && Object.op_Inequality((Object) audio, (Object) null))
      {
        this.PlayingAudioList.RemoveAll((Predicate<AudioSource>) (x => Object.op_Equality((Object) x, (Object) null) || Object.op_Equality((Object) ((Component) x).get_gameObject(), (Object) null)));
        if (this._soundSystemInfo.Game3DMaxCount <= this.PlayingAudioList.Count)
        {
          int num = this.PlayingAudioList.Count - this._soundSystemInfo.Game3DMaxCount + 1;
          Transform cameraT = ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl.CameraComponent).get_transform();
          List<AudioSource> audioSourceList = ListPool<AudioSource>.Get();
          audioSourceList.AddRange((IEnumerable<AudioSource>) this.PlayingAudioList);
          audioSourceList.Sort((Comparison<AudioSource>) ((a1, a2) => this.GetSqrDistanceSort(cameraT, ((Component) a2).get_transform(), ((Component) a1).get_transform())));
          for (int index = 0; index < num; ++index)
          {
            AudioSource element = audioSourceList.GetElement<AudioSource>(index);
            this.PlayingAudioList.Remove(element);
            Object.Destroy((Object) ((Component) element).get_gameObject());
          }
          ListPool<AudioSource>.Release(audioSourceList);
        }
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) audio), (System.Action<M0>) (_ =>
        {
          if (!Object.op_Inequality((Object) audio, (Object) null))
            return;
          this.PlayingAudioList.Remove(audio);
        }));
        this.PlayingAudioList.Add(audio);
      }
      if (data is SoundPack.Data3D data3D)
      {
        audio.set_minDistance(data3D.MinDistance);
        audio.set_maxDistance(data3D.MaxDistance);
      }
      return audio;
    }

    public interface IData
    {
      string Summary { get; }

      string AssetBundleName { get; }

      string AssetName { get; }

      bool IsActive { get; }
    }

    [Serializable]
    public struct Data2D : SoundPack.IData
    {
      [SerializeField]
      private string _summary;
      [SerializeField]
      private string _assetBundleName;
      [SerializeField]
      private string _assetName;

      public string Summary
      {
        get
        {
          return this._summary;
        }
      }

      public string AssetBundleName
      {
        get
        {
          return this._assetBundleName;
        }
      }

      public string AssetName
      {
        get
        {
          return this._assetName;
        }
      }

      public bool IsActive
      {
        get
        {
          return !this.AssetBundleName.IsNullOrEmpty() && !this.AssetName.IsNullOrEmpty();
        }
      }
    }

    [Serializable]
    public struct Data3D : SoundPack.IData
    {
      [SerializeField]
      private string _summary;
      [SerializeField]
      private string _assetBundleName;
      [SerializeField]
      private string _assetName;
      [SerializeField]
      [LabelText("減衰開始距離")]
      private float _minDistance;
      [SerializeField]
      [LabelText("減衰終了距離")]
      private float _maxDistance;

      public string Summary
      {
        get
        {
          return this._summary;
        }
      }

      public string AssetBundleName
      {
        get
        {
          return this._assetBundleName;
        }
      }

      public string AssetName
      {
        get
        {
          return this._assetName;
        }
      }

      public float MinDistance
      {
        get
        {
          return this._minDistance;
        }
      }

      public float MaxDistance
      {
        get
        {
          return this._maxDistance;
        }
      }

      public bool IsActive
      {
        get
        {
          return !this.AssetBundleName.IsNullOrEmpty() && !this.AssetName.IsNullOrEmpty();
        }
      }
    }

    [Serializable]
    public class SoundSystemInfoGroup
    {
      [SerializeField]
      [FoldoutGroup("同時最大再生数", 0)]
      [LabelText("環境音同時最大再生数")]
      [MinValue(1.0)]
      private int _enviroSEMaxCount = 20;
      [SerializeField]
      [FoldoutGroup("同時最大再生数", 0)]
      [LabelText("３ＤＳＥ同時最大再生数")]
      [MinValue(1.0)]
      private int _game3DSEMaxCount = 20;

      public int EnviroSEMaxCount
      {
        get
        {
          return this._enviroSEMaxCount;
        }
      }

      public int Game3DMaxCount
      {
        get
        {
          return this._game3DSEMaxCount;
        }
      }
    }

    [Serializable]
    public class BGMInfoGroup
    {
      [SerializeField]
      [LabelText("マップ切り替わり時のフェードの時間(秒)")]
      private float _mapBGMFadeTime = 4f;

      public float MapBGMFadeTime
      {
        get
        {
          return this._mapBGMFadeTime;
        }
      }
    }

    [Serializable]
    public class Game3DSEInfoGroup
    {
      [SerializeField]
      [LabelText("減衰終了距離＋α 再生距離")]
      [MinValue(0.0)]
      private float _marginMaxDistance = 10f;
      [SerializeField]
      [LabelText("停止時のフェード時間(秒)")]
      [MinValue(0.0)]
      private float _stopFadeTime = 0.5f;

      public float MarginMaxDistance
      {
        get
        {
          return this._marginMaxDistance;
        }
      }

      public float StopFadeTime
      {
        get
        {
          return this._stopFadeTime;
        }
      }
    }

    [Serializable]
    public class EnviroSEInfoGroup
    {
      [SerializeField]
      [LabelText("環境音切り替わり時のフェードの時間(秒)")]
      private float _fadeTime = 4f;
      [SerializeField]
      [LabelText("減衰値Max＋αでアクティブになる")]
      private float _enableDistance = 1f;
      [SerializeField]
      [LabelText("減衰値Max＋αで非アクティブになる")]
      private float _disableDistance = 10f;
      [SerializeField]
      private float _lineAreaSEBlendDistance = 2f;
      [SerializeField]
      [FoldoutGroup("広範囲系", 0)]
      [LabelText("エリア切り替わり時のフェードの時間(秒)")]
      private float _wideRangeQuickFadeTime = 0.5f;
      [SerializeField]
      [FoldoutGroup("広範囲系", 0)]
      [LabelText("天候切り替わり時のフェードの時間(秒)")]
      private float _wideRangeSlowFadeTime = 4f;
      [SerializeField]
      [LabelText("ロールオフモード")]
      private AudioRolloffMode _rolloffMode;

      public float FadeTime
      {
        get
        {
          return this._fadeTime;
        }
      }

      public float EnableDistance
      {
        get
        {
          return this._enableDistance;
        }
      }

      public float DisableDistance
      {
        get
        {
          return this._disableDistance;
        }
      }

      public AudioRolloffMode RolloffMode
      {
        get
        {
          return this._rolloffMode;
        }
      }

      public float LineAreaSEBlendDistance
      {
        get
        {
          return this._lineAreaSEBlendDistance;
        }
      }

      public float WideRangeQuickFadeTime
      {
        get
        {
          return this._wideRangeQuickFadeTime;
        }
      }

      public float WideRangeSlowFadeTime
      {
        get
        {
          return this._wideRangeSlowFadeTime;
        }
      }
    }

    [Serializable]
    public class FootStepInfoGroup
    {
      [SerializeField]
      [LabelText("減衰開始距離")]
      [Range(0.1f, 1000f)]
      private float _minDistance = 1f;
      [SerializeField]
      [LabelText("減衰終了距離")]
      [Range(0.1f, 1000f)]
      private float _maxDistance = 15f;
      [SerializeField]
      [LabelText("減衰終了距離＋α 再生距離")]
      [MinValue(0.0)]
      private float _marginMaxDistance = 10f;
      [SerializeField]
      [LabelText("ロールオフモード")]
      private AudioRolloffMode _rolloffMode;

      public float MinDistance
      {
        get
        {
          return this._minDistance;
        }
      }

      public float MaxDistance
      {
        get
        {
          return this._maxDistance;
        }
      }

      public float MarginMaxDistance
      {
        get
        {
          return this._marginMaxDistance;
        }
      }

      public AudioRolloffMode RolloffMode
      {
        get
        {
          return this._rolloffMode;
        }
      }

      public float PlayEnableDistance
      {
        get
        {
          return this._maxDistance + this._marginMaxDistance;
        }
      }
    }

    [Serializable]
    public struct DoorSEIDInfo
    {
      [SerializeField]
      private int _openID;
      [SerializeField]
      private int _closeID;

      public int OpenID
      {
        get
        {
          return this._openID;
        }
      }

      public int CloseID
      {
        get
        {
          return this._closeID;
        }
      }
    }

    public enum PlayAreaType
    {
      Normal,
      Indoor,
    }

    public enum EnvSEWeatherType
    {
      Clear,
      LightCloud,
      HeavyCloud,
      LightRain,
      HeavyRain,
      Fog,
    }

    public enum SystemSE
    {
      Select,
      OK_L,
      OK_S,
      Cancel,
      Error,
      Save,
      Load,
      Shop,
      Popup,
      Page,
      Craft,
      Skill,
      BootDevice,
      Fishing_Result,
      Photo,
      Call,
      LevelUP,
      BoxOpen,
      BoxClose,
      Warp_In,
      Warp_Out,
    }

    [Serializable]
    public struct WideRangeEnviroInfo
    {
      [SerializeField]
      [FormerlySerializedAs("ClearID")]
      [LabelText("晴れ")]
      private int[] _clearID;
      [SerializeField]
      [FormerlySerializedAs("LightCloudID")]
      [LabelText("曇り")]
      private int[] _lightCloudID;
      [SerializeField]
      [FormerlySerializedAs("HeavyCloudID")]
      [LabelText("曇天")]
      private int[] _heavyCloudID;
      [SerializeField]
      [FormerlySerializedAs("LightRainID")]
      [LabelText("小雨")]
      private int[] _lightRainID;
      [SerializeField]
      [FormerlySerializedAs("HeavyRainID")]
      [LabelText("大雨")]
      private int[] _heavyRainID;
      [SerializeField]
      [FormerlySerializedAs("FogID")]
      [LabelText("霧")]
      private int[] _fogID;

      public int[] ClearID
      {
        get
        {
          return this._clearID;
        }
      }

      public int[] LightCloudID
      {
        get
        {
          return this._lightCloudID;
        }
      }

      public int[] HeavyCloudID
      {
        get
        {
          return this._heavyCloudID;
        }
      }

      public int[] LightRainID
      {
        get
        {
          return this._lightRainID;
        }
      }

      public int[] HeavyRainID
      {
        get
        {
          return this._heavyRainID;
        }
      }

      public int[] FogID
      {
        get
        {
          return this._fogID;
        }
      }
    }
  }
}

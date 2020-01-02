// Decompiled with JetBrains decompiler
// Type: Manager.Sound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.CustomAttributes;
using Illusion.Extensions;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager
{
  public sealed class Sound : Singleton<Manager.Sound>
  {
    [SerializeField]
    [NotEditable]
    private List<AudioClip> useAudioClipList = new List<AudioClip>();
    [SerializeField]
    [NotEditable]
    private Transform _listener;
    [SerializeField]
    [NotEditable]
    private GameObject _currentBGM;
    [SerializeField]
    [NotEditable]
    private GameObject oldBGM;
    [SerializeField]
    [NotEditable]
    private Transform listener;
    [SerializeField]
    [NotEditable]
    private Transform rootSetting;
    [SerializeField]
    [NotEditable]
    private Transform rootPlay;
    [SerializeField]
    [NotEditable]
    private Transform ASCacheRoot;
    [SerializeField]
    [NotEditable]
    private Transform[] typeObjects;
    [SerializeField]
    [NotEditable]
    private GameObject[] settingObjects;
    private Dictionary<int, Dictionary<string, AudioSource>> dicASCache;

    public static AudioMixer Mixer { get; private set; }

    public static GameObject PlayFade(
      GameObject fadeOut,
      AudioSource audio,
      float fadeTime = 0.0f)
    {
      if (Object.op_Inequality((Object) fadeOut, (Object) null))
        ((FadePlayer) fadeOut.GetComponent<FadePlayer>()).SafeProc<FadePlayer>((Action<FadePlayer>) (p => p.Stop(fadeTime)));
      GameObject gameObject = ((Component) audio).get_gameObject();
      ((FadePlayer) gameObject.AddComponent<FadePlayer>()).Play(fadeTime);
      return gameObject;
    }

    public AudioListener AudioListener
    {
      get
      {
        return (AudioListener) ((Component) this.listener).GetComponent<AudioListener>();
      }
    }

    public Transform Listener
    {
      get
      {
        return this._listener;
      }
      set
      {
        this._listener = value;
      }
    }

    public GameObject currentBGM
    {
      get
      {
        return this._currentBGM;
      }
      set
      {
        if (Object.op_Inequality((Object) this.oldBGM, (Object) null))
          Object.Destroy((Object) this.oldBGM);
        this.oldBGM = this._currentBGM;
        this._currentBGM = value;
      }
    }

    public void Register(AudioClip clip)
    {
      this.useAudioClipList.Add(clip);
    }

    public void Remove(AudioClip clip)
    {
      if (this.useAudioClipList.Remove(clip) && ((IEnumerable<AudioClip>) this.useAudioClipList).Count<AudioClip>((Func<AudioClip, bool>) (p => Object.op_Equality((Object) p, (Object) clip))) != 0)
        return;
      Resources.UnloadAsset((Object) clip);
    }

    public List<SoundSettingData.Param> settingDataList { get; private set; }

    public List<Sound3DSettingData.Param> setting3DDataList { get; private set; }

    public Manager.Sound.OutputSettingData AudioSettingData(AudioSource audio, int settingNo)
    {
      if (settingNo < 0)
        return (Manager.Sound.OutputSettingData) null;
      SoundSettingData.Param audioSettingData = this.GetAudioSettingData(settingNo);
      if (audioSettingData == null)
        return (Manager.Sound.OutputSettingData) null;
      audio.set_volume(audioSettingData.Volume);
      audio.set_pitch(audioSettingData.Pitch);
      audio.set_panStereo(audioSettingData.Pan);
      audio.set_spatialBlend(audioSettingData.Level3D);
      audio.set_priority(audioSettingData.Priority);
      audio.set_playOnAwake(audioSettingData.PlayAwake);
      audio.set_loop(audioSettingData.Loop);
      this.AudioSettingData3DOnly(audio, audioSettingData);
      return new Manager.Sound.OutputSettingData()
      {
        delayTime = audioSettingData.DelayTime
      };
    }

    public SoundSettingData.Param GetAudioSettingData(int settingNo)
    {
      return settingNo < 0 ? (SoundSettingData.Param) null : this.settingDataList[settingNo];
    }

    public void AudioSettingData3DOnly(AudioSource audio, int settingNo)
    {
      this.AudioSettingData3DOnly(audio, this.GetAudioSettingData(settingNo));
    }

    private void AudioSettingData3DOnly(AudioSource audio, SoundSettingData.Param param)
    {
      if (param == null || param.Setting3DNo < 0)
        return;
      Sound3DSettingData.Param setting3Ddata = this.setting3DDataList[param.Setting3DNo];
      if (setting3Ddata == null)
        return;
      audio.set_dopplerLevel(setting3Ddata.DopplerLevel);
      audio.set_spread(setting3Ddata.Spread);
      audio.set_minDistance(setting3Ddata.MinDistance);
      audio.set_maxDistance(setting3Ddata.MaxDistance);
      audio.set_rolloffMode((AudioRolloffMode) setting3Ddata.AudioRolloffMode);
    }

    public AudioSource Create(Manager.Sound.Type type, bool isCache = false)
    {
      int index = (int) type;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.settingObjects[index], !isCache ? this.typeObjects[index] : this.ASCacheRoot, false);
      gameObject.SetActive(true);
      return (AudioSource) gameObject.GetComponent<AudioSource>();
    }

    public void SetParent(Manager.Sound.Type type, Transform t)
    {
      t.SetParent(this.typeObjects[(int) type], false);
    }

    public Transform SetParent(
      Transform parent,
      LoadAudioBase script,
      GameObject settingObject)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) settingObject, parent, false);
      gameObject.SetActive(true);
      script.Init((AudioSource) gameObject.GetComponent<AudioSource>());
      return gameObject.get_transform();
    }

    public void Bind(LoadSound script)
    {
      if (Object.op_Equality((Object) script.audioSource, (Object) null))
      {
        int type = (int) script.type;
        this.SetParent(this.typeObjects[type], (LoadAudioBase) script, this.settingObjects[type]);
      }
      AudioSource audioSource = script.audioSource;
      audioSource.set_clip(script.clip);
      this.Register(script.clip);
      ((Object) audioSource).set_name(((Object) script.clip).get_name());
      Manager.Sound.OutputSettingData outputSettingData = this.AudioSettingData(audioSource, script.settingNo);
      if (outputSettingData == null || (double) script.delayTime > 0.0)
        return;
      script.delayTime = outputSettingData.delayTime;
    }

    public List<AudioSource> GetPlayingList(Manager.Sound.Type type)
    {
      List<AudioSource> audioSourceList = new List<AudioSource>();
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
        audioSourceList.Add((AudioSource) ((Component) typeObject.GetChild(index)).GetComponent<AudioSource>());
      return audioSourceList;
    }

    public bool IsPlay(Manager.Sound.Type type, string playName = null)
    {
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
      {
        if (playName.IsNullOrEmpty() || !(playName != ((Object) typeObject.GetChild(index)).get_name()))
          return true;
      }
      return false;
    }

    public bool IsPlay(Transform trans)
    {
      for (int index = 0; index < this.typeObjects.Length; ++index)
      {
        if (this.IsPlay((Manager.Sound.Type) index, trans))
          return true;
      }
      return false;
    }

    public bool IsPlay(Manager.Sound.Type type, Transform trans)
    {
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
      {
        if (Object.op_Equality((Object) typeObject.GetChild(index), (Object) trans))
          return true;
      }
      return false;
    }

    public Transform FindAsset(Manager.Sound.Type type, string assetName, string assetBundleName = null)
    {
      if (this.typeObjects == null)
        return (Transform) null;
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
      {
        Transform child = typeObject.GetChild(index);
        LoadAudioBase componentInChildren = (LoadAudioBase) ((Component) child).GetComponentInChildren<LoadAudioBase>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null) && Object.op_Inequality((Object) componentInChildren.clip, (Object) null) && componentInChildren.assetName == assetName && ((assetBundleName == null || componentInChildren.assetBundleName == assetBundleName) && (type != Manager.Sound.Type.BGM || Object.op_Inequality((Object) ((Component) child).get_gameObject(), (Object) this.oldBGM))))
          return child;
      }
      return (Transform) null;
    }

    public Transform Play(
      Manager.Sound.Type type,
      string assetBundleName,
      string assetName,
      float delayTime = 0.0f,
      float fadeTime = 0.0f,
      bool isAssetEqualPlay = true,
      bool isAsync = true,
      int settingNo = -1,
      bool isBundleUnload = true)
    {
      int index = (int) type;
      LoadSound loadSound = (LoadSound) new GameObject("Sound Loading").AddComponent<LoadSound>();
      loadSound.assetBundleName = assetBundleName;
      loadSound.assetName = assetName;
      loadSound.type = type;
      loadSound.delayTime = delayTime;
      loadSound.fadeTime = fadeTime;
      loadSound.isAssetEqualPlay = isAssetEqualPlay;
      loadSound.isAsync = isAsync;
      loadSound.settingNo = settingNo;
      loadSound.isBundleUnload = isBundleUnload;
      return this.SetParent(this.typeObjects[index], (LoadAudioBase) loadSound, this.settingObjects[index]);
    }

    public void Stop(Manager.Sound.Type type)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
        gameObjectList.Add(((Component) typeObject.GetChild(index)).get_gameObject());
      gameObjectList.ForEach((Action<GameObject>) (p => Object.Destroy((Object) p)));
    }

    public void Stop(Manager.Sound.Type type, Transform trans)
    {
      Transform typeObject = this.typeObjects[(int) type];
      for (int index = 0; index < typeObject.get_childCount(); ++index)
      {
        Transform child = typeObject.GetChild(index);
        if (Object.op_Equality((Object) child, (Object) trans))
        {
          Object.Destroy((Object) ((Component) child).get_gameObject());
          break;
        }
      }
    }

    public void Stop(Transform trans)
    {
      for (int index1 = 0; index1 < this.typeObjects.Length; ++index1)
      {
        Transform typeObject = this.typeObjects[index1];
        for (int index2 = 0; index2 < typeObject.get_childCount(); ++index2)
        {
          Transform child = typeObject.GetChild(index2);
          if (Object.op_Equality((Object) child, (Object) trans))
          {
            if (index1 == 0)
            {
              this.Stop(Manager.Sound.Type.BGM);
              return;
            }
            Object.Destroy((Object) ((Component) child).get_gameObject());
            return;
          }
        }
      }
    }

    public void PlayBGM(float fadeTime = 0.0f)
    {
      if (!Object.op_Inequality((Object) this.currentBGM, (Object) null))
        return;
      ((FadePlayer) this.currentBGM.GetComponent<FadePlayer>()).SafeProc<FadePlayer>((Action<FadePlayer>) (p => p.Play(fadeTime)));
    }

    public void PauseBGM()
    {
      if (!Object.op_Inequality((Object) this.currentBGM, (Object) null))
        return;
      ((FadePlayer) this.currentBGM.GetComponent<FadePlayer>()).SafeProc<FadePlayer>((Action<FadePlayer>) (p => p.Pause()));
    }

    public void StopBGM(float fadeTime = 0.0f)
    {
      if (Object.op_Inequality((Object) this.currentBGM, (Object) null))
        ((FadePlayer) this.currentBGM.GetComponent<FadePlayer>()).SafeProc<FadePlayer>((Action<FadePlayer>) (p => p.Stop(fadeTime)));
      List<GameObject> list = ((IEnumerable<GameObject>) ((Component) this.typeObjects[0]).get_gameObject().Children()).Where<GameObject>((Func<GameObject, bool>) (item => Object.op_Inequality((Object) item, (Object) this.currentBGM))).ToList<GameObject>();
      // ISSUE: reference to a compiler-generated field
      if (Manager.Sound.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Manager.Sound.\u003C\u003Ef__mg\u0024cache0 = new Action<GameObject>(Object.Destroy);
      }
      // ISSUE: reference to a compiler-generated field
      Action<GameObject> fMgCache0 = Manager.Sound.\u003C\u003Ef__mg\u0024cache0;
      list.ForEach(fMgCache0);
    }

    public AudioSource Play(Manager.Sound.Type type, AudioClip clip, float fadeTime = 0.0f)
    {
      AudioSource audioSource = this.Create(type, false);
      audioSource.set_clip(clip);
      ((Component) audioSource).GetOrAddComponent<FadePlayer>().SafeProc<FadePlayer>((Action<FadePlayer>) (p => p.Play(fadeTime)));
      return audioSource;
    }

    public AudioSource CreateCache(Manager.Sound.Type type, AssetBundleData data)
    {
      return this.CreateCache(type, data.bundle, data.asset, (string) null);
    }

    public AudioSource CreateCache(Manager.Sound.Type type, AssetBundleManifestData data)
    {
      return this.CreateCache(type, data.bundle, data.asset, data.manifest);
    }

    public AudioSource CreateCache(
      Manager.Sound.Type type,
      string bundle,
      string asset,
      string manifest = null)
    {
      int key = (int) type;
      Dictionary<string, AudioSource> dictionary1;
      if (!this.dicASCache.TryGetValue(key, out dictionary1))
      {
        Dictionary<string, AudioSource> dictionary2 = new Dictionary<string, AudioSource>();
        this.dicASCache[key] = dictionary2;
        dictionary1 = dictionary2;
      }
      AudioSource audioSource;
      if (!dictionary1.TryGetValue(asset, out audioSource))
      {
        audioSource = this.Create(type, true);
        ((Object) audioSource).set_name(asset);
        audioSource.set_clip(new AssetBundleManifestData(bundle, asset, manifest).GetAsset<AudioClip>());
        this.Register(audioSource.get_clip());
        dictionary1.Add(asset, audioSource);
      }
      return audioSource;
    }

    public void ReleaseCache(Manager.Sound.Type type, string bundle, string asset, string manifest = null)
    {
      int key = (int) type;
      Dictionary<string, AudioSource> dictionary;
      if (!this.dicASCache.TryGetValue(key, out dictionary))
        return;
      AudioSource audioSource;
      if (dictionary.TryGetValue(asset, out audioSource))
      {
        this.Remove(audioSource.get_clip());
        Object.Destroy((Object) ((Component) audioSource).get_gameObject());
        dictionary.Remove(asset);
        AssetBundleManager.UnloadAssetBundle(bundle, false, manifest, false);
      }
      if (((IEnumerable<KeyValuePair<string, AudioSource>>) dictionary).Any<KeyValuePair<string, AudioSource>>())
        return;
      this.dicASCache.Remove(key);
    }

    private void LoadSettingData()
    {
      AssetBundleData assetBundleData1 = new AssetBundleData("sound/setting/soundsettingdata/00.unity3d", (string) null);
      this.settingDataList = new List<SoundSettingData.Param>(((IEnumerable<SoundSettingData>) assetBundleData1.GetAllAssets<SoundSettingData>()).SelectMany<SoundSettingData, SoundSettingData.Param>((Func<SoundSettingData, IEnumerable<SoundSettingData.Param>>) (p => (IEnumerable<SoundSettingData.Param>) p.param)));
      this.settingDataList.Sort((Comparison<SoundSettingData.Param>) ((a, b) => a.No.CompareTo(b.No)));
      assetBundleData1.UnloadBundle(true, false);
      AssetBundleData assetBundleData2 = new AssetBundleData("sound/setting/sound3dsettingdata/00.unity3d", (string) null);
      this.setting3DDataList = new List<Sound3DSettingData.Param>(((IEnumerable<Sound3DSettingData>) assetBundleData2.GetAllAssets<Sound3DSettingData>()).SelectMany<Sound3DSettingData, Sound3DSettingData.Param>((Func<Sound3DSettingData, IEnumerable<Sound3DSettingData.Param>>) (p => (IEnumerable<Sound3DSettingData.Param>) p.param)));
      this.setting3DDataList.Sort((Comparison<Sound3DSettingData.Param>) ((a, b) => a.No.CompareTo(b.No)));
      assetBundleData2.UnloadBundle(true, false);
    }

    private void LoadSetting(Manager.Sound.Type type, int settingNo = -1)
    {
      string self = type.ToString();
      AssetBundleData assetBundleData = new AssetBundleData("sound/setting/object/00.unity3d", self.ToLower());
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) assetBundleData.GetAsset<GameObject>(), this.rootSetting, false);
      ((Object) gameObject).set_name(self + "_Setting");
      this.AudioSettingData((AudioSource) gameObject.GetComponent<AudioSource>(), settingNo);
      if (self.CompareParts("gamese", true))
        ;
      this.settingObjects[(int) type] = gameObject;
      assetBundleData.UnloadBundle(true, false);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      Manager.Sound.Mixer = new AssetBundleData("sound/data/mixer/00.unity3d", "master").GetAsset<AudioMixer>();
      this.rootSetting = new GameObject("SettingObject").get_transform();
      this.rootSetting.SetParent(((Component) this).get_transform(), false);
      this.rootPlay = new GameObject("PlayObject").get_transform();
      this.rootPlay.SetParent(((Component) this).get_transform(), false);
      this.LoadSettingData();
      this.settingObjects = new GameObject[Illusion.Utils.Enum<Manager.Sound.Type>.Length];
      this.typeObjects = new Transform[this.settingObjects.Length];
      for (int index = 0; index < this.settingObjects.Length; ++index)
      {
        Manager.Sound.Type type = (Manager.Sound.Type) index;
        this.LoadSetting(type, -1);
        Transform transform = new GameObject(type.ToString()).get_transform();
        transform.SetParent(this.rootPlay, false);
        this.typeObjects[index] = transform;
      }
      this.dicASCache = new Dictionary<int, Dictionary<string, AudioSource>>();
      this.ASCacheRoot = new GameObject("AudioSourceCache").get_transform();
      this.ASCacheRoot.SetParent(this.rootPlay, false);
      this.listener = new GameObject("Listener", new System.Type[1]
      {
        typeof (AudioListener)
      }).get_transform();
      this.listener.SetParent(((Component) this).get_transform(), false);
      if (!Object.op_Inequality((Object) Camera.get_main(), (Object) null))
        return;
      this._listener = ((Component) Camera.get_main()).get_transform();
    }

    private void Update()
    {
      if (Object.op_Inequality((Object) this._listener, (Object) null))
        this.listener.SetPositionAndRotation(this._listener.get_position(), this._listener.get_rotation());
      else
        this.listener.SetPositionAndRotation(Vector3.get_zero(), Quaternion.get_identity());
    }

    public enum Type
    {
      BGM,
      ENV,
      SystemSE,
      GameSE2D,
      GameSE3D,
    }

    public class OutputSettingData
    {
      public float delayTime;
    }
  }
}

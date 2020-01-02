// Decompiled with JetBrains decompiler
// Type: Manager.Voice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ConfigScene;
using Illusion.CustomAttributes;
using Illusion.Elements.Xml;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager
{
  public sealed class Voice : Singleton<Voice>
  {
    private Dictionary<int, Transform> voiceDic = new Dictionary<int, Transform>();
    private Dictionary<int, VoiceInfo.Param> _voiceInfoDic;
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
    private GameObject[] settingObjects;
    private Dictionary<int, Dictionary<string, AudioSource>> dicASCache;
    private const string UserPath = "config";
    private const string FileName = "voice.xml";
    private const string RootName = "Voice";
    private const string ElementName = "Volume";
    private Control xmlCtrl;

    public static AudioMixer Mixer
    {
      get
      {
        return Sound.Mixer;
      }
    }

    public Dictionary<int, VoiceInfo.Param> voiceInfoDic
    {
      get
      {
        return ((object) this).GetCache<Dictionary<int, VoiceInfo.Param>>(ref this._voiceInfoDic, (Func<Dictionary<int, VoiceInfo.Param>>) (() => this.voiceInfoList.ToDictionary<VoiceInfo.Param, int, VoiceInfo.Param>((Func<VoiceInfo.Param, int>) (v => v.No), (Func<VoiceInfo.Param, VoiceInfo.Param>) (v => v))));
      }
    }

    public List<VoiceInfo.Param> voiceInfoList { get; private set; }

    public VoiceSystem _Config { get; private set; }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.rootSetting = new GameObject("SettingObjectPCM").get_transform();
      this.rootSetting.SetParent(((Component) this).get_transform(), false);
      this.rootPlay = new GameObject("PlayObjectPCM").get_transform();
      this.rootPlay.SetParent(((Component) this).get_transform(), false);
      this.settingObjects = new GameObject[Illusion.Utils.Enum<Voice.Type>.Length];
      for (int index = 0; index < this.settingObjects.Length; ++index)
        this.LoadSetting((Voice.Type) index, -1);
      this.dicASCache = new Dictionary<int, Dictionary<string, AudioSource>>();
      this.ASCacheRoot = new GameObject("AudioSourceCache").get_transform();
      this.ASCacheRoot.SetParent(this.rootPlay, false);
      string str1 = AssetBundleManager.BaseDownloadingURL + "sound/data/pcm/";
      List<VoiceInfo.Param> sortList = new List<VoiceInfo.Param>();
      HashSet<int> distinctCheck = new HashSet<int>();
      CommonLib.GetAssetBundleNameListFromPath("etcetra/list/config/", true).ForEach((Action<string>) (file =>
      {
        foreach (List<VoiceInfo.Param> source in ((IEnumerable<VoiceInfo>) AssetBundleManager.LoadAllAsset(file, typeof (VoiceInfo), (string) null).GetAllAssets<VoiceInfo>()).Select<VoiceInfo, List<VoiceInfo.Param>>((Func<VoiceInfo, List<VoiceInfo.Param>>) (p => p.param)))
        {
          foreach (VoiceInfo.Param obj in source.Where<VoiceInfo.Param>((Func<VoiceInfo.Param, bool>) (p => !distinctCheck.Add(p.No))))
          {
            VoiceInfo.Param p = obj;
            Debug.LogWarning((object) ("Remove Voice No:" + (object) p.No));
            sortList.Remove(sortList.FirstOrDefault<VoiceInfo.Param>((Func<VoiceInfo.Param, bool>) (l => l.No == p.No)));
          }
          sortList.AddRange((IEnumerable<VoiceInfo.Param>) source);
        }
        AssetBundleManager.UnloadAssetBundle(file, false, (string) null, false);
      }));
      sortList.Sort((Comparison<VoiceInfo.Param>) ((a, b) => a.Sort.CompareTo(b.Sort)));
      this.voiceInfoList = sortList;
      Dictionary<int, string> dic = new Dictionary<int, string>();
      this.voiceInfoList.ForEach((Action<VoiceInfo.Param>) (p =>
      {
        string str2 = string.Format("c{0}", (object) p.No.MinusThroughToString("00"));
        dic.Add(p.No, str2);
        Transform transform = new GameObject(str2).get_transform();
        transform.SetParent(this.rootPlay, false);
        this.voiceDic.Add(p.No, transform);
      }));
      this._Config = new VoiceSystem("Volume", dic);
      this.xmlCtrl = new Control("config", "voice.xml", nameof (Voice), new Illusion.Elements.Xml.Data[1]
      {
        (Illusion.Elements.Xml.Data) this._Config
      });
      this.Load();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      Object.Destroy((Object) ((Component) this.rootSetting).get_gameObject());
      Object.Destroy((Object) ((Component) this.rootPlay).get_gameObject());
      this.voiceDic.Clear();
    }

    private void LoadSetting(Voice.Type type, int settingNo = -1)
    {
      string str = type.ToString();
      AssetBundleData assetBundleData = new AssetBundleData("sound/setting/object/00.unity3d", str.ToLower());
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) assetBundleData.GetAsset<GameObject>(), this.rootSetting, false);
      ((Object) gameObject).set_name(str + "_Setting");
      Singleton<Sound>.Instance.AudioSettingData((AudioSource) gameObject.GetComponent<AudioSource>(), settingNo);
      this.settingObjects[(int) type] = gameObject;
      assetBundleData.UnloadBundle(true, false);
    }

    public void SetParent(int no, Transform t)
    {
      Transform transform;
      if (!this.voiceDic.TryGetValue(no, out transform))
        return;
      t.SetParent(transform, false);
    }

    public void Bind(LoadVoice script)
    {
      if (Object.op_Equality((Object) script.audioSource, (Object) null))
      {
        Transform parent;
        if (!this.voiceDic.TryGetValue(script.no, out parent))
          return;
        Singleton<Sound>.Instance.SetParent(parent, (LoadAudioBase) script, this.settingObjects[(int) script.type]);
      }
      AudioSource audioSource = script.audioSource;
      audioSource.set_clip(script.clip);
      Singleton<Sound>.Instance.Register(script.clip);
      ((Object) audioSource).set_name(((Object) audioSource.get_clip()).get_name());
      audioSource.set_volume(this.GetVolume(script.no));
      Sound.OutputSettingData outputSettingData = Singleton<Sound>.Instance.AudioSettingData(audioSource, script.settingNo);
      if (outputSettingData == null)
        return;
      script.delayTime = outputSettingData.delayTime;
    }

    public List<AudioSource> GetPlayingList(int no)
    {
      List<AudioSource> audioSourceList = new List<AudioSource>();
      Transform transform;
      if (!this.voiceDic.TryGetValue(no, out transform))
        return audioSourceList;
      for (int index = 0; index < transform.get_childCount(); ++index)
        audioSourceList.Add((AudioSource) ((Component) transform.GetChild(index)).GetComponent<AudioSource>());
      return audioSourceList;
    }

    public bool IsVoiceCheck(int no)
    {
      Transform transform;
      return this.voiceDic.TryGetValue(no, out transform) && transform.get_childCount() != 0;
    }

    public bool IsVoiceCheck(Transform voiceTrans, bool isLoopCheck = true)
    {
      using (Dictionary<int, Transform>.ValueCollection.Enumerator enumerator = this.voiceDic.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          for (int index = 0; index < current.get_childCount(); ++index)
          {
            LoadVoice componentInChildren = (LoadVoice) ((Component) current.GetChild(index)).GetComponentInChildren<LoadVoice>();
            if (Object.op_Equality((Object) componentInChildren.voiceTrans, (Object) voiceTrans) && (isLoopCheck || !componentInChildren.audioSource.get_loop()))
              return true;
          }
        }
      }
      return false;
    }

    public bool IsVoiceCheck(int no, Transform voiceTrans, bool isLoopCheck = true)
    {
      Transform transform;
      if (!this.voiceDic.TryGetValue(no, out transform))
        return false;
      for (int index = 0; index < transform.get_childCount(); ++index)
      {
        LoadVoice componentInChildren = (LoadVoice) ((Component) transform.GetChild(index)).GetComponentInChildren<LoadVoice>();
        if (Object.op_Equality((Object) componentInChildren.voiceTrans, (Object) voiceTrans) && (isLoopCheck || !componentInChildren.audioSource.get_loop()))
          return true;
      }
      return false;
    }

    public bool IsVoiceCheck()
    {
      return ((IEnumerable<Transform>) this.voiceDic.Values).Any<Transform>((Func<Transform, bool>) (v => v.get_childCount() != 0));
    }

    public Transform Play(
      int no,
      string assetBundleName,
      string assetName,
      float pitch = 1f,
      float delayTime = 0.0f,
      float fadeTime = 0.0f,
      bool isAsync = true,
      Transform voiceTrans = null,
      Voice.Type type = Voice.Type.PCM,
      int settingNo = -1,
      bool isPlayEndDelete = true,
      bool isBundleUnload = true,
      bool is2D = false)
    {
      LoadVoice loadVoice = (LoadVoice) new GameObject("Voice Loading").AddComponent<LoadVoice>();
      loadVoice.no = no;
      loadVoice.assetBundleName = assetBundleName;
      loadVoice.assetName = assetName;
      loadVoice.pitch = pitch;
      loadVoice.delayTime = delayTime;
      loadVoice.fadeTime = fadeTime;
      loadVoice.isAsync = isAsync;
      loadVoice.voiceTrans = voiceTrans;
      loadVoice.type = type;
      loadVoice.settingNo = settingNo;
      loadVoice.isPlayEndDelete = isPlayEndDelete;
      loadVoice.isBundleUnload = isBundleUnload;
      loadVoice.is2D = is2D;
      Transform parent;
      return !this.voiceDic.TryGetValue(no, out parent) ? (Transform) null : Singleton<Sound>.Instance.SetParent(parent, (LoadAudioBase) loadVoice, this.settingObjects[(int) type]);
    }

    public Transform OnecePlay(
      int no,
      string assetBundleName,
      string assetName,
      float pitch = 1f,
      float delayTime = 0.0f,
      float fadeTime = 0.0f,
      bool isAsync = true,
      Transform voiceTrans = null,
      Voice.Type type = Voice.Type.PCM,
      int settingNo = -1,
      bool isPlayEndDelete = true,
      bool isBundleUnload = true,
      bool is2D = false)
    {
      this.StopAll(true);
      return this.Play(no, assetBundleName, assetName, pitch, delayTime, fadeTime, isAsync, voiceTrans, type, settingNo, isPlayEndDelete, isBundleUnload, is2D);
    }

    public Transform OnecePlayChara(
      int no,
      string assetBundleName,
      string assetName,
      float pitch = 1f,
      float delayTime = 0.0f,
      float fadeTime = 0.0f,
      bool isAsync = true,
      Transform voiceTrans = null,
      Voice.Type type = Voice.Type.PCM,
      int settingNo = -1,
      bool isPlayEndDelete = true,
      bool isBundleUnload = true,
      bool is2D = false)
    {
      if (Object.op_Inequality((Object) voiceTrans, (Object) null))
        this.Stop(no, voiceTrans);
      else
        this.Stop(no);
      return this.Play(no, assetBundleName, assetName, pitch, delayTime, fadeTime, isAsync, voiceTrans, type, settingNo, isPlayEndDelete, isBundleUnload, is2D);
    }

    public void StopAll(bool isLoopStop = true)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      using (Dictionary<int, Transform>.ValueCollection.Enumerator enumerator = this.voiceDic.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          for (int index = 0; index < current.get_childCount(); ++index)
          {
            Transform child = current.GetChild(index);
            if (!isLoopStop)
            {
              AudioSource componentInChildren = (AudioSource) ((Component) child).GetComponentInChildren<AudioSource>();
              if (Object.op_Inequality((Object) componentInChildren, (Object) null) && componentInChildren.get_loop())
                continue;
            }
            gameObjectList.Add(((Component) child).get_gameObject());
          }
        }
      }
      gameObjectList.ForEach((Action<GameObject>) (p => Object.Destroy((Object) p)));
    }

    public void Stop(int no)
    {
      Transform transform;
      if (!this.voiceDic.TryGetValue(no, out transform))
        return;
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < transform.get_childCount(); ++index)
        gameObjectList.Add(((Component) transform.GetChild(index)).get_gameObject());
      gameObjectList.ForEach((Action<GameObject>) (p => Object.Destroy((Object) p)));
    }

    public void Stop(Transform voiceTrans)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      using (Dictionary<int, Transform>.ValueCollection.Enumerator enumerator = this.voiceDic.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Transform current = enumerator.Current;
          for (int index = 0; index < current.get_childCount(); ++index)
          {
            Transform child = current.GetChild(index);
            if (Object.op_Equality((Object) ((LoadVoice) ((Component) child).GetComponentInChildren<LoadVoice>()).voiceTrans, (Object) voiceTrans))
              gameObjectList.Add(((Component) child).get_gameObject());
          }
        }
      }
      gameObjectList.ForEach((Action<GameObject>) (p => Object.Destroy((Object) p)));
    }

    public void Stop(int no, Transform voiceTrans)
    {
      Transform transform;
      if (!this.voiceDic.TryGetValue(no, out transform))
        return;
      List<GameObject> gameObjectList1 = new List<GameObject>();
      for (int index = 0; index < transform.get_childCount(); ++index)
      {
        Transform child = transform.GetChild(index);
        if (Object.op_Equality((Object) ((LoadVoice) ((Component) child).GetComponentInChildren<LoadVoice>()).voiceTrans, (Object) voiceTrans))
          gameObjectList1.Add(((Component) child).get_gameObject());
      }
      List<GameObject> gameObjectList2 = gameObjectList1;
      // ISSUE: reference to a compiler-generated field
      if (Voice.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Voice.\u003C\u003Ef__mg\u0024cache0 = new Action<GameObject>(Object.Destroy);
      }
      // ISSUE: reference to a compiler-generated field
      Action<GameObject> fMgCache0 = Voice.\u003C\u003Ef__mg\u0024cache0;
      gameObjectList2.ForEach(fMgCache0);
    }

    public float GetVolume(int charaNo)
    {
      VoiceSystem.Voice voice;
      if (this._Config.chara.TryGetValue(charaNo, out voice))
        return voice.sound.GetVolume();
      Debug.LogError((object) ("charaNo:" + (object) charaNo + "VolumeNone"));
      return 0.0f;
    }

    public AudioSource CreateCache(int voiceNo, AssetBundleData data)
    {
      return this.CreateCache(voiceNo, data.bundle, data.asset, (string) null);
    }

    public AudioSource CreateCache(int voiceNo, AssetBundleManifestData data)
    {
      return this.CreateCache(voiceNo, data.bundle, data.asset, data.manifest);
    }

    public AudioSource CreateCache(
      int voiceNo,
      string bundle,
      string asset,
      string manifest = null)
    {
      Dictionary<string, AudioSource> dictionary1;
      if (!this.dicASCache.TryGetValue(voiceNo, out dictionary1))
      {
        Dictionary<string, AudioSource> dictionary2 = new Dictionary<string, AudioSource>();
        this.dicASCache[voiceNo] = dictionary2;
        dictionary1 = dictionary2;
      }
      AudioSource component;
      if (!dictionary1.TryGetValue(asset, out component))
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.settingObjects[0], this.ASCacheRoot, false);
        ((Object) gameObject).set_name(asset);
        gameObject.SetActive(true);
        component = (AudioSource) gameObject.GetComponent<AudioSource>();
        component.set_clip(AssetBundleManager.LoadAsset(bundle, asset, typeof (AudioClip), manifest).GetAsset<AudioClip>());
        Singleton<Sound>.Instance.Register(component.get_clip());
        dictionary1.Add(asset, component);
      }
      return component;
    }

    public void ReleaseCache(int voiceNo, string bundle, string asset, string manifest = null)
    {
      Dictionary<string, AudioSource> dictionary;
      if (!this.dicASCache.TryGetValue(voiceNo, out dictionary))
        return;
      AudioSource audioSource;
      if (dictionary.TryGetValue(asset, out audioSource))
      {
        Singleton<Sound>.Instance.Remove(audioSource.get_clip());
        Object.Destroy((Object) ((Component) audioSource).get_gameObject());
        dictionary.Remove(asset);
        AssetBundleManager.UnloadAssetBundle(bundle, false, manifest, false);
      }
      if (((IEnumerable<KeyValuePair<string, AudioSource>>) dictionary).Any<KeyValuePair<string, AudioSource>>())
        return;
      this.dicASCache.Remove(voiceNo);
    }

    public void Reset()
    {
      if (this.xmlCtrl == null)
        return;
      this.xmlCtrl.Init();
    }

    public void Load()
    {
      this.xmlCtrl.Read();
    }

    public void Save()
    {
      this.xmlCtrl.Write();
    }

    public enum Type
    {
      PCM,
    }
  }
}

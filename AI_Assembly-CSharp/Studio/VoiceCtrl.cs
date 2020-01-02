// Decompiled with JetBrains decompiler
// Type: Studio.VoiceCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class VoiceCtrl
  {
    public List<VoiceCtrl.VoiceInfo> list = new List<VoiceCtrl.VoiceInfo>();
    public int index = -1;
    public const string savePath = "studio/voicelist";
    public const string saveExtension = ".dat";
    public const string saveIdentifyingCode = "【voice】";
    public VoiceCtrl.Repeat repeat;
    private Transform m_TransformHead;
    private VoiceEndChecker voiceEndChecker;

    public OCIChar ociChar { get; set; }

    public Transform transVoice { get; private set; }

    public bool isPlay
    {
      get
      {
        return Singleton<Manager.Voice>.IsInstance() && Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.personality, this.transHead, true);
      }
    }

    private int personality
    {
      get
      {
        return this.ociChar != null ? this.ociChar.charInfo.fileParam.personality : 0;
      }
    }

    private float pitch
    {
      get
      {
        return this.ociChar != null ? this.ociChar.charInfo.fileParam.voicePitch : 1f;
      }
    }

    private Transform transHead
    {
      get
      {
        if (Object.op_Equality((Object) this.m_TransformHead, (Object) null))
        {
          GameObject gameObject = this.ociChar == null ? (GameObject) null : this.ociChar.charInfo.GetReferenceInfo(ChaReference.RefObjKey.HeadParent);
          this.m_TransformHead = !Object.op_Inequality((Object) gameObject, (Object) null) ? (Transform) null : gameObject.get_transform();
        }
        return this.m_TransformHead;
      }
    }

    public bool Play(int _idx)
    {
      if (!Singleton<Info>.IsInstance() || this.list.Count == 0)
        return false;
      if (!MathfEx.RangeEqualOn<int>(0, _idx, this.list.Count - 1))
      {
        this.index = -1;
        return false;
      }
      this.Stop();
      VoiceCtrl.VoiceInfo voiceInfo = this.list[_idx];
      Info.LoadCommonInfo loadInfo = this.GetLoadInfo(voiceInfo.group, voiceInfo.category, voiceInfo.no);
      if (loadInfo == null)
        return false;
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int personality = this.personality;
      string bundlePath = loadInfo.bundlePath;
      string fileName = loadInfo.fileName;
      float pitch = this.pitch;
      Transform transHead = this.transHead;
      int no = personality;
      string assetBundleName = bundlePath;
      string assetName = fileName;
      double num = (double) pitch;
      Transform voiceTrans = transHead;
      this.transVoice = instance.Play(no, assetBundleName, assetName, (float) num, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
      if (Object.op_Equality((Object) this.transVoice, (Object) null))
        return false;
      this.index = _idx;
      this.voiceEndChecker = (VoiceEndChecker) ((Component) this.transVoice).get_gameObject().AddComponent<VoiceEndChecker>();
      this.voiceEndChecker.onEndFunc += new VoiceEndChecker.OnEndFunc(this.NextVoicePlay);
      this.ociChar.SetVoice();
      return true;
    }

    public void Stop()
    {
      if (Object.op_Inequality((Object) this.voiceEndChecker, (Object) null))
        this.voiceEndChecker.onEndFunc = (VoiceEndChecker.OnEndFunc) null;
      if (Object.op_Inequality((Object) this.transVoice, (Object) null))
        Singleton<Manager.Voice>.Instance.Stop(this.personality, this.transHead);
      this.transVoice = (Transform) null;
      this.ociChar.SetVoice();
    }

    public void Save(BinaryWriter _writer, Version _version)
    {
      int count = this.list.Count;
      _writer.Write(count);
      for (int index = 0; index < count; ++index)
      {
        VoiceCtrl.VoiceInfo voiceInfo = this.list[index];
        _writer.Write(voiceInfo.group);
        _writer.Write(voiceInfo.category);
        _writer.Write(voiceInfo.no);
      }
      _writer.Write((int) this.repeat);
    }

    public void Load(BinaryReader _reader, Version _version)
    {
      int num = _reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        int _group = _reader.ReadInt32();
        int _category = _reader.ReadInt32();
        int _no = _reader.ReadInt32();
        if (this.GetLoadInfo(_group, _category, _no) != null)
          this.list.Add(new VoiceCtrl.VoiceInfo(_group, _category, _no));
      }
      this.repeat = (VoiceCtrl.Repeat) _reader.ReadInt32();
    }

    public void SaveList(string _name)
    {
      using (FileStream fileStream = new FileStream(UserData.Create("studio/voicelist") + Utility.GetCurrentTime() + ".dat", FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          binaryWriter.Write("【voice】");
          binaryWriter.Write(_name);
          int count = this.list.Count;
          binaryWriter.Write(count);
          for (int index = 0; index < count; ++index)
          {
            VoiceCtrl.VoiceInfo voiceInfo = this.list[index];
            binaryWriter.Write(voiceInfo.group);
            binaryWriter.Write(voiceInfo.category);
            binaryWriter.Write(voiceInfo.no);
          }
        }
      }
    }

    public bool LoadList(string _path, bool _import = false)
    {
      if (!_import)
        this.list.Clear();
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          if (string.Compare(binaryReader.ReadString(), "【voice】") != 0)
            return false;
          binaryReader.ReadString();
          int num = binaryReader.ReadInt32();
          for (int index = 0; index < num; ++index)
          {
            int _group = binaryReader.ReadInt32();
            int _category = binaryReader.ReadInt32();
            int _no = binaryReader.ReadInt32();
            if (this.GetLoadInfo(_group, _category, _no) != null)
              this.list.Add(new VoiceCtrl.VoiceInfo(_group, _category, _no));
          }
        }
      }
      return true;
    }

    public static string LoadListName(string _path)
    {
      string empty = string.Empty;
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
          return string.Compare(binaryReader.ReadString(), "【voice】") != 0 ? string.Empty : binaryReader.ReadString();
      }
    }

    public static bool CheckIdentifyingCode(string _path)
    {
      bool flag = true;
      using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          if (string.Compare(binaryReader.ReadString(), "【voice】") != 0)
            flag = false;
        }
      }
      return flag;
    }

    private Info.LoadCommonInfo GetLoadInfo(int _group, int _category, int _no)
    {
      Dictionary<int, Dictionary<int, Info.LoadCommonInfo>> dictionary1 = (Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>) null;
      if (!Singleton<Info>.Instance.dicVoiceLoadInfo.TryGetValue(_group, out dictionary1))
        return (Info.LoadCommonInfo) null;
      Dictionary<int, Info.LoadCommonInfo> dictionary2 = (Dictionary<int, Info.LoadCommonInfo>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return (Info.LoadCommonInfo) null;
      Info.LoadCommonInfo loadCommonInfo = (Info.LoadCommonInfo) null;
      return !dictionary2.TryGetValue(_no, out loadCommonInfo) ? (Info.LoadCommonInfo) null : loadCommonInfo;
    }

    private void NextVoicePlay()
    {
      this.transVoice = (Transform) null;
      switch (this.repeat)
      {
        case VoiceCtrl.Repeat.None:
          ++this.index;
          this.Play(this.index);
          break;
        case VoiceCtrl.Repeat.All:
          if (this.list.Count == 0)
            break;
          this.index = (this.index + 1) % this.list.Count;
          this.Play(this.index);
          break;
        case VoiceCtrl.Repeat.Select:
          this.Play(this.index);
          break;
      }
    }

    public class VoiceInfo
    {
      public VoiceInfo(int _group, int _category, int _no)
      {
        this.group = _group;
        this.category = _category;
        this.no = _no;
      }

      public int group { get; private set; }

      public int category { get; private set; }

      public int no { get; private set; }
    }

    public enum Repeat
    {
      None,
      All,
      Select,
    }
  }
}

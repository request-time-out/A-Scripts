// Decompiled with JetBrains decompiler
// Type: Studio.OICharInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class OICharInfo : ObjectInfo
  {
    public OICharInfo.AnimeInfo animeInfo = new OICharInfo.AnimeInfo();
    public int[] handPtn = new int[2];
    public bool lipSync = true;
    public bool[] activeIK = new bool[5]
    {
      true,
      true,
      true,
      true,
      true
    };
    public bool[] activeFK = new bool[7]
    {
      false,
      true,
      false,
      true,
      false,
      false,
      false
    };
    public bool[] expression = new bool[8];
    public float animeSpeed = 1f;
    public bool animeOptionVisible = true;
    public VoiceCtrl voiceCtrl = new VoiceCtrl();
    public float sonLength = 1f;
    public float[] animeOptionParam = new float[2];
    public byte[] siru = new byte[5];
    public OICharInfo.KinematicMode kinematicMode;
    public float mouthOpen;
    public bool enableIK;
    public bool enableFK;
    public float animePattern;
    public bool isAnimeForceLoop;
    public bool visibleSon;
    public float nipple;
    public bool visibleSimple;
    public Color simpleColor;
    public byte[] neckByteData;
    public byte[] eyesByteData;
    public float animeNormalizedTime;
    public Dictionary<int, TreeNodeObject.TreeState> dicAccessGroup;
    public Dictionary<int, TreeNodeObject.TreeState> dicAccessNo;

    public OICharInfo(ChaFileControl _charFile, int _key)
      : base(_key)
    {
      this.sex = _charFile == null ? 0 : (int) _charFile.parameter.sex;
      this.charFile = _charFile;
      this.bones = new Dictionary<int, OIBoneInfo>();
      this.ikTarget = new Dictionary<int, OIIKTargetInfo>();
      this.child = new Dictionary<int, List<ObjectInfo>>();
      foreach (int index in Singleton<Info>.Instance.AccessoryPointsIndex)
        this.child[index] = new List<ObjectInfo>();
      this.simpleColor = Color.get_blue();
      this.dicAccessGroup = new Dictionary<int, TreeNodeObject.TreeState>();
      this.dicAccessNo = new Dictionary<int, TreeNodeObject.TreeState>();
    }

    public override int kind
    {
      get
      {
        return 0;
      }
    }

    public int sex { get; private set; }

    public ChaFileControl charFile { get; private set; }

    public Dictionary<int, OIBoneInfo> bones { get; private set; }

    public Dictionary<int, OIIKTargetInfo> ikTarget { get; private set; }

    public Dictionary<int, List<ObjectInfo>> child { get; private set; }

    public LookAtTargetInfo lookAtTarget { get; set; }

    public float WetRate
    {
      get
      {
        return this.charFile.status.wetRate;
      }
    }

    public float SkinTuyaRate
    {
      get
      {
        return this.charFile.status.skinTuyaRate;
      }
    }

    public override void Save(BinaryWriter _writer, Version _version)
    {
      base.Save(_writer, _version);
      _writer.Write(this.sex);
      this.charFile.SaveCharaFile(_writer, false);
      int count1 = this.bones.Count;
      _writer.Write(count1);
      foreach (KeyValuePair<int, OIBoneInfo> bone in this.bones)
      {
        int key = bone.Key;
        _writer.Write(key);
        bone.Value.Save(_writer, _version);
      }
      int count2 = this.ikTarget.Count;
      _writer.Write(count2);
      foreach (KeyValuePair<int, OIIKTargetInfo> keyValuePair in this.ikTarget)
      {
        int key = keyValuePair.Key;
        _writer.Write(key);
        keyValuePair.Value.Save(_writer, _version);
      }
      int count3 = this.child.Count;
      _writer.Write(count3);
      foreach (KeyValuePair<int, List<ObjectInfo>> keyValuePair in this.child)
      {
        int key = keyValuePair.Key;
        _writer.Write(key);
        int count4 = keyValuePair.Value.Count;
        _writer.Write(count4);
        for (int index = 0; index < count4; ++index)
          keyValuePair.Value[index].Save(_writer, _version);
      }
      _writer.Write((int) this.kinematicMode);
      _writer.Write(this.animeInfo.group);
      _writer.Write(this.animeInfo.category);
      _writer.Write(this.animeInfo.no);
      for (int index = 0; index < 2; ++index)
        _writer.Write(this.handPtn[index]);
      _writer.Write(this.nipple);
      _writer.Write(this.siru);
      _writer.Write(this.mouthOpen);
      _writer.Write(this.lipSync);
      this.lookAtTarget.Save(_writer, _version);
      _writer.Write(this.enableIK);
      for (int index = 0; index < 5; ++index)
        _writer.Write(this.activeIK[index]);
      _writer.Write(this.enableFK);
      for (int index = 0; index < 7; ++index)
        _writer.Write(this.activeFK[index]);
      for (int index = 0; index < 8; ++index)
        _writer.Write(this.expression[index]);
      _writer.Write(this.animeSpeed);
      _writer.Write(this.animePattern);
      _writer.Write(this.animeOptionVisible);
      _writer.Write(this.isAnimeForceLoop);
      this.voiceCtrl.Save(_writer, _version);
      _writer.Write(this.visibleSon);
      _writer.Write(this.sonLength);
      _writer.Write(this.visibleSimple);
      _writer.Write(JsonUtility.ToJson((object) this.simpleColor));
      _writer.Write(this.animeOptionParam[0]);
      _writer.Write(this.animeOptionParam[1]);
      _writer.Write(this.neckByteData.Length);
      _writer.Write(this.neckByteData);
      _writer.Write(this.eyesByteData.Length);
      _writer.Write(this.eyesByteData);
      _writer.Write(this.animeNormalizedTime);
      int num1 = this.dicAccessGroup == null ? 0 : this.dicAccessGroup.Count;
      _writer.Write(num1);
      if (num1 != 0)
      {
        foreach (KeyValuePair<int, TreeNodeObject.TreeState> keyValuePair in this.dicAccessGroup)
        {
          _writer.Write(keyValuePair.Key);
          _writer.Write((int) keyValuePair.Value);
        }
      }
      int num2 = this.dicAccessNo == null ? 0 : this.dicAccessNo.Count;
      _writer.Write(num2);
      if (num2 == 0)
        return;
      foreach (KeyValuePair<int, TreeNodeObject.TreeState> keyValuePair in this.dicAccessNo)
      {
        _writer.Write(keyValuePair.Key);
        _writer.Write((int) keyValuePair.Value);
      }
    }

    public override void Load(BinaryReader _reader, Version _version, bool _import, bool _tree = true)
    {
      base.Load(_reader, _version, _import, true);
      this.sex = _reader.ReadInt32();
      this.charFile = new ChaFileControl();
      this.charFile.LoadCharaFile(_reader, true, false);
      int num1 = _reader.ReadInt32();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int index2 = _reader.ReadInt32();
        this.bones[index2] = new OIBoneInfo(_import ? Studio.Studio.GetNewIndex() : -1);
        this.bones[index2].Load(_reader, _version, _import, true);
      }
      int num2 = _reader.ReadInt32();
      for (int index1 = 0; index1 < num2; ++index1)
      {
        int index2 = _reader.ReadInt32();
        this.ikTarget[index2] = new OIIKTargetInfo(_import ? Studio.Studio.GetNewIndex() : -1);
        this.ikTarget[index2].Load(_reader, _version, _import, true);
      }
      int num3 = _reader.ReadInt32();
      for (int index1 = 0; index1 < num3; ++index1)
      {
        int index2 = _reader.ReadInt32();
        ObjectInfoAssist.LoadChild(_reader, _version, this.child[index2], _import);
      }
      this.kinematicMode = (OICharInfo.KinematicMode) _reader.ReadInt32();
      this.animeInfo.group = _reader.ReadInt32();
      this.animeInfo.category = _reader.ReadInt32();
      this.animeInfo.no = _reader.ReadInt32();
      for (int index = 0; index < 2; ++index)
        this.handPtn[index] = _reader.ReadInt32();
      this.nipple = _reader.ReadSingle();
      this.siru = _reader.ReadBytes(5);
      this.mouthOpen = _reader.ReadSingle();
      this.lipSync = _reader.ReadBoolean();
      if (this.lookAtTarget == null)
        this.lookAtTarget = new LookAtTargetInfo(_import ? Studio.Studio.GetNewIndex() : -1);
      this.lookAtTarget.Load(_reader, _version, _import, true);
      this.enableIK = _reader.ReadBoolean();
      for (int index = 0; index < 5; ++index)
        this.activeIK[index] = _reader.ReadBoolean();
      this.enableFK = _reader.ReadBoolean();
      for (int index = 0; index < 7; ++index)
        this.activeFK[index] = _reader.ReadBoolean();
      for (int index = 0; index < 8; ++index)
        this.expression[index] = _reader.ReadBoolean();
      this.animeSpeed = _reader.ReadSingle();
      this.animePattern = _reader.ReadSingle();
      this.animeOptionVisible = _reader.ReadBoolean();
      this.isAnimeForceLoop = _reader.ReadBoolean();
      this.voiceCtrl.Load(_reader, _version);
      this.visibleSon = _reader.ReadBoolean();
      this.sonLength = _reader.ReadSingle();
      this.visibleSimple = _reader.ReadBoolean();
      this.simpleColor = (Color) JsonUtility.FromJson<Color>(_reader.ReadString());
      this.animeOptionParam[0] = _reader.ReadSingle();
      this.animeOptionParam[1] = _reader.ReadSingle();
      int count1 = _reader.ReadInt32();
      this.neckByteData = _reader.ReadBytes(count1);
      int count2 = _reader.ReadInt32();
      this.eyesByteData = _reader.ReadBytes(count2);
      this.animeNormalizedTime = _reader.ReadSingle();
      int num4 = _reader.ReadInt32();
      if (num4 != 0)
        this.dicAccessGroup = new Dictionary<int, TreeNodeObject.TreeState>();
      for (int index = 0; index < num4; ++index)
        this.dicAccessGroup[_reader.ReadInt32()] = (TreeNodeObject.TreeState) _reader.ReadInt32();
      int num5 = _reader.ReadInt32();
      if (num5 != 0)
        this.dicAccessNo = new Dictionary<int, TreeNodeObject.TreeState>();
      for (int index = 0; index < num5; ++index)
        this.dicAccessNo[_reader.ReadInt32()] = (TreeNodeObject.TreeState) _reader.ReadInt32();
    }

    public override void DeleteKey()
    {
      Studio.Studio.DeleteIndex(this.dicKey);
      foreach (KeyValuePair<int, OIBoneInfo> bone in this.bones)
        Studio.Studio.DeleteIndex(bone.Value.dicKey);
      foreach (KeyValuePair<int, OIIKTargetInfo> keyValuePair in this.ikTarget)
        Studio.Studio.DeleteIndex(keyValuePair.Value.dicKey);
      foreach (KeyValuePair<int, List<ObjectInfo>> keyValuePair in this.child)
      {
        int count = keyValuePair.Value.Count;
        for (int index = 0; index < count; ++index)
          keyValuePair.Value[index].DeleteKey();
      }
      Studio.Studio.DeleteIndex(this.lookAtTarget.dicKey);
    }

    public enum IKTargetEN
    {
      Body,
      LeftShoulder,
      LeftArmChain,
      LeftHand,
      RightShoulder,
      RightArmChain,
      RightHand,
      LeftThigh,
      LeftLegChain,
      LeftFoot,
      RightThigh,
      RightLegChain,
      RightFoot,
    }

    public enum KinematicMode
    {
      None,
      FK,
      IK,
    }

    public class AnimeInfo
    {
      public int group;
      public int category;
      public int no;

      public void Set(int _group, int _category, int _no)
      {
        this.group = _group;
        this.category = _category;
        this.no = _no;
      }

      public bool exist
      {
        get
        {
          return this.group != -1 & this.category != -1 & this.no != -1;
        }
      }

      public void Copy(OICharInfo.AnimeInfo _src)
      {
        this.group = _src.group;
        this.category = _src.category;
        this.no = _src.no;
      }
    }
  }
}

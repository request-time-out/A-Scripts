// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileParameter
  {
    [IgnoreMember]
    public static readonly string BlockName = "Parameter";

    public ChaFileParameter()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public byte sex { get; set; }

    public string fullname { get; set; }

    public int personality { get; set; }

    public byte birthMonth { get; set; }

    public byte birthDay { get; set; }

    [IgnoreMember]
    public string strBirthDay
    {
      get
      {
        return ChaFileDefine.GetBirthdayStr((int) this.birthMonth, (int) this.birthDay, "ja-JP");
      }
    }

    public float voiceRate { get; set; }

    [IgnoreMember]
    public float voicePitch
    {
      get
      {
        return Mathf.Lerp(0.94f, 1.06f, this.voiceRate);
      }
    }

    public HashSet<int> hsWish { get; set; }

    [IgnoreMember]
    public int wish01
    {
      get
      {
        return this.hsWish.Count == 0 ? -1 : this.hsWish.ToArray<int>()[0];
      }
    }

    [IgnoreMember]
    public int wish02
    {
      get
      {
        return 1 >= this.hsWish.Count ? -1 : this.hsWish.ToArray<int>()[1];
      }
    }

    [IgnoreMember]
    public int wish03
    {
      get
      {
        return 2 >= this.hsWish.Count ? -1 : this.hsWish.ToArray<int>()[2];
      }
    }

    public bool futanari { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileParameterVersion;
      this.sex = (byte) 0;
      this.fullname = string.Empty;
      this.personality = 0;
      this.birthMonth = (byte) 1;
      this.birthDay = (byte) 1;
      this.voiceRate = 0.5f;
      this.hsWish = new HashSet<int>();
      this.futanari = false;
    }

    public void Copy(ChaFileParameter src)
    {
      this.version = src.version;
      this.sex = src.sex;
      this.fullname = src.fullname;
      this.personality = src.personality;
      this.birthMonth = src.birthMonth;
      this.birthDay = src.birthDay;
      this.voiceRate = src.voiceRate;
      this.hsWish = new HashSet<int>((IEnumerable<int>) src.hsWish);
      this.futanari = src.futanari;
    }

    public void ComplementWithVersion()
    {
      if (this.version < new Version("0.0.1"))
        this.hsWish = new HashSet<int>();
      this.version = ChaFileDefine.ChaFileParameterVersion;
    }
  }
}

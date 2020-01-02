// Decompiled with JetBrains decompiler
// Type: AmplifyOcclusion.VersionInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AmplifyOcclusion
{
  [Serializable]
  public class VersionInfo
  {
    public const byte Major = 2;
    public const byte Minor = 0;
    public const byte Release = 5;
    public const byte Revision = 0;
    [SerializeField]
    private int m_major;
    [SerializeField]
    private int m_minor;
    [SerializeField]
    private int m_release;

    private VersionInfo()
    {
      this.m_major = 2;
      this.m_minor = 0;
      this.m_release = 5;
    }

    private VersionInfo(byte major, byte minor, byte release)
    {
      this.m_major = (int) major;
      this.m_minor = (int) minor;
      this.m_release = (int) release;
    }

    public static string StaticToString()
    {
      return string.Format("{0}.{1}.{2}", (object) (byte) 2, (object) (byte) 0, (object) (byte) 5) + string.Empty;
    }

    public override string ToString()
    {
      return string.Format("{0}.{1}.{2}", (object) this.m_major, (object) this.m_minor, (object) this.m_release) + string.Empty;
    }

    public int Number
    {
      get
      {
        return this.m_major * 100 + this.m_minor * 10 + this.m_release;
      }
    }

    public static VersionInfo Current()
    {
      return new VersionInfo((byte) 2, (byte) 0, (byte) 5);
    }

    public static bool Matches(VersionInfo version)
    {
      return version.m_major == 2 && version.m_minor == 0 && 5 == version.m_release;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Studio.ColorInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class ColorInfo
  {
    public Color mainColor = Color.get_white();
    public PatternInfo pattern = new PatternInfo();
    public float metallic;
    public float glossiness;

    public virtual void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write(JsonUtility.ToJson((object) this.mainColor));
      _writer.Write(this.metallic);
      _writer.Write(this.glossiness);
      this.pattern.Save(_writer, _version);
    }

    public virtual void Load(BinaryReader _reader, Version _version)
    {
      this.mainColor = (Color) JsonUtility.FromJson<Color>(_reader.ReadString());
      this.metallic = _reader.ReadSingle();
      this.glossiness = _reader.ReadSingle();
      this.pattern.Load(_reader, _version);
    }
  }
}

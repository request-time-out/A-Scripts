// Decompiled with JetBrains decompiler
// Type: Studio.ChangeAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

namespace Studio
{
  public class ChangeAmount
  {
    protected Vector3 m_Pos = Vector3.get_zero();
    protected Vector3 m_Rot = Vector3.get_zero();
    protected Vector3 m_Scale = Vector3.get_one();
    public Action onChangePos;
    public Action onChangePosAfter;
    public Action onChangeRot;
    public Action<Vector3> onChangeScale;

    public ChangeAmount()
    {
      this.m_Pos = Vector3.get_zero();
      this.m_Rot = Vector3.get_zero();
      this.m_Scale = Vector3.get_one();
    }

    public Vector3 pos
    {
      get
      {
        return this.m_Pos;
      }
      set
      {
        if (!Utility.SetStruct<Vector3>(ref this.m_Pos, value) || this.onChangePos == null)
          return;
        this.onChangePos();
        if (this.onChangePosAfter == null)
          return;
        this.onChangePosAfter();
      }
    }

    public Vector3 rot
    {
      get
      {
        return this.m_Rot;
      }
      set
      {
        if (!Utility.SetStruct<Vector3>(ref this.m_Rot, value) || this.onChangeRot == null)
          return;
        this.onChangeRot();
      }
    }

    public Vector3 scale
    {
      get
      {
        return this.m_Scale;
      }
      set
      {
        if (!Utility.SetStruct<Vector3>(ref this.m_Scale, value) || this.onChangeScale == null)
          return;
        this.onChangeScale(value);
      }
    }

    public Vector3 defRot { get; set; } = Vector3.get_zero();

    public void Save(BinaryWriter _writer)
    {
      _writer.Write((float) this.m_Pos.x);
      _writer.Write((float) this.m_Pos.y);
      _writer.Write((float) this.m_Pos.z);
      _writer.Write((float) this.m_Rot.x);
      _writer.Write((float) this.m_Rot.y);
      _writer.Write((float) this.m_Rot.z);
      _writer.Write((float) this.m_Scale.x);
      _writer.Write((float) this.m_Scale.y);
      _writer.Write((float) this.m_Scale.z);
    }

    public void Load(BinaryReader _reader)
    {
      Vector3 pos = this.m_Pos;
      ((Vector3) ref pos).Set(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
      this.m_Pos = pos;
      Vector3 rot = this.m_Rot;
      ((Vector3) ref rot).Set(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
      this.m_Rot = rot;
      Vector3 scale = this.m_Scale;
      ((Vector3) ref scale).Set(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
      this.m_Scale = scale;
    }

    public ChangeAmount Clone()
    {
      return new ChangeAmount()
      {
        pos = new Vector3((float) this.m_Pos.x, (float) this.m_Pos.y, (float) this.m_Pos.z),
        rot = new Vector3((float) this.m_Rot.x, (float) this.m_Rot.y, (float) this.m_Rot.z),
        scale = new Vector3((float) this.m_Scale.x, (float) this.m_Scale.y, (float) this.m_Scale.z)
      };
    }

    public void Copy(ChangeAmount _source, bool _pos = true, bool _rot = true, bool _scale = true)
    {
      if (_pos)
        this.pos = _source.pos;
      if (_rot)
        this.rot = _source.rot;
      if (!_scale)
        return;
      this.scale = _source.scale;
    }

    public void OnChange()
    {
      if (this.onChangePos != null)
      {
        this.onChangePos();
        if (this.onChangePosAfter != null)
          this.onChangePosAfter();
      }
      if (this.onChangeRot != null)
        this.onChangeRot();
      if (this.onChangeScale == null)
        return;
      this.onChangeScale(this.scale);
    }

    public void Reset()
    {
      this.pos = Vector3.get_zero();
      this.rot = Vector3.get_zero();
      this.scale = Vector3.get_one();
    }
  }
}

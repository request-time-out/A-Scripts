// Decompiled with JetBrains decompiler
// Type: Studio.PatternInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace Studio
{
  public class PatternInfo
  {
    private IntReactiveProperty _key = new IntReactiveProperty(0);
    private StringReactiveProperty _filePath = new StringReactiveProperty(string.Empty);
    public Color color = Color.get_white();
    public bool clamp = true;
    public Vector4 uv = new Vector4(0.0f, 0.0f, 1f, 1f);
    public float rot;

    public PatternInfo()
    {
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._key, (Action<M0>) (_ =>
      {
        if (_ == -1)
          return;
        ((ReactiveProperty<string>) this._filePath).set_Value(string.Empty);
      }));
      ObservableExtensions.Subscribe<string>((IObservable<M0>) this._filePath, (Action<M0>) (_ =>
      {
        if (_.IsNullOrEmpty())
          return;
        ((ReactiveProperty<int>) this._key).set_Value(-1);
      }));
    }

    public int key
    {
      get
      {
        return ((ReactiveProperty<int>) this._key).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._key).set_Value(value);
      }
    }

    public string filePath
    {
      get
      {
        return ((ReactiveProperty<string>) this._filePath).get_Value();
      }
      set
      {
        ((ReactiveProperty<string>) this._filePath).set_Value(value);
      }
    }

    public float ut
    {
      get
      {
        return (float) this.uv.z;
      }
      set
      {
        this.uv.z = (__Null) (double) value;
      }
    }

    public float vt
    {
      get
      {
        return (float) this.uv.w;
      }
      set
      {
        this.uv.w = (__Null) (double) value;
      }
    }

    public float us
    {
      get
      {
        return (float) this.uv.x;
      }
      set
      {
        this.uv.x = (__Null) (double) value;
      }
    }

    public float vs
    {
      get
      {
        return (float) this.uv.y;
      }
      set
      {
        this.uv.y = (__Null) (double) value;
      }
    }

    public string name
    {
      get
      {
        int _key = this.key;
        if (_key != -1)
        {
          PatternSelectInfo patternSelectInfo = Singleton<Studio.Studio>.Instance.patternSelectListCtrl.lstSelectInfo.Find((Predicate<PatternSelectInfo>) (p => p.index == _key));
          return patternSelectInfo != null ? patternSelectInfo.name : "なし";
        }
        return this.filePath.IsNullOrEmpty() ? "なし" : Path.GetFileNameWithoutExtension(this.filePath);
      }
    }

    public void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write(JsonUtility.ToJson((object) this.color));
      _writer.Write(((ReactiveProperty<int>) this._key).get_Value());
      _writer.Write(((ReactiveProperty<string>) this._filePath).get_Value());
      _writer.Write(this.clamp);
      _writer.Write(JsonUtility.ToJson((object) this.uv));
      _writer.Write(this.rot);
    }

    public void Load(BinaryReader _reader, Version _version)
    {
      this.color = (Color) JsonUtility.FromJson<Color>(_reader.ReadString());
      ((ReactiveProperty<int>) this._key).set_Value(_reader.ReadInt32());
      ((ReactiveProperty<string>) this._filePath).set_Value(_reader.ReadString());
      this.clamp = _reader.ReadBoolean();
      this.uv = (Vector4) JsonUtility.FromJson<Vector4>(_reader.ReadString());
      this.rot = _reader.ReadSingle();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Studio.ObjectInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;

namespace Studio
{
  public abstract class ObjectInfo
  {
    public ObjectInfo(int _key)
    {
      this.dicKey = _key;
      this.changeAmount = new ChangeAmount();
      this.treeState = TreeNodeObject.TreeState.Close;
      this.visible = true;
      if (this.dicKey == -1)
        return;
      Studio.Studio.AddChangeAmount(this.dicKey, this.changeAmount);
    }

    public int dicKey { get; private set; }

    public abstract int kind { get; }

    public ChangeAmount changeAmount { get; protected set; }

    public TreeNodeObject.TreeState treeState { get; set; }

    public bool visible { get; set; }

    public virtual int[] kinds
    {
      get
      {
        return new int[1]{ this.kind };
      }
    }

    public virtual void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write(this.kind);
      _writer.Write(this.dicKey);
      this.changeAmount.Save(_writer);
      _writer.Write((int) this.treeState);
      _writer.Write(this.visible);
    }

    public virtual void Load(BinaryReader _reader, Version _version, bool _import, bool _other = true)
    {
      if (!_import)
        this.dicKey = Studio.Studio.SetNewIndex(_reader.ReadInt32());
      else
        _reader.ReadInt32();
      this.changeAmount.Load(_reader);
      if (this.dicKey != -1 && !_import)
        Studio.Studio.AddChangeAmount(this.dicKey, this.changeAmount);
      if (_other)
        this.treeState = (TreeNodeObject.TreeState) _reader.ReadInt32();
      if (!_other)
        return;
      this.visible = _reader.ReadBoolean();
    }

    public abstract void DeleteKey();
  }
}

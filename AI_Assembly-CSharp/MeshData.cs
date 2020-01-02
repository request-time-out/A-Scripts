// Decompiled with JetBrains decompiler
// Type: MeshData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public class MeshData
{
  public int _instID;
  public int _vertCount;
  public int _materialCount;
  public float _boundSize;
  public float _camDist;

  public void Write(UsCmd cmd)
  {
    cmd.WriteInt32(this._instID);
    cmd.WriteInt32(this._vertCount);
    cmd.WriteInt32(this._materialCount);
    cmd.WriteFloat(this._boundSize);
    cmd.WriteFloat(this._camDist);
  }
}

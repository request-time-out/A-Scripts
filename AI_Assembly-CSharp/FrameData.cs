// Decompiled with JetBrains decompiler
// Type: FrameData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class FrameData
{
  public List<int> _frameMeshes = new List<int>();
  public List<int> _frameMaterials = new List<int>();
  public List<int> _frameTextures = new List<int>();
  public int _frameCount;
  public float _frameDeltaTime;
  public float _frameRealTime;
  public float _frameStartTime;

  public UsCmd CreatePacket()
  {
    UsCmd c = new UsCmd();
    c.WriteNetCmd(eNetCmd.SV_FrameDataV2);
    c.WriteInt32(this._frameCount);
    c.WriteFloat(this._frameDeltaTime);
    c.WriteFloat(this._frameRealTime);
    c.WriteFloat(this._frameStartTime);
    UsCmdUtil.WriteIntList(c, this._frameMeshes);
    UsCmdUtil.WriteIntList(c, this._frameMaterials);
    UsCmdUtil.WriteIntList(c, this._frameTextures);
    return c;
  }
}

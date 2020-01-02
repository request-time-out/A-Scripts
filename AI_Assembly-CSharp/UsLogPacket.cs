// Decompiled with JetBrains decompiler
// Type: UsLogPacket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

public class UsLogPacket
{
  public const int MAX_CONTENT_LEN = 1024;
  public const int MAX_CALLSTACK_LEN = 1024;
  public ushort SeqID;
  public UsLogType LogType;
  public string Content;
  public float RealtimeSinceStartup;
  public string Callstack;

  public UsLogPacket()
  {
    this.SeqID = ushort.MaxValue;
  }

  public UsLogPacket(UsCmd c)
  {
    this.SeqID = (ushort) c.ReadInt16();
    this.LogType = (UsLogType) c.ReadInt32();
    this.Content = c.ReadString();
    this.RealtimeSinceStartup = c.ReadFloat();
  }

  public UsCmd CreatePacket()
  {
    UsCmd usCmd = new UsCmd();
    usCmd.WriteNetCmd(eNetCmd.SV_App_Logging);
    usCmd.WriteInt16((short) this.SeqID);
    usCmd.WriteInt32((int) this.LogType);
    usCmd.WriteStringStripped(this.Content, (short) 1024);
    usCmd.WriteFloat(this.RealtimeSinceStartup);
    return usCmd;
  }
}

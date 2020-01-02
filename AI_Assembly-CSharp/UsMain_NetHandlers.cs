// Decompiled with JetBrains decompiler
// Type: UsMain_NetHandlers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using usmooth;

public class UsMain_NetHandlers
{
  private int SLICE_COUNT = 50;
  public static UsMain_NetHandlers Instance;

  public UsMain_NetHandlers(UsCmdParsing exec)
  {
    exec.RegisterHandler(eNetCmd.CL_Handshake, new UsCmdHandler(this.NetHandle_Handshake));
    exec.RegisterHandler(eNetCmd.CL_KeepAlive, new UsCmdHandler(this.NetHandle_KeepAlive));
    exec.RegisterHandler(eNetCmd.CL_ExecCommand, new UsCmdHandler(this.NetHandle_ExecCommand));
    exec.RegisterHandler(eNetCmd.CL_RequestFrameData, new UsCmdHandler(this.NetHandle_RequestFrameData));
    exec.RegisterHandler(eNetCmd.CL_FrameV2_RequestMeshes, new UsCmdHandler(this.NetHandle_FrameV2_RequestMeshes));
    exec.RegisterHandler(eNetCmd.CL_FrameV2_RequestNames, new UsCmdHandler(this.NetHandle_FrameV2_RequestNames));
    exec.RegisterHandler(eNetCmd.CL_QuerySwitches, new UsCmdHandler(this.NetHandle_QuerySwitches));
    exec.RegisterHandler(eNetCmd.CL_QuerySliders, new UsCmdHandler(this.NetHandle_QuerySliders));
  }

  private bool NetHandle_Handshake(eNetCmd cmd, UsCmd c)
  {
    Debug.Log((object) "executing handshake.");
    if (!string.IsNullOrEmpty(LogService.LastLogFile))
      Debug.Log((object) ("Log Path: " + LogService.LastLogFile));
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_HandshakeResponse);
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private bool NetHandle_KeepAlive(eNetCmd cmd, UsCmd c)
  {
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_KeepAliveResponse);
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private bool NetHandle_ExecCommand(eNetCmd cmd, UsCmd c)
  {
    string fullcmd = c.ReadString();
    bool flag = UsvConsole.Instance.ExecuteCommand(fullcmd);
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_ExecCommandResponse);
    cmd1.WriteInt32(!flag ? 0 : 1);
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private bool NetHandle_RequestFrameData(eNetCmd cmd, UsCmd c)
  {
    if (DataCollector.Instance == null)
      return true;
    FrameData frameData = DataCollector.Instance.CollectFrameData();
    UsNet.Instance.SendCommand(frameData.CreatePacket());
    UsNet.Instance.SendCommand(DataCollector.Instance.CreateMaterialCmd());
    UsNet.Instance.SendCommand(DataCollector.Instance.CreateTextureCmd());
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_FrameDataEnd);
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private bool NetHandle_FrameV2_RequestMeshes(eNetCmd cmd, UsCmd c)
  {
    if (DataCollector.Instance != null)
    {
      foreach (List<int> intList in UsGeneric.Slice<int>(UsCmdUtil.ReadIntList(c), this.SLICE_COUNT))
      {
        UsCmd cmd1 = new UsCmd();
        cmd1.WriteNetCmd(eNetCmd.SV_FrameDataV2_Meshes);
        cmd1.WriteInt32(intList.Count);
        foreach (int instID in intList)
          DataCollector.Instance.MeshTable.WriteMesh(instID, cmd1);
        UsNet.Instance.SendCommand(cmd1);
      }
    }
    return true;
  }

  private bool NetHandle_FrameV2_RequestNames(eNetCmd cmd, UsCmd c)
  {
    if (DataCollector.Instance != null)
    {
      foreach (List<int> intList in UsGeneric.Slice<int>(UsCmdUtil.ReadIntList(c), this.SLICE_COUNT))
      {
        UsCmd cmd1 = new UsCmd();
        cmd1.WriteNetCmd(eNetCmd.SV_FrameDataV2_Names);
        cmd1.WriteInt32(intList.Count);
        foreach (int instID in intList)
          DataCollector.Instance.WriteName(instID, cmd1);
        UsNet.Instance.SendCommand(cmd1);
      }
    }
    return true;
  }

  private bool NetHandle_QuerySwitches(eNetCmd cmd, UsCmd c)
  {
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_QuerySwitchesResponse);
    cmd1.WriteInt32(GameInterface.ObjectNames.Count);
    foreach (KeyValuePair<string, string> objectName in GameInterface.ObjectNames)
    {
      cmd1.WriteString(objectName.Key);
      cmd1.WriteString(objectName.Value);
      cmd1.WriteInt16((short) 1);
    }
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private bool NetHandle_QuerySliders(eNetCmd cmd, UsCmd c)
  {
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_QuerySlidersResponse);
    cmd1.WriteInt32(GameInterface.VisiblePercentages.Count);
    foreach (KeyValuePair<string, double> visiblePercentage in GameInterface.VisiblePercentages)
    {
      cmd1.WriteString(visiblePercentage.Key);
      cmd1.WriteFloat(0.0f);
      cmd1.WriteFloat(100f);
      cmd1.WriteFloat((float) visiblePercentage.Value);
    }
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }
}

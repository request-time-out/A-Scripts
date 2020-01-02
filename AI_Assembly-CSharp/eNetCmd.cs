// Decompiled with JetBrains decompiler
// Type: eNetCmd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public enum eNetCmd
{
  None = 0,
  CL_CmdBegin = 1000, // 0x000003E8
  CL_Handshake = 1001, // 0x000003E9
  CL_KeepAlive = 1002, // 0x000003EA
  CL_ExecCommand = 1003, // 0x000003EB
  CL_RequestFrameData = 1004, // 0x000003EC
  CL_FrameV2_RequestMeshes = 1005, // 0x000003ED
  CL_FrameV2_RequestNames = 1006, // 0x000003EE
  CL_QuerySwitches = 1007, // 0x000003EF
  CL_QuerySliders = 1008, // 0x000003F0
  CL_RequestStackSummary = 1009, // 0x000003F1
  CL_StartAnalysePixels = 1010, // 0x000003F2
  CL_RequestStackData = 1011, // 0x000003F3
  CL_CmdEnd = 1012, // 0x000003F4
  SV_CmdBegin = 2000, // 0x000007D0
  SV_HandshakeResponse = 2001, // 0x000007D1
  SV_KeepAliveResponse = 2002, // 0x000007D2
  SV_ExecCommandResponse = 2003, // 0x000007D3
  SV_FrameDataV2 = 2004, // 0x000007D4
  SV_FrameDataV2_Meshes = 2005, // 0x000007D5
  SV_FrameDataV2_Names = 2006, // 0x000007D6
  SV_FrameData_Material = 2007, // 0x000007D7
  SV_FrameData_Texture = 2008, // 0x000007D8
  SV_FrameDataEnd = 2009, // 0x000007D9
  SV_App_Logging = 2010, // 0x000007DA
  SV_QuerySwitchesResponse = 2011, // 0x000007DB
  SV_QuerySlidersResponse = 2012, // 0x000007DC
  SV_QueryStacksResponse = 2013, // 0x000007DD
  SV_VarTracerJsonParameter = 2014, // 0x000007DE
  SV_StressTestNames = 2015, // 0x000007DF
  SV_StressTestResult = 2016, // 0x000007E0
  SV_StartAnalysePixels = 2017, // 0x000007E1
  SV_CmdEnd = 2018, // 0x000007E2
  SV_SendLuaProfilerMsg = 2019, // 0x000007E3
  SV_StartLuaProfilerMsg = 2020, // 0x000007E4
}

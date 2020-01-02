// Decompiled with JetBrains decompiler
// Type: Utility.Network
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Net.NetworkInformation;
using UnityEngine;

namespace Utility
{
  public static class Network
  {
    public static string GetMACAddress()
    {
      string empty = string.Empty;
      NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
      if (networkInterfaces != null)
      {
        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
          PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
          if (physicalAddress != null && physicalAddress.GetAddressBytes().Length == 6)
          {
            string str = physicalAddress.ToString();
            empty += str;
            Debug.Log((object) str);
          }
        }
      }
      return empty;
    }
  }
}

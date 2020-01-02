// Decompiled with JetBrains decompiler
// Type: IPManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Net.NetworkInformation;
using System.Net.Sockets;

public class IPManager
{
  public static string GetIP(ADDRESSFAM addfam)
  {
    if (addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
      return (string) null;
    string empty = string.Empty;
    foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
    {
      NetworkInterfaceType networkInterfaceType1 = NetworkInterfaceType.Wireless80211;
      NetworkInterfaceType networkInterfaceType2 = NetworkInterfaceType.Ethernet;
      if ((networkInterface.NetworkInterfaceType == networkInterfaceType1 || networkInterface.NetworkInterfaceType == networkInterfaceType2) && networkInterface.OperationalStatus == OperationalStatus.Up)
      {
        foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
        {
          switch (addfam)
          {
            case ADDRESSFAM.IPv4:
              if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
              {
                empty = unicastAddress.Address.ToString();
                continue;
              }
              continue;
            case ADDRESSFAM.IPv6:
              if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetworkV6)
              {
                empty = unicastAddress.Address.ToString();
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    return empty;
  }
}

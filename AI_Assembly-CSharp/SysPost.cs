// Decompiled with JetBrains decompiler
// Type: SysPost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.ComponentModel;

public class SysPost
{
  public static bool AssertException(bool expr, string msg)
  {
    if (expr)
      return true;
    throw new Exception(msg);
  }

  public static void InvokeMulticast(object sender, MulticastDelegate md)
  {
    if ((object) md == null)
      return;
    SysPost.InvokeMulticast(sender, md, (EventArgs) null);
  }

  public static void InvokeMulticast(object sender, MulticastDelegate md, EventArgs e)
  {
    if ((object) md == null)
      return;
    foreach (SysPost.StdMulticastDelegation invocation in md.GetInvocationList())
    {
      ISynchronizeInvoke target = invocation.Target as ISynchronizeInvoke;
      try
      {
        if (target != null && target.InvokeRequired)
          target.Invoke((Delegate) invocation, new object[2]
          {
            sender,
            (object) e
          });
        else
          invocation(sender, e);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Event handling failed. \n");
        Console.WriteLine("{0}:\n", (object) ex.ToString());
      }
    }
  }

  public delegate void StdMulticastDelegation(object sender, EventArgs e);
}

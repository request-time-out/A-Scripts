// Decompiled with JetBrains decompiler
// Type: DefaultBytesProviders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

internal static class DefaultBytesProviders
{
  private static Dictionary<System.Type, object> _providers;

  static DefaultBytesProviders()
  {
    Dictionary<System.Type, object> dictionary1 = new Dictionary<System.Type, object>();
    Dictionary<System.Type, object> dictionary2 = dictionary1;
    System.Type key1 = typeof (int);
    // ISSUE: reference to a compiler-generated field
    if (DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache0 = new Func<int, byte[]>(BitConverter.GetBytes);
    }
    // ISSUE: reference to a compiler-generated field
    BytesProvider<int> bytesProvider1 = new BytesProvider<int>(DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache0);
    dictionary2.Add(key1, (object) bytesProvider1);
    Dictionary<System.Type, object> dictionary3 = dictionary1;
    System.Type key2 = typeof (long);
    // ISSUE: reference to a compiler-generated field
    if (DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache1 = new Func<long, byte[]>(BitConverter.GetBytes);
    }
    // ISSUE: reference to a compiler-generated field
    BytesProvider<long> bytesProvider2 = new BytesProvider<long>(DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache1);
    dictionary3.Add(key2, (object) bytesProvider2);
    Dictionary<System.Type, object> dictionary4 = dictionary1;
    System.Type key3 = typeof (short);
    // ISSUE: reference to a compiler-generated field
    if (DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache2 = new Func<short, byte[]>(BitConverter.GetBytes);
    }
    // ISSUE: reference to a compiler-generated field
    BytesProvider<short> bytesProvider3 = new BytesProvider<short>(DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache2);
    dictionary4.Add(key3, (object) bytesProvider3);
    Dictionary<System.Type, object> dictionary5 = dictionary1;
    System.Type key4 = typeof (float);
    // ISSUE: reference to a compiler-generated field
    if (DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache3 = new Func<float, byte[]>(BitConverter.GetBytes);
    }
    // ISSUE: reference to a compiler-generated field
    BytesProvider<float> bytesProvider4 = new BytesProvider<float>(DefaultBytesProviders.\u003C\u003Ef__mg\u0024cache3);
    dictionary5.Add(key4, (object) bytesProvider4);
    DefaultBytesProviders._providers = dictionary1;
  }

  public static BytesProvider<T> GetDefaultProvider<T>()
  {
    return (BytesProvider<T>) DefaultBytesProviders._providers[typeof (T)];
  }
}

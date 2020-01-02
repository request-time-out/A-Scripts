// Decompiled with JetBrains decompiler
// Type: AIProject.SystemUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace AIProject
{
  public static class SystemUtil
  {
    public static Task TryProcAsync(Task task)
    {
      // ISSUE: variable of a compiler-generated type
      SystemUtil.\u003CTryProcAsync\u003Ec__async0 procAsyncCAsync0;
      // ISSUE: reference to a compiler-generated field
      procAsyncCAsync0.task = task;
      // ISSUE: reference to a compiler-generated field
      procAsyncCAsync0.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref procAsyncCAsync0.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<SystemUtil.\u003CTryProcAsync\u003Ec__async0>((M0&) ref procAsyncCAsync0);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task<T> TryProcAsync<T>(Task<T> task)
    {
      // ISSUE: variable of a compiler-generated type
      SystemUtil.\u003CTryProcAsync\u003Ec__async1<T> procAsyncCAsync1;
      // ISSUE: reference to a compiler-generated field
      procAsyncCAsync1.task = task;
      // ISSUE: reference to a compiler-generated field
      procAsyncCAsync1.\u0024builder = AsyncTaskMethodBuilder<T>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<T> local = ref procAsyncCAsync1.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<T>) ref local).Start<SystemUtil.\u003CTryProcAsync\u003Ec__async1<T>>((M0&) ref procAsyncCAsync1);
      return ((AsyncTaskMethodBuilder<T>) ref local).get_Task();
    }

    public static bool TryParse(string input, out int result)
    {
      if (int.TryParse(input, out result))
        return true;
      Debug.LogError((object) string.Format("読み込み失敗 入力値: {0}", (object) input));
      return false;
    }

    public static bool TryParse(string input, out float result)
    {
      if (float.TryParse(input, out result))
        return true;
      Debug.LogError((object) string.Format("読み込み失敗 入力値: {0}", (object) input));
      return false;
    }

    public static bool TryParse<TEnum>(string input, out TEnum result) where TEnum : struct
    {
      if (Enum.TryParse<TEnum>(input, out result))
        return true;
      Debug.LogError((object) string.Format("読み込み失敗 <型: {0}  入力値: {1}>", (object) nameof (TEnum), (object) input));
      return false;
    }

    public static bool SetSafeStruct<T>(ref T destination, T newValue) where T : struct
    {
      if (destination.Equals((object) newValue))
        return false;
      destination = newValue;
      return true;
    }

    public static bool SetSafeClass<T>(ref T destination, T newValue) where T : class
    {
      if ((object) destination == null && (object) newValue == null || (object) destination != null && destination.Equals((object) newValue))
        return false;
      destination = newValue;
      return true;
    }

    public static string Replace(this string source, string newValue, params string[] oldValues)
    {
      if (source.IsNullOrEmpty())
        return source;
      string str = source;
      foreach (string oldValue in oldValues)
        str = str.Replace(oldValue, newValue);
      return str;
    }
  }
}

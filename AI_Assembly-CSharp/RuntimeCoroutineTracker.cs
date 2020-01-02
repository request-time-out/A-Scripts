// Decompiled with JetBrains decompiler
// Type: RuntimeCoroutineTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class RuntimeCoroutineTracker
{
  public static Coroutine InvokeStart(MonoBehaviour initiator, IEnumerator routine)
  {
    if (!CoroutineRuntimeTrackingConfig.EnableTracking)
      return initiator.StartCoroutine(routine);
    try
    {
      return initiator.StartCoroutine((IEnumerator) new TrackedCoroutine(routine));
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      return (Coroutine) null;
    }
  }

  public static Coroutine InvokeStart(
    MonoBehaviour initiator,
    string methodName,
    object arg = null)
  {
    if (!CoroutineRuntimeTrackingConfig.EnableTracking)
      return initiator.StartCoroutine(methodName, arg);
    try
    {
      System.Type type = ((object) initiator).GetType();
      if (type == (System.Type) null)
        throw new ArgumentNullException(nameof (initiator), "invalid initiator (null type)");
      MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
      if (method == (MethodInfo) null)
        throw new ArgumentNullException(nameof (methodName), string.Format("Invalid method {0} (method not found)", (object) methodName));
      object[] parameters = (object[]) null;
      if (arg != null)
        parameters = new object[1]{ arg };
      if (!(method.Invoke((object) initiator, parameters) is IEnumerator routine))
        throw new ArgumentNullException(nameof (methodName), string.Format("Invalid method {0} (not an IEnumerator)", (object) methodName));
      return RuntimeCoroutineTracker.InvokeStart(initiator, routine);
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      return (Coroutine) null;
    }
  }
}

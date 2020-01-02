// Decompiled with JetBrains decompiler
// Type: SampleRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class SampleRunner : MonoBehaviour
{
  public SampleRunner()
  {
    base.\u002Ector();
  }

  private void SendEditorCommand(string cmd)
  {
  }

  private void Start()
  {
    CoroutineRuntimeTrackingConfig.EnableTracking = true;
    this.StartCoroutine(RuntimeCoroutineStats.Instance.BroadcastCoroutine());
    this.SendEditorCommand("AppStarted");
    ((Component) this).get_gameObject().AddComponent<TestPluginRunner>();
    // ISSUE: reference to a compiler-generated field
    if (SampleRunner.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      SampleRunner.\u003C\u003Ef__mg\u0024cache0 = new CoroutineStartHandler_IEnumerator((object) null, __methodptr(InvokeStart));
    }
    // ISSUE: reference to a compiler-generated field
    CoroutinePluginForwarder.InvokeStart_IEnumerator = (__Null) SampleRunner.\u003C\u003Ef__mg\u0024cache0;
    // ISSUE: reference to a compiler-generated field
    if (SampleRunner.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      SampleRunner.\u003C\u003Ef__mg\u0024cache1 = new CoroutineStartHandler_String((object) null, __methodptr(InvokeStart));
    }
    // ISSUE: reference to a compiler-generated field
    CoroutinePluginForwarder.InvokeStart_String = (__Null) SampleRunner.\u003C\u003Ef__mg\u0024cache1;
    CoroutineSpawner coroutineSpawner = (CoroutineSpawner) ((Component) this).get_gameObject().AddComponent<CoroutineSpawner>();
    RuntimeCoroutineTracker.InvokeStart((MonoBehaviour) coroutineSpawner, "Co01_WaitForSeconds", (object) null);
    RuntimeCoroutineTracker.InvokeStart((MonoBehaviour) coroutineSpawner, "Co02_PerFrame_NULL", (object) null);
    RuntimeCoroutineTracker.InvokeStart((MonoBehaviour) coroutineSpawner, "Co03_PerFrame_EOF", (object) null);
    RuntimeCoroutineTracker.InvokeStart((MonoBehaviour) coroutineSpawner, "Co04_PerFrame_ARG", (object) 0.683f);
  }

  private void OnDestroy()
  {
    this.SendEditorCommand("AppDestroyed");
  }
}

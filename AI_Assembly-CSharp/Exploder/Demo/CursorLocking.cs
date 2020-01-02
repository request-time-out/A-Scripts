// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.CursorLocking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Exploder.Utils;
using UnityEngine;

namespace Exploder.Demo
{
  public class CursorLocking : MonoBehaviour
  {
    public bool LockCursor;
    public KeyCode LockKey;
    public KeyCode UnlockKey;
    public static bool IsLocked;
    private static CursorLocking instance;

    public CursorLocking()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      CursorLocking.instance = this;
    }

    private void Update()
    {
      if (this.LockCursor)
        CursorLocking.Lock();
      else
        CursorLocking.Unlock();
      CursorLocking.IsLocked = Compatibility.IsCursorLocked();
      if (Input.GetKeyDown(this.LockKey))
        CursorLocking.Lock();
      if (Input.GetKeyDown(this.UnlockKey))
        CursorLocking.Unlock();
      if (Compatibility.IsCursorLocked())
        return;
      Compatibility.SetCursorVisible(true);
    }

    public static void Lock()
    {
      Compatibility.LockCursor(true);
      Compatibility.SetCursorVisible(false);
      CursorLocking.instance.LockCursor = true;
    }

    public static void Unlock()
    {
      Compatibility.LockCursor(false);
      Compatibility.SetCursorVisible(true);
      CursorLocking.instance.LockCursor = false;
    }
  }
}

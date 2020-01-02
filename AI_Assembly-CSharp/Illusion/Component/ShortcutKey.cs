// Decompiled with JetBrains decompiler
// Type: Illusion.Component.ShortcutKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Illusion.Component
{
  public class ShortcutKey : MonoBehaviour
  {
    private static bool _Enable = true;
    private static ShortcutKey.ShortcutKeyList list = new ShortcutKey.ShortcutKeyList();
    public List<ShortcutKey.Proc> procList;

    public ShortcutKey()
    {
      base.\u002Ector();
    }

    public static bool Enable
    {
      set
      {
        ShortcutKey._Enable = value;
      }
    }

    private void Start()
    {
      ShortcutKey.list.Add(this);
    }

    private void OnDestroy()
    {
      ShortcutKey.list.Remove(this);
    }

    private void Update()
    {
      if (!ShortcutKey._Enable)
        return;
      foreach (ShortcutKey.Proc proc in this.procList)
      {
        if (proc.enabled && Input.GetKeyDown(proc.keyCode))
          proc.call.Invoke();
      }
    }

    private bool IsReglate(string sceneName)
    {
      if (!Singleton<Scene>.IsInstance())
        return true;
      switch (Singleton<Scene>.Instance.LoadSceneName)
      {
        case "Init":
        case "Logo":
          return true;
        default:
          return Singleton<Scene>.Instance.IsNowLoadingFade || Singleton<Scene>.Instance.NowSceneNames.Contains(sceneName);
      }
    }

    public void _GameEnd()
    {
      if (this.IsReglate("Exit") || Object.op_Implicit((Object) Singleton<Game>.Instance.ExitScene))
        return;
      Singleton<Scene>.Instance.GameEnd(true);
    }

    [Serializable]
    public class Proc
    {
      public bool enabled = true;
      public UnityEvent call = new UnityEvent();
      public KeyCode keyCode;

      public int refCount { get; set; }
    }

    private class ShortcutKeyList
    {
      private List<ShortcutKey> list = new List<ShortcutKey>();

      public void Add(ShortcutKey sk)
      {
        foreach (ShortcutKey shortcutKey in this.list)
        {
          foreach (ShortcutKey.Proc proc1 in shortcutKey.procList)
          {
            foreach (ShortcutKey.Proc proc2 in sk.procList)
            {
              if (proc2.keyCode == proc1.keyCode)
              {
                ++proc1.refCount;
                proc1.enabled = false;
              }
            }
          }
        }
        this.list.Insert(0, sk);
      }

      public bool Remove(ShortcutKey sk)
      {
        if (!this.list.Remove(sk))
          return false;
        foreach (ShortcutKey shortcutKey in this.list)
        {
          foreach (ShortcutKey.Proc proc1 in shortcutKey.procList)
          {
            foreach (ShortcutKey.Proc proc2 in sk.procList)
            {
              if (proc2.keyCode == proc1.keyCode && --proc1.refCount == 0)
                proc1.enabled = true;
            }
          }
        }
        return true;
      }
    }
  }
}

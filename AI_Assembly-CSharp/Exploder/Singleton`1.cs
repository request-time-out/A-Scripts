// Decompiled with JetBrains decompiler
// Type: Exploder.Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T instance;
    private static bool isQuitting;

    public Singleton()
    {
      base.\u002Ector();
    }

    public static T Instance
    {
      get
      {
        if (Singleton<T>.isQuitting)
          return (T) null;
        if (Object.op_Equality((Object) (object) Singleton<T>.instance, (Object) null))
        {
          Singleton<T>.instance = (T) Object.FindObjectOfType(typeof (T));
          if (Object.FindObjectsOfType(typeof (T)).Length > 1)
          {
            Debug.LogWarning((object) "More than 1 singleton opened!");
            return Singleton<T>.instance;
          }
          if (Object.op_Equality((Object) (object) Singleton<T>.instance, (Object) null))
          {
            GameObject gameObject = new GameObject("ExploderCore");
            Singleton<T>.instance = (T) gameObject.AddComponent<T>();
            Object.DontDestroyOnLoad((Object) gameObject);
          }
        }
        return Singleton<T>.instance;
      }
    }

    public virtual void OnDestroy()
    {
      Singleton<T>.isQuitting = true;
    }
  }
}

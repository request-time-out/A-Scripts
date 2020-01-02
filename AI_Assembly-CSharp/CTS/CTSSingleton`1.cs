// Decompiled with JetBrains decompiler
// Type: CTS.CTSSingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CTS
{
  public class CTSSingleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static object _lock = new object();
    private static T _instance;

    public CTSSingleton()
    {
      base.\u002Ector();
    }

    public static T Instance
    {
      get
      {
        lock (CTSSingleton<T>._lock)
        {
          if (Object.op_Equality((Object) (object) CTSSingleton<T>._instance, (Object) null))
          {
            GameObject gameObject = new GameObject();
            CTSSingleton<T>._instance = (T) gameObject.AddComponent<T>();
            ((Object) gameObject).set_name(typeof (T).ToString());
            ((Object) gameObject).set_hideFlags((HideFlags) 61);
            if (Application.get_isPlaying())
              Object.DontDestroyOnLoad((Object) gameObject);
          }
          return CTSSingleton<T>._instance;
        }
      }
    }
  }
}

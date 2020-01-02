// Decompiled with JetBrains decompiler
// Type: SimpleSingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public class SimpleSingleton<T> where T : class, new()
{
  private static T instance;

  private SimpleSingleton()
  {
  }

  public static T Instance
  {
    get
    {
      if ((object) SimpleSingleton<T>.instance == null)
        SimpleSingleton<T>.instance = new T();
      return SimpleSingleton<T>.instance;
    }
  }
}

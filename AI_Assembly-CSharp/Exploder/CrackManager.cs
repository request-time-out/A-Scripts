// Decompiled with JetBrains decompiler
// Type: Exploder.CrackManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class CrackManager
  {
    private readonly Dictionary<GameObject, CrackedObject> crackedObjects;

    public CrackManager(Core core)
    {
      this.crackedObjects = new Dictionary<GameObject, CrackedObject>();
    }

    public CrackedObject Create(GameObject originalObject, ExploderParams parameters)
    {
      CrackedObject crackedObject = new CrackedObject(originalObject, parameters);
      this.crackedObjects[originalObject] = crackedObject;
      return crackedObject;
    }

    public long Explode(GameObject gameObject)
    {
      if (this.crackedObjects.ContainsKey(gameObject))
      {
        long num = 0;
        CrackedObject crackedObject;
        if (this.crackedObjects.TryGetValue(gameObject, out crackedObject))
        {
          num = crackedObject.Explode();
          this.crackedObjects.Remove(gameObject);
        }
        return num;
      }
      Debug.LogErrorFormat("GameObject {0} not cracked, Call CrackObject first!", new object[1]
      {
        (object) ((Object) gameObject).get_name()
      });
      return 0;
    }

    public long ExplodePartial(
      GameObject gameObject,
      Vector3 shotDir,
      Vector3 hitPosition,
      float bulletSize)
    {
      if (this.crackedObjects.ContainsKey(gameObject))
      {
        long num = 0;
        CrackedObject crackedObject;
        if (this.crackedObjects.TryGetValue(gameObject, out crackedObject))
          num = crackedObject.ExplodePartial(gameObject, shotDir, hitPosition, bulletSize);
        return num;
      }
      Debug.LogErrorFormat("GameObject {0} not cracked, Call CrackObject first!", new object[1]
      {
        (object) ((Object) gameObject).get_name()
      });
      return 0;
    }

    public long ExplodeAll()
    {
      long num = 0;
      using (Dictionary<GameObject, CrackedObject>.ValueCollection.Enumerator enumerator = this.crackedObjects.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CrackedObject current = enumerator.Current;
          num += current.Explode();
        }
      }
      this.crackedObjects.Clear();
      return num;
    }

    public bool IsCracked(GameObject gameObject)
    {
      return this.crackedObjects.ContainsKey(gameObject);
    }
  }
}

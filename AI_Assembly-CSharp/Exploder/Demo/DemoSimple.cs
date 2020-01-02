// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.DemoSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Exploder.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exploder.Demo
{
  public class DemoSimple : MonoBehaviour
  {
    private ExploderObject Exploder;
    private GameObject[] DestroyableObjects;

    public DemoSimple()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.Exploder = ExploderSingleton.Instance;
      if (this.Exploder.DontUseTag)
      {
        Object[] objectsOfType = Object.FindObjectsOfType(typeof (Explodable));
        List<GameObject> gameObjectList = new List<GameObject>(objectsOfType.Length);
        gameObjectList.AddRange(((IEnumerable) objectsOfType).Cast<Explodable>().Where<Explodable>((Func<Explodable, bool>) (ex => Object.op_Implicit((Object) ex))).Select<Explodable, GameObject>((Func<Explodable, GameObject>) (ex => ((Component) ex).get_gameObject())));
        this.DestroyableObjects = gameObjectList.ToArray();
      }
      else
        this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
    }

    private void OnGUI()
    {
      if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Explode!") && Object.op_Implicit((Object) this.Exploder))
        this.Exploder.ExplodeRadius();
      if (!GUI.Button(new Rect(130f, 10f, 100f, 30f), "Reset"))
        return;
      ExploderUtils.SetActive(((Component) this.Exploder).get_gameObject(), true);
      if (this.Exploder.DestroyOriginalObject)
        return;
      foreach (GameObject destroyableObject in this.DestroyableObjects)
        ExploderUtils.SetActiveRecursively(destroyableObject, true);
      ExploderUtils.SetActive(((Component) this.Exploder).get_gameObject(), true);
    }
  }
}

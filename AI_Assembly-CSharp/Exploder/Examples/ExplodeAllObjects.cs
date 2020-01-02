// Decompiled with JetBrains decompiler
// Type: Exploder.Examples.ExplodeAllObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Exploder.Utils;
using UnityEngine;

namespace Exploder.Examples
{
  public class ExplodeAllObjects : MonoBehaviour
  {
    private ExploderObject Exploder;
    private GameObject[] DestroyableObjects;

    public ExplodeAllObjects()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.DestroyableObjects = GameObject.FindGameObjectsWithTag("Exploder");
      this.Exploder = ExploderSingleton.Instance;
    }

    private void Update()
    {
      if (!Input.GetKeyDown((KeyCode) 13))
        return;
      this.Exploder.ExplodeObjects(this.DestroyableObjects);
    }

    private void ExplodeObject(GameObject gameObject)
    {
      ExploderUtils.SetActive(((Component) this.Exploder).get_gameObject(), true);
      ((Component) this.Exploder).get_transform().set_position(ExploderUtils.GetCentroid(gameObject));
      this.Exploder.Radius = 1f;
      this.Exploder.ExplodeRadius();
    }

    private void OnGUI()
    {
      GUI.Label(new Rect(200f, 10f, 300f, 30f), "Hit enter to explode everything!");
    }
  }
}

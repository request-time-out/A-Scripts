// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.DemoClickPartialExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Exploder.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exploder.Demo
{
  public class DemoClickPartialExplode : MonoBehaviour
  {
    public GameObject[] toCrack;
    private ExploderObject exploder;

    public DemoClickPartialExplode()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Application.set_targetFrameRate(60);
      this.exploder = ExploderSingleton.Instance;
      foreach (GameObject gameObject in this.toCrack)
        this.exploder.CrackObject(gameObject);
    }

    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
        return;
      Ray ray = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
      RaycastHit raycastHit;
      if (!Physics.Raycast(ray, ref raycastHit))
        return;
      this.exploder.ExplodePartial(((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject(), ((Ray) ref ray).get_direction(), ((RaycastHit) ref raycastHit).get_point(), 1f, (ExploderObject.OnExplosion) ((ms, state) => Debug.LogFormat("Explosion callback: {0}", new object[1]
      {
        (object) state
      })));
    }

    private void OnGUI()
    {
      if (!GUI.Button(new Rect(10f, 10f, 100f, 30f), "Reset"))
        ;
      if (!GUI.Button(new Rect(10f, 50f, 100f, 30f), "NextScene"))
        return;
      SceneManager.LoadScene(1);
    }
  }
}

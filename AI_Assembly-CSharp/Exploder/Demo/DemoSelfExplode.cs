// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.DemoSelfExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class DemoSelfExplode : MonoBehaviour
  {
    public Camera Camera;

    public DemoSelfExplode()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Application.set_targetFrameRate(60);
      if (Object.op_Implicit((Object) this.Camera))
        return;
      this.Camera = Camera.get_main();
    }

    private bool IsExplodable(GameObject obj)
    {
      return obj.CompareTag(ExploderObject.Tag);
    }

    private void Update()
    {
      RaycastHit raycastHit;
      if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) || !Physics.Raycast(!Object.op_Implicit((Object) this.Camera) ? Camera.get_main().ScreenPointToRay(Input.get_mousePosition()) : this.Camera.ScreenPointToRay(Input.get_mousePosition()), ref raycastHit))
        return;
      GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject();
      if (!this.IsExplodable(gameObject) || !Input.GetMouseButtonDown(0))
        return;
      this.ExplodeObject(gameObject);
    }

    private void ExplodeObject(GameObject obj)
    {
      ExploderObject component = (ExploderObject) obj.GetComponent<ExploderObject>();
      if (!Object.op_Implicit((Object) component))
        return;
      component.ExplodeObject(((Component) this).get_gameObject(), new ExploderObject.OnExplosion(this.OnExplosion));
    }

    private void OnExplosion(float time, ExploderObject.ExplosionState state)
    {
      if (state != ExploderObject.ExplosionState.ExplosionFinished)
        ;
    }
  }
}

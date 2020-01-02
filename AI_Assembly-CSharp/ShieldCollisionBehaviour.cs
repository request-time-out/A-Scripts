// Decompiled with JetBrains decompiler
// Type: ShieldCollisionBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ShieldCollisionBehaviour : MonoBehaviour
{
  public GameObject EffectOnHit;
  public GameObject ExplosionOnHit;
  public bool IsWaterInstance;
  public float ScaleWave;
  public bool CreateMechInstanceOnHit;
  public Vector3 AngleFix;
  public int currentQueue;

  public ShieldCollisionBehaviour()
  {
    base.\u002Ector();
  }

  public void ShieldCollisionEnter(CollisionInfo e)
  {
    if (!Object.op_Inequality((Object) ((RaycastHit) ref e.Hit).get_transform(), (Object) null))
      return;
    if (this.IsWaterInstance)
    {
      Transform transform = ((GameObject) Object.Instantiate<GameObject>((M0) this.ExplosionOnHit)).get_transform();
      transform.set_parent(((Component) this).get_transform());
      float num = (float) ((Component) this).get_transform().get_localScale().x * this.ScaleWave;
      transform.set_localScale(new Vector3(num, num, num));
      transform.set_localPosition(new Vector3(0.0f, 1f / 1000f, 0.0f));
      transform.LookAt(((RaycastHit) ref e.Hit).get_point());
    }
    else
    {
      if (Object.op_Inequality((Object) this.EffectOnHit, (Object) null))
      {
        if (!this.CreateMechInstanceOnHit)
        {
          Renderer componentInChildren = (Renderer) ((Component) ((RaycastHit) ref e.Hit).get_transform()).GetComponentInChildren<Renderer>();
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.EffectOnHit);
          gameObject.get_transform().set_parent(((Component) componentInChildren).get_transform());
          gameObject.get_transform().set_localPosition(Vector3.get_zero());
          AddMaterialOnHit component = (AddMaterialOnHit) gameObject.GetComponent<AddMaterialOnHit>();
          component.SetMaterialQueue(this.currentQueue);
          component.UpdateMaterial(e.Hit);
        }
        else
        {
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.EffectOnHit);
          Transform transform = gameObject.get_transform();
          transform.set_parent(((Component) ((Component) this).GetComponent<Renderer>()).get_transform());
          transform.set_localPosition(Vector3.get_zero());
          transform.set_localScale(Vector3.op_Multiply(((Component) this).get_transform().get_localScale(), this.ScaleWave));
          transform.LookAt(((RaycastHit) ref e.Hit).get_point());
          transform.Rotate(this.AngleFix);
          ((Renderer) gameObject.GetComponent<Renderer>()).get_material().set_renderQueue(this.currentQueue - 1000);
        }
      }
      if (this.currentQueue > 4000)
        this.currentQueue = 3001;
      else
        ++this.currentQueue;
      if (!Object.op_Inequality((Object) this.ExplosionOnHit, (Object) null))
        return;
      ((GameObject) Object.Instantiate<GameObject>((M0) this.ExplosionOnHit, ((RaycastHit) ref e.Hit).get_point(), (Quaternion) null)).get_transform().set_parent(((Component) this).get_transform());
    }
  }

  private void Update()
  {
  }
}

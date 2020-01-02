// Decompiled with JetBrains decompiler
// Type: ETFXProjectileScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ETFXProjectileScript : MonoBehaviour
{
  public GameObject impactParticle;
  public GameObject projectileParticle;
  public GameObject muzzleParticle;
  public GameObject[] trailParticles;
  [HideInInspector]
  public Vector3 impactNormal;
  private bool hasCollided;

  public ETFXProjectileScript()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.projectileParticle = (GameObject) Object.Instantiate<GameObject>((M0) this.projectileParticle, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
    this.projectileParticle.get_transform().set_parent(((Component) this).get_transform());
    if (!Object.op_Implicit((Object) this.muzzleParticle))
      return;
    this.muzzleParticle = (GameObject) Object.Instantiate<GameObject>((M0) this.muzzleParticle, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
    Object.Destroy((Object) this.muzzleParticle, 1.5f);
  }

  private void OnCollisionEnter(Collision hit)
  {
    if (this.hasCollided)
      return;
    this.hasCollided = true;
    this.impactParticle = (GameObject) Object.Instantiate<GameObject>((M0) this.impactParticle, ((Component) this).get_transform().get_position(), Quaternion.FromToRotation(Vector3.get_up(), this.impactNormal));
    if (hit.get_gameObject().get_tag() == "Destructible")
      Object.Destroy((Object) hit.get_gameObject());
    foreach (Object trailParticle in this.trailParticles)
    {
      GameObject gameObject = ((Component) ((Component) this).get_transform().Find(((Object) this.projectileParticle).get_name() + "/" + trailParticle.get_name())).get_gameObject();
      gameObject.get_transform().set_parent((Transform) null);
      Object.Destroy((Object) gameObject, 3f);
    }
    Object.Destroy((Object) this.projectileParticle, 3f);
    Object.Destroy((Object) this.impactParticle, 5f);
    Object.Destroy((Object) ((Component) this).get_gameObject());
    ParticleSystem[] componentsInChildren = (ParticleSystem[]) ((Component) this).GetComponentsInChildren<ParticleSystem>();
    for (int index = 1; index < componentsInChildren.Length; ++index)
    {
      ParticleSystem particleSystem = componentsInChildren[index];
      if (((Object) ((Component) particleSystem).get_gameObject()).get_name().Contains("Trail"))
      {
        ((Component) particleSystem).get_transform().SetParent((Transform) null);
        Object.Destroy((Object) ((Component) particleSystem).get_gameObject(), 2f);
      }
    }
  }
}

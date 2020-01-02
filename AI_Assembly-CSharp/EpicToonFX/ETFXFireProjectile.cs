// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXFireProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicToonFX
{
  public class ETFXFireProjectile : MonoBehaviour
  {
    private RaycastHit hit;
    public GameObject[] projectiles;
    public Transform spawnPosition;
    [HideInInspector]
    public int currentProjectile;
    public float speed;
    private ETFXButtonScript selectedProjectileButton;

    public ETFXFireProjectile()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.selectedProjectileButton = (ETFXButtonScript) GameObject.Find("Button").GetComponent<ETFXButtonScript>();
    }

    private void Update()
    {
      if (Input.GetKeyDown((KeyCode) 275))
        this.nextEffect();
      if (Input.GetKeyDown((KeyCode) 100))
        this.nextEffect();
      if (Input.GetKeyDown((KeyCode) 97))
        this.previousEffect();
      else if (Input.GetKeyDown((KeyCode) 276))
        this.previousEffect();
      if (Input.GetKeyDown((KeyCode) 323) && !EventSystem.get_current().IsPointerOverGameObject() && Physics.Raycast(Camera.get_main().ScreenPointToRay(Input.get_mousePosition()), ref this.hit, 100f))
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.projectiles[this.currentProjectile], this.spawnPosition.get_position(), Quaternion.get_identity());
        gameObject.get_transform().LookAt(((RaycastHit) ref this.hit).get_point());
        ((Rigidbody) gameObject.GetComponent<Rigidbody>()).AddForce(Vector3.op_Multiply(gameObject.get_transform().get_forward(), this.speed));
        ((ETFXProjectileScript) gameObject.GetComponent<ETFXProjectileScript>()).impactNormal = ((RaycastHit) ref this.hit).get_normal();
      }
      Ray ray1 = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
      Vector3 origin = ((Ray) ref ray1).get_origin();
      Ray ray2 = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
      Vector3 vector3 = Vector3.op_Multiply(((Ray) ref ray2).get_direction(), 100f);
      Color yellow = Color.get_yellow();
      Debug.DrawRay(origin, vector3, yellow);
    }

    public void nextEffect()
    {
      if (this.currentProjectile < this.projectiles.Length - 1)
        ++this.currentProjectile;
      else
        this.currentProjectile = 0;
      this.selectedProjectileButton.getProjectileNames();
    }

    public void previousEffect()
    {
      if (this.currentProjectile > 0)
        --this.currentProjectile;
      else
        this.currentProjectile = this.projectiles.Length - 1;
      this.selectedProjectileButton.getProjectileNames();
    }

    public void AdjustSpeed(float newSpeed)
    {
      this.speed = newSpeed;
    }
  }
}

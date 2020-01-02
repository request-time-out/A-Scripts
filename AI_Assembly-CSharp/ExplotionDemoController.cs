// Decompiled with JetBrains decompiler
// Type: ExplotionDemoController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using UnityEngine;

public class ExplotionDemoController : MonoBehaviour
{
  public GameObject[] rangerPrefabs;
  public EffectorExplode currentRanger;
  public RadialFillCursor cursor;
  public Transform spawnPosition;
  private int lastRangerIndex;

  public ExplotionDemoController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.cursor.Show(true);
  }

  private void Update()
  {
    if (Input.GetKeyDown((KeyCode) 49))
      this.SpawnRanger(0);
    if (Input.GetKeyDown((KeyCode) 50))
      this.SpawnRanger(1);
    if (Input.GetKeyDown((KeyCode) 51))
      this.SpawnRanger(2);
    if (!Input.GetMouseButtonUp(0))
      return;
    this.Explode();
  }

  public void SpawnRanger(int index)
  {
    this.CancelInvoke(nameof (SpawnRanger));
    if (index >= 0)
      this.lastRangerIndex = index;
    if (Object.op_Implicit((Object) this.currentRanger) && !this.currentRanger.exploded)
      Object.DestroyImmediate((Object) ((Component) this.currentRanger).get_gameObject());
    this.currentRanger = (EffectorExplode) ((GameObject) Object.Instantiate<GameObject>((M0) this.rangerPrefabs[this.lastRangerIndex], this.spawnPosition.get_position(), Quaternion.get_identity())).GetComponent<EffectorExplode>();
    this.cursor.Show(true);
  }

  public void SpawnRanger()
  {
    this.SpawnRanger(-1);
  }

  public void Explode()
  {
    if (Object.op_Implicit((Object) this.currentRanger))
    {
      this.currentRanger.ExplodeAt(((Component) this.cursor).get_transform().get_position(), this.cursor.radius, this.cursor.angle, this.cursor.rotationAngle, this.cursor.strenght);
      ((Behaviour) ((Component) this.currentRanger).GetComponent<BoxCollider2D>()).set_enabled(false);
    }
    this.Invoke("SpawnRanger", 1f);
    this.cursor.Show(false);
  }
}

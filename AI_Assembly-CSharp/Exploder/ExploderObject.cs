// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class ExploderObject : MonoBehaviour
  {
    public static string Tag = "Exploder";
    public bool DontUseTag;
    public float Radius;
    public Vector3 CubeRadius;
    public bool UseCubeRadius;
    public Vector3 ForceVector;
    public bool UseForceVector;
    public float Force;
    public float FrameBudget;
    public int TargetFragments;
    public ExploderObject.ThreadOptions ThreadOption;
    public bool ExplodeSelf;
    public bool DisableRadiusScan;
    public bool HideSelf;
    public bool DestroyOriginalObject;
    public bool UniformFragmentDistribution;
    public bool SplitMeshIslands;
    public bool DisableTriangulation;
    public int FragmentPoolSize;
    public bool Use2DCollision;
    public ExploderObject.CuttingStyleOption CuttingStyle;
    public FragmentDeactivation FragmentDeactivation;
    public FragmentSFX FragmentSFX;
    public FragmentOption FragmentOptions;
    private Core core;

    public ExploderObject()
    {
      base.\u002Ector();
    }

    public void ExplodeRadius()
    {
      this.ExplodeRadius((ExploderObject.OnExplosion) null);
    }

    public void ExplodeRadius(ExploderObject.OnExplosion callback)
    {
      this.core.Enqueue(this, callback, false, (GameObject[]) null);
    }

    public void ExplodeObject(GameObject obj)
    {
      this.ExplodeObject(obj, (ExploderObject.OnExplosion) null);
    }

    public void ExplodeObject(GameObject obj, ExploderObject.OnExplosion callback)
    {
      this.core.Enqueue(this, callback, false, obj);
    }

    public void ExplodeObjects(params GameObject[] objects)
    {
      this.ExplodeObjects((ExploderObject.OnExplosion) null, objects);
    }

    public void ExplodeObjects(ExploderObject.OnExplosion callback, params GameObject[] objects)
    {
      this.core.Enqueue(this, callback, false, objects);
    }

    public void ExplodePartial(
      GameObject obj,
      Vector3 shotDir,
      Vector3 hitPosition,
      float bulletSize)
    {
      this.ExplodePartial(obj, shotDir, hitPosition, bulletSize, (ExploderObject.OnExplosion) null);
    }

    public void ExplodePartial(
      GameObject obj,
      Vector3 shotDir,
      Vector3 hitPosition,
      float bulletSize,
      ExploderObject.OnExplosion callback)
    {
      this.core.ExplodePartial(obj, shotDir, hitPosition, bulletSize, callback);
    }

    public void CrackRadius()
    {
      this.CrackRadius((ExploderObject.OnExplosion) null);
    }

    public void CrackRadius(ExploderObject.OnExplosion callback)
    {
      this.core.Enqueue(this, callback, true, (GameObject[]) null);
    }

    public void CrackObject(GameObject obj)
    {
      this.CrackObject(obj, (ExploderObject.OnExplosion) null);
    }

    public void CrackObject(GameObject obj, ExploderObject.OnExplosion callback)
    {
      this.core.Enqueue(this, callback, true, obj);
    }

    public bool CanCrack()
    {
      return this.TargetFragments < FragmentPool.Instance.GetAvailableCrackFragmentsCount();
    }

    public bool IsCracked(GameObject gm)
    {
      return this.core.IsCracked(gm);
    }

    public void ExplodeCracked(GameObject obj, ExploderObject.OnExplosion callback)
    {
      this.core.ExplodeCracked(obj, callback);
    }

    public void ExplodeCracked(GameObject obj)
    {
      this.core.ExplodeCracked(obj, (ExploderObject.OnExplosion) null);
    }

    public void ExplodeCracked(ExploderObject.OnExplosion callback)
    {
      this.ExplodeCracked((GameObject) null, callback);
    }

    public void ExplodeCracked()
    {
      this.ExplodeCracked((GameObject) null, (ExploderObject.OnExplosion) null);
    }

    public int ProcessingFrames
    {
      get
      {
        return this.core.processingFrames;
      }
    }

    private void Awake()
    {
      this.core = Singleton<Core>.Instance;
      this.core.Initialize(this);
    }

    private void OnDrawGizmos()
    {
      if (!((Behaviour) this).get_enabled() || this.ExplodeSelf && this.DisableRadiusScan)
        return;
      Gizmos.set_color(Color.get_red());
      if (this.UseCubeRadius)
      {
        Vector3 centroid = ExploderUtils.GetCentroid(((Component) this).get_gameObject());
        Gizmos.set_matrix(((Component) this).get_transform().get_localToWorldMatrix());
        Gizmos.DrawWireCube(((Component) this).get_transform().InverseTransformPoint(centroid), this.CubeRadius);
      }
      else
        Gizmos.DrawWireSphere(ExploderUtils.GetCentroid(((Component) this).get_gameObject()), this.Radius);
    }

    private bool IsExplodable(GameObject obj)
    {
      return this.core.parameters.DontUseTag ? Object.op_Inequality((Object) obj.GetComponent<Explodable>(), (Object) null) : obj.CompareTag(ExploderObject.Tag);
    }

    public enum ThreadOptions
    {
      WorkerThread1x,
      WorkerThread2x,
      WorkerThread3x,
      Disabled,
    }

    public enum CuttingStyleOption
    {
      Random,
      RectangularRandom,
      RectangularRegular,
    }

    public delegate void OnExplosion(float timeMS, ExploderObject.ExplosionState state);

    public enum ExplosionState
    {
      ExplosionStarted,
      ExplosionFinished,
      ObjectCracked,
    }
  }
}

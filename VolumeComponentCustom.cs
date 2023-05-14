#region Assembly Unity.RenderPipelines.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// D:\[GAME DEV COURSE]\UNITY PROJECT LATEST\Emesal_Dark_Corners_of_the_Earth_v0.1\Library\ScriptAssemblies\Unity.RenderPipelines.Core.Runtime.dll
#endregion

using System;
using System.Collections.ObjectModel;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEngine.Rendering
{
    public class VolumeComponentCustom : ScriptableObject
    {
        public bool active;
        
        //public VolumeComponentCustom();

        public string displayName { get; protected set; }
        public ReadOnlyCollection<VolumeParameter> parameters { get; set; }

        public static implicit operator VolumeComponentCustom(Vignette v)
        {
            throw new NotImplementedException();
        }
        /*
public override int GetHashCode();
public virtual void Override(VolumeComponent state, float interpFactor);
public void Release();
public void SetAllOverridesTo(bool state);
protected virtual void OnDestroy();
protected virtual void OnDisable();
protected virtual void OnEnable();

public sealed class Indent : PropertyAttribute
{
   public readonly int relativeAmount;

   public Indent(int relativeAmount = 1);
}
*/
    }
}
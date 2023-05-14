using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[ExecuteInEditMode]// Make water live-update even when not in play mode
public class WaterScript : MonoBehaviour
{
    public enum WaterMode
    {
        Simple = 0,
        Reflective = 1,
        Refractive = 2,
    };

    public WaterMode _WaterMode = WaterMode.Refractive;
    public bool _DisablePixelLights = true;
    public int _TextureSize = 256;
    public float _ClipPlaneOffset = 0.07f;
    public LayerMask _ReflectLayers = -1;
    public LayerMask _RefractLayers = -1;


    private Dictionary<Camera, Camera> _ReflectionCameras = new Dictionary<Camera, Camera>(); // Camera -> Camera table
    private Dictionary<Camera, Camera> _RefractionCameras = new Dictionary<Camera, Camera>(); // Camera -> Camera table
    private RenderTexture _ReflectionTexture;
    private RenderTexture _RefractionTexture;
    private WaterMode _HardwareWaterSupport = WaterMode.Refractive;
    private int _OldReflectionTextureSize;
    private int _OldRefractionTextureSize;
    private static bool _Underwater;



    public Material mat;
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private Material noSkybox;
    public GameObject PlayerCamera;

    


    void Start()
    {
        RenderSettings.fog = false;
        
    }





    // This is called when it's known that the object will be rendered by some
    // camera. We render reflections / refractions and do other updates here.
    // Because the script executes in edit mode, reflections for the scene view
    // camera will just work!
    public void OnWillRenderObject()
    {
        if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial ||
            !GetComponent<Renderer>().enabled)
        {
            return;
        }

        Camera cam = Camera.current;
        if (!cam)
        {
            return;
        }

        // Safeguard from recursive water reflections.
        if (_Underwater)
        {
            return;
        }
        _Underwater = true;

        // Actual water rendering mode depends on both the current setting AND
        // the hardware support. There's no point in rendering refraction textures
        // if they won't be visible in the end.
        _HardwareWaterSupport = FindHardwareWaterSupport();
        WaterMode mode = GetWaterMode();

        Camera _ReflectionCamera, _RefractionCamera;
        CreateWaterObjects(cam, out _ReflectionCamera, out _RefractionCamera);

        // find out the reflection plane: position and normal in world space
        Vector3 pos = transform.position;
        Vector3 normal = transform.up;

        // Optionally disable pixel lights for reflection/refraction
        int oldPixelLightCount = QualitySettings.pixelLightCount;
        if (_DisablePixelLights)
        {
            QualitySettings.pixelLightCount = 0;
        }

        UpdateCameraModes(cam, _ReflectionCamera);
        UpdateCameraModes(cam, _RefractionCamera);

        // Render reflection if needed
        if (mode >= WaterMode.Reflective)
        {
            // Reflect camera around reflection plane
            float d = -Vector3.Dot(normal, pos) - _ClipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

            Matrix4x4 reflection = Matrix4x4.zero;
            CalculateReflectionMatrix(ref reflection, reflectionPlane);
            Vector3 oldpos = cam.transform.position;
            Vector3 newpos = reflection.MultiplyPoint(oldpos);
            _ReflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;

            // Setup oblique projection matrix so that near plane is our reflection
            // plane. This way we clip everything below/above it for free.
            Vector4 clipPlane = CameraSpacePlane(_ReflectionCamera, pos, normal, 1.0f);
            _ReflectionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);

            // Set custom culling matrix from the current camera
            _ReflectionCamera.cullingMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;

            _ReflectionCamera.cullingMask = ~(1 << 4) & _ReflectLayers.value; // never render water layer
            _ReflectionCamera.targetTexture = _ReflectionTexture;
            bool oldCulling = GL.invertCulling;
            GL.invertCulling = !oldCulling;
            _ReflectionCamera.transform.position = newpos;
            Vector3 euler = cam.transform.eulerAngles;
            _ReflectionCamera.transform.eulerAngles = new Vector3(-euler.x, euler.y, euler.z);
            _ReflectionCamera.Render();
            _ReflectionCamera.transform.position = oldpos;
            GL.invertCulling = oldCulling;
            GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", _ReflectionTexture);
        }

        // Render refraction
        if (mode >= WaterMode.Refractive)
        {
            _RefractionCamera.worldToCameraMatrix = cam.worldToCameraMatrix;

            // Setup oblique projection matrix so that near plane is our reflection
            // plane. This way we clip everything below/above it for free.

            Vector4 clipPlane = CameraSpacePlane(_RefractionCamera, pos, normal, -1.0f);  //-1.0f
            _RefractionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);

            // Set custom culling matrix from the current camera
            _RefractionCamera.cullingMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;

            _RefractionCamera.cullingMask = ~(1 << 4) & _RefractLayers.value; // never render water layer
            _RefractionCamera.targetTexture = _RefractionTexture;
            _RefractionCamera.transform.position = cam.transform.position;
            _RefractionCamera.transform.rotation = cam.transform.rotation;
            _RefractionCamera.Render();
            GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", _RefractionTexture);
        }

        // Restore pixel light count
        if (_DisablePixelLights)
        {
            QualitySettings.pixelLightCount = oldPixelLightCount;
        }

        // Setup shader keywords based on water mode
        switch (mode)
        {
            case WaterMode.Simple:
                Shader.EnableKeyword("WATER_SIMPLE");
                Shader.DisableKeyword("WATER_REFLECTIVE");
                Shader.DisableKeyword("WATER_REFRACTIVE");
                break;
            case WaterMode.Reflective:
                Shader.DisableKeyword("WATER_SIMPLE");
                Shader.EnableKeyword("WATER_REFLECTIVE");
                Shader.DisableKeyword("WATER_REFRACTIVE");
                break;
            case WaterMode.Refractive:
                Shader.DisableKeyword("WATER_SIMPLE");
                Shader.DisableKeyword("WATER_REFLECTIVE");
                Shader.EnableKeyword("WATER_REFRACTIVE");
                break;
        }

        _Underwater = false;
    }


    // Cleanup all the objects we possibly have created
    void OnDisable()
    {
        if (_ReflectionTexture)
        {
            DestroyImmediate(_ReflectionTexture);
            _ReflectionTexture = null;
        }
        if (_RefractionTexture)
        {
            DestroyImmediate(_RefractionTexture);
            _RefractionTexture = null;
        }
        foreach (var kvp in _ReflectionCameras)
        {
            DestroyImmediate((kvp.Value).gameObject);
        }
        _ReflectionCameras.Clear();
        foreach (var kvp in _RefractionCameras)
        {
            DestroyImmediate((kvp.Value).gameObject);
        }
        _RefractionCameras.Clear();
    }


    // This just sets up some matrices in the material; for really
    // old cards to make water texture scroll.
    void Update()
    {
        if (!GetComponent<Renderer>())
        {

            return;
        }
        Material mat = GetComponent<Renderer>().sharedMaterial;
        if (!mat)
        {
            return;
        }

        //Vector2 _WaveSpeed = mat.GetVector("WaveSpeed");
        //var _WaveScale = mat.GetFloat("WaveScale");
       

        

        //Set the background color

        //GameObject lake1 = gameObject;
        //OnTriggerExit(lake1.GetComponent<BoxCollider>());
        //OnTriggerEnter(lake1.GetComponent<BoxCollider>());





    }

    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerControllerScript>())
        {
            PlayerControllerScript pcs = col.gameObject.GetComponent<PlayerControllerScript>();
            pcs._WalkSpeed /=2;
            //run
            //pcs._JumpSpeed /=3;
            pcs._StaminaDrainRate /= 2;
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0.2f;
            RenderSettings.fogColor = Color32.Lerp(Color.black, Color.green, 0.12f);
            pcs.m_Camera.fieldOfView = 50;
            pcs._GravityForce = 0.1f;
            
            
            

        }

        




        {
            

            //RenderSettings.fog = true;
            //RenderSettings.fogColor = PlayerCamera.GetComponent<Camera>().backgroundColor;
            //RenderSettings.fogDensity = 0.3f;
            //RenderSettings.skybox = defaultSkybox;

            //PostProcessProfile ppprofile = Main
            //PlayerCamera.GetComponent<Camera>().farClipPlane = 30;


            //adjust player prefs

        }




    }

    void OnTriggerExit(Collider col)
    {
        PlayerControllerScript pcs = col.gameObject.GetComponent<PlayerControllerScript>();
        pcs._WalkSpeed = 2;

        RenderSettings.fog = false;



        {


            //RenderSettings.fog = true;
            //RenderSettings.fogColor = PlayerCamera.GetComponent<Camera>().backgroundColor;
            //RenderSettings.fogDensity = 0.3f;
            //RenderSettings.skybox = defaultSkybox;

            //PostProcessProfile ppprofile = Main
            //PlayerCamera.GetComponent<Camera>().farClipPlane = 30;


            //adjust player prefs

            /*
            GameObject Player = GameObject.FindGameObjectWithTag("Player");

            if (col.CompareTag("MainCamera"))
            {
                if (Player.transform.position.y < 7.9f)
                {

                }
                PlayerCamera.GetComponent<Camera>().backgroundColor = new Color(0, 0.1f, 0.1f, 0.1f);
                RenderSettings.fog = defaultFog;
                RenderSettings.fogColor = defaultFogColor;
                RenderSettings.fogDensity = defaultFogDensity;
                PlayerCamera.GetComponent<Camera>().farClipPlane = 1000;
            }*/
        }




    }


    void UpdateCameraModes(Camera src, Camera dest)
    {
        if (dest == null)
        {
            return;
        }
        // set water camera to clear the same way as current camera
        dest.clearFlags = src.clearFlags;
        dest.backgroundColor = src.backgroundColor;
        if (src.clearFlags == CameraClearFlags.Skybox)
        {
            Skybox sky = src.GetComponent<Skybox>();
            Skybox mysky = dest.GetComponent<Skybox>();
            if (!sky || !sky.material)
            {
                mysky.enabled = false;
            }
            else
            {
                mysky.enabled = true;
                mysky.material = sky.material;
            }
        }
        // update other values to match current camera.
        // even if we are supplying custom camera&projection matrices,
        // some of values are used elsewhere (e.g. skybox uses far plane)
        dest.farClipPlane = src.farClipPlane;
        dest.nearClipPlane = src.nearClipPlane;
        dest.orthographic = src.orthographic;
        dest.fieldOfView = src.fieldOfView;
        dest.aspect = src.aspect;
        dest.orthographicSize = src.orthographicSize;


    }


    // On-demand create any objects we need for water
    void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
    {
        WaterMode mode = GetWaterMode();

        reflectionCamera = null;
        refractionCamera = null;

        if (mode >= WaterMode.Reflective)
        {
            // Reflection render texture
            if (!_ReflectionTexture || _OldReflectionTextureSize != _TextureSize)
            {
                if (_ReflectionTexture)
                {
                    DestroyImmediate(_ReflectionTexture);
                }
                _ReflectionTexture = new RenderTexture(_TextureSize, _TextureSize, 16);
                _ReflectionTexture.name = "__WaterReflection" + GetInstanceID();
                _ReflectionTexture.isPowerOfTwo = true;
                _ReflectionTexture.hideFlags = HideFlags.DontSave;
                _OldReflectionTextureSize = _TextureSize;
            }

            // Camera for reflection
            _ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
            if (!reflectionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
            {
                GameObject go = new GameObject("Water Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
                reflectionCamera = go.GetComponent<Camera>();
                reflectionCamera.enabled = false;
                reflectionCamera.transform.position = transform.position;
                reflectionCamera.transform.rotation = transform.rotation;
                reflectionCamera.gameObject.AddComponent<FlareLayer>();
                go.hideFlags = HideFlags.HideAndDontSave;
                _ReflectionCameras[currentCamera] = reflectionCamera;
            }
        }

        if (mode >= WaterMode.Refractive)
        {
            // Refraction render texture
            if (!_RefractionTexture || _OldRefractionTextureSize != _TextureSize)
            {
                if (_RefractionTexture)
                {
                    DestroyImmediate(_RefractionTexture);
                }
                _RefractionTexture = new RenderTexture(_TextureSize, _TextureSize, 16);
                _RefractionTexture.name = "__WaterRefraction" + GetInstanceID();
                _RefractionTexture.isPowerOfTwo = true;
                _RefractionTexture.hideFlags = HideFlags.DontSave;
                _OldRefractionTextureSize = _TextureSize;
            }

            // Camera for refraction
            _RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
            if (!refractionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
            {
                GameObject go =
                    new GameObject("Water Refr Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(),
                        typeof(Camera), typeof(Skybox));
                refractionCamera = go.GetComponent<Camera>();
                refractionCamera.enabled = false;
                refractionCamera.transform.position = transform.position;
                refractionCamera.transform.rotation = transform.rotation;
                refractionCamera.gameObject.AddComponent<FlareLayer>();
                go.hideFlags = HideFlags.HideAndDontSave;
                _RefractionCameras[currentCamera] = refractionCamera;
            }
        }
    }

    WaterMode GetWaterMode()
    {
        if (_HardwareWaterSupport < _WaterMode)
        {
            return _HardwareWaterSupport;
        }
        return _WaterMode;
    }

    WaterMode FindHardwareWaterSupport()
    {
        if (!GetComponent<Renderer>())
        {
            return WaterMode.Simple;
        }

        Material mat = GetComponent<Renderer>().sharedMaterial;
        if (!mat)
        {
            return WaterMode.Simple;
        }

        string mode = mat.GetTag("WATERMODE", false);
        if (mode == "Refractive")
        {
            return WaterMode.Refractive;
        }
        if (mode == "Reflective")
        {
            return WaterMode.Reflective;
        }

        return WaterMode.Simple;
    }

    // Given position/normal of the plane, calculates plane in camera space.
    Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Vector3 offsetPos = pos + normal * _ClipPlaneOffset;
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint(offsetPos);
        Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
    }

    // Calculates reflection matrix around the given plane
    static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }


}


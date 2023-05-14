using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    // Start is called before the first frame update
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform TestItemPrefab;

    public Sprite StaffIcon;
    public Sprite MetalBarIcon;
    public Sprite RockIcon;
}

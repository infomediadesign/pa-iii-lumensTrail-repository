using System.Numerics;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D normalCursorTex;
    [SerializeField] private Texture2D disabledCursorTex;

    UnityEngine.Vector2 hotSpot;
    private static CursorManager instance;

    void Start()
    {
        if (instance == null) instance = this;
        hotSpot = new UnityEngine.Vector2(normalCursorTex.width / 2, normalCursorTex.height / 2);
        Cursor.SetCursor(normalCursorTex, hotSpot, CursorMode.Auto);
    }

    public static void SetNormalCursor() 
    {
        Cursor.SetCursor(instance.normalCursorTex, instance.hotSpot, CursorMode.Auto);
    }

    public static void SetDisabledCursor() 
    {
        Cursor.SetCursor(instance.disabledCursorTex, instance.hotSpot, CursorMode.Auto);
    }
}

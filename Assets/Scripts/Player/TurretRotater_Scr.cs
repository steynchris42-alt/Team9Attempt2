using UnityEngine;

public class TurretRotater_Scr : MonoBehaviour
{
    public Transform TurretRotator_bone;
    public Transform Cursor;

    void Update()
    {
        TurretRotator_bone.LookAt(Cursor.position);
    }
}

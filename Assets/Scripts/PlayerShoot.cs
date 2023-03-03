using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Aucune caméra associé au système de tir.");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (PlayerUI.isPaused)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log("On a touché " + hit.collider.name);

            if (hit.collider.tag == "Player")
            {

            }
        }
    }
}

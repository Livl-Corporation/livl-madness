using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RaycastScanner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    
    [Header("Laser")]
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private LineRenderer laserLine;
    
    [Header("Scanner")]
    [SerializeField] private float gunRange = 50f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float laserDuration = 0.05f;
    
    private float fireTimer;

    void Update()
    {
        if (!isLocalPlayer) return;

        fireTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate)
        {
            fireTimer = 0;
            CmdShootLaser();
        }
    }

    [Command]
    void CmdShootLaser()
    {
        RpcShowLaser();
        StartCoroutine(ShootLaser());
    }

    [ClientRpc]
    void RpcShowLaser()
    {
        laserLine.SetPosition(0, laserOrigin.position);
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
        }
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
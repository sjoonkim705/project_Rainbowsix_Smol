using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float RotationSpeed = 5f;


    public Camera cam; // 게임의 메인 카메라

    void Update()
    {
        RotateTowardsMouseCursor();
    }

    void RotateTowardsMouseCursor()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position); // 플레이어 위치에서 수평면 생성
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); // 마우스 위치로부터 Ray 생성
        float hitdist = 0.0f;

        // Ray가 playerPlane과 교차하는 지점 계산
        if (playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist); // 교차 지점의 좌표
            Debug.DrawRay(transform.position, targetPoint);

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position); // 회전해야 할 방향
            targetRotation.y = 0; // X축 회전은 0으로 고정 (상체만 회전)
            targetRotation.z = 0; // Z축 회전도 0으로 고정
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime); // 부드럽게 회전
        }
    }

}

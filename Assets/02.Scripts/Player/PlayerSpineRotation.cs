using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerSpineRotation : MonoBehaviour
{
    public float rotationSpeed = 5f; // 회전 속도
    public GameObject Torso; // 상체를 나타내는 게임 오브젝트

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y - transform.position.y;

        // 마우스 커서 위치를 월드 좌표로 변환
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = targetPosition - transform.position;


        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Torso.transform.localRotation = Quaternion.Slerp(Torso.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


}

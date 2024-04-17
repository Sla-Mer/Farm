using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(24);
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawnPosition = transform.position;

        // ���������� ������ ����������
        float radius = 1.5f;

        // ���������� ��������� ���� � ��������
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // ��������� ��������� ����� �� ���������� �����
        Vector2 randomOnCircle = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;

        // ������� ������� � ��������� ��� � ��������� ������� �� ���������� �����
        Collectable droppedItem = Instantiate(item, spawnPosition + randomOnCircle, Quaternion.identity);

        // ��� ������� ����� �������� ���� � ��������
        droppedItem.rb2d.AddForce(randomOnCircle.normalized * 0.5f, ForceMode2D.Impulse);
    }
}

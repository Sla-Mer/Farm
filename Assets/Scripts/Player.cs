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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Vector3Int pos = new Vector3Int((int)transform.position.x,
                (int)transform.position.y,
                0);
            if(GameManager.instance.tileManager.IsInteractable(pos))
            {
                GameManager.instance.tileManager.SetInteracted(pos);
            }
        }
    }
    public void DropItem(Item item)
    {
        Vector2 spawnPosition = transform.position;

        // Определяем радиус окружности
        float radius = 1.5f;

        // Генерируем случайный угол в радианах
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // Вычисляем случайную точку на окружности круга
        Vector2 randomOnCircle = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;

        // Создаем предмет и добавляем его к случайной позиции на окружности круга
        Item droppedItem = Instantiate(item, spawnPosition + randomOnCircle, Quaternion.identity);

        // При желании можно добавить силу к предмету
        droppedItem.rb2d.AddForce(randomOnCircle.normalized * 0.5f, ForceMode2D.Impulse);
    }
}

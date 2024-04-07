using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 20; // Количество патронов, которое добавляется при подборе
    public shoot playerShoot; // Ссылка на скрипт стрельбы игрока

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Если игрок подходит к объекту
        {
            if (playerShoot != null) // Если у игрока есть скрипт стрельбы
            {
                playerShoot.totalAmmo += ammoAmount; // Добавляем патроны к общему количеству патронов игрока
                Destroy(gameObject); // Уничтожаем объект с патронами после подбора
            }
        }
    }
}

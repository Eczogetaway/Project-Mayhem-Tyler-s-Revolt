using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f; // Здоровье цели

    public void TakeDamage(float amount)
    {
        health -= amount; // Уменьшаем здоровье цели на величину урона

        if (health <= 0f) // Если здоровье цели стало меньше или равно нулю
        {
            Die(); // Цель умирает
        }
    }

    void Die()
    {
        Destroy(gameObject); // Уничтожаем объект
    }
}

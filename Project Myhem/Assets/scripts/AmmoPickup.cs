using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 20; // ���������� ��������, ������� ����������� ��� �������
    public shoot playerShoot; // ������ �� ������ �������� ������

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // ���� ����� �������� � �������
        {
            if (playerShoot != null) // ���� � ������ ���� ������ ��������
            {
                playerShoot.totalAmmo += ammoAmount; // ��������� ������� � ������ ���������� �������� ������
                Destroy(gameObject); // ���������� ������ � ��������� ����� �������
            }
        }
    }
}

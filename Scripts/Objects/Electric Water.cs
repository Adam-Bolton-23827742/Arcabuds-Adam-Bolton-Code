using System.Collections;
using UnityEngine;

public class ElectricWater : MonoBehaviour
{
    public float SinkAmount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(100);
            StartCoroutine(Drown(collision.transform));
        }

        else if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<ZappyStateMachine>() == null)
        {
            collision.gameObject.GetComponent<EnemyStats>().TakeDamage(100, null, null);
        }
    }

    public IEnumerator Drown(Transform Char)
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Char.position = new Vector3(Char.position.x, Char.position.y - SinkAmount, Char.position.z);
        }
    }
}

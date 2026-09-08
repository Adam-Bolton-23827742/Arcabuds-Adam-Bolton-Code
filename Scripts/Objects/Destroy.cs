using UnityEngine;

public class Destroy : MonoBehaviour
{
    private AddCharacter AddCharacter;
    [SerializeField] private GameObject ZappyPlayerPrefab;

    private void Start()
    {
        AddCharacter = GetComponent<AddCharacter>();
    }

    public void DestroyObject()
    {
        if (transform.root.gameObject != null)
        {
            if (AddCharacter != null)
            {
                AddCharacter.Add(ZappyPlayerPrefab, transform.position, 2, "Zappy Profile", "Zappy Moves", "Zappy Attack", "Zappy Special");
            }
            
            Destroy(transform.root.gameObject);
        }
    }
}

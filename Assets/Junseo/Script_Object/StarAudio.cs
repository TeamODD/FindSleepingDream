using UnityEngine;

public class StarAudio : MonoBehaviour
{
    public AudioSource audiosource;
    private bool canCollect = false; // ���� ��Ҵ��� ���� ����

    void Update()
    {
        if (canCollect && Input.GetKeyDown(KeyCode.C))
        {
            if (!audiosource.isPlaying)
                audiosource.Play();

           
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("item"))
        {
            canCollect = true; // ���� CŰ �Է��� ���� �� ����
        }
    }

    
}
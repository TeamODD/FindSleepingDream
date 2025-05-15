using UnityEngine;

public class StarAudio : MonoBehaviour
{
    public AudioSource audiosource;
    private bool canCollect = false; // 별에 닿았는지 여부 저장

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
            canCollect = true; // 이제 C키 입력을 받을 수 있음
        }
    }

    
}
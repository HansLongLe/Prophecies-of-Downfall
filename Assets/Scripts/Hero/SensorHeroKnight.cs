using UnityEngine;

public class SensorHeroKnight : MonoBehaviour
{

    private int mColCount;

    private float mDisableTimer;

    private void OnEnable()
    {
        mColCount = 0;
    }

    public bool State()
    {
        if (mDisableTimer > 0)
            return false;
        return mColCount > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            mColCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            mColCount--;
        }
    }

    private void Update()
    {
        mDisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        mDisableTimer = duration;
    }
}

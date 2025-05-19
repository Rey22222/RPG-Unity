using UnityEngine;
using Newtonsoft.Json;

public class PlayerStats
{
    public float MaxHP { get; set; }
    public float CurrentHP { get; set; }

    public float MaxMP { get; set; }
    public float CurrentMP { get; set; }

    public float PlayerSpeed { get; set; }
    public float SprintSpeed { get; set; }

    public PlayerStats() { }
    [System.Serializable]
    public class SerializableVector3 
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3() { }

        public SerializableVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    public SerializableVector3 Position { get;  set; }

    public PlayerStats(float maxHP, float maxMP, float playerSpeed, float sprintSpeed, Vector3 startPosition)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;

        MaxMP = maxMP;
        CurrentMP = maxMP;

        PlayerSpeed = playerSpeed;
        SprintSpeed = sprintSpeed;

        Position = new SerializableVector3(startPosition);
    }

    public void SetPosition(Vector3 position)
    {
        Position = new SerializableVector3(position);
    }

    public Vector3 GetPosition()
    {
        if (Position == null)
            return Vector3.zero;
        return Position.ToVector3();
    }


    public void TakeDamage(float damage)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - damage);
    }

    public void UseMana(float cost)
    {
        CurrentMP = Mathf.Max(0, CurrentMP - cost);
    }

    public void RestoreHP(float value)
    {
        CurrentHP = Mathf.Min(MaxHP, CurrentHP + value);
    }

    public void SetCurrentHP(float hp)
    {
        CurrentHP = Mathf.Clamp(hp, 0, MaxHP);
    }
}

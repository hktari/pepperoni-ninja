using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public List<Collider2D> AllColliders = new List<Collider2D>();
    public Collider2D FirstCollider => AllColliders.FirstOrDefault();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AllColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AllColliders.Remove(collision);
    }
}

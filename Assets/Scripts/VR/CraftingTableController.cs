using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableController : MonoBehaviour
{
    public GameObject DiamondPickaxePrefab;
    public AudioClip CraftSound;
    public GameObject CraftParticleSystem;
    public Transform SpawnPoint;

    private AudioSource AudioSource;

    private enum Items
    {
        Diamond,
        Stick
    }

    private Dictionary<string, Items> NameToItems = new Dictionary<string, Items>
    {
        {"diamond", Items.Diamond},
        {"stick", Items.Stick}
    };

    private Dictionary<Items, GameObject> ItemsToGameobject = new Dictionary<Items, GameObject>();

    private int[] CurrentItems = new int[2];
    private List<GameObject>[] CurrentItemsGO = new List<GameObject>[2];
    private bool IsCover = false;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < CurrentItemsGO.Length; ++i)
        {
            CurrentItemsGO[i] = new List<GameObject>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NameToItems.TryGetValue(other.name, out Items item))
        {
            CurrentItems[(int)item] += 1;
            CurrentItemsGO[(int)item].Add(other.gameObject);
            if (IsCover) Craft();
        }
        if (other.name == "tapaCraftingtable")
        {
            IsCover = true;
            Craft();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (NameToItems.TryGetValue(other.name, out Items item))
        {
            CurrentItems[(int)item] -= 1;
            List<GameObject> list = CurrentItemsGO[(int)item];
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == other.gameObject)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }
        if (other.name == "tapaCraftingtable")
        {
            IsCover = false;
        }
    }

    private void Craft()
    {
        if (CurrentItems[(int)Items.Diamond] >= 3 &&
            CurrentItems[(int)Items.Stick] >= 2)
        {
            AudioSource.PlayOneShot(CraftSound, 1.0f);
            Instantiate(CraftParticleSystem, SpawnPoint.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
            Instantiate(DiamondPickaxePrefab, SpawnPoint.transform.position, Quaternion.identity);

            CurrentItems[(int)Items.Diamond] -= 3;
            CurrentItems[(int)Items.Stick] -= 2;

            List<GameObject> list = CurrentItemsGO[(int)Items.Diamond];
            for (int i = list.Count - 3; i < list.Count; ++i)
            {
                Destroy(list[i].transform.parent.gameObject);
                list.RemoveAt(i--);
            }

            list = CurrentItemsGO[(int)Items.Stick];
            for (int i = list.Count - 2; i < list.Count; ++i)
            {
                Destroy(list[i].transform.parent.parent.gameObject);
                list.RemoveAt(i--);
            }
        }
    }
}

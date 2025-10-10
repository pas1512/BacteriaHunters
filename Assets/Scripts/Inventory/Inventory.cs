using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const string INVENTORY_KEY = "Inventory";

    [SerializeField] private List<ItemType> _types;
    [SerializeField] private List<int> _numbers;

    private static Inventory _instance;

    [Serializable]
    private struct Data
    {
        [SerializeField] private int[] _ids;
        public int[] ids => _ids;

        [SerializeField] private int[] _numbers;
        public int[] numbers => _numbers;

        public Data(List<ItemType> types, List<int> numbers)
        {
            _ids = types.Select(t => t.index).ToArray();
            _numbers = numbers.ToArray();
        }

        public List<ItemType> GetTypesList()
        {
            int[] ids = _ids;
            List<ItemType> types = new List<ItemType>(_ids.Length);
            ItemType[] allTypes = ItemType.GetAll();
            Dictionary<int, ItemType> dictionary = allTypes.ToDictionary(t => t.index, t => t);

            for (int i = 0; i < _ids.Length; i++)
                types.Add(dictionary[ids[i]]);

            return types;
        }

        public List<int> GetNumbersList() => new List<int>(_numbers);
    }

    private void Start()
    {
        _instance = this;
        Load();
    }

    public (T, int)[] GetAll<T>() where T: ItemType
    {
        List<(T, int)> result = new List<(T, int)>();

        for (int i = 0; i < _types.Count; i++)
        {
            if (_numbers[i] > 0 && _types[i] is T type)
                result.Add((type, _numbers[i]));
        }

        return result.ToArray();
    }

    public int GetCount(ItemType type)
    {
        int id = _types.IndexOf(type);

        if (id == -1)
            return 0;

        return _numbers[id];
    }

    public void Add(DropedItem dropedItem)
    {
        ItemType itemType = dropedItem.type;
        int itemNumber = dropedItem.number;

        if(_types == null)
        {
            _types = new List<ItemType>();
            _numbers = new List<int>();
        }

        if (_types.Contains(itemType))
        {
            int id = _types.IndexOf(itemType);
            _numbers[id] += itemNumber; 
        }
        else
        {
            _types.Add(itemType);
            _numbers.Add(itemNumber);
        }
    }

    public void Set(ItemType type, int number)
    {
        int id = _types.IndexOf(type);

        if (id >= 0)
        {
            _numbers[id] = number;
        }
        else
        {
            _types.Add(type);
            _numbers.Add(number);
        }
    }

    public void Change(ItemType type, int number)
    {
        int id = _types.IndexOf(type);

        if (id >= 0)
        {
            _numbers[id] += number;

            if (_numbers[id] < 0)
                _numbers[id] = 0;
        }
        else
        {
            _types.Add(type);
            _numbers.Add(number);
        }
    }
    

    public static void SaveStat() => _instance.Save();
    public void Save()
    {
        print("Saved");
        Data data = new Data(_types, _numbers);
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(INVENTORY_KEY, jsonData);
    }

    public static void LoadStat() => _instance.Load();
    public void Load()
    {
        if(PlayerPrefs.HasKey(INVENTORY_KEY))
        {
            print("Loaded");
            string jsonData = PlayerPrefs.GetString(INVENTORY_KEY);
            Data data = JsonUtility.FromJson<Data>(jsonData);
            _types = data.GetTypesList();
            _numbers = data.GetNumbersList();
        }
    }
}
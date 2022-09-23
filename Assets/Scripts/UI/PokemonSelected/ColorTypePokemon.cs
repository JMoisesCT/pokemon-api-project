using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTypePokemon : MonoBehaviour
{
    [Serializable]
    public class TypeAndColor
    {
        public string typeName;
        public Color typeColor;
    }

    [SerializeField] private TypeAndColor[] _typeAndColorList;
    private Dictionary<string, Color> _dictionaryTypeColors;

    private void Awake()
    {
        _dictionaryTypeColors = new Dictionary<string, Color>();
        for (int i = 0; i < _typeAndColorList.Length; ++i)
        {
            _dictionaryTypeColors.Add(_typeAndColorList[i].typeName, _typeAndColorList[i].typeColor);
        }
    }

    public Color GetColorType(string type)
    {
        if (!_dictionaryTypeColors.ContainsKey(type))
        {
            return Color.black;
        }
        return _dictionaryTypeColors[type];
    }
}

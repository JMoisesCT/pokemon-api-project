using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectablePokemon : MonoBehaviour
{
    [SerializeField] private string _name;

    [Header("UI")]
    [SerializeField] private Image _image;
    [SerializeField] private Text _textName;
    [SerializeField] private Text _textNumber;

    public void Initialize(string name)
    {
        _name = name;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectablePokemon : MonoBehaviour
{
    [SerializeField] private string _name;

    [Header("UI")]
    [SerializeField] private RawImage _image;
    [SerializeField] private Text _textName;
    [SerializeField] private Text _textNumber;
    [SerializeField] private Button _button;
    private DataManager.PokemonDetail _pokemonDetail;

    [Header("Sender Events")]
    [SerializeField] private PokemonDetailEventChannelSO _eventLoadPokemonDetail;
    [Header("Listener Events")]
    [SerializeField] private PokemonDetailEventChannelSO _eventCreateAndSavePokemonDetail;

    private void Awake()
    {
        _button.onClick.AddListener(OpenPokemonSelectedScreen);
        _eventCreateAndSavePokemonDetail.OnEventRaised += LoadAndSaveDetail;
    }

    private void OpenPokemonSelectedScreen()
    {
        _eventLoadPokemonDetail.RaiseEvent(_pokemonDetail);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OpenPokemonSelectedScreen);
        _eventCreateAndSavePokemonDetail.OnEventRaised -= LoadAndSaveDetail;
    }

    public void Initialize(string pokemonName)
    {
        _name = pokemonName;
    }

    public void LoadAndSaveDetail(DataManager.PokemonDetail pokemonDetail)
    {
        if (pokemonDetail.name != _name)
        {
            return;
        }
        _pokemonDetail = pokemonDetail;

        _image.texture = _pokemonDetail.imageDefault;
        _textName.text = _name;
        _textNumber.text = pokemonDetail.id.GetPokemonFormat();
    }
}

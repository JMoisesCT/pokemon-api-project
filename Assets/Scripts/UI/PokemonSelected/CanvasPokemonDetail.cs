using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPokemonDetail : MonoBehaviour
{
    [SerializeField] private RawImage _imagePokemon;
    [SerializeField] private Text _textPokemonNumber;
    [SerializeField] private Text _textPokemonName;
    [SerializeField] private Text _textPokemonType;
    [SerializeField] private Text _textPokemonHeight;
    [SerializeField] private Text _textPokemonWeight;
    [SerializeField] private Button _buttonClose;

    [Header("Listener Events")]
    [SerializeField] private PokemonDetailEventChannelSO _eventLoadPokemonDetail;
    
    private void Awake()
    {
        _buttonClose.onClick.AddListener(CloseScreen);
        _eventLoadPokemonDetail.OnEventRaised += OpenScreen;
    }

    private void Start()
    {
        // After the event was saved we close this screen.
        CloseScreen();
    }

    private void OnDestroy()
    {
        _buttonClose.onClick.RemoveListener(CloseScreen);
        _eventLoadPokemonDetail.OnEventRaised -= OpenScreen;
    }

    private void CloseScreen()
    {
        gameObject.SetActive(false);
    }

    private void OpenScreen(DataManager.PokemonDetail pokemonDetail)
    {
        LoadPokemonDetail(pokemonDetail);
        gameObject.SetActive(true);
    }

    private void LoadPokemonDetail(DataManager.PokemonDetail pokemonDetail)
    {
        _imagePokemon.texture = pokemonDetail.imageDefault;
        _textPokemonNumber.text = pokemonDetail.id.GetPokemonFormat();
        _textPokemonName.text = pokemonDetail.name;
        _textPokemonType.text = pokemonDetail.types[0];
        _textPokemonHeight.text = pokemonDetail.height;
        _textPokemonWeight.text = pokemonDetail.weight;
    }
}

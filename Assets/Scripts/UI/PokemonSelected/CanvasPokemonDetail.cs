using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPokemonDetail : MonoBehaviour
{
    [Serializable]
    public class PokemonTypeContainer
    {
        public Image imageContainer;
        public Text textType;
    }

    [SerializeField] private RawImage _imagePokemon;
    [SerializeField] private Text _textPokemonNumber;
    [SerializeField] private Text _textPokemonName;
    [SerializeField] private PokemonTypeContainer[] _pokemonTypeList;
    [SerializeField] private Text _textPokemonHeight;
    [SerializeField] private Text _textPokemonWeight;
    [SerializeField] private Button _buttonClose;

    [Header("Listener Events")]
    [SerializeField] private PokemonDetailEventChannelSO _eventLoadPokemonDetail;

    private ColorTypePokemon _colorTypePokemon;
    
    private void Awake()
    {
        _buttonClose.onClick.AddListener(CloseScreen);
        _colorTypePokemon = GetComponent<ColorTypePokemon>();
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
        LoadTypes(pokemonDetail);
        _textPokemonHeight.text = pokemonDetail.height;
        _textPokemonWeight.text = pokemonDetail.weight;
    }

    private void LoadTypes(DataManager.PokemonDetail pokemonDetail)
    {
        for (int i = 0; i < _pokemonTypeList.Length; ++i)
        {
            _pokemonTypeList[i].imageContainer.gameObject.SetActive(false);
        }
        for (int i = 0; i < pokemonDetail.types.Length; ++i)
        {
            string type = pokemonDetail.types[i];
            _pokemonTypeList[i].imageContainer.gameObject.SetActive(true);
            _pokemonTypeList[i].imageContainer.color = _colorTypePokemon.GetColorType(type);
            _pokemonTypeList[i].textType.text = type;
        }
    }
}

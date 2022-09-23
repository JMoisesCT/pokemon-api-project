using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    // For Pokemon API.
    [Serializable]
    public class PokemonDetail
    {
        public string name;
        public int id;
        public string height;
        public string weight;
        public Texture imageDefault;
        public string[] types;
        public int base_hp;
        public int base_attack;
        public int base_defense;
        public int base_speed;
    }

    [Serializable]
    public class PokemonStats
    {
        public int base_stat;
        public PokemonStatDetail stat;
    }

    [Serializable]
    public class PokemonStatDetail
    {
        public int name;
    }

    [Serializable]
    public class PokemonTypes
    {
        public PokemonTypeDetail type;
    }

    [Serializable]
    public class PokemonTypeDetail
    {
        public string name;
        public string url;
    }

    [Serializable]
    public class PokemonDetailDB
    {
        public string name;
        public int id;
        public Sprites sprites;
        public string height;
        public string weight;
        public PokemonTypes[] types;
        public PokemonStats[] stats;
    }

    [Serializable]
    public class Sprites
    {
        public string front_default;
    }

    [Serializable]
    public class PokemonResult
    {
        public string name;
        public string url;
    }

    [Serializable]
    public class PokemonList
    {
        public string count;
        public string next;
        public string previous;
        public PokemonResult[] results;
    }
    //--- End of Json Classes ---

    const string POKEMON_API = "https://pokeapi.co/api/v2/pokemon";
    const string POKEMON_LIST_URL_1 = "?limit=";
    const string POKEMON_LIST_URL_2 = "&offset=0";

    public static int POKEMON_MAX_HP = 0;
    public static int POKEMON_MAX_ATTACK = 0;
    public static int POKEMON_MAX_DEFENSE = 0;
    public static int POKEMON_MAX_SPEED = 0;

    [SerializeField] private int _maxPokemon;
    [Header("Sender Events")]
    [SerializeField] private StringEventChannelSO _eventCreateSelectablePokemon;
    [SerializeField] private PokemonDetailEventChannelSO _eventSavePokemonDetail;

    private void Start()
    {
        string listUrl = $"{POKEMON_LIST_URL_1}{_maxPokemon}{POKEMON_LIST_URL_2}";
        StartCoroutine(LoadPokemonList(listUrl));
    }

    private IEnumerator LoadPokemonList(string listUrl)
    {
        var request = new UnityWebRequest(POKEMON_API + listUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error Pokemon API: " + request.error);
        }
        else
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonPokemonList = request.downloadHandler.text;
                PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(jsonPokemonList);
                for (int i = 0; i < pokemonList.results.Length; ++i)
                {
                    _eventCreateSelectablePokemon.RaiseEvent(pokemonList.results[i].name);
                    StartCoroutine(LoadPokemonDetail(pokemonList.results[i].url));
                }
                Debug.Log($"jsonPokemon {jsonPokemonList}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Unathourized Pokemon API");
            }
            else
            {
                Debug.Log($"Error Pokemon API: { request.error}");
            }
        }
    }

    private IEnumerator LoadPokemonDetail(string urlPokemon)
    {
        var request = new UnityWebRequest(urlPokemon, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error Pokemon API: " + request.error);
        }
        else
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonPokemon = request.downloadHandler.text;
                PokemonDetailDB pokemonDetailFromDatabase = JsonUtility.FromJson<PokemonDetailDB>(jsonPokemon);
                // Fill data for each pokemon.
                PokemonDetail pokemonDetail = new PokemonDetail();
                pokemonDetail.name = pokemonDetailFromDatabase.name;
                pokemonDetail.id = pokemonDetailFromDatabase.id;
                pokemonDetail.height = pokemonDetailFromDatabase.height;
                pokemonDetail.weight = pokemonDetailFromDatabase.weight;
                int countTypes = pokemonDetailFromDatabase.types.Length;
                pokemonDetail.types = new string[countTypes];
                for (int i = 0; i < countTypes; ++i)
                {
                    pokemonDetail.types[i] = pokemonDetailFromDatabase.types[i].type.name;
                }
                int countStats = pokemonDetailFromDatabase.stats.Length;
                for (int i = 0; i < countStats; ++i)
                {
                    switch (i)
                    {
                        case 0:
                            // HP.
                            pokemonDetail.base_hp = pokemonDetailFromDatabase.stats[i].base_stat;
                            if (pokemonDetail.base_hp > POKEMON_MAX_HP)
                            {
                                POKEMON_MAX_HP = pokemonDetail.base_hp;
                            }
                            break;
                        case 1:
                            // Attack.
                            pokemonDetail.base_attack = pokemonDetailFromDatabase.stats[i].base_stat;
                            if (pokemonDetail.base_attack > POKEMON_MAX_ATTACK)
                            {
                                POKEMON_MAX_ATTACK = pokemonDetail.base_attack;
                            }
                            break;
                        case 2:
                            // Defense.
                            pokemonDetail.base_defense = pokemonDetailFromDatabase.stats[i].base_stat;
                            if (pokemonDetail.base_defense > POKEMON_MAX_DEFENSE)
                            {
                                POKEMON_MAX_DEFENSE = pokemonDetail.base_defense;
                            }
                            break;
                        case 5:
                            // Speed.
                            pokemonDetail.base_speed = pokemonDetailFromDatabase.stats[i].base_stat;
                            if (pokemonDetail.base_speed > POKEMON_MAX_SPEED)
                            {
                                POKEMON_MAX_SPEED = pokemonDetail.base_speed;
                            }
                            break;
                    }
                }

                StartCoroutine(LoadTexture(pokemonDetailFromDatabase.sprites.front_default, pokemonDetail));
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Unathourized Pokemon API");
            }
            else
            {
                Debug.Log($"Error Pokemon API: { request.error}");
            }
        }
    }

    IEnumerator LoadTexture(string url, PokemonDetail pokemonDetail)
    {
        UnityWebRequest requestTexture = UnityWebRequestTexture.GetTexture(url);
        yield return requestTexture.SendWebRequest();

        if (requestTexture.result == UnityWebRequest.Result.ConnectionError ||
            requestTexture.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(requestTexture.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)requestTexture.downloadHandler).texture;
            pokemonDetail.imageDefault = myTexture;
            // After the texture is saved, we can send the pokemon detail class to the scroll list.
            _eventSavePokemonDetail.RaiseEvent(pokemonDetail);
        }
    }
}

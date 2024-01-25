using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ETRE REMPLACER A LA CORRECTION
*/
public class ECSController : MonoBehaviour
{
    #region Constants
    private const float MinInstantColorUpdate = 0.25f;
    private const float ColorUpdateSpeed = 4f;
    #endregion
    
    #region Private attributes
    [SerializeField]
    private Config config;

    [SerializeField]
    private GameObject circlePrefab;

    private readonly Dictionary<uint, GameObject> _gameObjectsForDisplay = new();
    private readonly Dictionary<uint, SpriteRenderer> _spriteRenderersCache = new();
    private readonly Dictionary<uint, float> _lastColorUpdate = new();
    private readonly Dictionary<uint, Color> _nextColorUpdate = new();
    private readonly Stack<uint> _nextColorToDelete = new();
    #endregion

    #region Singleton
    private static ECSController _instance;
    public static ECSController Instance
    {
        get
        {
            if (_instance) return _instance;
            
            _instance = FindObjectOfType<ECSController>();
            if (!_instance)
            {
                Debug.LogError("Can't find instance in scene!!");
            }
            return _instance;
        }
    }
    
    private ECSController() { }
    #endregion

    #region Public API
    public Config Config => config;

    public List<ISystem> AllSystems => _allSystems;
    
    public void CreateShape(uint id, int initialSize)
    {
        var instance = Instantiate(circlePrefab);
        instance.transform.localScale *= initialSize;
        _gameObjectsForDisplay[id] = instance;
        _spriteRenderersCache.Add(id, instance.GetComponent<SpriteRenderer>()) ;
        _lastColorUpdate.Add(id, 0);
    }

    public void DestroyShape(uint id)
    {
        Destroy(_gameObjectsForDisplay[id]);
        _gameObjectsForDisplay.Remove(id);
        _spriteRenderersCache.Remove(id);
        _lastColorUpdate.Remove(id);
    }

    public void UpdateShapePosition(uint id, Vector2 position)
    {
        _gameObjectsForDisplay[id].transform.position = position;
    }

    public void UpdateShapeSize(uint id, float size)
    {
        _gameObjectsForDisplay[id].transform.localScale = Vector2.one * size;
    }

    public void UpdateShapeColor(uint id, Color color)
    {
        if (_nextColorUpdate.ContainsKey(id) && _nextColorUpdate[id] == color)
            return;
        
        if (_spriteRenderersCache[id].color == color)
            return;

        var time = Time.time;
        if (time - _lastColorUpdate[id] > MinInstantColorUpdate)
        {
            _lastColorUpdate[id] = time;
            if (_nextColorUpdate.ContainsKey(id))
                _nextColorUpdate.Remove(id);
            
            _spriteRenderersCache[id].color = color;
            return;
        }

        _nextColorUpdate[id] = color;
    }
    #endregion

    #region System Management
    private List<ISystem> _allSystems = new();

    private void Awake()
    {
        _allSystems = RegisterSystems.GetListOfSystems();

        // If system missing from config, add it and enable it.
        foreach (var system in _allSystems.Where(system => !Config.SystemsEnabled.ContainsKey(system.Name)))
        {
            Config.SystemsEnabled[system.Name] = true;
        }
    }
    
    // Update is called once per frame
    private void ManageColor()
    {
        foreach (var id in _nextColorUpdate.Keys)
        {
            if (!_spriteRenderersCache.ContainsKey(id))
            {
                _nextColorToDelete.Push(id);
                continue;
            }
            
            var currentColor = _spriteRenderersCache[id].color;
            _spriteRenderersCache[id].color = Color.Lerp(currentColor, _nextColorUpdate[id], Time.deltaTime * ColorUpdateSpeed);

            if (currentColor == _nextColorUpdate[id])
                _nextColorToDelete.Push(id);
        }

        while (_nextColorToDelete.Count > 0)
            _nextColorUpdate.Remove(_nextColorToDelete.Pop());
    }
 
    private void Update()
    {
        ManageColor();
        
        foreach (var system in _allSystems.Where(system => Config.SystemsEnabled[system.Name])) {
            system.UpdateSystem();
        }
    }
    #endregion
    
}

public interface ISystem
{
    void UpdateSystem();
    string Name { get; }
}

public interface IComponent
{
    // vide
    // utile pour la correction
}
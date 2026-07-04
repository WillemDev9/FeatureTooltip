using Blazored.LocalStorage;
using FeatureTooltipExt.Core.Models;

namespace FeatureTooltipExt.Core.Services;

public class FeatureService
{
    public FeatureItem? SelectedFeature {get; set;}
    public event Action? OnSelectionChanged;
    public event Action? OnListChanged;
    private readonly ILocalStorageService _localStorage;
    private const string STORAGE_KEY = "featuresList";
    public FeatureService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        Console.WriteLine("Created Feature Service");
    }
    public async Task AddFeature(FeatureItem item)
    {
        var currentList = await _localStorage.GetItemAsync<List<FeatureItem>>(STORAGE_KEY) ?? new List<FeatureItem>();

        currentList.Add(item);

        await _localStorage.SetItemAsync(STORAGE_KEY, currentList);

        OnListChanged?.Invoke();

    }

    public async Task RemoveFeature(FeatureItem feature)
    {
        var currentList = await _localStorage.GetItemAsync<List<FeatureItem>>(STORAGE_KEY) ?? new List<FeatureItem>();

        var itemToRemove = currentList.FirstOrDefault(x => x.Id ==feature.Id);
        if(itemToRemove != null)
        {
            currentList.Remove(itemToRemove);

            await _localStorage.SetItemAsync(STORAGE_KEY, currentList);
            
            OnListChanged?.Invoke();
        }
    }

    public async Task<List<FeatureItem>> GetFeatureList()
    {
        var featureList = await _localStorage.GetItemAsync<List<FeatureItem>>(STORAGE_KEY) ?? new List<FeatureItem>();

        return featureList; 
    }
    
    public void SelectFeature(FeatureItem? item)
    {
        SelectedFeature = item;
        OnSelectionChanged?.Invoke();
    }
}
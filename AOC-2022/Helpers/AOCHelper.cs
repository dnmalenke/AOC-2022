using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net;

namespace AOC_2022.Helpers
{
    public class AOCHelper
    {
        private readonly ILocalStorageService _lStorage;

        public AOCHelper(ILocalStorageService lStorage)
        {
            _lStorage = lStorage;
        }

        public async Task<string> LoadInput(int day, int slot)
        {
            return await _lStorage.GetItemAsync<string>($"d{day}s{slot}");
        }

        public async Task SaveInput(int day, int slot, string input)
        {
            await _lStorage.SetItemAsync($"d{day}s{slot}", input);
        }
    }
}

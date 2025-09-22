﻿using Avenga.NotesApp.Dto.FruitDtos;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface IFruitService
    {
        Task<FruitDto> GetFruitInfoAsync(string fruitName);
        Task<List<FruitDto>> GetFruitsByOrderAsync(string orderName);
        Task<List<FruitDto>> GetFruitsByGenusAsync(string genusName);
        Task<List<FruitDto>> GetFruitsByFamilyAsync(string familyName);
        Task<List<FruitDto>> GetAllFruitsAsync();
    }
}

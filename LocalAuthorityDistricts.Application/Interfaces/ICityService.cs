﻿using LocalAuthorityDistricts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.Application
{
    public interface ICityService
    {
        Task<List<City>> GetCitiesAsync(string searchQuery = "");
    }
}
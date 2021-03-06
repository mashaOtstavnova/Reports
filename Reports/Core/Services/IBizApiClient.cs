﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using IikoBizApi;

namespace Core.Services
{
    public interface IBizApiClient
    {
       void Init(Settings settings, HttpClient client = null);
       Task<T> GetAsync<T>(string path, object additionalQueryValues = null, bool needsAccessToken = true);
      
    }
}

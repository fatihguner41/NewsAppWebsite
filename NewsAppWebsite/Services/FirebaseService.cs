﻿namespace NewsAppWebsite.Services
{
    using dotenv.net;
    using FireSharp.Config;
    using FireSharp.Interfaces;
    using FireSharp.Response;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NewsAppWebsite.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FirebaseService : IFirebaseService
    {
        private readonly IFirebaseClient _firebaseClient;

        public FirebaseService()
        {

            // Firebase yapılandırma bilgilerini IConfiguration üzerinden alın
            IFirebaseConfig config = new FirebaseConfig
            {
                // add your secret to .env file
                AuthSecret = Environment.GetEnvironmentVariable("FIREBASE_SECRET"),
                BasePath = "https://haberler-a63c9-default-rtdb.europe-west1.firebasedatabase.app/"
            };

            _firebaseClient = new FireSharp.FirebaseClient(config);
        }

        public IFirebaseClient FirebaseClient => _firebaseClient;
    }

}

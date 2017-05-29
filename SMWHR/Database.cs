using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using SMWHR.Models;

namespace SMWHR
{
    public class Database
    {
        private FirebaseClient Client { get; set; }
        public Database()
        {
            Client = new FirebaseClient("https://smwhr-9ba85.firebaseio.com/");
        }

        public async Task<List<Place>> GetPlaces()
        {
            var items = await Client
              .Child("places")
              //.WithAuth("<Authentication Token>") // <-- Add Auth token if required. Auth instructions further down in readme.
              .OrderByKey()
              //.LimitToFirst(2)
              .OnceAsync<Place>();

            var places = items.Select(item => item.Object).ToList();
            return places;
        }

        public void CreatePlace(string name, string description)
        {
            var item = Client
              .Child("places")
              //.Child("Ricardo")
              //.WithAuth("<Authentication Token>") // <-- Add Auth token if required. Auth instructions further down in readme.
              .PutAsync(new Place(name, description));

            item.Wait();
        }

    }
}
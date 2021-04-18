using Bogus;
using CountryData.Bogus;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fake_users.User;

namespace fake_users
{
    class Generate
    {
        private readonly string _region;
        private readonly int _usersNumber;
        

        private enum Russian
        {
            Россия,
            РФ,
        }

        private enum USA
        {
            USA,
            US,
        }

        public Generate(int usersNumber, string region)
        {
            _region = region;
            _usersNumber = usersNumber;
        }
           
        private Faker<User> getGeneratorUser()
        {

            var localcode = _region != "en_US" ? _region.Substring(0, 2) : _region; 
            
            return new Faker<User>(localcode)
                .RuleFor(u => u.FullName, f => f.Name.FullName())                
                .Rules((f, u) =>
                {
                    if (localcode == "ru")
                    {    
                        u.Country = f.PickRandom<Russian>().ToString();
                    }
                    else if (localcode == "en_US") 
                    {
                          u.Country = f.PickRandom<USA>().ToString();
                    }

                    else 
                    {
                          u.Country = "Україна";
                    }
                })
                .RuleFor(u => u.ZipCode, f => f.Address.ZipCode())
                .RuleFor(u => u.City, f => f.Address.City())
                .RuleFor(u => u.Street, f => f.Address.StreetAddress())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());

        }
        public void Start()
        {
            printUsers();
        }


            private void printUsers()
            {
                Faker<User> generatorUser = getGeneratorUser();
                List<User> users = generatorUser.Generate(_usersNumber);
                ServiceStack.Text.CsvConfig.ItemSeperatorString = ";";
                var str = CsvSerializer.SerializeToCsv<User>(users);
                Console.WriteLine(str);
            }
            


        
    }


        

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expeditors_Coding_Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            string[][] rawData = { new string[6] { "Dave", "Smith", "123 main st.", "seattle", "wa", "43" },
            new string[6] { "Alice", "Smith", "123 Main St.", "Seattle", "WA", "45"},
            new string[6] { "Bob", "Williams", "234 2nd Ave.", "Tacoma", "WA", "26"},
            new string[6] { "Carol", "Johnson", "234 2nd Ave", "Seattle", "WA", "67"},
            new string[6] { "Eve", "Smith", "234 2nd Ave.", "Tacoma", "WA", "25"},
            new string[6] { "Frank", "Jones", "234 2nd Ave.", "Tacoma", "FL", "23"},
            new string[6] { "George", "Brown", "345 3rd Blvd., Apt. 200", "Seattle", "WA", "18"},
            new string[6] { "Helen", "Brown", "345 3rd Blvd. Apt. 200", "Seattle", "WA", "18"},
            new string[6] { "Ian", "Smith", "123 main st ", "Seattle", "Wa", "18"},
            new string[6] { "Jane", "Smith", "123 Main St.", "Seattle", "WA", "13"}
            };

            var personArray = Person.ToObjArray(rawData);
            var HouseholdList = new List<Household>();

            foreach (Person person in personArray)
            {
                TrimAddress(person);
                person.City = char.ToUpper(person.City[0]) + person.City.Substring(1);
            }

            HouseholdList = initializeHouseholds(personArray, HouseholdList);
            NumberHouseholds(HouseholdList);
            CorrespondHouseholds(personArray, HouseholdList);

            foreach (Household household in HouseholdList)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("Household: {0}, Number of Occupants: {1}\nAdults:", household.Number, household.NumberOfPeople);
                var HouseholdMembers = from p in personArray
                                       where p.HouseholdNumber == household.Number && p.Age > 18
                                       orderby p.FirstName
                                       orderby p.LastName
                                       select p;
                foreach (Person p in HouseholdMembers)
                {
                    Console.WriteLine("{0} {1}, {2} {3}, {4}; {5}", p.FirstName, p.LastName, p.Address, p.City, p.State.ToUpper(), p.Age);
                }
            }
            Console.ReadLine();
        }

        public static void TrimAddress(Person person)
        {
            // this function removes punctuation and spaces to check if address is the same
            var trimmedAddress = new StringBuilder();
            foreach (char c in person.Address)
            {
                if (!char.IsPunctuation(c))
                    trimmedAddress.Append(c);
            }

            string addressVerifier = trimmedAddress.ToString().TrimEnd();
            string completedAddress = UppercaseFirstEach(addressVerifier);
            person.Address = completedAddress;
        }

        public static string UppercaseFirstEach(string s)
        {
            // this function fixes capitalization issues in the address
            char[] a = s.ToLower().ToCharArray();

            for (int i = 0; i < a.Count(); i++)
            {
                a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];
            }

            return new string(a);
        }

        public static List<Household> initializeHouseholds(Person[] personArray, List<Household> HouseholdList)
        {
            for (int i = 0; i< personArray.Length; i++)
            {
                Household newHousehold = new Household();
                newHousehold.Address = personArray[i].Address;
                newHousehold.City = personArray[i].City;
                newHousehold.State = personArray[i].State;
                newHousehold.NumberOfPeople = 1;
                HouseholdList.Add(newHousehold);
            }

            List<Household> newList = HouseholdList.OrderBy(o => o.Address).ToList();
            newList = newList.OrderBy(o => o.City).ToList();
            newList = newList.OrderBy(o => o.State).ToList();
            for (int i = 0; i< newList.Count-1; i++)
            {
                if(newList[i].Address == newList[i + 1].Address 
                    && newList[i].City.ToLower() == newList[i + 1].City.ToLower()
                    && newList[i].State.ToLower() == newList[i + 1].State.ToLower())
                {
                    newList[i].NumberOfPeople += 1;
                    newList.Remove(newList[i + 1]);
                    i = -1;
                }
            }
            return newList;
        }

        public static void NumberHouseholds(List<Household> HouseholdList)
        {
            for (int i = 0; i < HouseholdList.Count; i++)
            {
                Household household = HouseholdList[i];
                household.Number = i + 1;
            }
        }

        public static void CorrespondHouseholds(Person[] personArray, List<Household> HouseholdList)
        {
            foreach (Person person in personArray)
            {
                for(int i = 0; i< HouseholdList.Count; i++)
                {
                    if(person.Address == HouseholdList[i].Address 
                        && person.City.ToLower() == HouseholdList[i].City.ToLower()
                        && person.State.ToLower() == HouseholdList[i].State.ToLower())
                    {
                        person.HouseholdNumber = i+1;
                        continue;
                    }
                    else { continue; }
                }
            }
        }

    }

    public class Person
    {
        public string FirstName;
        public string LastName;
        public string Address;
        public string City;
        public string State;
        public int Age;
        public int HouseholdNumber;

        public static Person[] ToObjArray(string[][] rawData)
        {
            Person[] personArray = new Person[rawData.Length];
            for(int i = 0; i<rawData.Length; i++)
            {
                Person person = new Person();
                person.FirstName = rawData[i][0];
                person.LastName = rawData[i][1];
                person.Address = rawData[i][2];
                person.City = rawData[i][3];
                person.State = rawData[i][4];
                // should validate
                person.Age = int.Parse(rawData[i][5]);
                personArray[i] = person;
            }
            return personArray;
        }
    }
    
    public class Household
    {
        public int Number;
        public string Address;
        public string City;
        public string State;
        public int NumberOfPeople;
    }
}

using Dal.Models;
using ExpressionLogic;
using ExpressionLogic.FilteredProperty;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=(localdb)\\mssqllocaldb;" +
                    "Database=aspnet-NumberGuesserWeb;" +
                    "Trusted_Connection=True;MultipleActiveResultSets=true";
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.AspNetUsers", conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add(new Player
                        {
                            Login = (string)reader[1],
                            Score = (int)reader[17],
                            Wins = (int)reader[18],
                            Loses = (int)reader[16],
                            Birthday = (DateTime)reader[19]
                        });
                    }
                }
                // use the connection here
            }
            foreach (var player in players)
            {
                Console.WriteLine(player);
            }
            Console.WriteLine("**************");

            var builder = new FilteredPropertyBuilder(typeof(Player));
            var chainCreator = new ChainCreator(builder);
            var filteredFields = new List<IFilteredProperty>();
            filteredFields.Add(builder.Build(nameof(Player.Login)));
            filteredFields.Add(builder.Build(nameof(Player.Score)));
            filteredFields.Add(builder.Build(nameof(Player.Wins)));
            filteredFields.Add(builder.Build(nameof(Player.Loses)));
            filteredFields.Add(builder.Build(nameof(Player.Birthday)));

            var dynamicFilter = new DynamicFilter<Player>();

            while (true)
            {
                var prediction = new Prediction();
                for (int i = 0; i < filteredFields.Count; i++)
                {
                    Console.WriteLine($"{i}) {filteredFields[i].FieldName}");
                }
                Console.Write("Choose field (index) or -1 to end: ");
                var indexOfField = ReadObj<int>();
                if (indexOfField == -1)
                {
                    break;
                }
                var filteredField = filteredFields[indexOfField];
                var complex = filteredField as IComplexFilteredProperty;
                if (complex != null)
                {
                    var properties = complex.GetProperties();
                    for (var i = 0; i < properties.Count; i++)
                        Console.WriteLine($"{i}) {properties[i].Name}");
                    Console.Write("Choose property (index): or -1 to continue ");
                    var index = ReadObj<int>();
                    if (index != -1)
                    {
                        chainCreator.CreateChain(prediction, filteredField, properties[index]);
                    }
                }

                prediction.PropertyName = filteredField.FieldName;

                for (int i = 0; i < filteredField.AvailableActions.Count; i++)
                {
                    Console.WriteLine($"{i}) {filteredField.AvailableActions[i]}");
                }
                Console.Write("Choose action (index): ");
                var indexOfAction = ReadObj<int>();
                prediction.CompareAction = filteredField.AvailableActions[indexOfAction];

                if (prediction.NeedRight())
                {
                    Console.Write("Set right part: ");
                    var rightPart = ReadObj(filteredField.FieldType);
                    prediction.RightValue = rightPart;
                }

                dynamicFilter.AddPredict(prediction);
            }

            Console.WriteLine($"lambda: {dynamicFilter.GetLambda()}");

            var filtered = dynamicFilter.Filter(players);
            foreach (var round in filtered)
            {
                Console.WriteLine(round);
            }

            Console.Read();
        }

        private static T ReadObj<T>()
        {
            return (T)ReadObj(typeof(T));
        }

        private static object ReadObj(Type type)
        {
            var line = Console.ReadLine();
            object obj = null;

            while (obj == null)
            {
                try
                {
                    obj = Convert.ChangeType(line, type);
                }
                catch (Exception)
                {
                    Console.WriteLine($"'{line}' it is not a {type.Name}. Please enter {type.Name}");
                    line = Console.ReadLine();
                }
            }

            return obj;
        }
    }
}

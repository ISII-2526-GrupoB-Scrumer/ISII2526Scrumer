using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API;
using Humanizer;

namespace AppForSEII2526
{
    public class GetCochesParaAlquilar_test : AppForSEII25264SqliteUT
    {
        public GetCochesParaAlquilar_test()
        {

            var Models = new List<Model>()
            {
                new Model("1", "Q5"),
                new Model("2", "508"),
                new Model("3", "Civic"),
                new Model("4", "Corolla")
            };



            var Cars = new List<Car>()
            {
                new Car("Turimo","Blanco","","1.8","Gasolina", 1,"Estandar","Audi", 18000, 2, 4, 180, 18, Models[0]),
                new Car("Turimo", "Negro", "", "1.5", "Diesel", 2, "Deportivo", "Peugeot", 15000, 2, 4, 200, 15, Models[1]),
                new Car("Familiar", "Azul", "", "2.0", "Hibrido", 3, "Estandar", "Honda", 22000, 4, 5, 160, 20, Models[2]),
                new Car("Familiar", "Rojo", "", "1.6", "Gasolina", 4, "Estandar", "Toyota", 17000, 4, 5, 140, 17, Models[3])
            };


            var users = new List<ApplicationUser>()
            {
                new ApplicationUser("Albacete 1","600000001","Manuel","Garcia","manolito"),
                new ApplicationUser("Albacete 2","600000002","Lucia","Martinez","luci"),
                new ApplicationUser("Albacete 3","600000003","Carlos","Lopez","carlitos"),
                new ApplicationUser("Albacete 4","600000004","Ana","Gonzalez","anita")
            };

            var rental = new Rental("Albacete center", new DateTime(2025, 10, 20), 1, new DateTime(2025, 10, 15), new DateTime(2025, 10, 16), 180,"Tarjeta");

            var rentalItem = new RentalItem(1,1,1);



            _context.AddRange(Models);
            _context.AddRange(Cars);
            _context.AddRange(users);
            _context.SaveChanges();

        }


        

        

    }
}

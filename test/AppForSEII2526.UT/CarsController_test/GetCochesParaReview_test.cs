using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.API.Controllers;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CarsController_test
{
    public class GetCochesParaReview_test: AppForSEII25264SqliteUT
    {
        public GetCochesParaReview_test() {

            var models = new List<Model>(){
                new Model("1", "Corolla"),
                new Model("2", "Civic"),
                new Model("3", "Q5"),
                new Model("4", "Model_3")

            };

            var cars = new List<Car>(){
                new Car("Turismo", "Negro", "Muy bonito y economico", "1.8", "Gasolina", 1, "Estandar", "Toyota", 20000, 7, 3, 80, 19, models[0]),
                new Car("Turismo", "Rojo", "Deportivo y rapido", "2.0", "Gasolina", 2, "Estandar", "Honda", 25000, 5, 2, 90, 18, models[1]),
                new Car("SUV", "Azul", "Comodo y espacioso", "2.5", "Diesel", 3, "Premium", "Audi", 40000, 4, 1, 120, 20, models[2]),
                new Car("Sedan", "Blanco", "Tecnologia avanzada", "0.0", "Electrico", 4, "Premium", "Tesla", 50000, 6, 2, 150, 21, models[3])

            };

            var users = new List<ApplicationUser>(){
                new ApplicationUser("Albacete 1", "9999999", "Manolo", "Cifuentes", "Manolito_1"),
                new ApplicationUser("Albacete 2", "8888888", "Armando", "Camela", "Caramelito_99"),
                new ApplicationUser("Albacete 3", "7777777", "Ana", "Paredes", "Anita_123"),
                new ApplicationUser("Albacete 4", "6666666", "Benito", "Lama", "Beni_456")

            };
        }
    }
}
